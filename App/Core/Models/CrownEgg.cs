using App.Core.Enums;

namespace App.Core.Models;

public record class CrownEgg
(
    string? Id,
    DateTime Timestamp,
    CrownOrEgg Type,
    string PlayerName
);
