using App.ViewModels;

namespace App.Pages;

public partial class AddUpdateCrownEggPage : ContentPage
{
    public AddUpdateCrownEggPage(AddUpdateCrownEggViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        Dispatcher.Dispatch(async () =>
        {
            if (BindingContext is AddUpdateCrownEggViewModel viewModel)
            {
                await viewModel.InitializeAsync();
            }
        });

        base.OnAppearing();
    }
}
