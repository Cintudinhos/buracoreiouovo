#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

using App.ViewModels;

namespace App.Core.Extensions;

internal static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAppServices()
        {
            services.AddSingleton<MainViewModel>();

            services.AddTransient<ConfigViewModel>();

            return services;
        }
    }
}
