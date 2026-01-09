#pragma warning disable MVVMTK0045

using System.Collections.ObjectModel;
using App.Core.Enums;
using App.Core.Models;
using App.Infrastructure.Repositories;
using App.Pages;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public partial class AddUpdateCrownEggViewModel(ICrownEggService crownEggService,
                                                IPreferencesRepository preferencesRepository)
    : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    private ObservableCollection<string> _crownOrEggs = [];

    [ObservableProperty]
    private string? _selectedCrownOrEgg;

    [ObservableProperty]
    private string? _addOrUpdate;

    [ObservableProperty]
    private string? _playerName;

    private readonly ICrownEggService _crownEggService = crownEggService;
    private readonly IPreferencesRepository _preferencesRepository = preferencesRepository;

    private CrownEgg? _crownEgg;
    private bool _isInitialized;
    private bool _isUpdate;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _isUpdate = query.TryGetValue("isUpdate", out object? isUpdateValue) && (bool)isUpdateValue;

        if (_isUpdate)
        {
            _crownEgg = query["CrownEgg"] as CrownEgg;

            AddOrUpdate = "Atualizar Rei ou Ovo";
        }
        else
        {
            AddOrUpdate = "Adicionar Rei ou Ovo";
        }
    }

    public async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            foreach (CrownOrEgg crownOrEgg in Enum.GetValues<CrownOrEgg>())
            {
                string crownOrEggString = GetCrownOrEggString(crownOrEgg);

                CrownOrEggs.Add(crownOrEggString);
            }

            SelectedCrownOrEgg = GetCrownOrEggString(CrownOrEgg.Crown);
        }

        _isInitialized = true;
    }

    [RelayCommand]
    private async Task SaveCrownOrEgg()
    {
        AppPreferences appPreferences = await _preferencesRepository.GetPreferencesAsync();

        CrownEgg crownEgg = _isUpdate
            ? new
            (
                Id: _crownEgg?.Id,
                PlayerName: PlayerName ?? "NO NAME",
                Timestamp: _crownEgg?.Timestamp ?? DateTime.UtcNow,
                Type: GetCrownOrEggEnum(SelectedCrownOrEgg ?? "Rei")
            )
            : new
            (
                Id: null,
                PlayerName: PlayerName ?? "NO NAME",
                Timestamp: DateTime.UtcNow,
                Type: GetCrownOrEggEnum(SelectedCrownOrEgg ?? "Rei")
            );

        await _crownEggService.CreateCrownEggAsync(crownEgg, appPreferences?.CollectionId ?? "DEFAULT");

        await Shell.Current.GoToAsync("..");
    }

    private static string GetCrownOrEggString(CrownOrEgg crownOrEgg)
    {
        string crownOrEggString = crownOrEgg switch
        {
            CrownOrEgg.Crown => "Rei",
            CrownOrEgg.Egg => "Ovo",

            _ => throw new NotImplementedException($"CrownOrEgg '{crownOrEgg}' not implemented"),
        };

        return crownOrEggString;
    }

    private static CrownOrEgg GetCrownOrEggEnum(string crownOrEggString)
    {
        CrownOrEgg crownOrEgg = crownOrEggString switch
        {
            "Rei" => CrownOrEgg.Crown,
            "Ovo" => CrownOrEgg.Egg,

            _ => throw new NotImplementedException($"CrownOrEgg '{crownOrEggString}' not implemented"),
        };

        return crownOrEgg;
    }
}
