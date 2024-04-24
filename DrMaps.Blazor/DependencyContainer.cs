﻿namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddMapsService(this IServiceCollection services)
    {
        services.AddScoped<GeolocationService>();
        services.AddScoped<MapService>();
        return services;
    }
}