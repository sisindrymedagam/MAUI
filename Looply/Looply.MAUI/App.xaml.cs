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
            return new Window(new AppShell());
        }
        else
        {
            return new Window(new LoginPage(serviceProvider));
        }
    }
}