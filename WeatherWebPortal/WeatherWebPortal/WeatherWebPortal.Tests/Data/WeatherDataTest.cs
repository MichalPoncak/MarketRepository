using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeatherWebPortal.Data;
using WeatherWebPortal.Models;
using WeatherWebPortal.Utility;
using Xunit;

namespace WeatherWebPortal.Tests.Data
{
    public class WeatherDataTest
    {
        [Fact]
        public void TestGetWeather_WithDataInput_ShouldReturnData()
        {
            // Arrange
            string location = "44544";
            DateTime date = DateTime.Today;
            var webAPIError = new WebAPIError()
            {
                APIMessage = "Success",
                APIStatusCode = 200,
                APISystemExceptionList = new List<Exception>() { }
            };

            var dataFromWebAPI = new List<WeatherModel>()
            {
                new WeatherModel()
                {
                    WeatherStateName = "Showers",
                    WeatherStateAbbreviation = "s",
                    ApplicableDateTime = new DateTime(2020, 8, 14)
                }
            };

            var mockAPIClient = new Mock<IWebAPIClientManager>();
            mockAPIClient.Setup(x => x.Get<List<WeatherModel>>(null)).Returns(dataFromWebAPI);
            mockAPIClient.Setup(x => x.GetAPIError()).Returns(webAPIError);

            IOptions<WebAPISettingsModel> mockConfigOptions = Options.Create<WebAPISettingsModel>(
                new WebAPISettingsModel()
                {
                    WebAPIURL = "https://www.metaweather.com",
                    WebAPIComponents = new string[] { "api/location", "api/4dimensionalTimeline" }
                });

            IWeatherData data = new WeatherData(mockAPIClient.Object, mockConfigOptions);

            // Act
            var result = data.GetWeather(location, date);

            // Assert
            Assert.Equal(webAPIError.APISystemExceptionList.Count, result.TransResult.APISystemExceptionList.Count);
            Assert.Equal(200, result.TransResult.APIStatusCode);
            Assert.Equal("14/08/2020", result.DataList.FirstOrDefault().ApplicableDate);
        }

        [Fact]
        public void TestGetWeather_WithDataInput_ShouldReturnError()
        {
            // Arrange
            string location = "44544";
            DateTime date = DateTime.Today;
            var webAPIError = new WebAPIError()
            {
                APIMessage = "Failure",
                APIStatusCode = 500,
                APISystemExceptionList = new List<Exception>()
                {
                    new Exception("Server is offline!")
                }
            };

            var dataFromWebAPI = new List<WeatherModel>()
            {
                new WeatherModel()
                {
                    WeatherStateName = "Showers",
                    WeatherStateAbbreviation = "s",
                    ApplicableDateTime = DateTime.Today
                }
            };

            var mockAPIClient = new Mock<IWebAPIClientManager>();
            mockAPIClient.Setup(x => x.Get<List<WeatherModel>>(null)).Returns(dataFromWebAPI);
            mockAPIClient.Setup(x => x.GetAPIError()).Returns(webAPIError);

            IOptions<WebAPISettingsModel> mockConfigOptions = Options.Create<WebAPISettingsModel>(
                new WebAPISettingsModel()
                {
                    WebAPIURL = "https://www.metaweather.com",
                    WebAPIComponents = new string[] { "api/location", "api/4dimensionalTimeline" }
                });

            IWeatherData data = new WeatherData(mockAPIClient.Object, mockConfigOptions);

            // Act
            var result = data.GetWeather(location, date);

            // Assert
            Assert.Equal(webAPIError.APISystemExceptionList.FirstOrDefault().Message, result.TransResult.APISystemExceptionList.FirstOrDefault().Message);
            Assert.Equal(webAPIError.APIStatusCode, result.TransResult.APIStatusCode);
            Assert.True(!string.IsNullOrEmpty(result.TransResult.APIMessage));
        }
    }
}
