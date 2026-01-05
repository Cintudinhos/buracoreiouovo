#pragma warning disable MVVMTK0045

using System.Collections.ObjectModel;
using App.Pages;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public partial class CrownEggViewModel(ICrownEggService crownEggService)
    : ObservableObject
{
    public record class CrownEggItem
    (
        Color BackgroundColor,
        int Quantity,
        string PlayerName
    );

    [ObservableProperty]
    private ObservableCollection<CrownEggItem> _crowns = [];

    [ObservableProperty]
    private ObservableCollection<CrownEggItem> _eggs = [];

    [ObservableProperty]
    private ObservableCollection<int> _years = [];

    [ObservableProperty]
    private int _selectedYear;

    private readonly ICrownEggService _crownEggService = crownEggService;
    private bool _isInitialized;

    public async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            _crownEggService.GetCrownEggEntriesAsync();


            //CrownEggEntry[] crownEntries =
            //[
            //    new CrownEggEntry(CrownOrEgg.Crown, 3, "Player 1"),
            //    new CrownEggEntry(CrownOrEgg.Crown, 1, "Player 2"),
            //    new CrownEggEntry(CrownOrEgg.Crown, 2, "Player 3"),
            //    new CrownEggEntry(CrownOrEgg.Crown, 7, "Player 4"),
            //    new CrownEggEntry(CrownOrEgg.Crown, 2, "Player 5"),
            //    new CrownEggEntry(CrownOrEgg.Crown, 4, "Player 6"),
            //    new CrownEggEntry(CrownOrEgg.Crown, 5, "Player 7"),
            //];

            //foreach (CrownEggEntry? crownEntry in crownEntries.OrderByDescending(entry => entry.Quantity))
            //{
            //    Crowns.Add(crownEntry);
            //}

            //CrownEggEntry[] eggEntries =
            //[
            //    new CrownEggEntry(CrownOrEgg.Egg, 4, "Player 1"),
            //    new CrownEggEntry(CrownOrEgg.Egg, 5, "Player 2"),
            //    new CrownEggEntry(CrownOrEgg.Egg, 2, "Player 3"),
            //    new CrownEggEntry(CrownOrEgg.Egg, 1, "Player 4"),
            //    new CrownEggEntry(CrownOrEgg.Egg, 3, "Player 5"),
            //    new CrownEggEntry(CrownOrEgg.Egg, 6, "Player 6"),
            //    new CrownEggEntry(CrownOrEgg.Egg, 7, "Player 7"),
            //];

            //foreach (CrownEggEntry? eggEntry in eggEntries.OrderByDescending(entry => entry.Quantity))
            //{
            //    Eggs.Add(eggEntry);
            //}

            Years = [2026];
            SelectedYear = 2026;
        }

        _isInitialized = true;
    }

    [RelayCommand]
    private async Task GoToConfigPage()
    {
        await Shell.Current.GoToAsync($"//{nameof(ConfigPage)}");
    }
}
