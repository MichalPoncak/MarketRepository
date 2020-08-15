using Microsoft.Extensions.Options;
using Moq;
using StarwarsWebPortal.Models;
using StarwarsWebPortal.Utility;
using StarwarsWebPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace StarwarsWebPortal.Data.Tests
{
    public class StarshipDataTests
    {
        [Fact]
        public void TestGetStarships_WithDataInput_ShouldReturnData()
        {
            // Arrange
            int pageNumber = 1;
            var webAPIError = new WebAPIError()
            {
                APIMessage = "Success",
                APIStatusCode = 200,
                APISystemExceptionList = new List<Exception>() { }
            };

            var starshipDataFromWebAPI = new StarshipViewModel()
            {
                Count = 1,
                Next = "http://swapi.dev/api/starships/?page=2",
                Previous = null,
                Starships = new List<StarshipModel>()
                {
                    new StarshipModel()
                    {
                        Name = "Rebel transport",
                        Megalights = "20",
                        ConsumableRate = "6 months"
                    },
                    new StarshipModel()
                    {
                        Name = "V-wing",
                        Megalights = "unknown",
                        ConsumableRate = "15 hours"
                    },
                    new StarshipModel()
                    {
                        Name = "Naboo star skiff",
                        Megalights = "unknown",
                        ConsumableRate = "unknown"
                    }
                }
            };

            var mockAPIClient = new Mock<IWebAPIClientManager>();
            mockAPIClient.Setup(x => x.Get<StarshipViewModel>(null)).Returns(starshipDataFromWebAPI);
            mockAPIClient.Setup(x => x.GetAPIError()).Returns(webAPIError);

            IOptions<WebAPISettingsModel> mockConfigOptions = Options.Create<WebAPISettingsModel>(
                new WebAPISettingsModel()
                {
                    WebAPIURL = "http://swapi.dev/",
                    WebAPIComponents = new string[] { "api/starships/", "api/people/", "api/planets/" }
                });

            IStarshipData data = new StarshipData(mockAPIClient.Object, mockConfigOptions);

            // Act
            var result = data.GetStarships(pageNumber);

            // Assert
            Assert.Equal(webAPIError.APISystemExceptionList.Count, result.TransResult.APISystemExceptionList.Count);
            Assert.Equal(200, result.TransResult.APIStatusCode);
        }

        [Fact]
        public void TestGetStarships_WithDataInput_ShouldReturnError()
        {
            // Arrange
            int pageNumber = 1;
            var webAPIError = new WebAPIError()
            {
                APIMessage = "Failure",
                APIStatusCode = 500,
                APISystemExceptionList = new List<Exception>()
                {
                    new Exception("Server is offline!")
                }
            };

            var starshipDataFromWebAPI = new StarshipViewModel()
            {
                Count = 1,
                Next = "http://swapi.dev/api/starships/?page=2",
                Previous = null,
                Starships = new List<StarshipModel>()
                {
                    new StarshipModel()
                    {
                        Name = "Rebel transport",
                        Megalights = "20",
                        ConsumableRate = "6 months"
                    },
                    new StarshipModel()
                    {
                        Name = "V-wing",
                        Megalights = "unknown",
                        ConsumableRate = "15 hours"
                    },
                    new StarshipModel()
                    {
                        Name = "Naboo star skiff",
                        Megalights = "unknown",
                        ConsumableRate = "unknown"
                    }
                }
            };

            var mockAPIClient = new Mock<IWebAPIClientManager>();
            mockAPIClient.Setup(x => x.Get<StarshipViewModel>(null)).Returns(starshipDataFromWebAPI);
            mockAPIClient.Setup(x => x.GetAPIError()).Returns(webAPIError);

            IOptions<WebAPISettingsModel> mockConfigOptions = Options.Create<WebAPISettingsModel>(
                new WebAPISettingsModel()
                {
                    WebAPIURL = "http://swapi.dev/",
                    WebAPIComponents = new string[] { "api/starships/", "api/people/", "api/planets/" }
                });

            IStarshipData data = new StarshipData(mockAPIClient.Object, mockConfigOptions);

            // Act
            var result = data.GetStarships(pageNumber);

            // Assert
            Assert.Equal(webAPIError.APISystemExceptionList.FirstOrDefault().Message, result.TransResult.APISystemExceptionList.FirstOrDefault().Message);
            Assert.Equal(webAPIError.APIStatusCode, result.TransResult.APIStatusCode);
            Assert.True(!string.IsNullOrEmpty(result.TransResult.APIMessage));
        }
    }
}
