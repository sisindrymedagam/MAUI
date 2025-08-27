using Loop.MAUI.Handlers;
using Loop.MAUI.Pages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Loop.MAUI.Services;

public class ApiService
{
    private readonly IServiceProvider serviceProvider;

    private readonly HttpClient _http;

    public ApiService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        _http = new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = System.Net.DecompressionMethods.All,
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        })
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
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

            SecureStorage.RemoveAll();
            Preferences.Clear();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                NavigationHandler.NavigateTo(new LoginPage(serviceProvider));
            });
        }
    }

    public async Task<T> PostAsync<T>(string url, object payload)
    {
        string json = JsonSerializer.Serialize(payload);
        using StringContent content = new(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _http.PostAsync(url, content);
        if (!response.IsSuccessStatusCode)
        {
            HandleUnauthorized(response);
            string body = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"{(int)response.StatusCode} {response.ReasonPhrase}: {body}");
        }

        string str = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(str, JsonOptions)!;
    }

    public async Task<T> GetAsync<T>(string url, string token)
    {
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            HandleUnauthorized(response);
            string body = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"{(int)response.StatusCode} {response.ReasonPhrase}: {body}");
        }

        string str = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(str, JsonOptions)!;
    }
}
