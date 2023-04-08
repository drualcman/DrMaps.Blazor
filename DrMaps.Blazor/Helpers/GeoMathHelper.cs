namespace DrMaps.Blazor.Helpers;
internal class GeoMathHelper
{
	internal double CalculateRandomAngleInRadians()
	{
		Random random = new Random();
		double randomAngle = random.NextDouble() * 2 * Math.PI;
		return randomAngle;
	}

	internal double ConvertToRadians(double grados) =>
		grados * (Math.PI / 180.0);

	internal double ConvertToGrades(double rad) =>
		rad * (180.0 / Math.PI);

	internal double CalculateDistance(double distance)
	{
		Random random = new Random();
		double result = distance * Math.Sqrt(random.NextDouble());
		return result;
	}

	internal double CalculateHaversine(
		double originLatInRad, double originLongitudInRad,
		double destinationLatiInRad, double destinationLongitudeInRad)
	{
		double difLatitud = CalculateDiference(destinationLatiInRad, originLatInRad);
		double difLongitud = CalculateDiference(destinationLongitudeInRad, originLongitudInRad);

		//haversine
		double angle = Math.Sin(difLatitud / 2) * Math.Sin(difLatitud / 2) +
				   Math.Cos(originLatInRad) * Math.Cos(destinationLatiInRad) *
				   Math.Sin(difLongitud / 2) * Math.Sin(difLongitud / 2);
		double haversine = 2 * Math.Atan2(Math.Sqrt(angle), Math.Sqrt(1 - angle));
		return haversine;
	}

	double CalculateDiference(double firstValue, double secondValue) =>
		firstValue - secondValue;
}
