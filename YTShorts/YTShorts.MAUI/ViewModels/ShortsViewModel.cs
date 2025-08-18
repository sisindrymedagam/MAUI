using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using YTShorts.MAUI.Services;
using YTShorts.MAUI.Models;

namespace YTShorts.MAUI.ViewModels;

public partial class ShortsViewModel : INotifyPropertyChanged
{
    private readonly SyncService _syncService;

    public ObservableCollection<ShortsListDto> Shorts { get; } = new();

    private bool _isBusy;
    private string _error = "";

    public bool IsBusy { get => _isBusy; set => Set(ref _isBusy, value); }
    public string Error { get => _error; set => Set(ref _error, value); }

    public ShortsViewModel(SyncService syncService)
    {
        _syncService = syncService;
    }

    public async Task LoadOrSyncAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Error = "";

            var token = Preferences.Get("AuthToken", string.Empty);
            var exp = Preferences.Get("AuthTokenExpiration", DateTime.MinValue);
            if (string.IsNullOrWhiteSpace(token) || exp <= DateTime.UtcNow)
            {
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            var items = await _syncService.SyncAsync(token);

            Shorts.Clear();

            foreach (var s in items)
                Shorts.Add(s);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            Error = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    [RelayCommand]
    private async Task NavigateSettingsAsync()
    {
        await Shell.Current.GoToAsync("//SettingsPage");
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void Set<T>(ref T field, T value, [CallerMemberName] string? prop = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
