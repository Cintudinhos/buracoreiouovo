#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

using System.Reflection;
using App.Core.Models;
using Microsoft.Extensions.Configuration;

namespace App.Core.Extensions;

public static class MauiAppBuilderExtensions
{
    extension(MauiAppBuilder builder)
    {
        public MauiAppBuilder AddJsonConfiguration()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using Stream? stream = assembly.GetManifestResourceStream("App.appsettings.json");

            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonStream(stream!)
                .Build();

            builder.Configuration.AddConfiguration(config);
            builder.Services.AddOptions<Configuration>().Bind(builder.Configuration);

            return builder;
        }
    }
}
