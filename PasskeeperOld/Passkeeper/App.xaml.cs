using NewRelic.MAUI.Plugin;

namespace Passkeeper
{
    public partial class App : Application
    {
        public App()
        {
            var themeIndex = Preferences.Get("theme_index", 0);
            var theme = themeIndex switch
            {
                1 => AppTheme.Light,
                2 => AppTheme.Dark,
                _ => AppTheme.Unspecified
            };

            if (Application.Current != null)
            {
                Application.Current.UserAppTheme = theme;
            }
            InitializeComponent();

            CrossNewRelic.Current.HandleUncaughtException();
            CrossNewRelic.Current.TrackShellNavigatedEvents();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}