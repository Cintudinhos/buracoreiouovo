#pragma warning disable MVVMTK0045

using System.Collections.ObjectModel;
using App.Core.Models;
using App.Infrastructure.Repositories;
using App.Pages;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public record class CrownEggListItem
(
    Color BackgroundColor,
    DateTime Timestamp,
    string Id,
    string Image,
    string PlayerName
);

public partial class CrownEggListViewModel(ICrownEggService crownEggService,
                                           IPreferencesRepository preferencesRepository)
    : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<CrownEggListItem> _crownEggsList = [];

    [ObservableProperty]
    private ObservableCollection<int> _years = [];

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private int _selectedYear;

    private readonly ICrownEggService _crownEggService = crownEggService;
    private readonly IPreferencesRepository _preferencesRepository = preferencesRepository;

    private bool _isInitialized;

    public async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            Years = [2026];
            SelectedYear = DateTime.Now.Year;
        }

        _isInitialized = true;
    }

    public async Task NavigatedAsync()
    {
        await PopulateCrownEggsListAsync();
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

    [RelayCommand]
    private async Task DoubleTap(string id)
    {
        CrownEgg crownEgg = _crownEggService.GetCrownEgg(id);

        Dictionary<string, object> parameters = new()
        {
            { "CrownEgg", crownEgg },
            { "IsUpdate", true },
        };

        await Shell.Current.GoToAsync(nameof(AddUpdateCrownEggPage), parameters);
    }

    partial void OnSelectedYearChanged(int value)
    {
        _ = PopulateCrownEggsListAsync();
    }

    private async Task PopulateCrownEggsListAsync()
    {
        CrownEggsList.Clear();

        AppPreferences appPreferences = await _preferencesRepository.GetPreferencesAsync();

        CrownEgg[] crownEggs =
            await _crownEggService.GetCrownEggEntriesAsync(SelectedYear, appPreferences.CollectionId ?? "DEFAULT");

        Color _backgroundColor1 = Color.FromArgb("fffefefe");
        Color _backgroundColor2 = Color.FromArgb("ffefefef");

        int crownEggBackgroundColor = 0;

        foreach (CrownEgg crownEgg in crownEggs.OrderByDescending(crownEgg => crownEgg.Timestamp))
        {
            string image = crownEgg.Type.ToString().ToLower();

            CrownEggListItem crownEggListItem = new
            (
                BackgroundColor: crownEggBackgroundColor++ % 2 == 0 ? _backgroundColor1 : _backgroundColor2,
                Id: crownEgg.Id ?? string.Empty,
                Image: image,
                PlayerName: crownEgg.PlayerName,
                Timestamp: crownEgg.Timestamp
            );

            CrownEggsList.Add(crownEggListItem);
        }
    }

    [RelayCommand]
    private async Task Refresh()
    {
        AppPreferences appPreferences = await _preferencesRepository.GetPreferencesAsync();

        await _crownEggService.RefreshCrownEggEntriesAsync(SelectedYear, appPreferences.CollectionId ?? "DEFAULT");

        await PopulateCrownEggsListAsync();

        IsRefreshing = false;
    }
}
