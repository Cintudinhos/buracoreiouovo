using App.Core.Enums;
using App.Core.Models;
using App.Core.Models.Clients;
using App.Infrastructure.Clients;

namespace App.Services;

public interface ICrownEggService
{
    Task CreateCrownEggAsync(CrownEgg crownEgg, string collectionId);
    CrownEgg GetCrownEgg(string id);
    Task<CrownEgg[]> GetCrownEggEntriesAsync(int year, string collectionId);
    Task<CrownEgg[]> RefreshCrownEggEntriesAsync(int year, string collectionId);
    Task UpdateCrownEggAsync(CrownEgg crownEgg, string collectionId);
}

public class CrownEggService(IFirestoreClient firestoreClient)
    : ICrownEggService
{
    private readonly IFirestoreClient _firestoreClient = firestoreClient;

    private CrownEgg[]? _crownEggs;
    private int _year;
    private string? _collectionId;

    public async Task CreateCrownEggAsync(CrownEgg crownEgg, string collectionId)
    {
        CrownEggDocument crownEggDocument = new()
        {
            Fields = new CrownEggDocumentFields
            {
                PlayerName = new FirestoreString { StringValue = crownEgg.PlayerName },
                Timestamp = new FirestoreTimestamp { TimestampValue = crownEgg.Timestamp },
                Type = new FirestoreString { StringValue = crownEgg.Type.ToString() },
                Year = new FirestoreInteger { IntegerValue = crownEgg.Timestamp.Year },
            },
        };

        bool isSuccess = await _firestoreClient.PostCrownEggEntryAsync(crownEggDocument, collectionId);

        if (isSuccess)
        {
            await UpdateCrownEggsAsync(_year, collectionId);
        }
    }

    public CrownEgg GetCrownEgg(string id)
    {
        CrownEgg crownEgg = _crownEggs!.First(crownEgg => crownEgg.Id == id);

        return crownEgg;
    }

    public async Task<CrownEgg[]> GetCrownEggEntriesAsync(int year, string collectionId)
    {
        if (_crownEggs is not null && _year == year && _collectionId == collectionId)
        {
            return _crownEggs;
        }

        await UpdateCrownEggsAsync(year, collectionId);

        return _crownEggs ?? [];
    }

    public async Task<CrownEgg[]> RefreshCrownEggEntriesAsync(int year, string collectionId)
    {
        await UpdateCrownEggsAsync(year, collectionId);

        return _crownEggs ?? [];
    }

    public async Task UpdateCrownEggAsync(CrownEgg crownEgg, string collectionId)
    {
        CrownEggDocument crownEggDocument = new()
        {
            Fields = new CrownEggDocumentFields
            {
                PlayerName = new FirestoreString { StringValue = crownEgg.PlayerName },
                Timestamp = new FirestoreTimestamp { TimestampValue = crownEgg.Timestamp },
                Type = new FirestoreString { StringValue = crownEgg.Type.ToString() },
                Year = new FirestoreInteger { IntegerValue = crownEgg.Timestamp.Year },
            },
        };

        bool isSuccess = await _firestoreClient.PatchCrownEggEntryAsync(crownEggDocument, collectionId, crownEgg.Id!);

        if (isSuccess)
        {
            await UpdateCrownEggsAsync(_year, collectionId);
        }
    }

    private async Task UpdateCrownEggsAsync(int year, string collectionId)
    {
        FirestoreCrownEgg[] firestoreCrownEggs = await _firestoreClient.GetCrownEggEntriesAsync(year, collectionId);

        _crownEggs =
        [
            ..
            firestoreCrownEggs
                .Where(firestoreCrownEgg => firestoreCrownEgg.Document is not null)
                .Select(firestoreCrownEgg => new CrownEgg
                (
                    Id: firestoreCrownEgg.Document!.Id,
                    PlayerName: firestoreCrownEgg.Document.Fields.PlayerName.StringValue,
                    Timestamp: firestoreCrownEgg.Document.Fields.Timestamp.TimestampValue,
                    Type: Enum.Parse<CrownOrEgg>(firestoreCrownEgg.Document.Fields.Type.StringValue, true)
                )),
        ];

        _year = year;
        _collectionId = collectionId;
    }
}
