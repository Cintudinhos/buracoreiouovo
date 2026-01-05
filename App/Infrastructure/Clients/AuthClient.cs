using App.Core.Models;
using Microsoft.Extensions.Options;

namespace App.Infrastructure.Clients;

public interface IAuthClient
{
}

public class AuthClient(IOptions<Configuration> options,
                        HttpClient httpClient)
    : IAuthClient
{
    private readonly Configuration _configuration = options.Value;
    private readonly HttpClient _httpClient = httpClient;
}
