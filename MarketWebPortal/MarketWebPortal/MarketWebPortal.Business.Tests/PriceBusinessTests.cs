using MarketWebPortal.Data;
using MarketWebPortal.Model;
using MarketWebPortal.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MarketWebPortal.Business.Tests
{
    public class PriceBusinessTests
    {
        [Fact]
        public void TestGetPrices_WithoutDataInput_ShouldCallDataAccessWithoutData()
        {
            // Arrange
            var mockDataAccess = new Mock<IPriceData>();
            var pricesDummyOutput = new WebAPIOutput<PriceModel>();
            mockDataAccess.Setup(x => x.GetPrices()).Returns(pricesDummyOutput);
            var business = new PriceBusiness(mockDataAccess.Object);

            // Act
            var result = business.GetPrices();

            // Assert
            mockDataAccess.Verify(x => x.GetPrices());
            Assert.Equal(pricesDummyOutput.DataList.Count, result.DataList.Count);
        }

        [Fact]
        public void TestGetMaxPrice_WithDataInput_ShouldCallDataAccessWithData()
        {
            // Arrange
            DateTime inputStartDate = new DateTime(2019, 12, 20, 1, 0, 0);
            DateTime inputEndDate = new DateTime(2020, 12, 21, 1, 0, 0);
            var maxPriceDummyOutput = new MaxPriceViewModel()
            {
                GranularPrices = new List<PriceModel>()
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
                },
                MaxHourlyPrice = 10.50
            };
            var mockDataAccess = new Mock<IPriceData>();
            var pricesDummyOutput = new WebAPIOutput<PriceModel>()
            {
                DataList = new List<PriceModel>()
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
                },
                TransResult = new WebAPILog()
            };
            mockDataAccess.Setup(x => x.GetPrices()).Returns(pricesDummyOutput);
            var business = new PriceBusiness(mockDataAccess.Object);

            // Act
            var result = business.GetMaxPrice(inputStartDate, inputEndDate);

            // Assert
            mockDataAccess.Verify(x => x.GetPrices());
            Assert.Equal(maxPriceDummyOutput.MaxHourlyPrice, result.MaxHourlyPrice);
            Assert.Equal(maxPriceDummyOutput.GranularPrices.Count, result.GranularPrices.Count);
            Assert.Equal(maxPriceDummyOutput.GranularPrices[1].Date, result.GranularPrices[1].Date);
            Assert.Equal(maxPriceDummyOutput.GranularPrices[1].MarketPriceEX1, result.GranularPrices[1].MarketPriceEX1);
        }

        [Fact]
        public void TestGetMaxPrice_WithDataInput_ShouldCallDataAccessWithoutData()
        {
            // Arrange
            DateTime inputStartDate = new DateTime(2019, 12, 20, 1, 0, 0);
            DateTime inputEndDate = new DateTime(2020, 12, 21, 1, 0, 0);
            var maxPriceDummyOutput = new MaxPriceViewModel();
            var mockDataAccess = new Mock<IPriceData>();
            var pricesDummyOutput = new WebAPIOutput<PriceModel>();
            mockDataAccess.Setup(x => x.GetPrices()).Returns(pricesDummyOutput);
            var business = new PriceBusiness(mockDataAccess.Object);

            // Act
            var result = business.GetMaxPrice(inputStartDate, inputEndDate);

            // Assert
            mockDataAccess.Verify(x => x.GetPrices());
            Assert.Equal(maxPriceDummyOutput.MaxHourlyPrice, result.MaxHourlyPrice);
            Assert.Equal(maxPriceDummyOutput.GranularPrices.Count, result.GranularPrices.Count);
        }

        [Fact]
        public void TestAddPrices_WithDataInput_ShouldCallDataAccessWithData()
        {
            // Arrange
            string createdBy = "Michal";
            var mockDataAccess = new Mock<IPriceData>();
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
            var webAPIDummyOutput = new WebAPILog() { CreatedID = "10" };

            mockDataAccess.Setup(x => x.AddPrices(createdBy, pricesInput))
                .Returns(webAPIDummyOutput);
            var business = new PriceBusiness(mockDataAccess.Object);

            // Act
            var result = business.AddPrices(createdBy, pricesInput);

            // Assert
            mockDataAccess.Verify(x => x.AddPrices(createdBy, pricesInput));
            Assert.Equal(webAPIDummyOutput.CreatedID, result.CreatedID);
            Assert.True(string.IsNullOrEmpty(result.ReturnMessage));
        }

        [Fact]
        public void TestAddPrices_WithoutDataInput_ShouldCallDataAccessWithoutData()
        {
            // Arrange
            string createdBy = "Michal";
            var mockDataAccess = new Mock<IPriceData>();
            var pricesInput = new List<PriceModel>();
            var webAPIDummyOutput = new WebAPILog()
            {
                CreatedID = "0",
                ReturnMessage = "SQL truncation occured!"
            };

            mockDataAccess.Setup(x => x.AddPrices(createdBy, pricesInput))
                .Returns(webAPIDummyOutput);
            var business = new PriceBusiness(mockDataAccess.Object);

            // Act
            var result = business.AddPrices(createdBy, pricesInput);

            // Assert
            mockDataAccess.Verify(x => x.AddPrices(createdBy, pricesInput));
            Assert.Equal(webAPIDummyOutput.CreatedID, result.CreatedID);
            Assert.True(!string.IsNullOrEmpty(result.ReturnMessage));
        }
    }
}
