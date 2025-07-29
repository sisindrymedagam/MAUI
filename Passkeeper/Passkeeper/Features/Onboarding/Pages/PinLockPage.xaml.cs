using Plugin.Maui.Biometric;

namespace Passkeeper.Features.Onboarding.Pages;

public partial class PinLockPage : ContentPage
{
    public PinLockPage()
    {
        InitializeComponent();
        PinKeyboard.PinSubmitted += OnPinSubmitted;
        PinKeyboard.PinCleared += OnPinCleared;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        bool useBiometrics = Preferences.Get("use_biometrics", false);
        if (useBiometrics)
        {
            AuthenticationRequest authenticationRequest = new()
            {
                AllowPasswordAuth = true, // A chance to fallback to password auth
                Title = "Authenticate", // On iOS only the title is relevant, everything else is unused. 
                Subtitle = "Please authenticate using your biometric data",
                NegativeText = "Use Password", // if AllowPasswordAuth is set to true don't use this it will throw an exception on Android
                Description = "Biometric authentication is required for access",
                AuthStrength = AuthenticatorStrength.Strong // Only relevant on Android
            };
            AuthenticationResponse result = await BiometricAuthenticationService.Default.AuthenticateAsync(authenticationRequest, CancellationToken.None);
            if (result.Status == BiometricResponseStatus.Success)
            {
                SetShellPage();
            }
        }
    }

    private async void OnPinSubmitted(object? sender, string pin)
    {
        // Skipping the biometric fr pin submit
        string? storedPin = await SecureStorage.GetAsync("app_pin");

        if (string.IsNullOrWhiteSpace(storedPin))
        {
            SetShellPage();
            return;
        }

        //Todo: UseBCrypt to verify pin
        if (pin == storedPin)
        {
            ErrorLabel.IsVisible = false;
            SetShellPage();
        }
        else
        {
            ErrorLabel.IsVisible = true;
            PinKeyboard.ClearPin();

            // Add haptic feedback for wrong PIN
            try
            {
                HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            }
            catch
            {
                // Haptic feedback not available on all devices
            }
        }
    }

    private void OnPinCleared(object? sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;
    }

    private static void SetShellPage()
    {
        if (Application.Current?.Windows.Count > 0)
        {
            Application.Current.Windows[0].Page = new AppShell();
        }
    }
}