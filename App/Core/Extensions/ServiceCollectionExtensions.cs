#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

using App.Core.Handlers;
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
            services.AddHttpClient<IAuthClient, AuthClient>(client =>
            {
                client.BaseAddress = new Uri(configuration.BaseAddress.Auth);
            });

            services.AddHttpClient<ILoginClient, LoginClient>(client =>
            {
                client.BaseAddress = new Uri(configuration.BaseAddress.Login);
            });

            IHttpClientBuilder firestoreClientBuilder = services.AddHttpClient<IFirestoreClient, FirestoreClient>(client =>
            {
                client.BaseAddress = new Uri(configuration.BaseAddress.Firestore);
            });

            firestoreClientBuilder.AddHttpMessageHandler<AuthenticationHandler>();

            return services;
        }

        public IServiceCollection AddAppServices()
        {
            // handlers
            services.AddSingleton<AuthenticationHandler>();

            // pages
            services.AddTransient<AddUpdateCrownEggPage>();
            services.AddTransient<AppShell>();
            services.AddTransient<ConfigPage>();
            services.AddTransient<CrownEggPage>();
            services.AddTransient<CrownEggListPage>();
            services.AddTransient<MatchesListPage>();

            // repositories
            services.AddSingleton<IPreferencesRepository, PreferencesRepository>();
            services.AddSingleton<IStateRepository, StateRepository>();

            // services
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<ICrownEggService, CrownEggService>();

            // viewmodels
            services.AddTransient<AddUpdateCrownEggViewModel>();
            services.AddTransient<ConfigViewModel>();
            services.AddTransient<CrownEggViewModel>();
            services.AddTransient<CrownEggListViewModel>();
            services.AddTransient<MatchesListViewModel>();

            return services;
        }
    }
}
