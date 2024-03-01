using Microsoft.Extensions.DependencyInjection;

namespace EmergencyServicesWorldwideBot.Interaction.OSM.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocationService(this IServiceCollection services)
    {
        services.AddScoped<ILocationService, OsmLocationService>()
            .AddHttpClient<OsmLocationService>();

        return services;
    }
}