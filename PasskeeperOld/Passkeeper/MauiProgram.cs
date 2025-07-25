using CommunityToolkit.Maui;
using Maui.Biometric;
using MauiIcons.Core;
using MauiIcons.Fluent;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using NewRelic.MAUI.Plugin;

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
        builder.ConfigureLifecycleEvents(AppLifecycle =>
        {
#if ANDROID
      AppLifecycle.AddAndroid(android => android
        .OnCreate((activity, savedInstanceState) => StartNewRelic()));
#endif

#if IOS
      AppLifecycle.AddiOS(iOS => iOS.WillFinishLaunching((_,__) => {
        StartNewRelic();
        return false;
      }));
#endif
        });
        return builder.Build();
    }

    private static void StartNewRelic()
    {
        CrossNewRelic.Current.HandleUncaughtException();

        // Set optional agent configuration
        // Options are: crashReportingEnabled, loggingEnabled, logLevel, collectorAddress, 
        // crashCollectorAddress, analyticsEventEnabled, networkErrorRequestEnabled, 
        // networkRequestEnabled, interactionTracingEnabled, webViewInstrumentation, 
        // fedRampEnabled, offlineStorageEnabled, newEventSystemEnabled, backgroundReportingEnabled
        // AgentStartConfiguration agentConfig = new AgentStartConfiguration(crashReportingEnabled:false);

        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            CrossNewRelic.Current.Start("AA1ccaac4d3f374eb37606096686dd27009baa152b-NRMA");
            // Start with optional agent configuration
            // CrossNewRelic.Current.Start("APP_TOKEN_HERE", agentConfig);
        }
        else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
        {
            CrossNewRelic.Current.Start("AAa7b2683df7768234fa71af9834f4c80381cee8a2-NRMA");
            // Start with optional agent configuration
            // CrossNewRelic.Current.Start("APP_TOKEN_HERE", agentConfig);
        }
    }
}
