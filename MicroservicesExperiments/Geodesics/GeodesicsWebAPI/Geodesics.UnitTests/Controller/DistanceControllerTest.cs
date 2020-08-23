using Geodesics.Api.Business;
using Geodesics.Api.Controllers;
using Geodesics.Api.Model;
using Geodesics.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geodesics.UnitTests.Controller
{
    [TestFixture]
    public class DistanceControllerTest
    {
        [TestCase]
        public void TestGet_WithGeodesicCurveInKmAndValidData_ShouldReturnData()
        {
            // arrange
            var mockService = new Mock<IDistanceService>();
            DistanceMethod equation = DistanceMethod.GeodesicCurve;
            MeasureUnit units = MeasureUnit.Km;
            double inputPoint1Latitude = 53.297975;
            double inputPoint1Longitude = -6.372663;
            double inputPoint2Latitude = 41.385101;
            double inputPoint2Longitude = -81.440440;
            double dummyOutput = 5536.3386822666853;

            mockService.Setup(x => x.CalculateGeodesicCurve(
                It.IsAny<DistancePoint>(),
                It.IsAny<DistancePoint>(),
                It.IsAny<MeasureUnit>())).Returns(dummyOutput);

            var controller = new DistanceController(
                mockService.Object);

            // act
            var result = controller.Get(
                equation,
                units,
                inputPoint1Latitude,
                inputPoint1Longitude,
                inputPoint2Latitude,
                inputPoint2Longitude);

            // assert
            var okResult = result as ActionResult<DistanceResponse>;

            Assert.AreEqual(dummyOutput, result.Value.Distance);
        }

        [TestCase]
        public void TestGet_WithGeodesicCurveInKmAndInvalidData_ShouldReturnData()
        {
            // arrange
            var mockService = new Mock<IDistanceService>();
            DistanceMethod equation = DistanceMethod.GeodesicCurve;
            MeasureUnit units = MeasureUnit.Km;
            double inputPoint1Latitude = 91;
            double inputPoint1Longitude = -6.372663;
            double inputPoint2Latitude = 41.385101;
            double inputPoint2Longitude = -81.440440;
            double dummyOutput = 0;

            mockService.Setup(x => x.CalculateGeodesicCurve(
                It.IsAny<DistancePoint>(),
                It.IsAny<DistancePoint>(),
                It.IsAny<MeasureUnit>())).Returns(dummyOutput);

            var controller = new DistanceController(
                mockService.Object);

            // act
            var result = controller.Get(
                equation,
                units,
                inputPoint1Latitude,
                inputPoint1Longitude,
                inputPoint2Latitude,
                inputPoint2Longitude);
            
            Assert.AreEqual(null, result.Value);
        }

        [TestCase]
        public void TestGet_WithGeodesicCurveInMileAndValidData_ShouldReturnData()
        {
            // arrange
            var mockService = new Mock<IDistanceService>();
            DistanceMethod equation = DistanceMethod.GeodesicCurve;
            MeasureUnit units = MeasureUnit.Km;
            double inputPoint1Latitude = 53.297975;
            double inputPoint1Longitude = -6.372663;
            double inputPoint2Latitude = 41.385101;
            double inputPoint2Longitude = -81.440440;
            double dummyOutput = 3440.333517986785;

            mockService.Setup(x => x.CalculateGeodesicCurve(
                It.IsAny<DistancePoint>(),
                It.IsAny<DistancePoint>(),
                It.IsAny<MeasureUnit>())).Returns(dummyOutput);

            var controller = new DistanceController(
                mockService.Object);

            // act
            var result = controller.Get(
                equation,
                units,
                inputPoint1Latitude,
                inputPoint1Longitude,
                inputPoint2Latitude,
                inputPoint2Longitude);

            // assert
            var okResult = result as ActionResult<DistanceResponse>;

            Assert.AreEqual(dummyOutput, result.Value.Distance);
        }

        [TestCase]
        public void TestGet_WithGeodesicCurveInMileAndInvalidData_ShouldNotReturnData()
        {
            // arrange
            var mockService = new Mock<IDistanceService>();
            DistanceMethod equation = DistanceMethod.GeodesicCurve;
            MeasureUnit units = MeasureUnit.Km;
            double inputPoint1Latitude = 53.297975;
            double inputPoint1Longitude = -179;
            double inputPoint2Latitude = 41.385101;
            double inputPoint2Longitude = -81.440440;
            double dummyOutput = 0;

            mockService.Setup(x => x.CalculateGeodesicCurve(
                It.IsAny<DistancePoint>(),
                It.IsAny<DistancePoint>(),
                It.IsAny<MeasureUnit>())).Returns(dummyOutput);

            var controller = new DistanceController(
                mockService.Object);

            // act
            var result = controller.Get(
                equation,
                units,
                inputPoint1Latitude,
                inputPoint1Longitude,
                inputPoint2Latitude,
                inputPoint2Longitude);

            // assert
            var okResult = result as ActionResult<DistanceResponse>;

            Assert.AreEqual(dummyOutput, result.Value.Distance);
        }

        [TestCase]
        public void TestGet_WithPythagorasInKmAndValidData_ShouldReturnData()
        {
            // arrange
            var mockService = new Mock<IDistanceService>();
            DistanceMethod equation = DistanceMethod.GeodesicCurve;
            MeasureUnit units = MeasureUnit.Km;
            double inputPoint1Latitude = 53.297975;
            double inputPoint1Longitude = -6.372663;
            double inputPoint2Latitude = 41.385101;
            double inputPoint2Longitude = -81.440440;
            double dummyOutput = 5809.2968123283927;

            mockService.Setup(x => x.CalculateGeodesicCurve(
                It.IsAny<DistancePoint>(),
                It.IsAny<DistancePoint>(),
                It.IsAny<MeasureUnit>())).Returns(dummyOutput);

            var controller = new DistanceController(
                mockService.Object);

            // act
            var result = controller.Get(
                equation,
                units,
                inputPoint1Latitude,
                inputPoint1Longitude,
                inputPoint2Latitude,
                inputPoint2Longitude);

            // assert
            var okResult = result as ActionResult<DistanceResponse>;

            Assert.AreEqual(dummyOutput, result.Value.Distance);
        }

        [TestCase]
        public void TestGet_WithPythagorasInKmAndValidData_ShouldNotReturnData()
        {
            // arrange
            var mockService = new Mock<IDistanceService>();
            DistanceMethod equation = DistanceMethod.GeodesicCurve;
            MeasureUnit units = MeasureUnit.Km;
            double inputPoint1Latitude = 91;
            double inputPoint1Longitude = -6.372663;
            double inputPoint2Latitude = 41.385101;
            double inputPoint2Longitude = -81.440440;
            double dummyOutput = 0;

            mockService.Setup(x => x.CalculateGeodesicCurve(
                It.IsAny<DistancePoint>(),
                It.IsAny<DistancePoint>(),
                It.IsAny<MeasureUnit>())).Returns(dummyOutput);

            var controller = new DistanceController(
                mockService.Object);

            // act
            var result = controller.Get(
                equation,
                units,
                inputPoint1Latitude,
                inputPoint1Longitude,
                inputPoint2Latitude,
                inputPoint2Longitude);

            // assert
            var okResult = result as ActionResult<DistanceResponse>;

            Assert.AreEqual(null, result.Value);
        }

        [TestCase]
        public void TestGet_WithPythagorasInMileAndValidData_ShouldReturnData()
        {
            // arrange
            var mockService = new Mock<IDistanceService>();
            DistanceMethod equation = DistanceMethod.GeodesicCurve;
            MeasureUnit units = MeasureUnit.Km;
            double inputPoint1Latitude = 53.297975;
            double inputPoint1Longitude = -6.372663;
            double inputPoint2Latitude = 41.385101;
            double inputPoint2Longitude = -81.440440;
            double dummyOutput = 3609.9522963440759;

            mockService.Setup(x => x.CalculateGeodesicCurve(
                It.IsAny<DistancePoint>(),
                It.IsAny<DistancePoint>(),
                It.IsAny<MeasureUnit>())).Returns(dummyOutput);

            var controller = new DistanceController(
                mockService.Object);

            // act
            var result = controller.Get(
                equation,
                units,
                inputPoint1Latitude,
                inputPoint1Longitude,
                inputPoint2Latitude,
                inputPoint2Longitude);

            // assert
            var okResult = result as ActionResult<DistanceResponse>;

            Assert.AreEqual(dummyOutput, result.Value.Distance);
        }

        [TestCase]
        public void TestGet_WithPythagorasInMileAndValidData_ShouldNotReturnData()
        {
            // arrange
            var mockService = new Mock<IDistanceService>();
            DistanceMethod equation = DistanceMethod.GeodesicCurve;
            MeasureUnit units = MeasureUnit.Km;
            double inputPoint1Latitude = 53.297975;
            double inputPoint1Longitude = -179;
            double inputPoint2Latitude = 41.385101;
            double inputPoint2Longitude = -81.440440;
            double dummyOutput = 0;

            mockService.Setup(x => x.CalculateGeodesicCurve(
                It.IsAny<DistancePoint>(),
                It.IsAny<DistancePoint>(),
                It.IsAny<MeasureUnit>())).Returns(dummyOutput);

            var controller = new DistanceController(
                mockService.Object);

            // act
            var result = controller.Get(
                equation,
                units,
                inputPoint1Latitude,
                inputPoint1Longitude,
                inputPoint2Latitude,
                inputPoint2Longitude);

            // assert
            var okResult = result as ActionResult<DistanceResponse>;

            Assert.AreEqual(dummyOutput, result.Value.Distance);
        }
    }
}
