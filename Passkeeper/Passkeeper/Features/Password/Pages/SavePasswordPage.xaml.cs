using Passkeeper.Features.Password.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Passkeeper.Features.Password.Pages;

public partial class SavePasswordPage : ContentPage, INotifyPropertyChanged
{
    private readonly PasswordStorageService _passwordService;
    private Models.Password _password;
    public Models.Password Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public SavePasswordPage(PasswordStorageService passwordService, Models.Password? password = null)
    {
        InitializeComponent();
        _passwordService = passwordService;
        Password = password ?? new Models.Password();
        BindingContext = this;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Password.Url) || string.IsNullOrWhiteSpace(Password.Username) || string.IsNullOrWhiteSpace(Password.PasswordEncrypted))
        {
            await DisplayAlert("Missing Fields", "Website URL, Username, and Password are required.", "OK");
            return;
        }
        await _passwordService.SavePasswordAsync(Password);
        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}