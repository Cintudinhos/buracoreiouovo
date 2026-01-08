using System.Text.Json;
using App.Core.Models;

namespace App.Infrastructure.Repositories;

public interface IStateRepository
{
    AppState GetState();
    void SaveState(AppState appState);
}

public class StateRepository : IStateRepository
{
    private const string StateKey = "State";

    private AppState? _appState;

    public AppState GetState()
    {
        string appStateJson = Preferences.Get(StateKey, "{}");
        _appState ??= JsonSerializer.Deserialize<AppState>(appStateJson);

        return _appState ?? new AppState();
    }

    public void SaveState(AppState appState)
    {
        _appState = appState;

        string appStateJson = JsonSerializer.Serialize(appState);
        Preferences.Set(StateKey, appStateJson);
    }
}
