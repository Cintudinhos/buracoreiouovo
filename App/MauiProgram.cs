using App.Core.Extensions;
using App.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .AddJsonConfiguration()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        Configuration configuration = builder.Configuration.Get<Configuration>()
            ?? throw new OptionsValidationException(nameof(Configuration),
                                                    typeof(Configuration),
                                                    ["Failed loading the configuration"]);

        builder.Services.AddAppClients(configuration);
        builder.Services.AddAppServices();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
