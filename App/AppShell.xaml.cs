using App.Core.Models;
using App.Infrastructure.Repositories;
using App.Pages;

namespace App;

public partial class AppShell : Shell
{
    private readonly IPreferencesRepository _preferencesRepository;

    public AppShell(IPreferencesRepository preferencesRepository)
    {
        InitializeComponent();

        _preferencesRepository = preferencesRepository;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Dispatcher.Dispatch(async () =>
        {
            AppPreferences appPreferences = await _preferencesRepository.GetPreferencesAsync();

            if (!appPreferences.IsConfigured)
            {
                Page? crownEggPage = Application.Current?.Windows[0].Page;

                if (crownEggPage is not null)
                {
                    await crownEggPage.DisplayAlertAsync("Aplicativo não configurado",
                                                         "O aplicativo 'Buraco Rei ou Ovo' ainda não está corretamente configurado",
                                                         "Ir para configurações");
                }

                await GoToAsync($"//{nameof(ConfigPage)}");
            }
        });
    }
}
