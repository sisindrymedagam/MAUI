using Loop.MAUI.ViewModels;

namespace Loop.MAUI.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = serviceProvider.GetService<LoginViewModel>();
    }
}