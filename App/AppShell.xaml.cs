using App.Pages;

namespace App;

public partial class AppShell : Shell
{
    private const string PreferencesIsConfigured = "Preferences_IsConfigured";

    public AppShell()
    {
        InitializeComponent();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Dispatcher.Dispatch(async () =>
        {
            bool isConfigured = Preferences.Get(PreferencesIsConfigured, false);

            if (!isConfigured)
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
