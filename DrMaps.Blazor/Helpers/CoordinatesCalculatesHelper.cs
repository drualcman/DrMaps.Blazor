namespace DrMaps.Blazor.Helpers;
internal class CoordinatesCalculatesHelper
{
	internal const double RADIO_EARTH_IN_KM = 6371.0;
	readonly GeoMathHelper Maths;
	public CoordinatesCalculatesHelper()
	{
		Maths = new GeoMathHelper();
	}

	public LatLong CalculateRandomPoint(LatLong origin, double distanceInKm)
	{
		double latitudeInRadians = Maths.ConvertToRadians(origin.Latitude);
		double longitudeInRadians = Maths.ConvertToRadians(origin.Longitude);
		double angleRadians = Maths.CalculateRandomAngleInRadians();
		double distance = Maths.CalculateDistance(distanceInKm);
		double latitudePuntoRadians = CalculateLatitudeInsideRadio(latitudeInRadians, distance, angleRadians);
		double longitudePuntoRadians = CalculateLongitudeInsideRadio(longitudeInRadians, distance, angleRadians);
		return CreatePointFromRadians(latitudePuntoRadians, longitudePuntoRadians);
	}

	private double CalculateLatitudeInsideRadio(double latitudRad, double distanceInKm, double angleRad)
	{
		double result = latitudRad + (distanceInKm / RADIO_EARTH_IN_KM) * Math.Cos(angleRad);
		return result;
	}

	private double CalculateLongitudeInsideRadio(double longitude, double distanceInKm, double angleRad)
	{
		double result = longitude + (distanceInKm / RADIO_EARTH_IN_KM) * Math.Sign(angleRad);
		return result;
	}

	private LatLong CreatePointFromRadians(double latitudeInRadians, double longitudeInRadians)
	{
		double latitude = Maths.ConvertToGrades(latitudeInRadians);
		double longitude = EnsureLongitudeInsideCircle(Maths.ConvertToGrades(longitudeInRadians));
		return new LatLong(latitude, longitude);
	}

	private double EnsureLongitudeInsideCircle(double longitude)
	{
		if(longitude < -180)
		{
			longitude += 360;
		}
		else if(longitude > 180)
		{
			longitude -= 360;
		}
		return longitude;
	}

	internal double CalculateDistanceInKm(LatLong origin, LatLong destination)
	{
		double latitudARad = Maths.ConvertToRadians(origin.Latitude);
		double longitudARad = Maths.ConvertToRadians(origin.Longitude);
		double latitudBRad = Maths.ConvertToRadians(destination.Latitude);
		double longitudBRad = Maths.ConvertToRadians(destination.Longitude);
		double haversine = Maths.CalculateHaversine(latitudARad, longitudARad, latitudBRad, longitudBRad);
		double result = RADIO_EARTH_IN_KM * haversine;
		return result;
	}
}
