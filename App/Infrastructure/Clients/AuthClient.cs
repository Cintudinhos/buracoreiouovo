using System.Net.Http.Json;
using App.Core.Models.Clients;

namespace App.Infrastructure.Clients;

public interface IAuthClient
{
    Task<RefreshResponse?> RefreshAsync(string refreshToken);
}

public class AuthClient(HttpClient httpClient)
    : IAuthClient
{
    private const string ApiKey = "**********";
    private const string BaseUrlV1 = "v1/token";

    private readonly HttpClient _httpClient = httpClient;

    public async Task<RefreshResponse?> RefreshAsync(string refreshToken)
    {
        string requestUri = $"{BaseUrlV1}?key={ApiKey}";

        var body = new
        {
            grant_type = "refresh_token",
            refresh_token = refreshToken,
        };

        using HttpRequestMessage request = new(HttpMethod.Post, requestUri)
        {
            Content = JsonContent.Create(body),
        };

        using HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        RefreshResponse? refreshResponse = await response.Content.ReadFromJsonAsync<RefreshResponse>();

        return refreshResponse;
    }
}
