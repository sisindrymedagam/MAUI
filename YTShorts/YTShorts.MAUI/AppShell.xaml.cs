namespace YTShorts.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent(); 
        Routing.RegisterRoute("//LoginPage", typeof(Pages.LoginPage));
        Routing.RegisterRoute("//ShortsPage", typeof(Pages.ShortsPage));
        Routing.RegisterRoute("//SettingsPage", typeof(Pages.SettingsPage));
    }
}
