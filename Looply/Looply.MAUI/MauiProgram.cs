using CommunityToolkit.Maui;
using Looply.MAUI.Pages;
using Looply.MAUI.Services;
using Looply.MAUI.ViewModels;
using MauiIcons.Fluent;

namespace Looply.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement()
                .UseFluentMauiIcons()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ShortsDatabase>();

            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<SyncService>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<ShortsViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<SettingsPage>();
            return builder.Build();
        }
    }
}
