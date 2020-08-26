using MarketWebAPI.Features.Price;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xunit;

namespace MarketWebAPI.Tests.Features.Price
{
    public class PriceControllerTests
    {
        [Fact]
        public void TestGetPrices_WithDataReturnedFromService_ShouldReturnOKAndData()
        {
            // Arrange
            var mockService = new Mock<IPriceService>();
            var priceDummyOutput = new List<PriceOutput>
            {
                new PriceOutput
                {
                    PriceID = 4,
                    Date = DateTime.Now,
                    MarketPrice = 66.98999786m,
                    CreatedTimeStamp = DateTime.Now,
                    CreatedBy = "Michal",
                    UpdatedTimeStamp = DateTime.Now,
                    UpdatedBy = "John"
                }
            };

            mockService.Setup(x => x.GetPricesAsync()).ReturnsAsync(priceDummyOutput);

            var controller = new PriceController(mockService.Object);

            // Act
            var output = controller.GetPrices().Result;

            // Assert
            var okResult = output as OkObjectResult;

            Assert.Equal(priceDummyOutput[0].PriceID, ((List<PriceOutput>)okResult.Value)[0].PriceID);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void TestInsertPrices_WithValidData_ShouldCallServiceWithInputData()
        {
            // Arrange
            var mockService = new Mock<IPriceService>();
            string createdBy = "Michal";
            var priceInput = new Collection<PriceInput>()
            {
                new PriceInput { Date = new DateTime(2020, 1, 1), MarketPrice = 66.98999786m },
                new PriceInput { Date = DateTime.MaxValue, MarketPrice = 99.98999789m }
            };

            var controller = new PriceController(mockService.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["CreatedBy"] = createdBy;

            // Act
            controller.InsertPrice(priceInput);

            // Assert
            mockService.Verify(x => x.InsertPricesAsync(
                It.Is<string>(y => y == createdBy),
                It.Is<Collection<PriceInput>>(y =>
                    y.First().Date == new DateTime(2020, 1, 1) &&
                    y.First().MarketPrice == 66.98999786m)));
        }
    }
}
