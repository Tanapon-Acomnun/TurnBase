using Microsoft.Maui.Controls.Shapes;
using TurnBase.Model;
using TurnBase.Services; // 👈 เพิ่มบรรทัดนี้เพื่อใช้ระบบเซฟ
using System.Linq;       // 👈 เพิ่มบรรทัดนี้เพื่อจัดการ List สกิล

namespace TurnBase.Views;

public partial class UpgradePage : BaseGamePage
{
    private Character _player;

    public UpgradePage(Character player)
    {
        InitializeComponent();
        _player = player;
        RefreshUI();
        GenerateSkillUnlocks();
    }

    private void RefreshUI()
    {
        PointsLabel.Text = $"แต้มทั้งหมด: {_player.Currency}";
        AttackLabel.Text = $"ATK: {_player.Attack}";
        HPLabel.Text = $"HP: {_player.MaxHP}";
        MPLabel.Text = $"MP: {_player.MaxMP}";
    }

    private bool HasCurrency(int amount) => _player.Currency >= amount;

    // ==========================================
    // 💾 ฟังก์ชันช่วยบันทึกเกม (เรียกใช้ตอนอัปเกรดเสร็จ)
    // ==========================================
    private void SaveCurrentState()
    {
        var savedSkills = _player.Skills != null ? _player.Skills.Select(s => s.Name).Distinct().ToList() : new List<string>();
        var savedUnlockable = _player.UnlockableSkills != null ? _player.UnlockableSkills.Select(s => s.Name).Distinct().ToList() : new List<string>();

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
            Potions = _player.Potions,
            Ethers = _player.Ethers,
            UnlockedSkills = savedSkills,
            AvailableUnlockableSkills = savedUnlockable
        };

