namespace Loop.MAUI.Services;

public interface ISecureStorageService
{
    Task<string?> GetAsync(string key);
    Task SetAsync(string key, string value);
    Task RemoveAsync(string key);
    Task<bool> ContainsKeyAsync(string key);
}

public class SecureStorageService : ISecureStorageService
{
    public async Task<string?> GetAsync(string key)
    {
        try
        {
            return await SecureStorage.Default.GetAsync(key).ConfigureAwait(false);
        }
        catch
        {
            // Fallback to Preferences if SecureStorage fails
            return Preferences.Get(key, string.Empty);
        }
    }

    public async Task SetAsync(string key, string value)
    {
        try
        {
            await SecureStorage.Default.SetAsync(key, value).ConfigureAwait(false);
        }
        catch
        {
            // Fallback to Preferences if SecureStorage fails
            Preferences.Set(key, value);
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            SecureStorage.Default.Remove(key);
        }
        catch
        {
            // Fallback to Preferences if SecureStorage fails
            Preferences.Remove(key);
        }
        
        await Task.CompletedTask;
    }

    public async Task<bool> ContainsKeyAsync(string key)
    {
        try
        {
            var value = await SecureStorage.Default.GetAsync(key).ConfigureAwait(false);
            return !string.IsNullOrEmpty(value);
        }
        catch
        {
            // Fallback to Preferences if SecureStorage fails
            return Preferences.ContainsKey(key);
        }
    }
}
