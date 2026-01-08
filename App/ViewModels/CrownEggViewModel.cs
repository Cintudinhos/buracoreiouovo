#pragma warning disable MVVMTK0045

using System.Collections.ObjectModel;
using App.Core.Models;
using App.Infrastructure.Repositories;
using App.Pages;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public partial class CrownEggViewModel(IAuthService authService,
                                       ICrownEggService crownEggService,
                                       IPreferencesRepository preferencesRepository)
    : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<CrownEggItem> _crowns = [];

    [ObservableProperty]
    private ObservableCollection<CrownEggItem> _eggs = [];

    [ObservableProperty]
    private ObservableCollection<int> _years = [];

    [ObservableProperty]
    private int _selectedYear;

    public record class CrownEggItem
    (
        Color BackgroundColor,
        int Quantity,
        string PlayerName
    );

    public event Action<string>? ErrorOccurred;

    private readonly IAuthService _authService = authService;
    private readonly ICrownEggService _crownEggService = crownEggService;
    private readonly IPreferencesRepository _preferencesRepository = preferencesRepository;

    private bool _isInitialized;

    public async Task InitializeAsync()
    {
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
