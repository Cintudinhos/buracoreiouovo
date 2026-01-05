#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

using App.Core.Models;
using App.Infrastructure.Clients;
using App.Infrastructure.Repositories;
using App.Pages;
using App.Services;
using App.ViewModels;

namespace App.Core.Extensions;

internal static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAppClients(Configuration configuration)
        {
            IHttpClientBuilder authClientBuilder = services.AddHttpClient<IAuthClient, AuthClient>(client =>
            {
                client.BaseAddress = new Uri(configuration.BaseAddress.Auth);
            });

            return services;
        }

        public IServiceCollection AddAppServices()
        {
            // pages
            services.AddTransient<AppShell>();
            services.AddTransient<ConfigPage>();
            services.AddTransient<CrownEggPage>();
            services.AddTransient<CrownEggListPage>();
            services.AddTransient<MatchesListPage>();

            // repositories
            services.AddSingleton<IPreferencesRepository, PreferencesRepository>();

            // services
            services.AddTransient<ICrownEggService, CrownEggService>();

            // viewmodels
            services.AddTransient<ConfigViewModel>();
            services.AddTransient<CrownEggViewModel>();
            services.AddTransient<CrownEggListViewModel>();
            services.AddTransient<MatchesListViewModel>();

            return services;
        }
    }
}
