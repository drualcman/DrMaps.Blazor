namespace DrMaps.Blazor.Helpers;
internal class CoordinatesCalculatesHelper
{
	private const double RADIO_EARTH_ECUATORIAL_IN_METTERS = 6378137;
	private double DEGREES_PER_METTER_OF_LATTITUDE => 360 / (2 * Math.PI * RADIO_EARTH_ECUATORIAL_IN_METTERS);
	readonly GeoMathHelper Maths;
	public CoordinatesCalculatesHelper()
	{
		Maths = new GeoMathHelper();
	}
			   	
	internal double GetLatitudeFromDegreesPerMetter(double latitude, double angle, double distanceInMetters)
	{	  										
        double y = Math.Sin(angle) * distanceInMetters;
        return latitude + (y * DEGREES_PER_METTER_OF_LATTITUDE);
	} 

	internal double GetLongitudeFromDegreesPerMetter(double latitude, double longitude, double angle, double distanceInMetters)
	{	  										
        double x = Math.Cos(angle) * distanceInMetters;	
        double longitudeGradesToAdd = x * DEGREES_PER_METTER_OF_LATTITUDE;
        longitudeGradesToAdd /= Math.Cos(latitude * (Math.PI / 180));
        return longitude + longitudeGradesToAdd;
	}

	internal double CalculateDistanceInMetters(LatLong origin, LatLong destination)
	{
		double latitudARad = Maths.ConvertToRadians(origin.Latitude);
		double longitudARad = Maths.ConvertToRadians(origin.Longitude);
		double latitudBRad = Maths.ConvertToRadians(destination.Latitude);
		double longitudBRad = Maths.ConvertToRadians(destination.Longitude);
		double haversine = Maths.CalculateHaversine(latitudARad, longitudARad, latitudBRad, longitudBRad);
		double result = (RADIO_EARTH_ECUATORIAL_IN_METTERS) * haversine;
		return result;
	}
}
