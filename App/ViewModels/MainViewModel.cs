using App.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [RelayCommand]
    private async Task GoToConfigPage()
    {
        await Shell.Current.GoToAsync(nameof(ConfigPage));
    }
}
