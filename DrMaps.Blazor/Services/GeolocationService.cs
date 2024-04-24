namespace DrMaps.Blazor.Services;
public class GeolocationService : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> ModuleTask;

    public GeolocationService(IJSRuntime jsRuntime)
    {
        ModuleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", $"./{ContentHelper.ContentPath}/js/geolocation.js").AsTask());
    }

    public async ValueTask<LatLongPosition> GetPositionAsync()
    {
        var module = await ModuleTask.Value;
        LatLongPosition postition = default;
        try
        {
            postition = await module.InvokeAsync<LatLongPosition>("getPositionAsync");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetPosition: {ex.Message}");
        }
        return postition;
    }

    public async ValueTask<bool> IsGeoLocationGranted()
    {
        var module = await ModuleTask.Value;
        bool result;
        try
        {
            result = await module.InvokeAsync<bool>("checkGeolocationPermission");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"IsGeoLocationGranted: {ex.Message}");
            result = false;
        }
        return result;

    }

    public async ValueTask DisposeAsync()
    {
        if (ModuleTask.IsValueCreated)
        {
            var module = await ModuleTask.Value;
            await module.DisposeAsync();
        }
    }
}
