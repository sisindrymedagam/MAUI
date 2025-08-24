using Loop.MAUI.Models;

namespace Loop.MAUI.Services;

public class SyncService
{
    private readonly ApiService _api;
    private readonly ShortsDatabase _db;
    public SyncService(ApiService api, ShortsDatabase db)
    {
        _api = api;
        _db = db;
    }

    public async Task<List<ShortsListDto>> SyncAsync(string token, bool force = false)
    {
        try
        {
            DateTime lastSync = Preferences.Get(Constants.LastSyncUtcName, Constants.MinDateTime);

            if (force) lastSync = Constants.MinDateTime;

            string url = $"https://ytshort.azurewebsites.net/api/sync?lastSync={Uri.EscapeDataString(lastSync.ToString("O"))}";
            SyncViewModel<ShortsListDto> result = await _api.GetAsync<SyncViewModel<ShortsListDto>>(url, token);

            // Apply updates
            if (result.Updates?.Count > 0)
                await _db.SaveShortsAsync(result.Updates);

            // Apply deletes
            if (result.Deletes?.Count > 0)
                await _db.DeleteShortsAsync(result.Deletes);

            // Update sync timestamp
            Preferences.Set(Constants.LastSyncUtcName, result.ServerSyncTime);

            return await _db.GetShortsAsync();
        }
        catch
        {
            return await _db.GetShortsAsync();
        }
    }

    public async Task<List<ShortsListDto>> LoadFromDbAsync()
    {
        return await _db.GetShortsAsync();
    }

}
