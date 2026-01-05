using App.ViewModels;

namespace App.Pages;

public partial class CrownEggPage : ContentPage
{
    public CrownEggPage(CrownEggViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
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
}
