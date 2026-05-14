using TurnBase.Model;

namespace TurnBase.Views;

public partial class SkillPage : ContentPage
{
    private readonly Action<string> _onSkillSelected;

    private Character _player;

    public SkillPage(
        Character player,
        Action<string> onSkillSelected)
    {
        InitializeComponent();

        _player = player;

        _onSkillSelected = onSkillSelected;

        LoadSkills();
    }

    private void LoadSkills()
    {
        SkillContainer.Children.Clear();

        if (_player.Name == "Wizard")
        {
            AddSkillButton("Fireball");
        }

        if (_player.Name == "Knight")
        {
            AddSkillButton("Shield Bash");
        }

        if (_player.Name == "Berserker")
        {
            AddSkillButton("Rage Slash");
        }
    }

    private void AddSkillButton(string skillName)
    {
        Button skillButton = new Button
        {
            Text = skillName,
            FontSize = 18,
            BackgroundColor = Color.FromArgb("#5b21b6"),
            TextColor = Colors.White,
            CornerRadius = 10,
            Margin = new Thickness(0, 10)
        };

        skillButton.Clicked += async (s, e) =>
        {
            _onSkillSelected?.Invoke(skillName);

            await Navigation.PopAsync();
        };

        SkillContainer.Children.Add(skillButton);
    }
}