using MetroLog.MicrosoftExtensions;
using MetroLog.Operators;
using Plugin.Maui.Biometric;

namespace Passkeeper;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
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

        builder.Services.AddSingleton<IBiometric>(BiometricAuthenticationService.Default);

        builder.Logging
            .AddTraceLogger(
                options =>
                {
                    options.MinLevel = Microsoft.Extensions.Logging.LogLevel.Trace;
                    options.MaxLevel = Microsoft.Extensions.Logging.LogLevel.Critical;
                })
            .AddStreamingFileLogger(
                options =>
                {
                    options.MinLevel = Microsoft.Extensions.Logging.LogLevel.Trace;
                    options.MaxLevel = Microsoft.Extensions.Logging.LogLevel.Critical;
                    options.RetainDays = 2;
                    options.FolderPath = Path.Combine(FileSystem.Current.AppDataDirectory, "Logs");
                    Directory.CreateDirectory(options.FolderPath);
                })
            .AddConsoleLogger(
                options =>
                {
                    options.MinLevel = Microsoft.Extensions.Logging.LogLevel.Information;
                    options.MaxLevel = Microsoft.Extensions.Logging.LogLevel.Critical;
                });

        builder.Services.AddSingleton(LogOperatorRetriever.Instance);

        return builder.Build();
    }
}
