using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Loop.MAUI.Models;
using Loop.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Loop.MAUI.ViewModels;

public partial class ShortsViewModel : ObservableObject
{
    private readonly SyncService _syncService;
    private readonly MediaCacheService _cacheService;
    private readonly IServiceProvider serviceProvider;

    private int _index = 0;

    [ObservableProperty]
    private string playPosition = string.Empty; // <-- NEW

    [ObservableProperty]
    private ShortsListDto? currentVideo;

    public ObservableCollection<ShortsListDto> Shorts { get; } = [];

    public ShortsViewModel(SyncService syncService, MediaCacheService cacheService, IServiceProvider serviceProvider)
    {
        _syncService = syncService;
        _cacheService = cacheService;
        this.serviceProvider = serviceProvider;
    }

    public async Task LoadOrSyncAsync()
    {
        try
        {
            string token = Preferences.Get(Constants.TokenName, string.Empty);

            List<ShortsListDto> items = await _syncService.LoadFromDbAsync();

            _ = LoadListAsync(items);

            // 2. Background sync with API
            _ = Task.Run(async () =>
            {
                List<ShortsListDto> apiItems = await _syncService.SyncAsync(token);

                if (apiItems.Any())
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        _ = LoadListAsync(apiItems); // refresh UI
                    });
                }
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private async Task LoadListAsync(List<ShortsListDto> items)
    {
        Shorts.Clear();
        foreach (ShortsListDto s in items)
            Shorts.Add(s);

        if (Shorts.Count > 0)
        {
            _index = 0;
            var nextVideo = _index + 1 < Shorts.Count ? Shorts[_index + 1] : null;

            // Pre-cache next
            CacheVideo(nextVideo);

            await UpdatePlayPositionAsync(Shorts[_index]);
        }
    }

    private void CacheVideo(ShortsListDto? shortVid)
    {
        // Pre-cache next
        if (shortVid != null)
        {
            _ = Task.Run(() => _cacheService.GetOrDownloadAsync(shortVid.Id, shortVid.URL));
        }
    }

    [RelayCommand]
    private async Task Next()
    {
        if (Shorts.Count == 0) return;

        if (_index < Shorts.Count - 1)
            _index++;
        else
            _index = 0;

        await UpdatePlayPositionAsync(Shorts[_index]);

        var nextVideo = _index + 1 < Shorts.Count ? Shorts[_index + 1] : null;
        // Pre-cache next
        CacheVideo(nextVideo);

    }

    [RelayCommand]
    private async Task Previous()
    {
        if (Shorts.Count == 0) return;

        if (_index > 0)
            _index--;
        else
            _index = Shorts.Count - 1;

        await UpdatePlayPositionAsync(Shorts[_index]);
    }

    private async Task UpdatePlayPositionAsync(ShortsListDto shortVid)
    {
        shortVid.URL = await _cacheService.GetOrDownloadAsync(shortVid.Id, shortVid.URL);
        CurrentVideo = Shorts[_index];
        PlayPosition = $"{_index + 1}/{Shorts.Count}";
    }
}
