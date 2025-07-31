using Passkeeper.Features.Password.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Passkeeper.Features.Password.Pages;

public partial class SavePasswordPage : ContentPage, INotifyPropertyChanged
{
    private readonly PasswordStorageService _passwordService;
    private Models.PasswordDto _password;
    public Models.PasswordDto Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public SavePasswordPage(PasswordStorageService passwordService, Models.PasswordDto? password = null)
    {
        InitializeComponent();
        _passwordService = passwordService;
        Password = password ?? new Models.PasswordDto();
        BindingContext = this;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Password.Url) || string.IsNullOrWhiteSpace(Password.Username) || string.IsNullOrWhiteSpace(Password.Password))
        {
            await DisplayAlert("Missing Fields", "Website URL, Username, and Password are required.", "OK");
            return;
        }
        await _passwordService.SaveAsync(Password);
        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}