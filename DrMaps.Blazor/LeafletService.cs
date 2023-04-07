using DrMaps.Blazor.Helpers;

namespace DrMaps.Blazor;
public sealed class LeafletService : IAsyncDisposable
{
    readonly Lazy<Task<IJSObjectReference>> ModuleTask;

    public LeafletService(IJSRuntime jsRuntime)
    {
        ModuleTask = new Lazy<Task<IJSObjectReference>>(() => GetJSObjectReference(jsRuntime));
    }

    private Task<IJSObjectReference> GetJSObjectReference(IJSRuntime jsRuntime) =>
        jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", $"./{ContentHelper.ContentPath}/js/leafletService.js").AsTask();

    public async ValueTask DisposeAsync()
    {
        if(ModuleTask.IsValueCreated)
        {
            IJSObjectReference module = await ModuleTask.Value;
            await module.DisposeAsync();
        }
    }

    internal async Task<T> InvokeAsyc<T>(string methodName, params object[] parameters)
    {
        IJSObjectReference module = await ModuleTask.Value;
        T result = default;
        try
        {
            if(parameters != null)
                result = await module.InvokeAsync<T>(methodName, parameters);
            else
            {
                result = await module.InvokeAsync<T>(methodName);
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"LeafletService: {ex}");
        }
        return result;
    }
    internal async Task InvokeVoidAsync(string methodName, params object[] parameters)
    {
        IJSObjectReference module = await ModuleTask.Value;
        try
        {
            if(parameters != null)
                await module.InvokeVoidAsync(methodName, parameters);
            else
            {
                await module.InvokeVoidAsync(methodName);
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"LeafletService: {ex}");
        }
    }
}
