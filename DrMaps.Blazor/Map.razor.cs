using System.Drawing;

namespace DrMaps.Blazor
{
    public partial class Map : IAsyncDisposable
    {
        #region inyeccion de servicios
        [Inject] MapService LeafletService { get; set; }
        #endregion

        #region Parametros
        [Parameter] public LatLong OriginalPoint { get; set; } = new LatLong(15.192939, 120.586715);
        [Parameter] public byte ZoomLevel { get; set; } = 17;
        [Parameter] public EventCallback<Map> OnMapCreatedAsync { get; set; }
        #endregion

        #region variables
        string MapId = $"map-{Guid.NewGuid()}";
        bool IsMapReady = false;
        #endregion

        #region Overrides
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                await CreateMap(OriginalPoint, ZoomLevel);
            }
        }
        #endregion

        #region IAsyncDisposable        
        public async ValueTask DisposeAsync()
        {
            await LeafletService.InvokeVoidAsync("deleteMap", MapId);
        }
        #endregion

        #region Methods publicos          

        public Task<int> AddMarkerAsync(LatLong point, string title, string description) =>
            LeafletService.InvokeAsyc<int>("addMarker", MapId, point, title, description);

        public Task RemoveMarkersAsync() =>
            LeafletService.InvokeVoidAsync("removeMarkers", MapId);  

        public Task DrawCircleAsync(LatLong point, string color, string fillColor, double fillOpacity, double radius) =>
            LeafletService.InvokeVoidAsync("drawCircle", MapId, point, color, fillColor, fillOpacity, radius); 

        public Task SetViewAsync(LatLong point, byte zoomLevel = 17) =>
            LeafletService.InvokeVoidAsync("setView", MapId, point, zoomLevel);

        public async Task CreateMap(LatLong point, byte zoomLevel = 17)
        {
            try
            {                                              
                await LeafletService.InvokeVoidAsync("createMap", MapId, point, zoomLevel);
                if(OnMapCreatedAsync.HasDelegate)
                    await OnMapCreatedAsync.InvokeAsync(this);
            }
            catch(Exception ex)
            {
                await Console.Out.WriteAsync(ex.ToString());
            }
            IsMapReady = true;
            await InvokeAsync(StateHasChanged);
        }

        public  Task DeleteMap()  =>
            LeafletService.InvokeVoidAsync("deleteMap", MapId);

        public async Task<IEnumerable<AddressGeocoding>> GetAddress(Address geocoding)
        {
            return await LeafletService.InvokeAsyc<IEnumerable<AddressGeocoding>>("geoCoder.getFromAddress",
                geocoding.Street.ReplaceSpaceWithPlus(),
                geocoding.City.ReplaceSpaceWithPlus(),
                geocoding.State.ReplaceSpaceWithPlus(),
                geocoding.Postalcode.ReplaceSpaceWithPlus(),
                geocoding.Country.ReplaceSpaceWithPlus());
        }

        #endregion
    }
}