using System.Text.Json;
using App.Core.Models;

namespace App.Infrastructure.Repositories;

public interface IPreferencesRepository
{
    Task<AppPreferences> GetPreferencesAsync();
    Task SavePreferencesAsync(AppPreferences appPreferences);
}

public class PreferencesRepository : IPreferencesRepository
{
    private const string PreferencesKey = "Preferences";

    private AppPreferences? _appPreferences;

    public async Task<AppPreferences> GetPreferencesAsync()
    {
        string appPreferencesJson = await SecureStorage.GetAsync(PreferencesKey) ?? "{}";
        _appPreferences ??= JsonSerializer.Deserialize<AppPreferences>(appPreferencesJson);

        return _appPreferences ?? new AppPreferences();
    }

    public async Task SavePreferencesAsync(AppPreferences appPreferences)
    {
        _appPreferences = appPreferences;

        string appPreferencesJson = JsonSerializer.Serialize(appPreferences);
        await SecureStorage.SetAsync(PreferencesKey, appPreferencesJson);
    }
}
