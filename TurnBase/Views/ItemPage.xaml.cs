using TurnBase.Model;

namespace TurnBase.Views;

public partial class ItemPage : BaseGamePage
{
    private Character _player;

    public ItemPage(Character player)
    {
        InitializeComponent();

        _player = player;
    }

    private async void OnPotionClicked(object sender, EventArgs e)
    {
        if (_player.Potions <= 0)
        {
            await DisplayAlert("No Potions", "You have no Potions left.", "OK");
            return;
        }

        if (_player.CurrentHP >= _player.MaxHP)
        {
            await DisplayAlert("HP Full", "Your HP is already full.", "OK");
            return;
        }

        _player.Potions--;

        _player.CurrentHP += 20;

        if (_player.CurrentHP > _player.MaxHP)
            _player.CurrentHP = _player.MaxHP;

        await Navigation.PopAsync();
    }

    private async void OnEtherClicked(object sender, EventArgs e)
    {
        if (_player.Ethers <= 0)
        {
            await DisplayAlert("No Ethers", "You have no Ethers left.", "OK");
            return;
        }

        _player.Ethers--;

        _player.MP += 10;

        await Navigation.PopAsync();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}