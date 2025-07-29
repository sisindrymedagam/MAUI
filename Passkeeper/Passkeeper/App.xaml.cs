using Passkeeper.Features.Onboarding.Pages;

namespace Passkeeper
{
    public partial class App : Application
    {
        public App()
        {
            int themeIndex = Preferences.Get("theme_index", 0);
            AppTheme theme = themeIndex switch
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
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            string? pin = SecureStorage.GetAsync("app_pin").Result;

            if (string.IsNullOrEmpty(pin))
            {
                // Show PIN setup page for first-time users
                return new Window(new SetupPinPage());
            }
            else
            {
                // Show PIN lock page for returning users
                return new Window(new PinLockPage());
            }
        }

        protected override void OnResume()
        {
            string? pin = SecureStorage.GetAsync("app_pin").Result;

            if (string.IsNullOrEmpty(pin))
            {
                // Show PIN setup page for first-time users
                if (Application.Current?.Windows.Count > 0)
                {
                    Application.Current.Windows[0].Page = new SetupPinPage();
                }
            }
            else
            {
                // Show PIN lock page for returning users
                if (Application.Current?.Windows.Count > 0)
                {
                    Application.Current.Windows[0].Page = new PinLockPage();
                }
            }
        }
    }
}