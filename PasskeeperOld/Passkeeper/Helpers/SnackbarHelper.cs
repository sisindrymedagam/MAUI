using CommunityToolkit.Maui.Alerts;

namespace Passkeeper.Helpers;

internal static class SnackbarHelper
{
    public static async Task ShowAsync(string message, TimeSpan duration, Action? action, string actionButtonText = "Dismiss")
    {
        CancellationTokenSource cancellationTokenSource = new();

        CommunityToolkit.Maui.Core.ISnackbar snackbar = Snackbar.Make(message, action, actionButtonText, duration);

        await snackbar.Show(cancellationTokenSource.Token);
    }
}
