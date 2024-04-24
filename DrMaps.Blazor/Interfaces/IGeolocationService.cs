namespace DrMaps.Blazor.Interfaces;
public interface IGeolocationService
{
    ValueTask<ILatLong> GetPositionAsync();
    ValueTask<bool> GetGeoLocationGrantedAsync();
}
