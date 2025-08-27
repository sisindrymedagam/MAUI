using Loop.MAUI.Pages;
using Loop.MAUI.ViewModels;

namespace Loop.MAUI;

public partial class MainPage : ContentPage
{
    private ShortsViewModel? VM => BindingContext as ShortsViewModel;
    private bool _isPaused = false;
    private bool _isTransitioning = false;
    private double _containerHeight = 0;
    private bool _isDraggingProgress = false;
    private readonly IServiceProvider ServiceProvider;
    public MainPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = serviceProvider.GetService<ShortsViewModel>();
        ServiceProvider = serviceProvider;
        NavigationPage.SetHasNavigationBar(this, false);

        if (Player != null)
        {
            Player.MediaOpened += OnMediaOpened;
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        double h = VideoContainer?.Height > 0 ? VideoContainer.Height : RootGrid.Height;
        if (h > 0)
            _containerHeight = h;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (Player != null && Player.Source != null)
        {
            PlayPlayer();
            UpdateProgressFromPlayer();
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
        Navigation.PushAsync(new SettingsPage(ServiceProvider), true);
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
        if (VideoInfoContainer != null)
            VideoInfoContainer.InputTransparent = true;

        double height = _containerHeight > 0 ? _containerHeight : (VideoContainer.Height > 0 ? VideoContainer.Height : RootGrid.Height);
        if (height <= 0)
        {
            // layout not ready; skip animation but still switch
            await ExecuteSwitchAsync(next);
            if (VideoInfoContainer != null)
                VideoInfoContainer.InputTransparent = false;
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

        // slight cooldown to avoid rapid re-entry and let layout settle
        await Task.Delay(120);
        if (VideoInfoContainer != null)
            VideoInfoContainer.InputTransparent = false;
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
            // let UI/layout settle before resuming playback
            await Task.Yield();
            PlayPlayer();
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
            PauseOverlay.IsVisible = true;
            ProgressContainer.IsVisible = true;
            UpdateProgressFromPlayer();

            // Fade animation for overlay
            PlayOverlay.FadeTo(1, 250, Easing.CubicIn);
            PauseOverlay.FadeTo(0.35, 250, Easing.CubicIn);
            _isPaused = true;
            Grid.SetRowSpan(VideoInfoBoxViewContainer, 2);
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
            PauseOverlay.FadeTo(0, 200, Easing.CubicOut);
            PauseOverlay.IsVisible = false;
            ProgressContainer.IsVisible = false;
            _isPaused = false;
            Grid.SetRowSpan(VideoInfoBoxViewContainer, 1);
            VideoTitleLabel.LineBreakMode = LineBreakMode.TailTruncation;
        }
    }

    private void OnMediaOpened(object? sender, EventArgs e)
    {
        UpdateProgressFromPlayer();
    }

    private void UpdateProgressFromPlayer()
    {
        if (Player == null || ProgressSlider == null) return;
        double durationSeconds = Player.Duration.TotalSeconds;
        if (double.IsInfinity(durationSeconds) || double.IsNaN(durationSeconds) || durationSeconds <= 0)
        {
            // Unknown duration, set a safe max and reset value
            ProgressSlider.Maximum = 1;
            if (!_isDraggingProgress) ProgressSlider.Value = 0;
            return;
        }

        double totalSeconds = durationSeconds;
        double currentSeconds = Player.Position.TotalSeconds;
        if (currentSeconds < 0) currentSeconds = 0;
        if (currentSeconds > totalSeconds) currentSeconds = totalSeconds;
        ProgressSlider.Maximum = totalSeconds;
        if (!_isDraggingProgress)
            ProgressSlider.Value = currentSeconds;
    }

    private void OnProgressDragStarted(object sender, EventArgs e)
    {
        _isDraggingProgress = true;
    }

    private void OnProgressDragCompleted(object sender, EventArgs e)
    {
        if (Player == null) { _isDraggingProgress = false; return; }
        double targetSeconds = ProgressSlider?.Value ?? 0;
        if (targetSeconds < 0) targetSeconds = 0;
        double durationSeconds = Player.Duration.TotalSeconds;
        if (double.IsInfinity(durationSeconds) || double.IsNaN(durationSeconds) || durationSeconds <= 0)
            durationSeconds = 0;
        if (durationSeconds > 0 && targetSeconds > durationSeconds)
            targetSeconds = durationSeconds;
        try
        {
            Player.SeekTo(TimeSpan.FromSeconds(targetSeconds));
            if (ProgressSlider != null)
                ProgressSlider.Value = targetSeconds;
        }
        catch { }
        finally
        {
            _isDraggingProgress = false;
            // Stay paused; user can tap to play
            UpdateProgressFromPlayer();
        }
    }
}