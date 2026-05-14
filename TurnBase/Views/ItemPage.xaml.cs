using TurnBase.Model;

namespace TurnBase.Views;

public partial class ItemPage : BaseGamePage
{
    private readonly Action<string> _onItemSelected;

    private Character _player;

    public ItemPage(Character player, Action<string> onItemSelected)
    {
        InitializeComponent();

        _player = player;
        _onItemSelected = onItemSelected;
    }

    private async void OnPotionClicked(object sender, EventArgs e)
    {
        _onItemSelected.Invoke("Potion");

        await Navigation.PopAsync();
    }

    private async void OnEtherClicked(object sender, EventArgs e)
    {
        _onItemSelected.Invoke("Ether");

        await Navigation.PopAsync();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}