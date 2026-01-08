using App.Core.Models;
using App.Infrastructure.Clients;

namespace App.Services;

public interface ICrownEggService
{
    Task<CrownEggEntry[]> GetCrownEggEntriesAsync(int year);
}

public class CrownEggService(IFirestoreClient firestoreClient)
    : ICrownEggService
{
    private readonly IFirestoreClient _firestoreClient = firestoreClient;

    public async Task<CrownEggEntry[]> GetCrownEggEntriesAsync(int year)
    {


        return [];
    }
}
