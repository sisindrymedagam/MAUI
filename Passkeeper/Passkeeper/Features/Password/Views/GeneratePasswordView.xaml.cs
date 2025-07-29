using System.Text;

namespace Passkeeper.Features.Password.Views;

public partial class GeneratePasswordView : ContentView
{
    public GeneratePasswordView()
    {
        InitializeComponent();
        LengthSlider.Value = new Random().Next(8, 24);
        GeneratePassword();
        _ = new MauiIcon();
    }

    private void OnOptionChanged(object sender, EventArgs e)
    {
        LengthLabel.Text = $"Password Length: {(int)LengthSlider.Value}";
        GeneratePassword();
    }

    private void OnRegenerateClicked(object sender, EventArgs e)
    {
        GeneratePassword();
    }

    private async void OnCopyClicked(object sender, EventArgs e)
    {
        string password = GeneratedPasswordLabel.Text;
        if (!string.IsNullOrWhiteSpace(password) && !password.StartsWith('⚠'))
        {
            await Clipboard.SetTextAsync(password);
            await ToastHelper.ShowAsync("Password copied to clipboard.");
        }
        else
        {
            await SnackbarHelper.ShowAsync("No valid password to copy.", TimeSpan.FromSeconds(3), null);
        }
    }

    private void GeneratePassword()
    {
        int length = (int)LengthSlider.Value;
        StringBuilder sb = new();
        Random random = new();

        string lowercase = "abcdefghijklmnopqrstuvwxyz";
        string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string numbers = "0123456789";
        string special = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        string charset = "";
        if (LowercaseSwitch.IsToggled) charset += lowercase;
        if (UppercaseSwitch.IsToggled) charset += uppercase;
        if (NumbersSwitch.IsToggled) charset += numbers;
        if (SpecialSwitch.IsToggled) charset += special;

        if (string.IsNullOrEmpty(charset))
        {
            GeneratedPasswordLabel.Text = "⚠ Please select at least one character type.";
            return;
        }

        for (int i = 0; i < length; i++)
        {
            int index = random.Next(charset.Length);
            sb.Append(charset[index]);
        }

        GeneratedPasswordLabel.Text = sb.ToString();
    }
}