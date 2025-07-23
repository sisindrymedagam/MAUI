namespace Passkeeper.Features.Settings.Pages;

public partial class AppSettingsPage : ContentPage
{
    public AppSettingsPage()
    {
        InitializeComponent();
        LoadSettings();
    }

    private void LoadSettings()
    {
        var themeIndex = Preferences.Get("theme_index", 0);
        ThemePicker.SelectedIndex = themeIndex;

        var version = AppInfo.VersionString;
        var build = AppInfo.BuildString;
        AppVersionLabel.Text = $"Version: {version} (Build {build})";

#if ANDROID
        BatteryStatusLabel.Text = "Battery optimization is ON (background may be limited)";
#else
        BatteryStatusLabel.Text = "Battery settings not supported on this platform.";
#endif
    }

    private void OnThemeChanged(object sender, EventArgs e)
    {
        Preferences.Set("theme_index", ThemePicker.SelectedIndex);
        
        // Apply theme
        var theme = ThemePicker.SelectedIndex switch
        {
            1 => AppTheme.Light,
            2 => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
        
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = theme;
        }
    }

    private async void OnOpenBatterySettingsClicked(object sender, EventArgs e)
    {
#if ANDROID
        //BatteryHelper.PlatformOpenBatteryOptimizationSettings();
        await DisplayAlert("Not Implemented", "Battery settings feature not yet implemented.", "OK");
#else
        await DisplayAlert("Not Supported", "Battery settings are only available on Android.", "OK");
#endif
    }

    private async void OnCheckForUpdatesClicked(object sender, EventArgs e)
    {
#if ANDROID
        var url = $"https://play.google.com/store/apps/details?id={AppInfo.PackageName}";
#elif IOS
        var appStoreId = "1234567890"; // TODO: Replace with your app's ID
        var url = $"https://apps.apple.com/app/id{appStoreId}";
#else
        var url = "https://yourwebsite.com";
#endif
        await Launcher.Default.OpenAsync(url);
    }

    private async void OnExportDataClicked(object sender, EventArgs e)
    {
        try
        {
            // TODO: Implement data export functionality
            await DisplayAlert("Export Data", "Data export feature will be implemented in a future update.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to export data: {ex.Message}", "OK");
        }
    }

    private async void OnClearDataClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Clear All Data", 
            "This will permanently delete all your passwords and settings. This action cannot be undone.", 
            "Clear All", "Cancel");
        
        if (confirm)
        {
            try
            {
                // Clear all stored data
                SecureStorage.Remove("app_pin");
                Preferences.Clear();
                
                await DisplayAlert("Success", "All data has been cleared. The app will restart.", "OK");
                
                // Restart the app
                if (Application.Current != null)
                {
                    Application.Current.Quit();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to clear data: {ex.Message}", "OK");
            }
        }
    }
} 