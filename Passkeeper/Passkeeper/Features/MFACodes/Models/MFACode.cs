using System.ComponentModel;

namespace Passkeeper.Features.MFACodes.Models;

public class MFACode : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private string _secret = string.Empty;
    private string _issuer = string.Empty;
    private string _code = string.Empty;
    private int _timeRemaining = 30;
    private bool _isExpiringSoon = false;

    public int Id { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(DisplayName));
        }
    }

    public string Secret
    {
        get => _secret;
        set
        {
            _secret = value;
            OnPropertyChanged(nameof(Secret));
        }
    }

    public string Issuer
    {
        get => _issuer;
        set
        {
            _issuer = value;
            OnPropertyChanged(nameof(Issuer));
            OnPropertyChanged(nameof(DisplayName));
        }
    }

    public string Code
    {
        get => _code;
        set
        {
            _code = value;
            OnPropertyChanged(nameof(Code));
        }
    }

    public int TimeRemaining
    {
        get => _timeRemaining;
        set
        {
            _timeRemaining = value;
            OnPropertyChanged(nameof(TimeRemaining));
            IsExpiringSoon = value <= 5;
        }
    }

    public bool IsExpiringSoon
    {
        get => _isExpiringSoon;
        set
        {
            _isExpiringSoon = value;
            OnPropertyChanged(nameof(IsExpiringSoon));
        }
    }

    public string DisplayName => string.IsNullOrEmpty(Issuer) ? Name : $"{Issuer} ({Name})";

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}