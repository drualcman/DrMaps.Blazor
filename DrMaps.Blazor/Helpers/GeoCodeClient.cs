using System.Net.Http.Json;

namespace DrMaps.Blazor.Helpers;
internal sealed class GeoCodeClient
{
    const string BaseUrl = "https://geocode.maps.co/search?api_key=6617797aed65d703214260qyc8c56a0&";
    const string QueryStringVariables = "street=[street]&city=[city]&state=[state]&postalcode=[postalcode]&country=[country]";

    readonly string Url;

    public GeoCodeClient(Address address)
    {
        Url = BaseUrl;
        Url += QueryStringVariables.Replace("[street]", address.Street)
                                   .Replace("[city]", address.City)
                                   .Replace("[state]", address.State)
                                   .Replace("[postalcode]", address.Postalcode)
                                   .Replace("[country]", address.Country);
    }

    internal async Task<List<PlaceGeocoding>> GetGeocodings()
    {
        using HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri(Url)
        };
        using HttpResponseMessage response = await client.GetAsync("", CancellationToken.None);
        response.EnsureSuccessStatusCode();
        IEnumerable<GeoCodeResponse> places = await response.Content.ReadFromJsonAsync<IEnumerable<GeoCodeResponse>>();
        return places.Select(place => new PlaceGeocoding
        {
            Id = place.Place_Id,
            Point = new LatLong(place.Lat, place.Lon),
            DisplayName = place.Displace_Name
        }).ToList();
    }
}
