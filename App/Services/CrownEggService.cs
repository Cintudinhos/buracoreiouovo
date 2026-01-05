using App.Core.Enums;
using App.Core.Models;
using App.Infrastructure.Clients;

namespace App.Services;

public interface ICrownEggService
{
    Task<CrownEggEntry[]> GetCrownEggEntriesAsync();
}

public class CrownEggService(IAuthClient authClient)
    : ICrownEggService
{
    private readonly IAuthClient _authClient = authClient;

    public async Task<CrownEggEntry[]> GetCrownEggEntriesAsync()
    {
        // Simulate async data retrieval
        await Task.Delay(100);

        return
        [
            new CrownEggEntry(DateTime.Now.AddDays(-10), CrownOrEgg.Crown, "Player 1"),
            new CrownEggEntry(DateTime.Now.AddDays(-9), CrownOrEgg.Egg, "Player 2"),
            new CrownEggEntry(DateTime.Now.AddDays(-8), CrownOrEgg.Crown, "Player 3"),
            new CrownEggEntry(DateTime.Now.AddDays(-7), CrownOrEgg.Egg, "Player 4"),
            new CrownEggEntry(DateTime.Now.AddDays(-6), CrownOrEgg.Crown, "Player 5"),
        ];
    }
}
