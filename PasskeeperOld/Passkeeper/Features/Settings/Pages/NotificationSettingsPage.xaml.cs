namespace Passkeeper.Features.Settings.Pages;

public partial class NotificationSettingsPage : ContentPage
{
    public NotificationSettingsPage()
    {
        InitializeComponent();
        LoadSettings();
    }

    private void LoadSettings()
    {
        NotificationSwitch.IsToggled = Preferences.Get("notifications_enabled", false);
        DailyReminderSwitch.IsToggled = Preferences.Get("daily_reminder_enabled", false);
        FailedLoginSwitch.IsToggled = Preferences.Get("failed_login_alerts", true);
        PasswordExpirySwitch.IsToggled = Preferences.Get("password_expiry_alerts", true);

        string time = Preferences.Get("notification_time", "08:00");
        if (TimeSpan.TryParse(time, out TimeSpan parsedTime))
            NotificationTimePicker.Time = parsedTime;
    }

    private void OnNotificationToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("notifications_enabled", e.Value);
        if (e.Value)
            ScheduleDailyNotification();
        else
            CancelNotifications();
    }

    private void OnDailyReminderToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("daily_reminder_enabled", e.Value);
        if (e.Value && NotificationSwitch.IsToggled)
            ScheduleDailyNotification();
        else
            CancelNotifications();
    }

    private void OnFailedLoginToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("failed_login_alerts", e.Value);
    }

    private void OnPasswordExpiryToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("password_expiry_alerts", e.Value);
    }

    private void OnNotificationTimeChanged(object sender, TimeChangedEventArgs e)
    {
        Preferences.Set("notification_time", e.NewTime.ToString(@"hh\:mm"));
        if (NotificationSwitch.IsToggled)
            ScheduleDailyNotification();
    }

    private void OnNotificationTimePickerChanged(object sender, EventArgs e)
    {
        if (NotificationTimePicker != null)
        {
            Preferences.Set("notification_time", NotificationTimePicker.Time.ToString(@"hh\:mm"));
            if (NotificationSwitch.IsToggled && DailyReminderSwitch.IsToggled)
                ScheduleDailyNotification();
        }
    }

    private void ScheduleDailyNotification()
    {
        // Implement using Plugin.LocalNotification or similar
        Console.WriteLine("[DEBUG] Schedule daily notification at " + NotificationTimePicker.Time);
    }

    private void CancelNotifications()
    {
        Console.WriteLine("[DEBUG] Cancel all notifications");
    }
}