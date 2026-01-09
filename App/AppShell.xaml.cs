using App.Core.Models;
using App.Infrastructure.Repositories;
using App.Pages;

namespace App;

public partial class AppShell : Shell
{
    private readonly IStateRepository _stateRepository;

    public AppShell(IStateRepository stateRepository)
    {
        InitializeComponent();

        _stateRepository = stateRepository;

        Routing.RegisterRoute(nameof(AddUpdateCrownEggPage), typeof(AddUpdateCrownEggPage));
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Dispatcher.Dispatch(async () =>
        {
            AppState appState = _stateRepository.GetState();

            if (!appState.IsConfigured)
            {
                Page? crownEggPage = Application.Current?.Windows[0].Page;

                if (crownEggPage is not null)
                {
                    await crownEggPage.DisplayAlertAsync("Aplicativo não configurado",
                                                         "O aplicativo 'Buraco: Rei ou Ovo' ainda não está corretamente configurado",
                                                         "Ir para configurações");
                }

                await GoToAsync($"//{nameof(ConfigPage)}");
            }
        });
    }
}
