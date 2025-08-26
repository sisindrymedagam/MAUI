using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Loop.MAUI.Handlers;
using Loop.MAUI.Models;
using Loop.MAUI.Pages;
using Loop.MAUI.Services;
using System.Collections.ObjectModel;

namespace Loop.MAUI.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly SyncService _syncService;
    private readonly AuthService _authService;
    private readonly MediaCacheService _cacheService;
    private readonly ShortsDatabase _db;
    private readonly IServiceProvider serviceProvider;

    [ObservableProperty] private string userEmail;

    public ObservableCollection<SyncDetail> SyncDetails { get; set; }

    public SettingsViewModel(SyncService syncService,
        AuthService authService,
        ShortsDatabase db,
        MediaCacheService cacheService,
        IServiceProvider serviceProvider)
    {
        _syncService = syncService;
        _authService = authService;
        _db = db;
        _cacheService = cacheService;
        this.serviceProvider = serviceProvider;
        // Load from local DB
        _ = LoadInfo();
    }

    private async Task LoadInfo()
    {
        UserEmail = Preferences.Get(Constants.UserEmailName, "");

        SyncDetails =
    [
        new SyncDetail { Label = "Last sync: ", Value = Preferences.Get(Constants.LastSyncUtcName, Constants.MinDateTime).ToString("dd MMM yyyy HH:mm") },
        new SyncDetail { Label = "Videos count: ", Value = (await _db.GetShortsCountAsync()).ToString() },
        new SyncDetail { Label = "Database: ", Value = Constants.DatabasePath },
        new SyncDetail { Label = "Cache path: ", Value = FileSystem.CacheDirectory },
        new SyncDetail { Label = "Videos cached: ", Value = _cacheService.GetCachedFilesCount().ToString() }
    ];
        OnPropertyChanged(nameof(SyncDetails));
    }

    [RelayCommand]
    private async Task ForceSyncAsync()
    {
        string token = Preferences.Get(Constants.TokenName, string.Empty);
        DateTime exp = Preferences.Get(Constants.TokenExpirationName, DateTime.MinValue);
        if (string.IsNullOrWhiteSpace(token) || exp <= DateTime.UtcNow)
        {
            NavigationHandler.NavigateTo(new LoginPage(serviceProvider));
            return;
        }

        await _syncService.SyncAsync(token, true);
        _cacheService.ClearCache();
        await Application.Current.MainPage.DisplayAlert("Sync", "Sync completed successfully.", "OK");
        Application.Current.Quit();
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        bool confirm = await Application.Current.MainPage.DisplayAlert(
        "Logout",
        "This will clear all cached videos and log you out. Continue?",
        "Yes", "No");

        if (!confirm) return;

        await _authService.Logout();
        NavigationHandler.NavigateTo(new LoginPage(serviceProvider));
    }

}
