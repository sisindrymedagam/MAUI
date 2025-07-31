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

            if (Current != null)
            {
                Current.UserAppTheme = theme;
            }
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            //string? pin = SecureStorage.GetAsync("app_pin").Result;
            //string? intro = SecureStorage.GetAsync("intro_done").Result;
            //if (intro is not "1")
            //{
            //    if (string.IsNullOrEmpty(pin))
            //    {
            //        // Show PIN setup page for first-time users
            //        return new Window(new SetupPinPage());
            //    }
            //    else
            //    {
            //        // Show PIN lock page for returning users
            //        return new Window(new PinLockPage());
            //    }
            //}

            return new Window(new AppShell());
        }

        //protected override void OnResume()
        //{
        //    string? pin = SecureStorage.GetAsync("app_pin").Result;

        //    string? intro = SecureStorage.GetAsync("intro_done").Result;
        //    if (intro is not "1")
        //    {
        //        if (!string.IsNullOrEmpty(pin))
        //        {
        //            // Show PIN lock page for returning users
        //            if (Current?.Windows.Count > 0)
        //            {
        //                Current.Windows[0].Page = new PinLockPage();
        //            }
        //        }
        //        else
        //        {
        //            Current.Windows[0].Page = new AppShell();
        //        }
        //    }
        //    else
        //    {
        //        Current.Windows[0].Page = new AppShell();
        //    }
        //}
    }
}