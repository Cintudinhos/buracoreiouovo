using App.ViewModels;

namespace App.Pages;

public partial class MatchesListPage : ContentPage
{
    public MatchesListPage(MatchesListViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        Dispatcher.Dispatch(async () =>
        {
            if (BindingContext is MatchesListViewModel viewModel)
            {
                await viewModel.InitializeAsync();
            }
        });

        base.OnAppearing();
    }
}
