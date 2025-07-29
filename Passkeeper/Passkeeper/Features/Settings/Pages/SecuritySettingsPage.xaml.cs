using Plugin.Maui.Biometric;

namespace Passkeeper.Features.Settings.Pages;

public partial class SecuritySettingsPage : ContentPage
{
    public SecuritySettingsPage()
    {
        InitializeComponent();
        LoadSettings();
    }

    private void LoadSettings()
    {
        BiometricSwitch.IsToggled = Preferences.Get("use_biometrics", false);

        int autoLockIndex = Preferences.Get("auto_lock_index", 0);
        AutoLockPicker.SelectedIndex = autoLockIndex;
    }

    private async void OnSetOrChangePinClicked(object sender, EventArgs e)
    {
        string pin = await DisplayPromptAsync("Set PIN", "Enter a new 4-digit PIN:", keyboard: Keyboard.Numeric, maxLength: 4);
        if (!string.IsNullOrWhiteSpace(pin) && pin.Length == 4)
        {
            await SecureStorage.SetAsync("app_pin", pin);
            await DisplayAlert("Success", "PIN set successfully.", "OK");
        }
        else
        {
            await DisplayAlert("Error", "PIN must be exactly 4 digits.", "OK");
        }
    }

    private async void OnRemovePinClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Remove PIN", "Are you sure you want to remove the PIN lock?", "Yes", "No");
        if (confirm)
        {
            SecureStorage.Remove("app_pin");
            Preferences.Remove("use_biometrics");
            BiometricSwitch.IsToggled = false;
            await DisplayAlert("Success", "PIN removed successfully.", "OK");
        }
    }

    private async void OnBiometricToggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            BiometricType[] enrolledBioMetrics = await BiometricAuthenticationService.Default.GetEnrolledBiometricTypesAsync();
            if (enrolledBioMetrics.Length > 0)
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
                    Preferences.Set("use_biometrics", true);
                }
                else
                {
                    BiometricSwitch.IsToggled = false;
                    Preferences.Set("use_biometrics", false);
                }
            }
            else
            {
                await DisplayAlert("Unavailable", "Biometric authentication is not available on this device.", "OK");
                BiometricSwitch.IsToggled = false;
            }
        }
        else
        {
            Preferences.Set("use_biometrics", false);
        }
    }

    private void OnAutoLockChanged(object sender, EventArgs e)
    {
        Preferences.Set("auto_lock_index", AutoLockPicker.SelectedIndex);
    }
}