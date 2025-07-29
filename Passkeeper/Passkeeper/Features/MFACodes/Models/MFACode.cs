using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace Passkeeper.Features.MFACodes.Models;

[Table("MFACodes")]
public class MFACode : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private string _secret = string.Empty;
    private string _code = string.Empty;
    private int _timeRemaining = 30;
    private bool _isExpiringSoon = false;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Column("Name")]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(DisplayName));
        }
    }

    [Column("Secret")]
    public string Secret
    {
        get => _secret;
        set
        {
            _secret = value;
            OnPropertyChanged();
        }
    }

    [Column("Code")]
    public string Code
    {
        get => _code;
        set
        {
            _code = value;
            OnPropertyChanged();
        }
    }

    [Column("TimeRemaining")]
    public int TimeRemaining
    {
        get => _timeRemaining;
        set
        {
            _timeRemaining = value;
            OnPropertyChanged();
            IsExpiringSoon = value <= 5;
        }
    }

    [Column("IsExpiringSoon")]
    public bool IsExpiringSoon
    {
        get => _isExpiringSoon;
        set
        {
            _isExpiringSoon = value;
            OnPropertyChanged();
        }
    }

    public string DisplayName => Name;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}