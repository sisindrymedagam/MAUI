using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Passkeeper.Helpers;

public static class ToastHelper
{
    public static async Task ShowAsync(string message, double fontSize = 14, ToastDuration toastDuration = ToastDuration.Short)
    {
        CancellationTokenSource cancellationTokenSource = new();

        IToast toast = Toast.Make(message, toastDuration, fontSize);

        await toast.Show(cancellationTokenSource.Token);
    }
}
