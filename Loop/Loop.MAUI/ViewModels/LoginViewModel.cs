using Loop.MAUI.Handlers;
using Loop.MAUI.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Loop.MAUI.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly AuthService _authService;
    private readonly IValidationService _validationService;
    private readonly IServiceProvider serviceProvider;

    public LoginViewModel(AuthService authService, IValidationService validationService, IServiceProvider serviceProvider)
    {
        _authService = authService;
        _validationService = validationService;
        this.serviceProvider = serviceProvider;
        LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
    }

    private string _email = "";
    private string _password = "";
    private string _errorMessage = "";
    private bool _isBusy;
    private bool _isSuccess;

    public string Email { get => _email; set => Set(ref _email, value); }
    public string Password { get => _password; set => Set(ref _password, value); }
    public string ErrorMessage { get => _errorMessage; set => Set(ref _errorMessage, value); }
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

            // Use validation service
            var emailValidation = _validationService.ValidateEmail(Email);
            if (!emailValidation.IsValid)
                throw new InvalidOperationException(emailValidation.ErrorMessage);

            var passwordValidation = _validationService.ValidatePassword(Password);
            if (!passwordValidation.IsValid)
                throw new InvalidOperationException(passwordValidation.ErrorMessage);

            Models.AuthResponse resp = await _authService.LoginAsync(Email.Trim(), Password);
            if (string.IsNullOrWhiteSpace(resp.Token))
                throw new InvalidOperationException("Invalid token received.");

            // Note: AuthService now handles secure storage, so we don't need to set Preferences here
            // But we still need to set some preferences for backward compatibility
            Preferences.Set(Constants.LastSyncUtcName, Constants.MinDateTime);

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
