using Geodesics.Api.Model;
using Geodesics.Api.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Geodesics.Api.Business
{
    public interface IDistanceService
    {
        double CalculateGeodesicCurve(
            DistancePoint point1,
            DistancePoint point2,
            MeasureUnit measureUnit);
        double CalculatePythagoras(
            DistancePoint point1,
            DistancePoint point2,
            MeasureUnit measureUnit);
    }

    public class DistanceService : IDistanceService
    {
        private const double PointOriginDegrees = 90;
        private const double HalfCircleDegrees = 180.0;

        public double CalculateGeodesicCurve(
            DistancePoint point1,
            DistancePoint point2,
            MeasureUnit measureUnit)
        {
            var a = PointOriginDegrees - point2.Latitude;
            var b = PointOriginDegrees - point1.Latitude;
            var fi = point1.Longitude - point2.Longitude;
            var cosP =
                Math.Cos(DegreesToRadians(a)) *
                Math.Cos(DegreesToRadians(b)) +
                Math.Sin(DegreesToRadians(a)) *
                Math.Sin(DegreesToRadians(b)) *
                Math.Cos(DegreesToRadians(fi));
            var n = RadiansToDegrees(Math.Acos(cosP));
            var d = Math.PI * n * GetEarthRadius(measureUnit) / 180;
            return d;
        }

        public double CalculatePythagoras(
            DistancePoint point1,
            DistancePoint point2,
            MeasureUnit measureUnit)
        {
            var x = (
                DegreesToRadians(point2.Longitude) -
                DegreesToRadians(point1.Longitude)) *
                Math.Cos((DegreesToRadians(point1.Latitude) +
                DegreesToRadians(point2.Latitude)) / 2);
            var y = DegreesToRadians(point2.Latitude) - DegreesToRadians(point1.Latitude);
            var d = Math.Sqrt(x * x + y * y) * GetEarthRadius(measureUnit);
            return d;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / HalfCircleDegrees;
        }

        private double RadiansToDegrees(double radians)
        {
            return radians * HalfCircleDegrees / Math.PI;
        }

        private double GetEarthRadius(MeasureUnit measureUnit)
        {
            switch (measureUnit)
            {
                case MeasureUnit.Km:
                    return 6371;
                case MeasureUnit.Mile:
                    return 3959;
                default:
                    throw new ArgumentOutOfRangeException(nameof(measureUnit));
            }
        }
    }
}
