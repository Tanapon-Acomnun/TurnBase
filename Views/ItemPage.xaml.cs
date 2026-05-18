using TurnBase.Model;

namespace TurnBase.Views;

public partial class ItemPage : BaseGamePage
{
    private readonly Action<string> _onItemSelected;

    // เปลี่ยนเป็น Property เพื่อ Binding ได้
    public Character Player { get; set; }

    public ItemPage(Character player, Action<string> onItemSelected)
    {
        InitializeComponent();

        Player = player;

        _onItemSelected = onItemSelected;

        // สำคัญ
        BindingContext = this;
    }

    private async void OnPotionClicked(object sender, EventArgs e)
    {
        // เช็คจำนวนก่อนใช้
        if (Player.Potions <= 0)
        {
            await DisplayAlert("No Potion", "You don't have any potions left.", "OK");
            return;
        }

        _onItemSelected?.Invoke("Potion");

        await Navigation.PopAsync();
    }

    private async void OnEtherClicked(object sender, EventArgs e)
    {
        // เช็คจำนวนก่อนใช้
        if (Player.Ethers <= 0)
        {
            await DisplayAlert("No Ether", "You don't have any ethers left.", "OK");
            return;
        }

        _onItemSelected?.Invoke("Ether");

        await Navigation.PopAsync();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}