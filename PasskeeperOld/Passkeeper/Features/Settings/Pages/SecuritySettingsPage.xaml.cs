using Maui.Biometric;

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
        
        var autoLockIndex = Preferences.Get("auto_lock_index", 0);
        AutoLockPicker.SelectedIndex = autoLockIndex;
    }

    private async void OnSetOrChangePinClicked(object sender, EventArgs e)
    {
        var pin = await DisplayPromptAsync("Set PIN", "Enter a new 4-digit PIN:", keyboard: Keyboard.Numeric, maxLength: 4);
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
            var available = await BiometricAuthentication.Current.IsAvailableAsync();
            if (available)
            {
                var request = new AuthenticationRequest(title: "Authenticate", reason: "Please authenticate to enable biometric unlock");
                var result = await BiometricAuthentication.Current.AuthenticateAsync(request);
                if (result.IsSuccessful)
                {
                    Preferences.Set("use_biometrics", true);
                }
                else
                {
                    BiometricSwitch.IsToggled = false;
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