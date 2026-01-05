using App.ViewModels;

namespace App.Pages;

public partial class ConfigPage : ContentPage
{
    public ConfigPage(ConfigViewModel configViewModel)
    {
        InitializeComponent();

        BindingContext = configViewModel;
    }

    protected override void OnAppearing()
    {
        Dispatcher.Dispatch(async () =>
        {
            if (BindingContext is ConfigViewModel viewModel)
            {
                await viewModel.InitializeAsync();
            }
        });

        base.OnAppearing();
    }
}
