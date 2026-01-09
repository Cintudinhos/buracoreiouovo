using System.Net.Http.Headers;
using System.Net.Http.Json;
using App.Core.Models.Clients;

namespace App.Infrastructure.Clients;

public interface IFirestoreClient
{
    Task<FirestoreCrownEgg[]> GetCrownEggEntriesAsync(int year);
}

public class FirestoreClient(HttpClient httpClient)
    : IFirestoreClient
{
    private const string BaseUrlV1 = "v1/projects/buracoreiouovo/databases/(default)/documents";

    private readonly HttpClient _httpClient = httpClient;

    public async Task<FirestoreCrownEgg[]> GetCrownEggEntriesAsync(int year)
    {
        string requestUri = $"{BaseUrlV1}:runQuery";

        string body =
            $$"""
            {
              "structuredQuery": {
                "from": [
                  { "collectionId": "crown-egg" }
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

        if (!response.IsSuccessStatusCode)
        {
            return [];
        }

        FirestoreCrownEgg[]? firestoreCrownEggResponse =
            await response.Content.ReadFromJsonAsync<FirestoreCrownEgg[]>();

        return firestoreCrownEggResponse ?? [];
    }
}
