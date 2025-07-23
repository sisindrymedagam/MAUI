namespace Passkeeper.Components;

public partial class OptionTileComponent : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(OptionTileComponent), string.Empty);

    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(OptionTileComponent));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public event EventHandler? Tapped;

    public OptionTileComponent()
    {
        InitializeComponent();
        SetupGestureRecognizers();
    }

    private void SetupGestureRecognizers()
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnTapped;
        this.GestureRecognizers.Add(tapGesture);
    }

    private void OnTapped(object? sender, EventArgs e)
    {
        Tapped?.Invoke(this, EventArgs.Empty);
    }
}