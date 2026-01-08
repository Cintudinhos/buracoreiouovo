using System.Text.Json;
using System.Text.Json.Serialization;
using App.Core.Helpers;
using App.Core.Models;
using App.Core.Models.Clients;
using App.Infrastructure.Clients;
using App.Infrastructure.Repositories;

namespace App.Services;

public interface IAuthService
{
    DateTime Expiration { get; }
    string? IdToken { get; }

    Task<bool> InitializeAsync();
    Task<bool> LoginAsync(string email, string password);
    Task<bool> RefreshAsync();
}

public class AuthService(IAuthClient authClient,
                         ILoginClient loginClient,
                         IStateRepository stateRepository)
    : IAuthService
{
    private sealed class Jwt
    {
        [JsonPropertyName("exp")]
        public long Exp { get; set; }
    }

    private readonly IAuthClient _authClient = authClient;
    private readonly ILoginClient _loginClient = loginClient;
    private readonly IStateRepository _stateRepository = stateRepository;

    public DateTime Expiration { get; private set; }
    public string? IdToken { get; private set; }

    public async Task<bool> InitializeAsync()
    {
        AppState appState = _stateRepository.GetState();

        if (appState.RefreshToken is null)
        {
            return false;
        }

        if (Expiration < DateTime.UtcNow)
        {
            bool result = await RefreshAsync();

            return result;
        }

        return true;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        LoginResponse? loginResponse = await _loginClient.LoginAsync(email, password);

        if (loginResponse is not null)
        {
            AppState appState = _stateRepository.GetState();
            appState.RefreshToken = loginResponse.RefreshToken;
            _stateRepository.SaveState(appState);

            Jwt? jwt = GetJwt(loginResponse.IdToken);

            Expiration = DateTimeOffset.FromUnixTimeSeconds(jwt?.Exp ?? 0).DateTime;
            IdToken = loginResponse.IdToken;

            return true;
        }

        return false;
    }

    public async Task<bool> RefreshAsync()
    {
        AppState appState = _stateRepository.GetState();
        RefreshResponse? refreshResponse = await _authClient.RefreshAsync(appState.RefreshToken ?? string.Empty);

        if (refreshResponse is not null)
        {
            appState.RefreshToken = refreshResponse.RefreshToken;
            _stateRepository.SaveState(appState);

            Jwt? jwt = GetJwt(refreshResponse.IdToken);

            Expiration = DateTimeOffset.FromUnixTimeSeconds(jwt?.Exp ?? 0).DateTime;
            IdToken = refreshResponse.IdToken;

            return true;
        }

        return false;
    }

    private static Jwt? GetJwt(string idToken)
    {
        string jwtPayload = idToken.Split('.')[1];
        string jwtJson = Base64Helper.FromBase64(jwtPayload);
        Jwt? jwt = JsonSerializer.Deserialize<Jwt>(jwtJson);

        return jwt;
    }
}
