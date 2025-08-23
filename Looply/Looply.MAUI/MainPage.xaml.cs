using Looply.MAUI.ViewModels;

namespace Looply.MAUI;

public partial class MainPage : ContentPage
{
    private ShortsViewModel? VM => BindingContext as ShortsViewModel;
    private bool _isPaused = false;
    public MainPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = serviceProvider.GetService<ShortsViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (Player != null && Player.Source != null)
        {
            Player.Play();
        }
        else
        {
            if (VM is not null)
                await VM.LoadOrSyncAsync();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        Player?.Pause();
    }

    private void OnVideoTapped(object sender, TappedEventArgs e)
    {
        if (_isPaused)
        {
            Player.Play();
            PlayOverlay.IsVisible = false;
            PlayOverlay.Opacity = 0;
            _isPaused = false;
        }
        else
        {
            Player.Pause();
            PlayOverlay.IsVisible = true;

            // Fade animation for overlay
            PlayOverlay.FadeTo(1, 250, Easing.CubicIn);
            _isPaused = true;
        }
    }
}