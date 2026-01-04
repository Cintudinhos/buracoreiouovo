using App.ViewModels;

namespace App.Pages;

public partial class ConfigPage : ContentPage
{
    private readonly ConfigViewModel _configViewModel;

    public ConfigPage(ConfigViewModel configViewModel)
    {
        _configViewModel = configViewModel;

        InitializeComponent();

        BindingContext = configViewModel;
    }

    protected override async void OnAppearing()
    {
        await _configViewModel.InitializeAsync();
    }
}
