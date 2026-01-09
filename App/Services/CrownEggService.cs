using App.Core.Enums;
using App.Core.Models;
using App.Core.Models.Clients;
using App.Infrastructure.Clients;

namespace App.Services;

public interface ICrownEggService
{
    Task<CrownEgg[]> GetCrownEggEntriesAsync(int year);
    Task<CrownEgg[]> RefreshCrownEggEntriesAsync(int year);
}

public class CrownEggService(IFirestoreClient firestoreClient)
    : ICrownEggService
{
    private readonly IFirestoreClient _firestoreClient = firestoreClient;

    private CrownEgg[]? _crownEggs;
    private int _year;

    public async Task<CrownEgg[]> GetCrownEggEntriesAsync(int year)
    {
        if (_crownEggs is not null && _year == year)
        {
            return _crownEggs;
        }

        await UpdateCrownEggsAsync(year);

        return _crownEggs ?? [];
    }

    public async Task<CrownEgg[]> RefreshCrownEggEntriesAsync(int year)
    {
        await UpdateCrownEggsAsync(year);

        return _crownEggs ?? [];
    }

    private async Task UpdateCrownEggsAsync(int year)
    {
        FirestoreCrownEgg[] firestoreCrownEggs = await _firestoreClient.GetCrownEggEntriesAsync(year);

        _crownEggs =
        [
            ..
            firestoreCrownEggs.Select(firestoreCrownEgg => new CrownEgg
            (
                PlayerName: firestoreCrownEgg.Document.Fields.PlayerName.StringValue,
                Timestamp: firestoreCrownEgg.Document.Fields.Timestamp.TimestampValue,
                Type: Enum.Parse<CrownOrEgg>(firestoreCrownEgg.Document.Fields.Type.StringValue, true)
            )),
        ];

        _year = year;
    }
}
