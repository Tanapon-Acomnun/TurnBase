using TurnBase.Model;
using TurnBase.Services;

namespace TurnBase.Views;

public partial class VictoryPage : BaseGamePage
{
    private Character _player;

    private AudioService _audioService;

    public VictoryPage(Character player)
    {
        InitializeComponent();

        _player = player;

        _player.BattleNumber++;

        player.Currency += 3;

        BindingContext = this;

        player.Stage++;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _audioService =
            Application.Current?.Handler?.MauiContext?.Services
            ?.GetService<AudioService>();

        if (_audioService != null)
        {
            await _audioService.PlaySfx("victory-sound.mp3");
        }
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