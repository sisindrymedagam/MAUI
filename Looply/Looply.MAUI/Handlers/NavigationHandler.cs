namespace Looply.MAUI.Handlers;

public static class NavigationHandler
{
    public static void NavigateTo(Page page)
    {
        //Replace the root page inside the existing window

        var window = Application.Current?.Windows.FirstOrDefault();
        if (window != null)
        {
            window.Page = new NavigationPage(page);
        }
        //var windows = Application.Current?.Windows.Count;
        //var newWindow = new Window(page);
        //Application.Current?.OpenWindow(newWindow);
        //Application.Current?.ActivateWindow(newWindow);
    }
}
