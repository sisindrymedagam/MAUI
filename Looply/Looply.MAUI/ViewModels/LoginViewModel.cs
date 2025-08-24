using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Looply.MAUI.Handlers;
using Looply.MAUI.Services;

namespace Looply.MAUI.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly AuthService _authService;
    readonly IServiceProvider serviceProvider;

    public LoginViewModel(AuthService authService, IServiceProvider serviceProvider)
    {
        _authService = authService;
        this.serviceProvider = serviceProvider;
        LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
    }

    private string _email = "";
    private string _password = "";
    private string _errorMessage = "";
    private bool _isBusy;
    private bool _isSuccess;

    public string Email { get => _email; set { Set(ref _email, value); } }
    public string Password { get => _password; set { Set(ref _password, value); } }
    public string ErrorMessage { get => _errorMessage; set { Set(ref _errorMessage, value); } }
    public bool IsBusy { get => _isBusy; set { Set(ref _isBusy, value); ((Command)LoginCommand).ChangeCanExecute(); } }
    public bool IsSuccess { get => _isSuccess; set => Set(ref _isSuccess, value); }

    public ICommand LoginCommand { get; }

    private async Task LoginAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            ErrorMessage = "";
            IsSuccess = false;

            if (!IsValidEmail(Email))
                throw new InvalidOperationException("Please enter a valid email.");

            if (string.IsNullOrWhiteSpace(Password))
                throw new InvalidOperationException("Password is required.");

            var resp = await _authService.LoginAsync(Email.Trim(), Password);
            if (string.IsNullOrWhiteSpace(resp.Token))
                throw new InvalidOperationException("Invalid token received.");

            Preferences.Set(Constants.TokenName, resp.Token);
            Preferences.Set(Constants.TokenExpirationName, resp.Expiration.ToUniversalTime());
            Preferences.Set(Constants.LastSyncUtcName, Constants.MinDateTime);
            Preferences.Set(Constants.UserEmailName, Email);

            IsSuccess = true;

            NavigationHandler.NavigateTo(new MainPage(serviceProvider));
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static bool IsValidEmail(string email) =>
        !string.IsNullOrWhiteSpace(email) &&
        Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public event PropertyChangedEventHandler? PropertyChanged;
    private void Set<T>(ref T field, T value, [CallerMemberName] string? prop = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