        SaveService.SaveGame(mySave);
    }

    private void OnAttackUpgradeClicked(object sender, EventArgs e)
    {
        if (!HasCurrency(1)) return;
        _player.Attack += 2;
        _player.Currency -= 1;
        RefreshUI();
        GenerateSkillUnlocks();
        SaveCurrentState(); // 👈 บันทึกเกมทันที
    }

    private void OnHPUpgradeClicked(object sender, EventArgs e)
    {
        if (!HasCurrency(1)) return;
        _player.MaxHP += 5;
        _player.CurrentHP += 5;
        _player.Currency -= 1;
        RefreshUI();
        GenerateSkillUnlocks();
        SaveCurrentState(); // 👈 บันทึกเกมทันที
    }

    private void OnMPUpgradeClicked(object sender, EventArgs e)
    {
        if (!HasCurrency(1)) return;
        _player.MaxMP += 2;
        _player.MP += 2;
        _player.Currency -= 1;
        RefreshUI();
        GenerateSkillUnlocks();
        SaveCurrentState(); // 👈 บันทึกเกมทันที
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        SaveCurrentState(); // 👈 เซฟกันเหนียวก่อนไปหน้าต่อสู้
        await Navigation.PushAsync(new BattlePage(_player));
    }

    // ── เปลี่ยนแค่ตรงนี้ ──────────────────────────────
    private void GenerateSkillUnlocks()
    {
        SkillUnlockContainer.Children.Clear();

        foreach (var skill in _player.UnlockableSkills)
        {
            var capturedSkill = skill;

            var card = new Border
            {
                Stroke = new SolidColorBrush(Color.FromArgb("#7c3aed")),
                StrokeThickness = 1.5,
                BackgroundColor = Color.FromArgb("#12071f"),
                Padding = new Thickness(22, 18),
                Margin = new Thickness(0, 0, 0, 14)
            };
            card.StrokeShape = new RoundRectangle { CornerRadius = 10 };

            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = 60 },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                ColumnSpacing = 16
            };

            // Icon
            var iconBox = new Border
            {
                BackgroundColor = Color.FromArgb("#0d0418"),
                Stroke = new SolidColorBrush(Color.FromArgb("#7c3aed")),
                StrokeThickness = 1,
                WidthRequest = 54,
                HeightRequest = 54,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            iconBox.StrokeShape = new RoundRectangle { CornerRadius = 8 };
            iconBox.Content = new Label
            {
                Text = GetSkillIcon(skill.Dname),
                FontSize = 26,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            // Info
            var info = new VerticalStackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Spacing = 4
            };
            info.Add(new Label
            {
                Text = skill.Dname.ToUpper(),
                FontSize = 17,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#ddd6fe"),
                CharacterSpacing = 2,
                FontFamily = "Georgia"
            });
            info.Add(new Label
            {
                Text = $"ใช้: {skill.UnlockCost} แต้ม",
                FontSize = 13,
                TextColor = Color.FromArgb("#c4b5fd")
            });

            // Unlock button
            var btnBorder = new Border
            {
                BackgroundColor = Color.FromArgb("#1e0a38"),
                Stroke = new SolidColorBrush(Color.FromArgb("#7c3aed")),
                StrokeThickness = 1,
                Padding = new Thickness(12, 10),
                VerticalOptions = LayoutOptions.Center
            };
            btnBorder.StrokeShape = new RoundRectangle { CornerRadius = 8 };
            btnBorder.Content = new Label
            {
                Text = "ปลดล็อก",
                FontSize = 12,
                TextColor = Color.FromArgb("#c4b5fd"),
                HorizontalOptions = LayoutOptions.Center,
                FontFamily = "Georgia"
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => UnlockSkill(capturedSkill);
            btnBorder.GestureRecognizers.Add(tap);

            Grid.SetColumn(iconBox, 0);
            Grid.SetColumn(info, 1);
            Grid.SetColumn(btnBorder, 2);
            grid.Children.Add(iconBox);
            grid.Children.Add(info);
            grid.Children.Add(btnBorder);

            card.Content = grid;
            SkillUnlockContainer.Children.Add(card);
        }
    }

    private string GetSkillIcon(string skillName) => skillName switch
    {
        "ดูดเลือด" => "🩸",
        "พิฆาต" => "💀",
        "ลูกไฟ" => "🔥",
        "หอกน้ำแข็ง" => "❄️",
        "ฟื้นฟูมานา" => "💧",
        "ท่าตั้งรับสวนกลับ" => "🛡️",
        "ทุบยั่วยุ" => "💪",
        _ => "❔"
    };
    // ────────────────────────────────────────────────

    private async void UnlockSkill(Skill skill)
    {
        // 1. เช็กว่าแต้มพอไหม ถ้าไม่พอให้เด้งแจ้งเตือนแล้วหยุดทำงาน
        if (!HasCurrency(skill.UnlockCost))
        {
            await DisplayAlert("แต้มไม่พอ!", $"คุณต้องการ {skill.UnlockCost} แต้มเพื่อปลดล็อกสกิลนี้\n(ตอนนี้คุณมี {_player.Currency} แต้ม)", "ตกลง");
            return;
        }

        // 2. หักแต้ม
        _player.Currency -= skill.UnlockCost;

        // 3. เอาสกิลเข้าตัว
        if (skill.Name == "Lifesteal")
            _player.HasLifesteal = true;
        else
            _player.Skills.Add(skill);

        // 4. ลบสกิลออกจากรายการรอปลดล็อก
        _player.UnlockableSkills.Remove(skill);

        // 5. รีเฟรชหน้าจอ และเซฟเกมทันที
        RefreshUI();
        GenerateSkillUnlocks();
        SaveCurrentState();

        // 6. แจ้งเตือนว่าปลดล็อกสำเร็จ!
        //await DisplayAlert("สำเร็จ!", $"คุณได้เรียนรู้สกิล {skill.Name} เรียบร้อยแล้ว!", "เยี่ยมไปเลย");
    }
}