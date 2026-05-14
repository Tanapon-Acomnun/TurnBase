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

        player.Currency += 3;

        BindingContext = this;

        player.Stage++;
    }


    public string RewardText =>
    "Earned 10 Currency!";
    public string CurrencyText =>
    $"Total Currency: {_player.Currency}";

    private async void OnNextBattleClicked(object sender, EventArgs e)
    {

        if (_player.Stage % 3 == 0)
        {
            await Navigation.PushAsync(
                new UpgradePage(_player));
        }
        else
        {
            await Navigation.PushAsync(
                new BattlePage(_player));
        }
    }
}