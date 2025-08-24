using Looply.MAUI.Models;

namespace Looply.MAUI.Services;

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
        string token = Preferences.Get(Constants.TokenName, string.Empty);
        DateTime expiration = Preferences.Get(Constants.TokenExpirationName, DateTime.MinValue);

        bool isInValid = string.IsNullOrWhiteSpace(token) || expiration <= DateTime.UtcNow;
        return !isInValid;
    }

    public Task<AuthResponse> LoginAsync(string email, string password)
    {
        string url = "https://ytshort.azurewebsites.net/account/token";
        return _api.PostAsync<AuthResponse>(url, new { Email = email, Password = password });
    }

    public async Task Logout()
    {
        Preferences.Clear();
        await _db.DeleteAllShortsAsync();
    }
}
