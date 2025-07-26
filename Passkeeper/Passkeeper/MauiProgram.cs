using MetroLog.MicrosoftExtensions;
using MetroLog.Operators;
using Microsoft.Extensions.Logging;

namespace Passkeeper
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureMauiHandlers(handlers =>
                {
#if IOS || MACCATALYST
    				handlers.AddHandler<Microsoft.Maui.Controls.CollectionView, Microsoft.Maui.Controls.Handlers.Items2.CollectionViewHandler2>();
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                });

            builder.Logging
                .AddTraceLogger(
                    options =>
                    {
                        options.MinLevel = LogLevel.Trace;
                        options.MaxLevel = LogLevel.Critical;
                    }) // Will write to the Debug Output
                .AddConsoleLogger(
                    options =>
                    {
                        options.MinLevel = LogLevel.Information;
                        options.MaxLevel = LogLevel.Critical;
                    }) // Will write to the Console Output (logcat for android)
                .AddStreamingFileLogger(
                    options =>
                    {
                        options.RetainDays = 2;
                        options.FolderPath = Path.Combine(
                            FileSystem.AppDataDirectory,
                            "Logs");
                    }); // Will write to files

            builder.Services.AddSingleton(LogOperatorRetriever.Instance);
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddSingleton<ModalErrorHandler>();
            return builder.Build();
        }
    }
}
