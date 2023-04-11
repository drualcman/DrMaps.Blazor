namespace DrMaps.Blazor.ValueObjects;
public record struct LatLong(double Latitude, double Longitude) 
{
    public LatLong AddMetters(double angle, double distanceInMetters)
    {
        CoordinatesCalculatesHelper calculatesHelper = new CoordinatesCalculatesHelper();
        double latitude = calculatesHelper.GetLatitudeFromDegreesPerMetter(Latitude, angle, distanceInMetters);
        double longitude = calculatesHelper.GetLongitudeFromDegreesPerMetter(Latitude, Longitude, angle, distanceInMetters);
        return new LatLong(latitude, longitude);
    }

    public LatLong AddKm(double angle, double distanceInKm) =>
        AddMetters(angle, distanceInKm * 1000);   

    public LatLong AddCm(double angle, double distanceInKm) =>
        AddMetters(angle, distanceInKm / 100);
}
