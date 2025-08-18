using YTShorts.MAUI.ViewModels;

namespace YTShorts.MAUI.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}