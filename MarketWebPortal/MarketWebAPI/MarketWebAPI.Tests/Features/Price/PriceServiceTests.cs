using MarketWebAPI.Features.Price;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xunit;

namespace MarketWebAPI.Tests.Features.Price
{
    public class PriceServiceTests
    {
        [Fact]
        public void TestGetPrices_WithValidDataInput_ShouldCallDataAccessWithInputData()
        {
            // Arrange
            var mockDataAccess = new Mock<IPriceDataAccess>();
            var service = new PriceService(mockDataAccess.Object);

            // Act
            service.GetPrices();

            // Assert
            mockDataAccess.Verify(x => x.GetPrices());
        }

        [Fact]
        public void TestGetPrices_WithDataReturnedFromDataAccess_ShouldReturnListOfFees()
        {
            // Arrange
            var mockDataAccess = new Mock<IPriceDataAccess>();
            var pricesOutput = new List<PriceOutputDto>
            {
                new PriceOutputDto
                {
                    PriceID = 4,
                    Date = DateTime.Now,
                    MarketPrice = 66.98999786m,
                    CreatedTimeStamp = DateTime.Now,
                    CreatedBy = "Michal",
                    UpdatedTimeStamp = DateTime.Now,
                    UpdatedBy = "John"
                },
                new PriceOutputDto
                {
                    PriceID = 5,
                    Date = new DateTime(2020, 12, 20),
                    MarketPrice = 999999999.98999703m,
                    CreatedTimeStamp = new DateTime(2020, 12, 20),
                    CreatedBy = "Michal",
                    UpdatedTimeStamp = new DateTime(2020, 12, 20),
                    UpdatedBy = "John"
                }
            };

            mockDataAccess.Setup(x => x.GetPrices()).Returns(pricesOutput);

            var service = new PriceService(mockDataAccess.Object);

            // Act
            var result = service.GetPrices();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(pricesOutput[0].PriceID, result[0].PriceID);
            Assert.Equal(pricesOutput[0].Date, result[0].Date);
            Assert.Equal(pricesOutput[0].MarketPrice, result[0].MarketPrice);
            Assert.Equal(pricesOutput[0].CreatedTimeStamp, result[0].CreatedTimeStamp);
            Assert.Equal(pricesOutput[0].CreatedBy, result[0].CreatedBy);
            Assert.Equal(pricesOutput[0].UpdatedTimeStamp, result[0].UpdatedTimeStamp);
            Assert.Equal(pricesOutput[0].UpdatedBy, result[0].UpdatedBy);

            Assert.Equal(pricesOutput[1].PriceID, result[1].PriceID);
            Assert.Equal(pricesOutput[1].Date, result[1].Date);
            Assert.Equal(pricesOutput[1].MarketPrice, result[1].MarketPrice);
            Assert.Equal(pricesOutput[1].CreatedTimeStamp, result[1].CreatedTimeStamp);
            Assert.Equal(pricesOutput[1].CreatedBy, result[1].CreatedBy);
            Assert.Equal(pricesOutput[1].UpdatedTimeStamp, result[1].UpdatedTimeStamp);
            Assert.Equal(pricesOutput[1].UpdatedBy, result[1].UpdatedBy);
        }

        [Fact]
        public void TestGetPrices_WithNullReturnedFromDataAccess_ShouldReturnEmptyCollection()
        {
            // Arrange
            var mockDataAccess = new Mock<IPriceDataAccess>();
            mockDataAccess.Setup(x => x.GetPrices())
                .Returns((List<PriceOutputDto>)null);

            var service = new PriceService(mockDataAccess.Object);

            // Act
            var result = service.GetPrices();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void TestInsertPrice_WithValidData_ShouldCallDataAccessWithInputData()
        {
            // Arrange
            var mockDataAccess = new Mock<IPriceDataAccess>();
            var service = new PriceService(mockDataAccess.Object);
            string createdBy = "Michal";
            var priceInput = new Collection<PriceInput>()
            {
                new PriceInput { Date = DateTime.Now, MarketPrice = 66.98999786m },
                new PriceInput { Date = DateTime.MaxValue, MarketPrice = 99.98999789m }
            };

            // Act
            var result = service.InsertPrices(createdBy, priceInput);

            // Assert
            mockDataAccess.Verify(x => x.InsertPrices(
                It.Is<string>(y => y == createdBy),
                It.Is<string>(y => y == createdBy),
                It.Is<Collection<PriceInputDto>>(y =>
                    y.First().PriceDate == DateTime.Now &&
                    y.First().MarketPrice == 66.98999786m)));
        }

        [Fact]
        public void TestInsertPrice_WithInsertedIdReturned_ShouldReturnInsertedId()
        {
            // Arrange
            int insertedId = 5;
            string createdBy = "Michal";
            var priceInput = new Collection<PriceInput>()
            {
                new PriceInput { Date = DateTime.Now, MarketPrice = 66.98999786m }
            };

            var mockDataAccess = new Mock<IPriceDataAccess>();
            mockDataAccess.Setup(x => x.InsertPrices(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Collection<PriceInputDto>>()))
                .Returns(insertedId);
            var service = new PriceService(mockDataAccess.Object);

            // Act
            var result = service.InsertPrices(createdBy, priceInput);

            // Assert
            Assert.Equal(insertedId, result);
        }
    }
}
