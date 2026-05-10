namespace TurnBase.Views;

public partial class MainMenuPage : ContentPage
{
	public MainMenuPage()
	{
        InitializeComponent();
	}
    private async void OnStartClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CharacterSelectPage());
    }
}