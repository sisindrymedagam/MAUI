using Loop.MAUI.ViewModels;

namespace Loop.MAUI.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = serviceProvider.GetService<SettingsViewModel>();
    }
}