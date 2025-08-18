using YTShorts.MAUI.ViewModels;

namespace YTShorts.MAUI.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}