using System.Windows.Input;

namespace Passkeeper.Components;

public partial class CustomPinKeyboard : ContentView
{
    private string _currentPin = "";
    private readonly Label[] _pinDots;
    
    public event EventHandler<string>? PinSubmitted;
    public event EventHandler? PinCleared;

    public CustomPinKeyboard()
    {
        InitializeComponent();
        _pinDots = new[] { PinDot1, PinDot2, PinDot3, PinDot4 };
    }

    private void OnNumberClicked(object sender, EventArgs e)
    {
        if (sender is Button button && _currentPin.Length < 4)
        {
            _currentPin += button.Text;
            UpdatePinDisplay();
        }
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (_currentPin.Length > 0)
        {
            _currentPin = _currentPin.Substring(0, _currentPin.Length - 1);
            UpdatePinDisplay();
        }
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        _currentPin = "";
        UpdatePinDisplay();
        PinCleared?.Invoke(this, EventArgs.Empty);
    }

    private void OnSubmitClicked(object sender, EventArgs e)
    {
        if (_currentPin.Length == 4)
        {
            PinSubmitted?.Invoke(this, _currentPin);
        }
    }

    private void UpdatePinDisplay()
    {
        for (int i = 0; i < _pinDots.Length; i++)
        {
            _pinDots[i].IsVisible = i < _currentPin.Length;
            if (i < _currentPin.Length)
            {
                _pinDots[i].TextColor = Application.Current?.RequestedTheme == AppTheme.Dark 
                    ? Colors.White 
                    : Colors.Black;
            }
        }
    }

    public void ClearPin()
    {
        _currentPin = "";
        UpdatePinDisplay();
    }

    public void SetPin(string pin)
    {
        _currentPin = pin;
        UpdatePinDisplay();
    }
} 