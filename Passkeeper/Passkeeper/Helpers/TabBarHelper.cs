namespace Passkeeper.Helpers;

public static class TabBarHelper
{
    /// <summary>
    /// Hides the tabbar by setting the Shell's FlyoutBehavior to Disabled
    /// </summary>
    /// <param name="obj">The object that modifies the tabs visibility.</param>
    public static void HideTabBar(BindableObject obj)
    {
        Shell.SetTabBarIsVisible(obj, false);
    }

    /// <summary>
    /// Shows the tabbar by setting the Shell's FlyoutBehavior to Default
    /// </summary>
    /// <param name="obj">The object that modifies the tabs visibility.</param>
    public static void ShowTabBar(BindableObject obj)
    {
        Shell.SetTabBarIsVisible(obj, true);
    }

    /// <summary>
    /// Gets the current tabbar visibility state
    /// </summary>
    /// <param name="obj">The object that modifies the tabs visibility.</param>
    /// <returns>True if tabbar is visible, false otherwise</returns>
    public static bool IsTabBarVisible(BindableObject obj)
    {
        return Shell.GetTabBarIsVisible(obj);
    }
}