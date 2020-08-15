using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeatherWebPortal.Business;
using WeatherWebPortal.Controllers;
using WeatherWebPortal.Models;
using Xunit;

namespace WeatherWebPortal.Tests.Controller
{
    public class WeatherControllerTest
    {
        [Fact]
        public void TestIndex_WithoutDataReturned_ShouldReturnOK()
        {
            // Arrange
            var mockBusiness = new Mock<IWeatherBusiness>();
            var controller = new HomeController(mockBusiness.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void TestGetWeather_WithDataReturnedFromBusiness_ShouldReturnOKAndData()
        {
            // Arrange
            var mockBusiness = new Mock<IWeatherBusiness>();
            string location = "44544";
            DateTime date = new DateTime(2020, 12, 20);
            int days = 2;
            var dataOutput = new WebAPIOutputModel<WeatherModel>();
            dataOutput.DataList = new List<WeatherModel>() {
                new WeatherModel()
                {
                    WeatherStateName = "Showers",
                    WeatherStateAbbreviation = "s",
                    ApplicableDateTime = new DateTime(2020, 12, 20)
                },
                new WeatherModel()
                {
                    WeatherStateName = "Showers",
                    WeatherStateAbbreviation = "s",
                    ApplicableDateTime = new DateTime(2020, 12, 21)
                }
            };

            mockBusiness.Setup(x => x.GetWeather(location, It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(dataOutput);

            var controller = new HomeController(mockBusiness.Object);

            // Act
            var result = controller.GetWeather(location);

            // Assert
            Assert.IsAssignableFrom<WebAPIOutputModel<WeatherModel>>(result);
            Assert.Equal("21/12/2020", result.DataList.LastOrDefault().ApplicableDate);
            mockBusiness.Verify(x => x.GetWeather(
                It.Is<string>(x => x == location),
                It.IsAny<DateTime>(),
                It.IsAny<int>()));
        }

        [Fact]
        public void TestError_WithoutData_ShouldReturnOK()
        {
            // Arrange
            var mockBusiness = new Mock<IWeatherBusiness>();
            var controller = new HomeController(mockBusiness.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.TraceIdentifier = string.Empty;

            // Act
            var result = controller.Error();

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }
    }
}
