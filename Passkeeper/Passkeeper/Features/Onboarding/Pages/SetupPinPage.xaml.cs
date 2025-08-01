namespace Passkeeper.Features.Onboarding.Pages;

public partial class SetupPinPage : ContentPage
{
    private string _firstPin = "";
    private bool _isConfirming = false;

    public SetupPinPage()
    {
        InitializeComponent();
        PinKeyboard.PinSubmitted += OnPinSubmitted;
    }

    private async void OnPinSubmitted(object? sender, string pin)
    {
        if (!_isConfirming)
        {
            // First time entering PIN
            _firstPin = pin;
            _isConfirming = true;

            // Update UI for confirmation
            SetupTitle.Text = "Confirm Your PIN";
            SetupSubtitle.Text = "Enter the same PIN again to confirm";
            PinKeyboard.ClearPin();
        }
        else
        {
            // Confirming PIN
            if (pin == _firstPin)
            {
                // PINs match, save and proceed
                await SecureStorage.SetAsync("app_pin", pin);
                await DisplayAlert("Success", "PIN created successfully!", "OK");

                // Navigate to main app
                if (Application.Current?.Windows.Count > 0)
                {
                    Application.Current.Windows[0].Page = new AppShell();
                }
            }
            else
            {
                // PINs don't match, start over
                await DisplayAlert("Error", "PINs don't match. Please try again.", "OK");
                _firstPin = "";
                _isConfirming = false;
                SetupTitle.Text = "Create Your PIN";
                SetupSubtitle.Text = "Enter a 4-digit PIN to secure your passwords";
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
    }

    private async void OnSkipClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Skip PIN Setup",
            "You can set up a PIN later in Settings. Are you sure you want to continue without a PIN?",
            "Continue", "Cancel");

        if (confirm)
        {
            await SecureStorage.SetAsync("intro_done", "1");
            // Navigate to main app without PIN
            if (Application.Current?.Windows.Count > 0)
            {
                Application.Current.Windows[0].Page = new AppShell();
            }
        }
    }
}