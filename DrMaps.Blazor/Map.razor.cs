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
        [Parameter] public EventCallback<DragendMarkerEventArgs> OnDragendAsync { get; set; }
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
            ObjRef?.Dispose();
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

        public Task<int> AddMarkerAsync(LatLong point, string title, string description, string iconUrl)
        {
            return LeafletService.InvokeAsyc<int>("addMarker", MapId, point, title, description, iconUrl);
        }

        public Task<int> AddMarkerAsync(LatLong point, string title, string description, Icon icon = Icon.PIN) =>
            AddMarkerAsync(point, title, description, GetIconUrl(icon));

        public Task<int> AddMarkerAsync(LatLong point, string title, string description) =>
            AddMarkerAsync(point, title, description, "marker-icon");   

        public async Task<int> AddDraggableMarkerAsync(LatLong point, string title, string description, string iconUrl)
        {
            await SetDotNetObjectReference();
            return await LeafletService.InvokeAsyc<int>("addDraggableMarker", MapId, point, title, description, iconUrl);
        }

        async ValueTask SetDotNetObjectReference()
        {
            if(ObjRef is null)
            {
                ObjRef = DotNetObjectReference.Create(this);
                await LeafletService.InvokeVoidAsync("setMarkerHelper", MapId, ObjRef, nameof(OnDragend));
            }
        }

        public Task<int> AddDraggableMarkerAsync(LatLong point, string title, string description, Icon icon = Icon.PIN) =>
            AddDraggableMarkerAsync(point, title, description, GetIconUrl(icon));

        public Task<int> AddDraggableMarkerAsync(LatLong point, string title, string description) =>
            AddDraggableMarkerAsync(point, title, description, "marker-icon");

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
        
        public Task SetPopupMarkerContent(int markerId, string content) =>
            LeafletService.InvokeVoidAsync("setPopupMarkerContent", MapId, markerId, content);

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
        public Task OnDragend(DragendMarkerEventArgs e)
        {
            if(OnDragendAsync.HasDelegate)
                OnDragendAsync.InvokeAsync(e);
            return Task.CompletedTask;
        }
        #endregion
    }
}