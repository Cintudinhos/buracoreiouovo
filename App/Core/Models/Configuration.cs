namespace App.Core.Models;

public class Configuration
{
    public required BaseAddressConfiguration BaseAddress { get; init; }
    public required string ApiKey { get; init; }
}

public class BaseAddressConfiguration
{
    public required string Auth { get; init; }
    public required string Login { get; init; }
    public required string Firestore { get; init; }
}
