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
    }

    private void RefreshUI()
    {
        PointsLabel.Text = $"Available Stat Points: {_player.StatPoints}";

        AttackLabel.Text = $"ATK: {_player.Attack}";
        HPLabel.Text = $"HP: {_player.MaxHP}";
        MPLabel.Text = $"MP: {_player.MP}";
    }

    private bool HasPoints()
    {
        return _player.StatPoints > 0;
    }

    private void OnAttackUpgradeClicked(object sender, EventArgs e)
    {
        if (!HasPoints()) return;

        _player.Attack += 1;
        _player.StatPoints--;

        RefreshUI();
    }

    private void OnHPUpgradeClicked(object sender, EventArgs e)
    {
        if (!HasPoints()) return;

        _player.MaxHP += 5;
        _player.CurrentHP += 5;
        _player.StatPoints--;

        RefreshUI();
    }

    private void OnMPUpgradeClicked(object sender, EventArgs e)
    {
        if (!HasPoints()) return;

        _player.MaxMP += 5;
        _player.MP += 5;
        _player.StatPoints--;

        RefreshUI();
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        _player.Level++;

        // Move to next battle
        await Navigation.PushAsync(new BattlePage(_player));
    }
}