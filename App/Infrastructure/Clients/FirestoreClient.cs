using System.Net.Http.Headers;
using System.Net.Http.Json;
using App.Core.Models.Clients;

namespace App.Infrastructure.Clients;

public interface IFirestoreClient
{
    Task<FirestoreCrownEgg[]> GetCrownEggEntriesAsync(int year, string collectionId);
    Task<bool> PatchCrownEggEntryAsync(CrownEggDocument crownEggDocument, string collectionId, string id);
    Task<bool> PostCrownEggEntryAsync(CrownEggDocument crownEggDocument, string collectionId);
}

public class FirestoreClient(HttpClient httpClient)
    : IFirestoreClient
{
    private const string BaseUrlV1 = "v1/projects/buracoreiouovo/databases/(default)/documents";

    private readonly HttpClient _httpClient = httpClient;

    public async Task<FirestoreCrownEgg[]> GetCrownEggEntriesAsync(int year, string collectionId)
    {
        string requestUri = $"{BaseUrlV1}:runQuery";

        string body =
            $$"""
            {
              "structuredQuery": {
                "from": [
                  { "collectionId": "crown-egg-{{collectionId}}" }
                ],
                "where": {
                  "fieldFilter": {
                    "field": { "fieldPath": "year" },
                    "op": "EQUAL",
                    "value": { "integerValue": {{year}} }
                  }
                }
              }
            }
            """;

        using HttpRequestMessage request = new(HttpMethod.Post, requestUri)
        {
            Content = new StringContent(body, new MediaTypeHeaderValue("application/json")),
        };

        using HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            try
            {
                FirestoreCrownEgg[]? firestoreCrownEggResponse =
                    await response.Content.ReadFromJsonAsync<FirestoreCrownEgg[]>();

                return firestoreCrownEggResponse ?? [];
            }
            catch
            {
                // do nothing
            }
        }

        return [];
    }

    public async Task<bool> PatchCrownEggEntryAsync(CrownEggDocument crownEggDocument, string collectionId, string id)
    {
        string requestUri = $"{BaseUrlV1}/crown-egg-{collectionId}/{id}";

        using HttpRequestMessage request = new(HttpMethod.Patch, requestUri)
        {
            Content = JsonContent.Create(crownEggDocument),
        };

        using HttpResponseMessage response = await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> PostCrownEggEntryAsync(CrownEggDocument crownEggDocument, string collectionId)
    {
        string requestUri = $"{BaseUrlV1}/crown-egg-{collectionId}";

        using HttpRequestMessage request = new(HttpMethod.Post, requestUri)
        {
            Content = JsonContent.Create(crownEggDocument),
        };

        using HttpResponseMessage response = await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }
}
