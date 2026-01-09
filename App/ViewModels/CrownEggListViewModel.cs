#pragma warning disable MVVMTK0045

using System.Collections.ObjectModel;
using App.Core.Models;
using App.Pages;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public record class CrownEggListItem
(
    Color BackgroundColor,
    DateTime Timestamp,
    string Image,
    string PlayerName
);

public partial class CrownEggListViewModel(ICrownEggService crownEggService)
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

    [RelayCommand]
    private async Task GoToConfigPage()
    {
        await Shell.Current.GoToAsync($"//{nameof(ConfigPage)}");
    }

    partial void OnSelectedYearChanged(int value)
    {
        _ = PopulateCrownEggsListAsync();
    }

    private async Task PopulateCrownEggsListAsync()
    {
        CrownEggsList.Clear();

        CrownEgg[] crownEggs = await _crownEggService.GetCrownEggEntriesAsync(SelectedYear);

        Color _backgroundColor1 = Color.FromArgb("fffefefe");
        Color _backgroundColor2 = Color.FromArgb("ffefefef");

        int crownEggBackgroundColor = 0;

        foreach (CrownEgg crownEgg in crownEggs.OrderByDescending(crownEgg => crownEgg.Timestamp))
        {
            string image = crownEgg.Type.ToString().ToLower();

            CrownEggListItem crownEggListItem = new
            (
                BackgroundColor: crownEggBackgroundColor++ % 2 == 0 ? _backgroundColor1 : _backgroundColor2,
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
        await _crownEggService.RefreshCrownEggEntriesAsync(SelectedYear);

        await PopulateCrownEggsListAsync();

        IsRefreshing = false;
    }
}
