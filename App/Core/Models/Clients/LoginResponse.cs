namespace App.Core.Models.Clients;

public record class LoginResponse
(
    string IdToken,
    string RefreshToken
);
