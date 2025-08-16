using System.Text.Json;
using YTShorts.Models;

namespace YTShorts.Services;

public interface IVideoService
{
    Task<IEnumerable<VideoItem>> GetVideosAsync();
}

public class VideoService : IVideoService
{
    private readonly HttpClient _httpClient;
    private const string ApiEndpoint = "/api/shorts";

    public VideoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<VideoItem>> GetVideosAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(ApiEndpoint);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"API Error: {response.StatusCode}");

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<VideoItem>>(content)
                   ?? [];
        }
        catch (Exception ex)
        {
            // Handle errors (log, show alert, etc.)
            Console.WriteLine($"API Error: {ex.Message}");
            return [];
        }
    }
}