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

    private void ShortsCarousel_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {
        // Stop all visible videos
        foreach (var view in ShortsCarousel.VisibleViews)
        {
            var player = view.FindByName<MediaElement>("Player");
            player?.Pause();
        }

        // Play the newly active one
        if (e.CurrentItem != null)
        {
            var activeView = ShortsCarousel.VisibleViews.FirstOrDefault();
            if (activeView != null)
            {
                var activePlayer = activeView.FindByName<MediaElement>("Player");
                activePlayer?.Play();
            }
        }
    }
}