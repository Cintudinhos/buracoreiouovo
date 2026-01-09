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

public record class CrownEggItem
(
    Color BackgroundColor,
    int Quantity,
    string PlayerName
);

public partial class CrownEggViewModel(IAuthService authService,
                                       ICrownEggService crownEggService,
                                       IPreferencesRepository preferencesRepository,
                                       IStateRepository stateRepository)
    : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<CrownEggItem> _crowns = [];

    [ObservableProperty]
    private ObservableCollection<CrownEggItem> _eggs = [];

    [ObservableProperty]
    private ObservableCollection<int> _years = [];

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private int _selectedYear;

    public event Action<string>? ErrorOccurred;

    private readonly IAuthService _authService = authService;
    private readonly ICrownEggService _crownEggService = crownEggService;
    private readonly IPreferencesRepository _preferencesRepository = preferencesRepository;
    private readonly IStateRepository _stateRepository = stateRepository;

    private bool _isInitialized;

    public async Task InitializeAsync()
    {
        AppState appState = _stateRepository.GetState();

        if (!appState.IsConfigured)
        {
            return;
        }

        if (!_isInitialized)
        {
            bool isAuthServiceInitialized = await _authService.InitializeAsync();

            if (!isAuthServiceInitialized)
            {
                AppPreferences appPreferences = await _preferencesRepository.GetPreferencesAsync();
                string email = appPreferences.Email ?? string.Empty;
                string password = appPreferences.Password ?? string.Empty;

                bool loginSuccess = await _authService.LoginAsync(email, password);

                if (!loginSuccess)
                {
                    ErrorOccurred?.Invoke("Não foi possível fazer login");

                    return;
                }
            }

            Years = [2026];
            SelectedYear = DateTime.Now.Year;
        }

        _isInitialized = true;
    }

    public async Task NavigatedAsync()
    {
        await PopulateCrownsAndEggsAsync();
    }

    [RelayCommand]
    private async Task GoToAddCrownEggPage()
    {
        await Shell.Current.GoToAsync(nameof(AddUpdateCrownEggPage));
    }

    [RelayCommand]
    private async Task GoToConfigPage()
    {
        await Shell.Current.GoToAsync($"//{nameof(ConfigPage)}");
    }

    partial void OnSelectedYearChanged(int value)
    {
        _ = PopulateCrownsAndEggsAsync();
    }

    private async Task PopulateCrownsAndEggsAsync()
    {
        Crowns.Clear();
        Eggs.Clear();

        AppPreferences appPreferences = await _preferencesRepository.GetPreferencesAsync();

        CrownEgg[] crownEggs =
            await _crownEggService.GetCrownEggEntriesAsync(SelectedYear, appPreferences.CollectionId ?? "DEFAULT");

        var groupedCrownEggs = crownEggs
            .GroupBy(static crownEgg => new
            {
                crownEgg.PlayerName,
                crownEgg.Type,
            })
            .Select(groupedCrownEgg => new
            {
                groupedCrownEgg.Key.PlayerName,
                groupedCrownEgg.Key.Type,
                Quantity = groupedCrownEgg.Count(),
            })
            .OrderByDescending(groupedCrownEgg => groupedCrownEgg.Quantity);

        Color _backgroundColor1 = Color.FromArgb("fffefefe");
        Color _backgroundColor2 = Color.FromArgb("ffefefef");

        int crownBackgroundColor = 0;
        int eggBackgroundColor = 0;

        foreach (var groupedCrownEgg in groupedCrownEggs)
        {
            switch (groupedCrownEgg.Type)
            {
                case CrownOrEgg.Crown:
                    CrownEggItem crownItem = new
                    (
                        BackgroundColor: crownBackgroundColor++ % 2 == 0 ? _backgroundColor1 : _backgroundColor2,
                        PlayerName: groupedCrownEgg.PlayerName,
                        Quantity: groupedCrownEgg.Quantity
                    );

                    Crowns.Add(crownItem);
                    break;

                case CrownOrEgg.Egg:
                    CrownEggItem eggItem = new
                    (
                        BackgroundColor: eggBackgroundColor++ % 2 == 0 ? _backgroundColor1 : _backgroundColor2,
                        PlayerName: groupedCrownEgg.PlayerName,
                        Quantity: groupedCrownEgg.Quantity
                    );

                    Eggs.Add(eggItem);
                    break;
            }
        }
    }

    [RelayCommand]
    private async Task Refresh()
    {
        AppPreferences appPreferences = await _preferencesRepository.GetPreferencesAsync();

        await _crownEggService.RefreshCrownEggEntriesAsync(SelectedYear, appPreferences.CollectionId ?? "DEFAULT");

        await PopulateCrownsAndEggsAsync();

        IsRefreshing = false;
    }
}
