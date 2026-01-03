namespace App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(Pages.ConfigPage), typeof(Pages.ConfigPage));
    }
}
