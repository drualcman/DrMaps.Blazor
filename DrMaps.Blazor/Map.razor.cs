namespace DrMaps.Blazor
{
    public partial class Map : IAsyncDisposable
    {
        #region inyeccion de servicios
        [Inject] LeafletService LeafletService { get; set; }
        #endregion

        #region Parametros
        [Parameter] public LatLong OriginalPoint { get; set; } = new LatLong(15.192939, 120.586715);
        [Parameter] public byte ZoomLevel { get; set; } = 17;
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
                try
                {
                    await CreateMap(OriginalPoint, ZoomLevel);
                    await ShowPoint(OriginalPoint, "Mi Casita", "La mas bonita");
                }
                catch(Exception ex)
                {
                    await Console.Out.WriteAsync(ex.ToString());
                }
                IsMapReady = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        public async ValueTask DisposeAsync()
        {
            await LeafletService.InvokeVoidAsync("deleteMap", MapId);
        }
        #endregion

        #region Methods      
        public async Task CreateMap(LatLong point, byte zoomLevel = 17)
        {
            await LeafletService.InvokeVoidAsync("createMap", MapId, point, zoomLevel);
        }

        public async Task DeleteMap()
        {
            await LeafletService.InvokeVoidAsync("deleteMap", MapId);
        }

        public async Task ShowPoint(LatLong point, string title, string description)
        {
            await LeafletService.InvokeVoidAsync("addMarker", MapId, point, title, description);
        }

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