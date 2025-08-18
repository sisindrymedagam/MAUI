namespace YTShorts.MAUI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
    protected override void OnStart()
    {
        base.OnStart();
        RouteBasedOnToken();
    }

    //protected override void OnResume()
    //{
    //    base.OnResume();
    //    RouteBasedOnToken();
    //}

    private static void RouteBasedOnToken()
    {
        var token = Preferences.Get("AuthToken", string.Empty);
        var expiration = Preferences.Get("AuthTokenExpiration", DateTime.MinValue);

        var needsLogin = string.IsNullOrWhiteSpace(token) || expiration <= DateTime.UtcNow;
        var targetRoute = needsLogin ? "//LoginPage" : "//ShortsPage";

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync(targetRoute);
        });
    }
}