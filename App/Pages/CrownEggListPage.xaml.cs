using App.ViewModels;

namespace App.Pages;

public partial class CrownEggListPage : ContentPage
{
    public CrownEggListPage(CrownEggListViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        Dispatcher.Dispatch(async () =>
        {
            if (BindingContext is CrownEggListViewModel viewModel)
            {
                await viewModel.InitializeAsync();
            }
        });

        base.OnAppearing();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        Dispatcher.Dispatch(async () =>
        {
            if (BindingContext is CrownEggListViewModel viewModel)
            {
                await viewModel.NavigatedAsync();
            }
        });

        base.OnNavigatedTo(args);
    }
}
