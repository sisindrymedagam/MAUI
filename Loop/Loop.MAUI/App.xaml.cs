using Loop.MAUI.Pages;
using Loop.MAUI.Services;

namespace Loop.MAUI;

public partial class App : Application
{
    private readonly IServiceProvider serviceProvider;

    public App(IServiceProvider _serviceProvider)
    {
        serviceProvider = _serviceProvider;
        if (Current != null)
        {
            Current.UserAppTheme = AppTheme.Dark;
        }
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window;

        if (AuthService.IsLogedIn())
        {
            window = new Window(new NavigationPage(new MainPage(serviceProvider)));
        }
        else
        {
            window = new Window(new NavigationPage(new LoginPage(serviceProvider)));
        }

        window.Destroying += (s, e) => App.StopVideoIfPlaying();
        window.Deactivated += (s, e) => App.StopVideoIfPlaying();
        return window;
    }

    private static void StopVideoIfPlaying()
    {
        if (Current.MainPage is NavigationPage navigationPage)
        {
            MainPage? shortsPage = navigationPage.CurrentPage as MainPage;
            shortsPage?.StopVideo();
        }
    }
}