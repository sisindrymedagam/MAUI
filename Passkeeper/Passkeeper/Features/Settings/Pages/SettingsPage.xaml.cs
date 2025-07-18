using Maui.Biometric;

namespace Passkeeper.Features.Settings.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private async void OnSecuritySettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SecuritySettingsPage());
    }

    private async void OnNotificationSettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NotificationSettingsPage());
    }

    private async void OnAppSettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AppSettingsPage());
    }

    private async void OnAboutClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AboutPage());
    }

    private async void OnHelpClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HelpPage());
    }

    private async void OnFeedbackClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FeedbackPage());
    }
}