using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YTShorts.MAUI.Services;

namespace YTShorts.MAUI.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly SyncService _syncService;
    private readonly AuthService _authService;
    private readonly ShortsDatabase _db;

    [ObservableProperty] private string userEmail;
    [ObservableProperty] private DateTime? lastSyncTime;
    [ObservableProperty] private int videoCount;

    public SettingsViewModel(SyncService syncService, AuthService authService, ShortsDatabase db)
    {
        _syncService = syncService;
        _authService = authService;
        _db = db;

        // Load from local DB
        _ = LoadInfo();
    }

    private async Task LoadInfo()
    {
        UserEmail = Preferences.Get(Constants.UserEmailName, "");
        LastSyncTime = Preferences.Get(Constants.LastSyncUtcName, Constants.MinDateTime);
        VideoCount = await _db.GetShortsCountAsync();
    }

    [RelayCommand]
    private async Task ForceSyncAsync()
    {
        var token = Preferences.Get(Constants.TokenName, string.Empty);
        var exp = Preferences.Get(Constants.TokenExpirationName, DateTime.MinValue);
        if (string.IsNullOrWhiteSpace(token) || exp <= DateTime.UtcNow)
        {
            await Shell.Current.GoToAsync("//LoginPage");
            return;
        }

        await _syncService.SyncAsync(token, true);
        await Application.Current.MainPage.DisplayAlert("Sync", "Sync completed successfully.", "OK");
        await LoadInfo();
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
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
