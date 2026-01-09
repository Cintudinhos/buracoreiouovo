using App.Core.Enums;

namespace App.Core.Models;

public record class CrownEgg
(
    DateTime Timestamp,
    CrownOrEgg Type,
    string PlayerName
);
