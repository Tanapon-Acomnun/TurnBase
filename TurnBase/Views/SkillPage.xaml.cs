using TurnBase.Model;

namespace TurnBase.Views;

public partial class SkillPage : BaseGamePage
{
    private Character _player;

    private readonly Action<string> _onSkillSelected;

    public SkillPage(Character player, Action<string> onSkillSelected)
    {
        InitializeComponent();

        _player = player;
        _onSkillSelected = onSkillSelected;

        SetupSkills();
    }

    private void SetupSkills()
    {
        switch (_player.Name)
        {
            case "Wizard":
                SkillButton1.Text = "Fireball";
                break;

            case "Knight":
                SkillButton1.Text = "Shield Bash";
                break;

            case "Berserker":
                SkillButton1.Text = "Rage Slash";
                break;
        }
    }

    private async void OnSkill1Clicked(object sender, EventArgs e)
    {
        _onSkillSelected.Invoke(SkillButton1.Text);

        await Navigation.PopAsync();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}