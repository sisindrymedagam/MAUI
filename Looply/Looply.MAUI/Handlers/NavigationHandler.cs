namespace Looply.MAUI.Handlers;

public static class NavigationHandler
{
    public static async Task NavigateToAsync(Page page)
    {
#if ANDROID || IOS
        Application.Current.Windows[0].Page = page;
#else
        await Shell.Current.Navigation.PushAsync(page);
#endif
    }
}
