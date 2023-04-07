namespace DrMaps.Blazor;
public static class DependencyContainer
{
    public static IServiceCollection AddMapsService(this IServiceCollection services)
    {
        services.AddSingleton<LeafletService>();
        return services;
    }
}