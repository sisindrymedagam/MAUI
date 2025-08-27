using CommunityToolkit.Maui;
using Loop.MAUI.Pages;
using Loop.MAUI.Services;
using Loop.MAUI.ViewModels;
using MauiIcons.Fluent;
using Microsoft.Extensions.Logging;

namespace Loop.MAUI
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

            // Core services
            builder.Services.AddSingleton<ShortsDatabase>();
            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddSingleton<MediaCacheService>();

            // New services with interfaces
            builder.Services.AddSingleton<ISecureStorageService, SecureStorageService>();
            builder.Services.AddSingleton<IValidationService, ValidationService>();

            // Services that depend on the new services
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<SyncService>();

            // ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<ShortsViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();

            // Pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<SettingsPage>();

            return builder.Build();
        }
    }
}
