using MarketWebPortal.Model;
using MarketWebPortal.Utility;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MarketWebPortal.Data.Tests
{
    public class PriceDataTests
    {
        [Fact]
        public void TestGetPrices_WithoutDataInput_ShouldReturnData()
        {
            // Arrange
            var pricesOutput = new List<PriceModel>()
            {
                new PriceModel()
                {
                    Date = new DateTime(2020, 12, 20, 1, 0, 0),
                    MarketPriceEX1 = 5.25
                },
                new PriceModel()
                {
                    Date = new DateTime(2020, 12, 20, 1, 30, 0),
                    MarketPriceEX1 = 5.25
                }
            };

            var webAPILogOutput = new WebAPIError()
            {
                APIMessage = "xxx",
                APIStatusCode = 200
            };

            var mockWebAPI = new Mock<IWebAPIClientManager>();
            mockWebAPI.Setup(x => x.Get<List<PriceModel>>(null)).Returns(pricesOutput);
            mockWebAPI.Setup(x => x.GetAPIError()).Returns(webAPILogOutput);

            IOptions<WebAPISettings> mockConfigOptions = Options.Create<WebAPISettings>(
                new WebAPISettings()
                {
                    WebAPIURL = "http://localhost:54667/",
                    WebAPIComponents = new string[] { "market/price", "market/purchase", "market/portfolio" }
                });

            IPriceData data = new PriceData(mockWebAPI.Object, mockConfigOptions);

            // Act
            var result = data.GetPrices();

            // Assert
            Assert.Equal(pricesOutput.Count, result.DataList.Count);
            Assert.Equal(pricesOutput[0].Date, result.DataList[0].Date);
            Assert.Equal(pricesOutput[0].MarketPriceEX1, result.DataList[0].MarketPriceEX1);
            Assert.Equal(pricesOutput[1].Date, result.DataList[1].Date);
            Assert.Equal(pricesOutput[1].MarketPriceEX1, result.DataList[1].MarketPriceEX1);
        }

        [Fact]
        public void TestAddPrices_WithDataInput_ShouldReturnData()
        {
            // Arrange
            string createdBy = "Michal";
            var pricesInput = new List<PriceModel>()
            {
                new PriceModel()
                {
                    Date = new DateTime(2020, 12, 20, 1, 0, 0),
                    MarketPriceEX1 = 5.25
                },
                new PriceModel()
                {
                    Date = new DateTime(2020, 12, 20, 1, 30, 0),
                    MarketPriceEX1 = 5.25
                }
            };

            WebAPIResponse webAPIDummyResponse = new WebAPIResponse()
            {
                CreatedID = "1"
            };

            WebAPILog testReturnValues = new WebAPILog()
            {
                CreatedID = "1"
            };

            var mockWebAPI = new Mock<IWebAPIClientManager>();
            mockWebAPI.Setup(x => x.Post<List<PriceModel>, string>(pricesInput)).Returns(webAPIDummyResponse);

            IOptions<WebAPISettings> mockConfigOptions = Options.Create<WebAPISettings>(
                new WebAPISettings()
                {
                    WebAPIURL = "http://localhost:54667/",
                    WebAPIComponents = new string[] { "market/price", "market/purchase", "market/portfolio" }
                });

            IPriceData data = new PriceData(mockWebAPI.Object, mockConfigOptions);

            // Act
            var result = data.AddPrices(createdBy, pricesInput);

            // Assert
            Assert.Equal(webAPIDummyResponse.CreatedID, result.CreatedID);
            Assert.True(string.IsNullOrEmpty(result.ReturnMessage));
        }

        [Fact]
        public void TestAddPrices_WithDataInput_ShouldReturnError()
        {
            // Arrange
            string createdBy = "Michal";
            var pricesInput = new List<PriceModel>()
            {
                new PriceModel()
                {
                    Date = new DateTime(2020, 12, 20, 1, 0, 0),
                    MarketPriceEX1 = 5.25
                },
                new PriceModel()
                {
                    Date = new DateTime(2020, 12, 20, 1, 30, 0),
                    MarketPriceEX1 = 5.25
                }
            };

            WebAPIResponse webAPIDummyResponse = new WebAPIResponse()
            {
                ErrorList = new List<string>()
                {
                    "Response error!"
                }
            };

            WebAPILog testReturnValues = new WebAPILog()
            {
                CreatedID = "0",
                ReturnMessage = "Response error!"
            };

            var mockWebAPI = new Mock<IWebAPIClientManager>();
            mockWebAPI.Setup(x => x.Post<List<PriceModel>, string>(pricesInput)).Returns(webAPIDummyResponse);

            IOptions<WebAPISettings> mockConfigOptions = Options.Create<WebAPISettings>(
                new WebAPISettings()
                {
                    WebAPIURL = "http://localhost:54667/",
                    WebAPIComponents = new string[] { "market/price", "market/purchase", "market/portfolio" }
                });

            IPriceData data = new PriceData(mockWebAPI.Object, mockConfigOptions);

            // Act
            var result = data.AddPrices(createdBy, pricesInput);

            // Assert
            Assert.Equal("0", result.CreatedID);
            Assert.True(!string.IsNullOrEmpty(result.ReturnMessage));
        }
    }
}
