using Looply.MAUI.Pages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Looply.MAUI.Services;

public class ApiService
{
    readonly IServiceProvider serviceProvider;

    private readonly HttpClient _http;

    public ApiService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        _http = new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = System.Net.DecompressionMethods.All
        });
        _http.Timeout = TimeSpan.FromSeconds(30);
    }

    private static JsonSerializerOptions JsonOptions => new()
    {
        PropertyNameCaseInsensitive = true
    };

    private void HandleUnauthorized(HttpResponseMessage response)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // clear session and route to login
            Preferences.Remove(Constants.TokenName);
            Preferences.Remove(Constants.TokenExpirationName);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.Navigation.PushAsync(new LoginPage(serviceProvider));
            });
        }
    }

    public async Task<T> PostAsync<T>(string url, object payload)
    {
        var json = JsonSerializer.Serialize(payload);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.PostAsync(url, content);
        if (!response.IsSuccessStatusCode)
        {
            HandleUnauthorized(response);
            var body = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"{(int)response.StatusCode} {response.ReasonPhrase}: {body}");
        }

        var str = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(str, JsonOptions)!;
    }

    public async Task<T> GetAsync<T>(string url, string token)
    {
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            HandleUnauthorized(response);
            var body = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"{(int)response.StatusCode} {response.ReasonPhrase}: {body}");
        }

        var str = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(str, JsonOptions)!;
    }
}
