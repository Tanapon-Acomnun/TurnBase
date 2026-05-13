using Microsoft.Extensions.DependencyInjection;
using TurnBase.Views;

namespace TurnBase
{
    public partial class App : Application
    {
        public App()
        {
            MainPage = new NavigationPage(new MainMenuPage());
        }
    }
}