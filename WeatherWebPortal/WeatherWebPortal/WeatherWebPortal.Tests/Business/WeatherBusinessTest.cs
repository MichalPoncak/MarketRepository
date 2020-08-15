using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeatherWebPortal.Business;
using WeatherWebPortal.Data;
using WeatherWebPortal.Models;
using Xunit;

namespace WeatherWebPortal.Tests.Business
{
    public class WeatherBusinessTest
    {
        [Fact]
        public void TestGetWeather_WithDataInput_ShouldCallDataAccessWithData()
        {
            // Arrange
            var mockDataAccess = new Mock<IWeatherData>();
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

            mockDataAccess.Setup(x => x.GetWeather(location, It.IsAny<DateTime>())).Returns(dataOutput);
            // the reason for using the isany method is because with a hardcoded value other generated values in a loop will throw an exception
            var business = new WeatherBusiness(mockDataAccess.Object);

            // Act
            var result = business.GetWeather(location, date, days);

            // Assert
            mockDataAccess.Verify(x => x.GetWeather(location, date));
            Assert.Equal("21/12/2020", result.DataList.LastOrDefault().ApplicableDate);
            Assert.Equal("Showers", result.DataList.LastOrDefault().WeatherStateName);
            Assert.Equal("s", result.DataList.LastOrDefault().WeatherStateAbbreviation);
            Assert.Equal("<img src='https://www.metaweather.com/static/img/weather/s.svg' alt='Snow' style='width: 32px'>", result.DataList.LastOrDefault().IconLink);
        }
    }
}
