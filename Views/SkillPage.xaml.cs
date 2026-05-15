using TurnBase.Model;

namespace TurnBase.Views;

public partial class SkillPage : BaseGamePage
{
    private Character _player;

    private readonly Action<Skill> _onSkillSelected;

    private List<Skill> _skills;

    public SkillPage(Character player, Action<Skill> onSkillSelected)
    {
        InitializeComponent();

        _skills = player.Skills;

        _onSkillSelected = onSkillSelected;

        GenerateSkillButtons();
    }

    private void GenerateSkillButtons()
    {
        foreach (var skill in _skills)
        {
            var button = new Button
            {
                Text = skill.MPCost > 0
                ? $"{skill.Name} (-{skill.MPCost} MP)"
                : skill.SelfDamage > 0
                    ? $"{skill.Name} (-{skill.SelfDamage} HP)"
                    : skill.Name,

                BackgroundColor = Color.FromArgb("#1e1b4b"),

                TextColor = Colors.White,

                CornerRadius = 10,

                FontSize = 16
            };

            button.Clicked += async (s, e) =>
            {
                _onSkillSelected.Invoke(skill);

                await Navigation.PopAsync();
            };

            SkillContainer.Children.Add(button);
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}