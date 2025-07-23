namespace Passkeeper.Components;

public partial class BottomSheetComponent : ContentView
{
    public BottomSheetComponent()
    {
        InitializeComponent();
        TapGestureRecognizer tap = new();
        tap.Tapped += (s, e) => Hide();
        Overlay.GestureRecognizers.Add(tap);
    }

    /// <summary>
    /// This event fires immediately when the show instance method is called
    /// </summary>
    public event EventHandler? OnShow;

    /// <summary>
    /// This event is fired when the modal has been made visible to the user
    /// </summary>
    public event EventHandler? OnShown;

    /// <summary>
    /// This event is fired immediately when the hide instance method has been called
    /// </summary>
    public event EventHandler? OnHide;

    /// <summary>
    /// This event is fired when the modal has finished being hidden from the user
    /// </summary>
    public event EventHandler? OnHidden;

    public static readonly BindableProperty SheetContentProperty =
        BindableProperty.Create(nameof(SheetContent), typeof(View), typeof(BottomSheetComponent), propertyChanged: OnSheetContentChanged);

    public View SheetContent
    {
        get => (View)GetValue(SheetContentProperty);
        set => SetValue(SheetContentProperty, value);
    }

    private static void OnSheetContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        BottomSheetComponent control = (BottomSheetComponent)bindable;
        if (newValue is View content)
        {
            control.ContentArea.Content = content;
        }
    }

    /// <summary>
    /// Hides the Bottom sheet
    /// </summary>
    /// <param name="time">The time, in milliseconds, over which to animate the transition. The default is 250.</param>
    public async void Show(uint time = 250)
    {
        OnShow?.Invoke(this, EventArgs.Empty);
        IsVisible = true;

        Panel.TranslationY = Panel.Height;

        await Task.WhenAll(
            Panel.TranslateTo(0, 0, time, Easing.CubicOut),
            Overlay.FadeTo(1, time)
        );
        OnShown?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Hides the Bottom sheet
    /// </summary>
    /// <param name="time">The time, in milliseconds, over which to animate the transition. The default is 250.</param>
    public async void Hide(uint time = 250)
    {
        OnHide?.Invoke(this, EventArgs.Empty);
        await Task.WhenAll(
            Panel.TranslateTo(0, Panel.Height, time, Easing.CubicIn),
            Overlay.FadeTo(0, time)
        );
        IsVisible = false;
        OnHidden?.Invoke(this, EventArgs.Empty);
    }
}