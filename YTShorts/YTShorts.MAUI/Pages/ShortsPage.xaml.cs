using CommunityToolkit.Maui.Views;
using YTShorts.MAUI.ViewModels;

namespace YTShorts.MAUI.Pages;

public partial class ShortsPage : ContentPage
{
    private ShortsViewModel? VM => BindingContext as ShortsViewModel;
    public ShortsPage(ShortsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (VM is not null)
            await VM.LoadOrSyncAsync();
    }
}