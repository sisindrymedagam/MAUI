using Loop.MAUI.Pages;
using Loop.MAUI.ViewModels;

namespace Loop.MAUI;

public partial class MainPage : ContentPage
{
    private ShortsViewModel? VM => BindingContext as ShortsViewModel;
    private bool _isPaused = false;
    private bool _isTransitioning = false;
    private readonly IServiceProvider ServiceProvider;
    public MainPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = serviceProvider.GetService<ShortsViewModel>();
        ServiceProvider = serviceProvider;
        NavigationPage.SetHasNavigationBar(this, false);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (Player != null && Player.Source != null)
        {
            PlayPlayer();
        }
        else
        {
            if (VM is not null)
                await VM.LoadOrSyncAsync();
        }
    }

    public void StopVideo()
    {
        PausePlayer();
    }

    private void OnVideoTapped(object sender, TappedEventArgs e)
    {
        if (_isPaused)
        {
            PlayPlayer();
        }
        else
        {
            PausePlayer();
        }
    }

    private void NavigateToSettings(object sender, EventArgs e)
    {
        PausePlayer();
        Navigation.PushAsync(new NavigationPage(new SettingsPage(ServiceProvider)), true);
    }

    private async void OnSwipedUp(object sender, SwipedEventArgs e)
    {
        if (_isTransitioning || VM is null) return;
        await AnimateAndSwitchAsync(next: true);
    }

    private async void OnSwipedDown(object sender, SwipedEventArgs e)
    {
        if (_isTransitioning || VM is null) return;
        await AnimateAndSwitchAsync(next: false);
    }

    private async Task AnimateAndSwitchAsync(bool next)
    {
        if (VideoContainer == null) return;

        _isTransitioning = true;

        double height = VideoContainer.Height > 0 ? VideoContainer.Height : RootGrid.Height;
        if (height <= 0)
        {
            // layout not ready; skip animation but still switch
            await ExecuteSwitchAsync(next);
            _isTransitioning = false;
            return;
        }

        //PausePlayer();

        double outY = next ? -height : height;
        double inStartY = next ? height : -height;

        var fadeOutTask = VideoContainer.FadeTo(0, 150, Easing.CubicIn);
        var slideOutTask = VideoContainer.TranslateTo(0, outY, 200, Easing.CubicIn);
        await Task.WhenAll(fadeOutTask, slideOutTask);

        await ExecuteSwitchAsync(next);

        VideoContainer.TranslationY = inStartY;
        VideoContainer.Opacity = 0;

        var fadeInTask = VideoContainer.FadeTo(1, 180, Easing.CubicOut);
        var slideInTask = VideoContainer.TranslateTo(0, 0, 220, Easing.CubicOut);
        await Task.WhenAll(fadeInTask, slideInTask);

        //PlayPlayer();
        _isTransitioning = false;
    }

    private async Task ExecuteSwitchAsync(bool next)
    {
        try
        {
            if (next)
                await (VM?.NextCommand?.ExecuteAsync(null) ?? Task.CompletedTask);
            else
                await (VM?.PreviousCommand?.ExecuteAsync(null) ?? Task.CompletedTask);
        }
        catch
        {
            // ignore and continue to avoid crashing on gesture spam
        }
    }

    private void PausePlayer()
    {
        if (Player != null)
        {
            Player.Pause();
            PlayOverlay.IsVisible = true;

            // Fade animation for overlay
            PlayOverlay.FadeTo(1, 250, Easing.CubicIn);
            _isPaused = true;
            VideoTitleLabel.LineBreakMode = LineBreakMode.WordWrap;
        }
    }


    private void PlayPlayer()
    {
        if (Player != null)
        {
            Player.Play();
            PlayOverlay.IsVisible = false;
            PlayOverlay.Opacity = 0;
            _isPaused = false;
            VideoTitleLabel.LineBreakMode = LineBreakMode.TailTruncation;
        }
    }
}