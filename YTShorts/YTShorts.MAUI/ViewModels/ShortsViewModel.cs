using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using YTShorts.MAUI.Models;
using YTShorts.MAUI.Services;

namespace YTShorts.MAUI.ViewModels;

public partial class ShortsViewModel : ObservableObject
{
    private readonly SyncService _syncService;

    private int _index = 0;

    [ObservableProperty]
    private ShortsListDto? currentVideo;

    public ObservableCollection<ShortsListDto> Shorts { get; } = [];

    public ShortsViewModel(SyncService syncService)
    {
        _syncService = syncService;
    }

    public async Task LoadOrSyncAsync()
    {
        try
        {
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

            if (Shorts.Count > 0)
            {
                _index = 0;
                CurrentVideo = Shorts[_index];
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    [RelayCommand]
    private void Next()
    {
        if (Shorts.Count == 0) return;

        if (_index < Shorts.Count - 1)
            _index++;
        else
            _index = 0;

        CurrentVideo = Shorts[_index];
    }

    [RelayCommand]
    private void Previous()
    {
        if (Shorts.Count == 0) return;

        if (_index > 0)
            _index--;
        else
            _index = Shorts.Count - 1;

        CurrentVideo = Shorts[_index];
    }

    [RelayCommand]
    private static async Task NavigateSettings()
    {
        await Shell.Current.GoToAsync("//SettingsPage");
    }
}
