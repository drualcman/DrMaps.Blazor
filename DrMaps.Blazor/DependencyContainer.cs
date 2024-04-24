namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddMapsService(this IServiceCollection services)
    {
        services.AddScoped<MapService>();
        return services;
    }
    public static IServiceCollection AddGeoService(this IServiceCollection services)
    {
        services.AddScoped<IGeolocationService, GeolocationService>();
        return services;
    }
}