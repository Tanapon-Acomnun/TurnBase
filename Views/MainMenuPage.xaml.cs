using TurnBase.Model;
using TurnBase.Services;
using System.Linq; 

namespace TurnBase.Views;

public partial class MainMenuPage : ContentPage
{
    private AudioService _audioService;

    public MainMenuPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _audioService = Application.Current?.Handler?.MauiContext?.Services?.GetService<AudioService>();

        if (_audioService != null)
        {
            await _audioService.PlayBgm("main-menu-theme.mp3");
        }
    }

    // ปุ่มเริ่มเกมใหม่
    private async void OnStartClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StoryPage());
    }

    // ==========================================
    // 💾             Save
    // ==========================================
    private async void OnContinueClicked(object sender, EventArgs e)
    {
        // 1. เรียกข้อมูลจากเครื่องผ่าน SaveService
        SaveData savedData = SaveService.LoadGame();

        if (savedData != null)
        {
            // 2. ถ้ามีข้อมูลเซฟ ให้ประกอบร่าง Character ขึ้นมาใหม่
            Character loadedPlayer = new Character
            {
                Name = savedData.SelectedName,
                Sprite = savedData.Sprite,
                Stage = savedData.CurrentStage,
                BattleNumber = savedData.BattleNumber,
                Currency = savedData.TotalCurrency,
                MaxHP = savedData.MaxHP,
                CurrentHP = savedData.MaxHP, // โหลดมาแล้วเลือดเต็มหลอด
                Attack = savedData.Attack,
                MaxMP = savedData.MaxMP,
                MP = savedData.MaxMP, // โหลดมาแล้วมานาเต็มหลอด
                Potions = savedData.Potions,
                Ethers = savedData.Ethers
            };

            // ล้างกระเป๋าสกิลให้ว่างก่อน 1 รอบ ป้องกันของเก่าตกค้าง
            loadedPlayer.Skills.Clear();
            loadedPlayer.UnlockableSkills.Clear();

            // 3. ดึงสกิลกลับมาจาก Database ตามชื่อที่บันทึกไว้ (ใส่ .Distinct() เพื่อกันการซ้ำ)
            if (savedData.UnlockedSkills != null)
            {
                foreach (var skillName in savedData.UnlockedSkills.Distinct())
                {
                    loadedPlayer.Skills.Add(SkillDatabase.GetSkill(skillName));
                }
            }

            // 4. ดึงสกิลรอปลดล็อกกลับมา (ลบอันที่พิมพ์เบิ้ลออก เหลือแค่อันนี้อันเดียวพอครับ)
            if (savedData.AvailableUnlockableSkills != null)
            {
                foreach (var skillName in savedData.AvailableUnlockableSkills.Distinct())
                {
                    loadedPlayer.UnlockableSkills.Add(SkillDatabase.GetSkill(skillName));
                }
            }

            // 5. พาส่งไปหน้าต่อสู้ (BattlePage) ทันที
            if (loadedPlayer.BattleNumber > 15)
            {
                await Navigation.PushAsync(new EndStoryPage(loadedPlayer));
            }
            else if (loadedPlayer.Stage % 3 == 0 && loadedPlayer.Stage != 0)
            {
                // ถ้าด่านปัจจุบันหาร 3 ลงตัว ให้ส่งกลับไปหน้าอัปเกรดก่อน
                await Navigation.PushAsync(new UpgradePage(loadedPlayer));
            }
            else
            {
                // ถ้าเป็นด่านปกติ ค่อยส่งไปหน้าต่อสู้
                await Navigation.PushAsync(new BattlePage(loadedPlayer));
            }
        }
        else
        {
            // ถ้าไม่มีไฟล์เซฟ ให้แจ้งเตือนผู้เล่น
            await DisplayAlert("แจ้งเตือน", "ไม่พบข้อมูลการเซฟเกม กรุณาเริ่มเกมใหม่ครับ", "ตกลง");
        }
    }
}