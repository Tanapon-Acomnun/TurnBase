using Microsoft.Maui.Controls.Shapes;
using TurnBase.Model;

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
        PointsLabel.Text = $"Available Stat Points: {_player.Currency}";
        AttackLabel.Text = $"ATK: {_player.Attack}";
        HPLabel.Text = $"HP: {_player.MaxHP}";
        MPLabel.Text = $"MP: {_player.MaxMP}";
    }

    private bool HasCurrency(int amount) => _player.Currency >= amount;

    private void OnAttackUpgradeClicked(object sender, EventArgs e)
    {
        if (!HasCurrency(1)) return;
        _player.Attack += 1;
        _player.Currency -= 1;
        RefreshUI();
        GenerateSkillUnlocks();
    }

    private void OnHPUpgradeClicked(object sender, EventArgs e)
    {
        if (!HasCurrency(1)) return;
        _player.MaxHP += 5;
        _player.CurrentHP += 5;
        _player.Currency -= 1;
        RefreshUI();
        GenerateSkillUnlocks();
    }

    private void OnMPUpgradeClicked(object sender, EventArgs e)
    {
        if (!HasCurrency(1)) return;
        _player.MaxMP += 5;
        _player.MP += 5;
        _player.Currency -= 1;
        RefreshUI();
        GenerateSkillUnlocks();
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
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
                Text = GetSkillIcon(skill.Name),
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
                Text = skill.Name.ToUpper(),
                FontSize = 17,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#ddd6fe"),
                CharacterSpacing = 2,
                FontFamily = "Georgia"
            });
            info.Add(new Label
            {
                Text = $"Cost: {skill.UnlockCost} pts",
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
                Text = "UNLOCK",
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
        "Lifesteal" => "🩸",
        "Execute" => "💀",
        "Fireball" => "🔥",
        "Thunder" => "⚡",
        "Heal" => "💚",
        "Berserk" => "😤"
    };
    // ────────────────────────────────────────────────

    private void UnlockSkill(Skill skill)
    {
        if (!HasCurrency(skill.UnlockCost)) return;

        _player.Currency -= skill.UnlockCost;

        if (skill.Name == "Lifesteal")
            _player.HasLifesteal = true;
        else
            _player.Skills.Add(skill);

        _player.UnlockableSkills.Remove(skill);

        RefreshUI();
        GenerateSkillUnlocks();
    }
}