using Maui.Biometric;

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
            var request = new AuthenticationRequest(title: "Authenticate", reason: "Please authenticate to proceed");
            var result = await BiometricAuthentication.Current.AuthenticateAsync(request);
            if (result.IsSuccessful)
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