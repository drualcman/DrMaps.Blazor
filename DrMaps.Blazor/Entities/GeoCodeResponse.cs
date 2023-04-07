namespace DrMaps.Blazor.Entities;
internal sealed class GeoCodeResponse
{
    public int Place_Id { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string Displace_Name { get; set; }
}
