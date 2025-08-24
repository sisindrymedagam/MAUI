namespace Loop.MAUI.Services;
public class MediaCacheService
{
    private readonly HttpClient _httpClient = new();

    private string GetCachePath(int id, string url)
    {
        string ext = Path.GetExtension(url);
        if (string.IsNullOrWhiteSpace(ext))
            ext = ".mp4";

        return Path.Combine(FileSystem.CacheDirectory, $"{id}{ext}");
    }

    /// <summary>
    /// Returns cached file if available, otherwise downloads and caches it.
    /// </summary>
    public async Task<string> GetOrDownloadAsync(int id, string url)
    {
        string localPath = GetCachePath(id, url);

        if (File.Exists(localPath))
            return localPath;

        try
        {
            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = File.Create(localPath);
            await stream.CopyToAsync(fileStream);
        }
        catch
        {
            // If download fails but file was partially created, remove it
            if (File.Exists(localPath))
                File.Delete(localPath);
            throw;
        }

        return localPath;
    }

    /// <summary>
    /// Clears all cached videos.
    /// </summary>
    public void ClearCache()
    {
        string[] files = Directory.GetFiles(FileSystem.CacheDirectory, "*.mp4");
        foreach (string f in files)
        {
            try { File.Delete(f); } catch { }
        }
    }

    ///// <summary>
    ///// Limit cache size (e.g. keep only last N files or under X MB).
    ///// </summary>
    //public void EnforceCacheLimit(int maxFiles = 20, long maxSizeBytes = 500 * 1024 * 1024) // 500MB
    //{
    //    var files = new DirectoryInfo(FileSystem.CacheDirectory)
    //        .GetFiles("*.mp4")
    //        .OrderByDescending(f => f.LastAccessTimeUtc)
    //        .ToList();

    //    // Delete excess files
    //    foreach (var f in files.Skip(maxFiles))
    //    {
    //        try { f.Delete(); } catch { }
    //    }

    //    // Check size
    //    long totalSize = files.Sum(f => f.Length);
    //    if (totalSize > maxSizeBytes)
    //    {
    //        foreach (var f in files.OrderBy(f => f.LastAccessTimeUtc))
    //        {
    //            try { f.Delete(); } catch { }
    //            totalSize -= f.Length;
    //            if (totalSize <= maxSizeBytes) break;
    //        }
    //    }
    //}
}
