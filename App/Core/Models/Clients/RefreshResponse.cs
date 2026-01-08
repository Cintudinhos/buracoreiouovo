using System.Text.Json.Serialization;

namespace App.Core.Models.Clients;

public record class RefreshResponse
(
    [property:JsonPropertyName("id_token")]
    string IdToken,

    [property:JsonPropertyName("refresh_token")]
    string RefreshToken
);
