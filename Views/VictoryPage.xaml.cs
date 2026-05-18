using TurnBase.Model;
using TurnBase.Services;

namespace TurnBase.Views;

public partial class VictoryPage : BaseGamePage
{
    private Character _player;

    private AudioService _audioService;

    public VictoryPage(Character player)
    {
        InitializeComponent();

        _player = player;

        _player.BattleNumber++;

        player.Currency += 3;

        BindingContext = this;

        player.Stage++;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _audioService =
            Application.Current?.Handler?.MauiContext?.Services
            ?.GetService<AudioService>();

        if (_audioService != null)
        {
            await _audioService.PlaySfx("victory-sound.mp3");
        }
    }

    public string RewardText =>
    "ได้รับ 3 แต้ม !";

    public string CurrencyText =>
    $"แต้มทั้งหมด : {_player.Currency}";

    private async void OnNextBattleClicked(object sender, EventArgs e)
    {

        if (_player.BattleNumber > 15)
        {
            await Navigation.PushAsync(new EndStoryPage(_player));
        }
        else if (_player.BattleNumber == 5)
        {
            await Navigation.PushAsync(new MiniBossDialoguePage(_player, 1, "king.png"));
        }
        else if (_player.BattleNumber == 10)
        {
            await Navigation.PushAsync(new MiniBossDialoguePage(_player, 2, "orc.png"));
        }
        else if (_player.BattleNumber == 15)
        {
            await Navigation.PushAsync(new FinalBossDialoguePage(_player));
        }
        else if (_player.Stage % 3 == 0)
        {
            await Navigation.PushAsync(new UpgradePage(_player));
        }
        else
        {
            await Navigation.PushAsync(new BattlePage(_player));
        }

        Navigation.RemovePage(this);

        // ==========================================
        // 💾 โค้ดที่เพิ่มใหม่: ระบบปุ่มเล่นต่อ (Load Game)
        // ==========================================

        // 1. เตรียมรายชื่อสกิล (ดึงมาเฉพาะชื่อ)
        var savedSkills = _player.Skills != null
                ? _player.Skills.Select(s => s.Name).Distinct().ToList()
                : new List<string>();

        var savedUnlockable = _player.UnlockableSkills != null
            ? _player.UnlockableSkills.Select(s => s.Name).ToList()
            : new List<string>();

        // 2. สร้างก้อนข้อมูลเซฟที่รวมไอเท็มและสกิลแล้ว
        var mySave = new SaveData
        {
            SelectedName = _player.Name,
            Sprite = _player.Sprite,
            CurrentStage = _player.Stage,
            BattleNumber = _player.BattleNumber,
            TotalCurrency = _player.Currency,
            MaxHP = _player.MaxHP,
            Attack = _player.Attack,
            MaxMP = _player.MaxMP,


            // บันทึกไอเท็มและสกิลตรงนี้ครับ
            Potions = _player.Potions,
            Ethers = _player.Ethers,
            UnlockedSkills = savedSkills,
            AvailableUnlockableSkills = savedUnlockable
        };

        // 3. สั่งบันทึกลงเครื่อง
        SaveService.SaveGame(mySave);

        // ... ระบบเปลี่ยนหน้าคงเดิม ...
        if (_player.Stage % 3 == 0)
        {
            await Navigation.PushAsync(new UpgradePage(_player));
        }
        else
        {
            await Navigation.PushAsync(new BattlePage(_player));
        }
    }

}