using App.ViewModels;

namespace App.Pages;

public partial class CrownEggPage : ContentPage
{
    public CrownEggPage(CrownEggViewModel mainViewModel)
    {
        InitializeComponent();

        BindingContext = mainViewModel;
    }
}
