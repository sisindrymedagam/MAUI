namespace Looply.MAUI.Services;

public class MediaCacheService
{
    private readonly HttpClient _httpClient = new();

    private string GetCachePath(int id, string? url)
    {
        string ext = Path.GetExtension(url) ?? ".mp4";
        return Path.Combine(FileSystem.CacheDirectory, $"{id}{ext}");
    }

    public async Task<string> GetOrDownloadAsync(int id, string url)
    {
        string localPath = GetCachePath(id, url);

        if (!File.Exists(localPath))
        {
            byte[] bytes = await _httpClient.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(localPath, bytes);
        }

        return localPath;
    }

    public void ClearCache()
    {
        string[] files = Directory.GetFiles(FileSystem.CacheDirectory, "*.mp4");
        foreach (string f in files)
        {
            try { File.Delete(f); } catch { }
        }
    }
}
