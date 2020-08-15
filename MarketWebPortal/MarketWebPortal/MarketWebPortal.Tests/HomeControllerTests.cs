using Castle.Core.Logging;
using MarketWebPortal.Business;
using MarketWebPortal.Controllers;
using MarketWebPortal.Model;
using MarketWebPortal.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MarketWebPortal.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void TestIndex_WithDataReturnedFromBusiness_ShouldReturnOKAndData()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HomeController>>();
            var mockBusiness = new Mock<IPriceBusiness>();
            var priceDummyOutput = new WebAPIOutput<PriceModel>();
            priceDummyOutput.DataList = new List<PriceModel>() {
                new PriceModel
                {
                    Date = DateTime.Now,
                    MarketPriceEX1 = 10.9999d
                }
            };

            mockBusiness.Setup(x => x.GetPrices()).Returns(priceDummyOutput);

            var controller = new HomeController(mockLogger.Object, mockBusiness.Object);

            // Act
            var output = controller.Index();

            // Assert
            Assert.IsAssignableFrom<ViewResult>(output);
            mockBusiness.Verify(x => x.GetPrices());
            var okResult = output as OkObjectResult;
        }

        [Fact]
        public void TestIndex_WithoutDataReturnedFromBusiness_ShouldReturnOKAndNoData()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HomeController>>();
            var mockBusiness = new Mock<IPriceBusiness>();
            var priceDummyOutput = new WebAPIOutput<PriceModel>();
            priceDummyOutput.DataList = new List<PriceModel>();

            mockBusiness.Setup(x => x.GetPrices()).Returns(priceDummyOutput);

            var controller = new HomeController(mockLogger.Object, mockBusiness.Object);

            // Act
            var output = controller.Index();

            // Assert
            Assert.IsAssignableFrom<ViewResult>(output);
            mockBusiness.Verify(x => x.GetPrices());
            var okResult = output as OkObjectResult;
        }

        [Fact]
        public void TestGetMaxPrice_WithDataReturnedFromBusiness_ShouldReturnOKAndData()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HomeController>>();
            var mockBusiness = new Mock<IPriceBusiness>();
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

            mockBusiness.Setup(x => x.GetMaxPrice(inputStartDate, inputEndDate)).Returns(maxPriceDummyOutput);

            var controller = new HomeController(mockLogger.Object, mockBusiness.Object);

            // Act
            var output = controller.GetMaxPrice(inputStartDate, inputEndDate);

            // Assert
            mockBusiness.Verify(x => x.GetMaxPrice(inputStartDate, inputEndDate));
            var okResult = output as MaxPriceViewModel;
        }

        [Fact]
        public void TestGetMaxPrice_WithoutDataReturnedFromBusiness_ShouldReturnOKAndNoData()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HomeController>>();
            var mockBusiness = new Mock<IPriceBusiness>();
            DateTime inputStartDate = new DateTime(2019, 12, 20, 1, 0, 0);
            DateTime inputEndDate = new DateTime(2020, 12, 21, 1, 0, 0);
            var maxPriceDummyOutput = new MaxPriceViewModel();

            mockBusiness.Setup(x => x.GetMaxPrice(inputStartDate, inputEndDate)).Returns(maxPriceDummyOutput);

            var controller = new HomeController(mockLogger.Object, mockBusiness.Object);

            // Act
            var output = controller.GetMaxPrice(inputStartDate, inputEndDate);

            // Assert
            mockBusiness.Verify(x => x.GetMaxPrice(inputStartDate, inputEndDate));
            var okResult = output as MaxPriceViewModel;
        }

        [Fact]
        public void TestUpload_WithViewReturned_ShouldReturnOK()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HomeController>>();
            var mockBusiness = new Mock<IPriceBusiness>();
            var controller = new HomeController(mockLogger.Object, mockBusiness.Object);

            // Act
            var output = controller.Upload();

            // Assert
            Assert.IsAssignableFrom<ViewResult>(output);
            var okResult = output as OkObjectResult;
        }

        [Fact]
        public void TestUpload_WithData_ShouldReturnOK()
        {
            // Arrange
            string createdBy = "Michal";
            var pricesInFile = new List<PriceModel>()
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

            var mockLogger = new Mock<ILogger<HomeController>>();
            var mockBusiness = new Mock<IPriceBusiness>();
            var mockFormFile = new Mock<IFormFile>();
            mockFormFile.Setup(ff => ff.CopyToAsync(
                It.IsAny<Stream>(),
                It.IsAny<CancellationToken>()))
                .Returns<Stream, CancellationToken>((s, ct) =>
                {
                    byte[] buffer = Encoding.Default.GetBytes(pricesInFile.ToString());
                    s.Write(buffer, 0, buffer.Length);
                    return Task.CompletedTask;
                });

            var inputFile = new PriceViewModel()
            {
                FormFile = mockFormFile.Object
            };
            var maxPriceDummyOutput = new WebAPILog()
            {
                SQLState = string.Empty
            };

            mockBusiness.Setup(x => x.AddPrices(createdBy, pricesInFile))
                .Returns(maxPriceDummyOutput);

            var controller = new HomeController(mockLogger.Object, mockBusiness.Object);

            // Act
            var output = controller.Upload(inputFile);

            // Assert
            mockBusiness.Verify(z => z.AddPrices(
                It.IsAny<string>(),
                It.IsAny<List<PriceModel>>()));
            var okResult = output as Task<IActionResult>;
        }

        [Fact]
        public void TestUpload_WithoutData_ShouldReturnOK()
        {
            // Arrange
            string createdBy = "Michal";
            var pricesInFile = new List<PriceModel>();

            var mockLogger = new Mock<ILogger<HomeController>>();
            var mockBusiness = new Mock<IPriceBusiness>();
            var mockFormFile = new Mock<IFormFile>();
            mockFormFile.Setup(ff => ff.CopyToAsync(
                It.IsAny<Stream>(),
                It.IsAny<CancellationToken>()))
                .Returns<Stream, CancellationToken>((s, ct) =>
                {
                    byte[] buffer = Encoding.Default.GetBytes(string.Empty);
                    s.Write(buffer, 0, buffer.Length);
                    return Task.CompletedTask;
                });

            var inputFile = new PriceViewModel()
            {
                FormFile = mockFormFile.Object
            };
            var maxPriceDummyOutput = new WebAPILog()
            {
                SQLState = string.Empty
            };

            mockBusiness.Setup(x => x.AddPrices(createdBy, pricesInFile))
                .Returns(maxPriceDummyOutput);

            var controller = new HomeController(mockLogger.Object, mockBusiness.Object);

            // Act
            var output = controller.Upload(inputFile);

            // Assert
            var okResult = output as Task<IActionResult>;
        }

        [Fact]
        public void TestError_WithoutData_ShouldReturnOK()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HomeController>>();
            var mockBusiness = new Mock<IPriceBusiness>();
            var controller = new HomeController(mockLogger.Object, mockBusiness.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.TraceIdentifier = string.Empty;

            // Act
            var output = controller.Error();

            // Assert
            var okResult = output as ErrorViewModel;
        }
    }
}
