namespace TurnBase.Views
{
    public class BaseGamePage : ContentPage
    {
        public BaseGamePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            return true; // Disable Android/system back
        }
    }
}