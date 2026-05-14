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
        PointsLabel.Text =
            $"Available Stat Points: {_player.Currency}";

        AttackLabel.Text =
            $"ATK: {_player.Attack}";

        HPLabel.Text =
            $"HP: {_player.MaxHP}";

        MPLabel.Text =
            $"MP: {_player.MaxMP}";
    }

    private bool HasCurrency(int amount)
    {
        return _player.Currency >= amount;
    }

    private void OnAttackUpgradeClicked(object sender, EventArgs e)
    {
        if (!HasCurrency(1))
            return;

        _player.Attack += 1;

        _player.Currency -= 1;

        RefreshUI();

        GenerateSkillUnlocks();
    }

    private void OnHPUpgradeClicked(object sender, EventArgs e)
    {
        if (!HasCurrency(1))
            return;

        _player.MaxHP += 5;

        _player.CurrentHP += 5;

        _player.Currency -= 1;

        RefreshUI();

        GenerateSkillUnlocks();
    }

    private void OnMPUpgradeClicked(object sender, EventArgs e)
    {
        if (!HasCurrency(1))
            return;

        _player.MaxMP += 5;

        _player.MP += 5;

        _player.Currency -= 1;

        RefreshUI();

        GenerateSkillUnlocks();
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(
            new BattlePage(_player));
    }

    private void GenerateSkillUnlocks()
    {
        SkillUnlockContainer.Children.Clear();

        foreach (var skill in _player.UnlockableSkills)
        {
            var button = new Button
            {
                Text =
                    $"Unlock {skill.Name} ({skill.UnlockCost} Currency)"
            };

            button.Clicked += (s, e) =>
            {
                UnlockSkill(skill);
            };

            SkillUnlockContainer.Children.Add(button);
        }
    }

    private void UnlockSkill(Skill skill)
    {
        if (!HasCurrency(skill.UnlockCost))
            return;

        _player.Currency -= skill.UnlockCost;

        // Passive unlocks
        if (skill.Name == "Lifesteal")
        {
            _player.HasLifesteal = true;
        }
        else
        {
            _player.Skills.Add(skill);
        }

        _player.UnlockableSkills.Remove(skill);

        RefreshUI();

        GenerateSkillUnlocks();
    }
}