using YTShorts.MAUI.Services;
using YTShorts.MAUI.ViewModels;

namespace YTShorts.MAUI.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (AuthService.IsLogedIn())
        {
            await Shell.Current.GoToAsync("//ShortsPage");
        }
    }
}