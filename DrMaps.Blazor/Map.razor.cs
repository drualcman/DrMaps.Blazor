namespace DrMaps.Blazor
{
    public partial class Map : IAsyncDisposable
    {
        #region inyeccion de servicios
        [Inject] MapService LeafletService { get; set; }
        #endregion

        #region Parametros
        [Parameter] public LatLong OriginalPoint { get; set; } = new LatLong(15.192939, 120.586715);
        [Parameter] public byte ZoomLevel { get; set; } = 19;
        [Parameter] public EventCallback<Map> OnMapCreatedAsync { get; set; }
        [Parameter] public EventCallback<LatLong> OnDragAsync { get; set; }
        #endregion

        #region variables
        string MapId = $"map-{Guid.NewGuid()}";
        bool IsMapReady = false;
        #endregion

        #region Overrides
        protected override void OnInitialized()
        {
            ObjRef = DotNetObjectReference.Create(this);
        }
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
            ObjRef.Dispose();
        }
        #endregion

        #region Methods publicos  
        public async Task CreateMap(LatLong point, byte zoomLevel = 19)
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
        public Task SetViewAsync(LatLong point, byte zoomLevel = 19) =>
            LeafletService.InvokeVoidAsync("setView", MapId, point, zoomLevel);


        public Task<int> AddMarkerAsync(LatLong point, string title, string description, string iconUrl, bool dragdable)
        {
            return LeafletService.InvokeAsyc<int>("addMarker", MapId, point, title, description, iconUrl, dragdable, ObjRef);
        }

        public Task<int> AddMarkerAsync(LatLong point, string title, string description, Icon icon, bool dragdable = false) =>
            AddMarkerAsync(point, title, description, GetIconUrl(icon), dragdable);

        public Task<int> AddMarkerAsync(LatLong point, string title, string description, Icon icon = Icon.PIN) =>
            AddMarkerAsync(point, title, description, GetIconUrl(icon), false);

        public Task<int> AddMarkerAsync(LatLong point, string title, string description, bool dragdable) =>
            AddMarkerAsync(point, title, description, "marker-icon", dragdable);

        private string GetIconUrl(Icon icon)
        {
            string useIcon = icon switch
            {
                Icon.DRON => "drone",
                Icon.DESTINATION => "destination",
                _ => "marker-icon"
            };
            return $"./{ContentHelper.ContentPath}/css/images/{useIcon}.png";
        }

        public Task RemoveMarkersAsync() =>
            LeafletService.InvokeVoidAsync("removeMarkers", MapId);

        public Task DrawCircleAsync(LatLong point, string color, string fillColor, double fillOpacity, double radius) =>
            LeafletService.InvokeVoidAsync("drawCircle", MapId, point, color, fillColor, fillOpacity, radius);

        public Task DeleteMap() =>
            LeafletService.InvokeVoidAsync("deleteMap", MapId);

        public Task MoveMarketAsync(int markerId, LatLong newPosition) =>
            LeafletService.InvokeVoidAsync("moveMarker", MapId, markerId, newPosition);

        public async Task<IEnumerable<PlaceGeocoding>> GetAddress(Address address) =>
            await LeafletService.GetGeocodings(address);

        public double GetDistanceInMettersBetween(LatLong origin, LatLong destination)
        {
            CoordinatesCalculatesHelper calculates = new CoordinatesCalculatesHelper();
            return calculates.CalculateDistanceInMetters(origin, destination);
        }
        #endregion

        #region Javascript events
        DotNetObjectReference<Map> ObjRef;
        [JSInvokable]
        public Task OnDragend(LatLong point)
        {
            if(OnDragAsync.HasDelegate)
                OnDragAsync.InvokeAsync(point);
            return Task.CompletedTask;
        }
        #endregion
    }
}