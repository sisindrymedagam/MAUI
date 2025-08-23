using Looply.MAUI.ViewModels;

namespace Looply.MAUI.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = serviceProvider.GetService<SettingsViewModel>();
    }
}