using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Looply.MAUI.Models;
using Looply.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Looply.MAUI.ViewModels;

public partial class ShortsViewModel : ObservableObject
{
    private readonly SyncService _syncService;
    private readonly IServiceProvider serviceProvider;

    private int _index = 0;

    [ObservableProperty]
    private string playPosition = string.Empty; // <-- NEW

    [ObservableProperty]
    private ShortsListDto? currentVideo;

    public ObservableCollection<ShortsListDto> Shorts { get; } = [];

    public ShortsViewModel(SyncService syncService, IServiceProvider serviceProvider)
    {
        _syncService = syncService;
        this.serviceProvider = serviceProvider;
    }

    public async Task LoadOrSyncAsync()
    {
        try
        {
            string token = Preferences.Get(Constants.TokenName, string.Empty);

            List<ShortsListDto> items = await _syncService.LoadFromDbAsync();

            LoadList(items);

            // 2. Background sync with API
            _ = Task.Run(async () =>
            {
                List<ShortsListDto> apiItems = await _syncService.SyncAsync(token);

                if (apiItems.Any())
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        LoadList(apiItems); // refresh UI
                    });
                }
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
    private void LoadList(List<ShortsListDto> items)
    {
        Shorts.Clear();
        foreach (ShortsListDto s in items)
            Shorts.Add(s);

        if (Shorts.Count > 0)
        {
            _index = 0;
            CurrentVideo = Shorts[_index];
            UpdatePlayPosition();
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
        UpdatePlayPosition();
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
        UpdatePlayPosition();
    }

    private void UpdatePlayPosition()
    {
        PlayPosition = $"{_index + 1}/{Shorts.Count}";
    }
}
