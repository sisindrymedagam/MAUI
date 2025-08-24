using Looply.MAUI.Pages;
using Looply.MAUI.Services;

namespace Looply.MAUI;

public partial class App : Application
{
    readonly IServiceProvider serviceProvider;

    public App(IServiceProvider _serviceProvider)
    {
        serviceProvider = _serviceProvider;
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        if (AuthService.IsLogedIn())
        {
            var window = new Window(new NavigationPage(new MainPage(serviceProvider)));
            window.Destroying += (s, e) => App.StopVideoIfPlaying();
            window.Deactivated += (s, e) => App.StopVideoIfPlaying();
            return window;
        }
        else
        {
            return new Window(new NavigationPage(new LoginPage(serviceProvider)));
        }
    }

    private static void StopVideoIfPlaying()
    {
        if (Current.MainPage is NavigationPage navigationPage)
        {
            var shortsPage = navigationPage.CurrentPage as MainPage;
            shortsPage?.StopVideo();
        }
    }
}