using System.Net.Http.Json;
using App.Core.Models.Clients;

namespace App.Infrastructure.Clients;

public interface ILoginClient
{
    Task<LoginResponse?> LoginAsync(string email, string password);
}

public class LoginClient(HttpClient httpClient)
    : ILoginClient
{
    private const string ApiKey = "**********";
    private const string BaseUrlV1 = "v1/accounts";

    private readonly HttpClient _httpClient = httpClient;

    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        string requestUri = $"{BaseUrlV1}:signInWithPassword?key={ApiKey}";

        var body = new
        {
            email,
            password,
            returnSecureToken = true
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

        LoginResponse? loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

        return loginResponse;
    }
}
