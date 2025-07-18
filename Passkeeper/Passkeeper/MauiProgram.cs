using CommunityToolkit.Maui;
using Maui.Biometric;
using MauiIcons.Core;
using MauiIcons.Fluent;
using Microsoft.Extensions.Logging;

namespace Passkeeper;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseBiometricAuthentication()
            .UseMauiCommunityToolkit()
            .UseFluentMauiIcons()
            .UseMauiIconsCore(x =>
            {
                x.SetDefaultIconSize(24);
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
