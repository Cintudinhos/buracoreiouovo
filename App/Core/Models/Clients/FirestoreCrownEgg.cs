using System.Text.Json.Serialization;

namespace App.Core.Models.Clients;

public class FirestoreCrownEgg
{
    public CrownEggDocument? Document { get; set; }
}

public class CrownEggDocument
{
    public required CrownEggDocumentFields Fields { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public string Id => Name[(Name.LastIndexOf('/') + 1)..];

    public CrownEggDocument()
    {
        Name = string.Empty;
    }
}

public class CrownEggDocumentFields
{
    public required FirestoreInteger Year { get; set; }

    [JsonPropertyName("player-name")]
    public required FirestoreString PlayerName { get; set; }

    public required FirestoreString Type { get; set; }

    public required FirestoreTimestamp Timestamp { get; set; }
}

public class FirestoreInteger
{
    public int IntegerValue { get; set; }
}

public class FirestoreString
{
    public required string StringValue { get; set; }
}

public class FirestoreTimestamp
{
    public DateTime TimestampValue { get; set; }
}
