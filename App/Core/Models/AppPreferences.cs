namespace App.Core.Models;

public class AppPreferences
{
    public bool IsConfigured { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
