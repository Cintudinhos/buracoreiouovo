using App.ViewModels;

namespace App.Pages;

public partial class MainPage : ContentPage
{
    private const string PreferencesIsConfigured = "Preferences_IsConfigured";

    public MainPage(MainViewModel mainViewModel)
    {
        InitializeComponent();

        BindingContext = mainViewModel;
    }

    protected override async void OnAppearing()
    {
        bool isConfigured = Preferences.Get(PreferencesIsConfigured, false);

        if (!isConfigured)
        {
            Page? mainPage = Application.Current?.Windows[0].Page;

            if (mainPage is not null)
            {
                await mainPage.DisplayAlertAsync("Aplicativo não configurado",
                                                 "O aplicativo 'Buraco Rei ou Ovo' ainda não está corretamente configurado",
                                                 "Ir para configurações");
            }

            Dictionary<string, object> parameters = new()
            {
                { nameof(ConfigViewModel.IsFirstConfig), true },
            };

            await Shell.Current.GoToAsync(nameof(ConfigPage), parameters);
        }
    }
}
