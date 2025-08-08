using YTShorts.ViewModels;

namespace YTShorts;

public partial class MainPage : ContentPage
{
    private readonly ShortsViewModel _viewModel;
    private double _panY;

    public MainPage(ShortsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SetFullScreen(true);
        AddSwipeGestures();
    }

    private void AddSwipeGestures()
    {
        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        MainGrid.GestureRecognizers.Add(panGesture);
    }

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _panY = 0;
                break;

            case GestureStatus.Running:
                _panY = e.TotalY;
                break;

            case GestureStatus.Completed:
                const double swipeThreshold = 50;

                if (_panY < -swipeThreshold) // Swipe Up
                {
                    _viewModel.NextVideo();
                }
                else if (_panY > swipeThreshold) // Swipe Down
                {
                    _viewModel.PreviousVideo();
                }
                break;
        }
    }

    private void SetFullScreen(bool fullScreen)
    {
#if ANDROID
        if (Window != null)
        {
            var activity = Platform.CurrentActivity;
            var window = activity.Window;
            var decorView = window.DecorView;

            var uiOptions = (int)decorView.SystemUiVisibility;
            uiOptions |= (int)Android.Views.SystemUiFlags.Fullscreen;
            decorView.SystemUiVisibility = (Android.Views.StatusBarVisibility)uiOptions;
        }
#endif
    }
}
