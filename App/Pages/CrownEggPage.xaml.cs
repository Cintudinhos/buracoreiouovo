using App.ViewModels;

namespace App.Pages;

public partial class CrownEggPage : ContentPage
{
    public CrownEggPage(CrownEggViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
        viewModel.ErrorOccurred += CrownEggViewModel_ErrorOccurred;
    }

    protected override void OnAppearing()
    {
        Dispatcher.Dispatch(async () =>
        {
            if (BindingContext is CrownEggViewModel viewModel)
            {
                await viewModel.InitializeAsync();
            }
        });

        base.OnAppearing();
    }

    private async void CrownEggViewModel_ErrorOccurred(string message)
    {
        await DisplayAlertAsync("Crown Egg Error",
                                message,
                                "Ok");
    }
}
