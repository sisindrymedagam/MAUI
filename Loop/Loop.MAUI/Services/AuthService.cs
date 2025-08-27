using Loop.MAUI.Models;

namespace Loop.MAUI.Services;

public class AuthService
{
    private readonly ApiService _api;
    private readonly ShortsDatabase _db;
    private readonly ISecureStorageService _secureStorage;
    private readonly IValidationService _validationService;

    public AuthService(ApiService api, ShortsDatabase db, ISecureStorageService secureStorage, IValidationService validationService)
    {
        _api = api;
        _db = db;
        _secureStorage = secureStorage;
        _validationService = validationService;
    }

    public async Task<bool> IsLoggedInAsync()
    {
        try
        {
            string? token = await _secureStorage.GetAsync(Constants.TokenName);
            string? expirationStr = await _secureStorage.GetAsync(Constants.TokenExpirationName);

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(expirationStr))
                return false;

            if (!DateTime.TryParse(expirationStr, out DateTime expiration))
                return false;

            bool isValid = !string.IsNullOrWhiteSpace(token) && expiration > DateTime.UtcNow;
            
            // If token is expired, clean it up
            if (!isValid)
            {
                await LogoutAsync();
            }
            
            return isValid;
        }
        catch
        {
            return false;
        }
    }

    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        // Validate inputs
        var emailValidation = _validationService.ValidateEmail(email);
        if (!emailValidation.IsValid)
            throw new ArgumentException(emailValidation.ErrorMessage, nameof(email));

        var passwordValidation = _validationService.ValidatePassword(password);
        if (!passwordValidation.IsValid)
            throw new ArgumentException(passwordValidation.ErrorMessage, nameof(password));

        try
        {
            string url = "https://loop.coderons.com/account/token";
            var response = await _api.PostAsync<AuthResponse>(url, new { Email = email, Password = password });

            // Store credentials securely
            await _secureStorage.SetAsync(Constants.TokenName, response.Token);
            await _secureStorage.SetAsync(Constants.TokenExpirationName, response.Expiration.ToString("O"));
            await _secureStorage.SetAsync(Constants.UserEmailName, email);

            return response;
        }
        catch (Exception ex)
        {
            // Clear any partial data on error
            await _secureStorage.RemoveAsync(Constants.TokenName);
            await _secureStorage.RemoveAsync(Constants.TokenExpirationName);
            await _secureStorage.RemoveAsync(Constants.UserEmailName);
            throw;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            SecureStorage.RemoveAll();
            // Clear database
            await _db.DeleteAllShortsAsync();
            Preferences.Clear();
        }
        catch (Exception ex)
        {
            // Log error but don't throw - we want logout to succeed even if cleanup fails
            System.Diagnostics.Debug.WriteLine($"Logout cleanup error: {ex.Message}");
        }
    }

    public async Task<string?> GetCurrentTokenAsync()
    {
        try
        {
            if (await IsLoggedInAsync())
            {
                return await _secureStorage.GetAsync(Constants.TokenName);
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<string?> GetCurrentUserEmailAsync()
    {
        try
        {
            return await _secureStorage.GetAsync(Constants.UserEmailName);
        }
        catch
        {
            return null;
        }
    }
}
