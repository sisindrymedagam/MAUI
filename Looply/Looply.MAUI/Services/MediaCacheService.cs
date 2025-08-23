namespace Looply.MAUI.Services;

public class MediaCacheService
{
    private readonly HttpClient _httpClient = new();

    private string GetCachePath(int id, string? url)
    {
        var ext = Path.GetExtension(url) ?? ".mp4";
        return Path.Combine(FileSystem.CacheDirectory, $"{id}{ext}");
    }

    public async Task<string> GetOrDownloadAsync(int id, string url)
    {
        var localPath = GetCachePath(id, url);

        if (!File.Exists(localPath))
        {
            var bytes = await _httpClient.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(localPath, bytes);
        }

        return localPath;
    }

    public void ClearCache()
    {
        var files = Directory.GetFiles(FileSystem.CacheDirectory, "*.mp4");
        foreach (var f in files)
        {
            try { File.Delete(f); } catch { }
        }
    }
}
