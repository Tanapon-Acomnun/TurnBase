using TurnBase.Services;

namespace TurnBase.Views;

public partial class MainMenuPage : ContentPage
{
    private AudioService _audioService;

    public MainMenuPage()
    {
        InitializeComponent();

        NavigationPage.SetHasNavigationBar(this, false);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _audioService =
            Application.Current?.Handler?.MauiContext?.Services
            ?.GetService<AudioService>();

        if (_audioService != null)
        {
            await _audioService.PlayBgm("main-menu-theme.mp3");
        }
    }

    private async void OnStartClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CharacterSelectPage());
    }
}