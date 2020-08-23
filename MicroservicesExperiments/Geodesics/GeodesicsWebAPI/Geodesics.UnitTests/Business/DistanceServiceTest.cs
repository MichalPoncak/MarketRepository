using Geodesics.Api.Business;
using Geodesics.Api.Model;
using Geodesics.Api.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geodesics.UnitTests.Business
{
    [TestFixture]
    public class DistanceServiceTest
    {
        [TestCase]
        public void TestCalculateGeodesicCurve_WithValidDataInKm_ShouldReturnData()
        {
            // arrange
            MeasureUnit units = MeasureUnit.Km;
            var point1 = new DistancePoint()
            {
                Latitude = 53.297975,
                Longitude = -6.372663
            };

            var point2 = new DistancePoint()
            {
                Latitude = 41.385101,
                Longitude = -81.440440
            };

            double dummyOutput = 5536.3386822666853;

            var service = new DistanceService();

            // act
            var result = service.CalculateGeodesicCurve(
                point1,
                point2,
                units);

            // assert
            Assert.AreEqual(dummyOutput, result);
        }

        [TestCase]
        public void TestCalculateGeodesicCurve_WithValidDataInMiles_ShouldReturnData()
        {
            // arrange
            MeasureUnit units = MeasureUnit.Mile;
            var point1 = new DistancePoint()
            {
                Latitude = 53.297975,
                Longitude = -6.372663
            };

            var point2 = new DistancePoint()
            {
                Latitude = 41.385101,
                Longitude = -81.440440
            };

            double dummyOutput = 3440.333517986785;

            var service = new DistanceService();

            // act
            var result = service.CalculateGeodesicCurve(
                point1,
                point2,
                units);

            // assert
            Assert.AreEqual(dummyOutput, result);
        }

        [TestCase]
        public void TestCalculatePythagoras_WithValidDataInKm_ShouldReturnData()
        {
            // arrange
            MeasureUnit units = MeasureUnit.Km;
            var point1 = new DistancePoint()
            {
                Latitude = 53.297975,
                Longitude = -6.372663
            };

            var point2 = new DistancePoint()
            {
                Latitude = 41.385101,
                Longitude = -81.440440
            };

            double dummyOutput = 5809.2968123283927;

            var service = new DistanceService();

            // act
            var result = service.CalculatePythagoras(
                point1,
                point2,
                units);

            // assert
            Assert.AreEqual(dummyOutput, result);
        }

        [TestCase]
        public void TestCalculatePythagoras_WithValidDataInMiles_ShouldReturnData()
        {
            // arrange
            MeasureUnit units = MeasureUnit.Mile;
            var point1 = new DistancePoint()
            {
                Latitude = 53.297975,
                Longitude = -6.372663
            };

            var point2 = new DistancePoint()
            {
                Latitude = 41.385101,
                Longitude = -81.440440
            };

            double dummyOutput = 3609.9522963440759;

            var service = new DistanceService();

            // act
            var result = service.CalculatePythagoras(
                point1,
                point2,
                units);

            // assert
            Assert.AreEqual(dummyOutput, result);
        }
    }
}
