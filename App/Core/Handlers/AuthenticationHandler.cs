using System.Net.Http.Headers;
using App.Services;

namespace App.Core.Handlers;

public partial class AuthenticationHandler(IAuthService authService)
    : DelegatingHandler
{
    private readonly IAuthService _authService = authService;

    protected override async Task<HttpResponseMessage> SendAsync
    (
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        if (_authService.Expiration < DateTime.UtcNow)
        {
            await _authService.RefreshAsync();
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authService.IdToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
