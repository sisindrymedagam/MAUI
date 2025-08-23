using Looply.MAUI.ViewModels;

namespace Looply.MAUI.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = serviceProvider.GetService<LoginViewModel>();
    }
}