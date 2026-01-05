using App.Core.Enums;

namespace App.Core.Models;

public record class CrownEggEntry
(
    DateTime Timestamp,
    CrownOrEgg Type,
    string PlayerName
);
