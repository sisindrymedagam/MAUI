using Loop.MAUI.Models;

namespace Loop.MAUI.Services;

public class SyncService
{
    private readonly ApiService _api;
    private readonly ShortsDatabase _db;
    private readonly AuthService _authService;

    public SyncService(ApiService api, ShortsDatabase db, AuthService authService)
    {
        _api = api;
        _db = db;
        _authService = authService;
    }

    public async Task<List<ShortsListDto>> SyncAsync(bool force = false)
    {
        try
        {
            string token = await _authService.GetCurrentTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                // No valid token, return cached data only
                return await _db.GetShortsAsync();
            }

            DateTime lastSync = Preferences.Get(Constants.LastSyncUtcName, Constants.MinDateTime);

            if (force) lastSync = Constants.MinDateTime;

            string url = $"https://loop.coderons.com/api/sync?lastSync={Uri.EscapeDataString(lastSync.ToString("O"))}";
            SyncViewModel<ShortsListDto> result = await _api.GetAsync<SyncViewModel<ShortsListDto>>(url, token);

            // Apply updates
            if (result.Updates?.Count > 0)
                await _db.SaveShortsAsync(result.Updates);

            // Apply deletes
            if (result.Deletes?.Count > 0)
                await _db.DeleteShortsAsync(result.Deletes);

            // Update sync timestamp
            Preferences.Set(Constants.LastSyncUtcName, result.ServerSyncTime);

            return result.Updates;
        }
        catch (Exception ex)
        {
            // Log error but return cached data
            System.Diagnostics.Debug.WriteLine($"Sync error: {ex.Message}");
            return await _db.GetShortsAsync();
        }
    }

    public async Task<List<ShortsListDto>> LoadFromDbAsync()
    {
        return await _db.GetShortsAsync();
    }
}
