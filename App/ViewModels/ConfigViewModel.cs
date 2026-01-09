#pragma warning disable MVVMTK0045

using System.Net.Mail;
using App.Core.Models;
using App.Infrastructure.Repositories;
using App.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public partial class ConfigViewModel(IPreferencesRepository preferencesRepository,
                                     IStateRepository stateRepository)
    : ObservableObject
{
    [ObservableProperty]
    private bool _canSave;

    [ObservableProperty]
    private string? _collectionId;

    [ObservableProperty]
    private string? _email;

    [ObservableProperty]
    private string? _password;

    private readonly IPreferencesRepository _preferencesRepository = preferencesRepository;
    private readonly IStateRepository _stateRepository = stateRepository;

    public async Task InitializeAsync()
    {
        AppPreferences appPreferences = await _preferencesRepository.GetPreferencesAsync();

        CollectionId = appPreferences.CollectionId;
        Email = appPreferences.Email;
        Password = appPreferences.Password;
    }

    partial void OnCollectionIdChanged(string? value)
    {
        CanSave = ValidateInputs();
    }

    partial void OnEmailChanged(string? value)
    {
        CanSave = ValidateInputs();
    }

    partial void OnPasswordChanged(string? value)
    {
        CanSave = ValidateInputs();
    }

    [RelayCommand]
    private async Task SaveConfig()
    {
        AppPreferences appPreferences = new()
        {
            CollectionId = CollectionId,
            Email = Email,
            Password = Password,
        };

        await _preferencesRepository.SavePreferencesAsync(appPreferences);

        AppState appState = new()
        {
            IsConfigured = true,
        };

        _stateRepository.SaveState(appState);

        await Shell.Current.GoToAsync($"//{nameof(CrownEggPage)}");
    }

    private bool ValidateInputs()
    {
        bool isValidCollectionId = CollectionId?.Length >= 3 && !CollectionId.Contains(' ');
        bool isValidEmail = MailAddress.TryCreate(Email, out _);
        bool isValidPassword = Password?.Length >= 5;

        return isValidCollectionId && isValidEmail && isValidPassword;
    }
}
