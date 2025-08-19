using YTShorts.MAUI.Models;

namespace YTShorts.MAUI.Services;

public class AuthService
{
    private readonly ApiService _api;
    private readonly ShortsDatabase _db;

    public AuthService(ApiService api, ShortsDatabase db)
    {
        _api = api;
        _db = db;
    }

    public static bool IsLogedIn()
    {
        var token = Preferences.Get("AuthToken", string.Empty);
        var expiration = Preferences.Get("AuthTokenExpiration", DateTime.MinValue);

        var needsLogin = string.IsNullOrWhiteSpace(token) || expiration <= DateTime.UtcNow;
        return needsLogin;
    }

    public Task<AuthResponse> LoginAsync(string email, string password)
    {
        var url = "https://ytshort.azurewebsites.net/account/token";
        return _api.PostAsync<AuthResponse>(url, new { Email = email, Password = password });
    }

    public async Task Logout()
    {
        Preferences.Clear();
        await _db.DeleteAllShortsAsync();
    }
}
