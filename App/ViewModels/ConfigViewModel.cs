#pragma warning disable MVVMTK0045

using System.Net.Mail;
using App.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public partial class ConfigViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _canSave;

    [ObservableProperty]
    private string? _email;

    [ObservableProperty]
    private string? _password;

    private const string PreferencesEmail = "Preferences_Email";
    private const string PreferencesIsConfigured = "Preferences_IsConfigured";
    private const string PreferencesPassword = "Preferences_Password";

    public async Task InitializeAsync()
    {
        Email = Preferences.Get(PreferencesEmail, string.Empty);
        Password = await SecureStorage.GetAsync(PreferencesPassword) ?? string.Empty;
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
        Preferences.Set(PreferencesIsConfigured, true);
        Preferences.Set(PreferencesEmail, Email);

        await SecureStorage.SetAsync(PreferencesPassword, Password ?? string.Empty);

        await Shell.Current.GoToAsync($"//{nameof(CrownEggPage)}");
    }

    private bool ValidateInputs()
    {
        bool isValidEmail = MailAddress.TryCreate(Email, out _);
        bool isValidPassword = Password?.Length >= 5;

        return isValidEmail && isValidPassword;
    }
}
