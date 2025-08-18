using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using YTShorts.MAUI.Pages;
using YTShorts.MAUI.Services;
using YTShorts.MAUI.ViewModels;

namespace YTShorts.MAUI
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

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ShortsPage>();
            builder.Services.AddTransient<SettingsPage>();
            return builder.Build();
        }
    }
}
