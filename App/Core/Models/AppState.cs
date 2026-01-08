namespace App.Core.Models;

public class AppState
{
    public bool IsConfigured { get; set; }
    public string? RefreshToken { get; set; }
}
