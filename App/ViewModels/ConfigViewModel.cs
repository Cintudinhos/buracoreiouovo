#pragma warning disable MVVMTK0045

using System.Net.Mail;
using App.Core.Models;
using App.Infrastructure.Repositories;
using App.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public partial class ConfigViewModel(IPreferencesRepository preferencesRepository)
    : ObservableObject
{
    [ObservableProperty]
    private bool _canSave;

    [ObservableProperty]
    private string? _email;

    [ObservableProperty]
    private string? _password;

    private readonly IPreferencesRepository _preferencesRepository = preferencesRepository;

    public async Task InitializeAsync()
    {
        AppPreferences appPreferences = await _preferencesRepository.GetPreferencesAsync();

        Email = appPreferences.Email;
        Password = appPreferences.Password;
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
            Email = Email,
            IsConfigured = true,
            Password = Password,
        };

        await _preferencesRepository.SavePreferencesAsync(appPreferences);

        await Shell.Current.GoToAsync($"//{nameof(CrownEggPage)}");
    }

    private bool ValidateInputs()
    {
        bool isValidEmail = MailAddress.TryCreate(Email, out _);
        bool isValidPassword = Password?.Length >= 5;

        return isValidEmail && isValidPassword;
    }
}
