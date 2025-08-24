using Looply.MAUI.Pages;
using Looply.MAUI.ViewModels;

namespace Looply.MAUI;

public partial class MainPage : ContentPage
{
    private ShortsViewModel? VM => BindingContext as ShortsViewModel;
    private bool _isPaused = false;
    IServiceProvider ServiceProvider;
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

    private void PausePlayer()
    {
        if (Player != null)
        {
            Player.Pause();
            PlayOverlay.IsVisible = true;

            // Fade animation for overlay
            PlayOverlay.FadeTo(1, 250, Easing.CubicIn);
            _isPaused = true;
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
        }
    }
}