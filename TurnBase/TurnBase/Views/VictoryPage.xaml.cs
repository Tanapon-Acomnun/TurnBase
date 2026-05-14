using TurnBase.Model;

namespace TurnBase.Views;

public partial class VictoryPage : BaseGamePage
{
    private Character _player;

    public VictoryPage(Character player)
    {
        InitializeComponent();

        _player = player;

        _player.BattleNumber++;

        _player.StatPoints += 3;

        VictoryMessageLabel.Text =
            $"{_player.Name} won the battle!\n\n+3 Stat Points!";
    }

    private async void OnUpgradeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UpgradePage(_player));
        Navigation.RemovePage(this);
    }

    private async void OnNextBattleClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BattlePage(_player));
        Navigation.RemovePage(this);
    }
}