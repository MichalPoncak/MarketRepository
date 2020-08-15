using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StarwarsWebPortal.Business;
using StarwarsWebPortal.Controllers;
using StarwarsWebPortal.Models;
using StarwarsWebPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace StarwarsWebPortal.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void TestIndex_WithoutDataReturned_ShouldReturnOK()
        {
            // Arrange
            var mockBusiness = new Mock<IStarshipBusiness>();
            var controller = new HomeController(mockBusiness.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void TestGetStarships_WithDataReturnedFromBusiness_ShouldReturnOKAndData()
        {
            // Arrange
            var mockBusiness = new Mock<IStarshipBusiness>();
            int distance = 1000000;
            int pageNumber = 1;
            var starshipOutput = new WebAPIOutputModel<StarshipViewModel>();
            starshipOutput.DataList = new List<StarshipViewModel>() {
                new StarshipViewModel
                {
                    Count = 1,
                    Next = "http://swapi.dev/api/starships/?page=2",
                    Previous = null,
                    PreviousPageNumber = 0,
                    CurrentPageNumber = 1,
                    NextPageNumber = 2,
                    Starships = new List<StarshipModel>()
                    {
                        new StarshipModel()
                        {
                            Name = "Rebel transport",
                            Megalights = "20",
                            ConsumableRate = "6 months",
                            RequiredResupplies = 11
                        }
                    }
                }
            };

            mockBusiness.Setup(x => x.GetStarships(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(starshipOutput);

            var controller = new HomeController(mockBusiness.Object);

            // Act
            var result = controller.GetStarships(distance, pageNumber);

            // Assert
            Assert.IsAssignableFrom<WebAPIOutputModel<StarshipViewModel>>(result);
            Assert.Equal(
                starshipOutput.DataList.FirstOrDefault().Starships.FirstOrDefault().RequiredResupplies,
                result.DataList.FirstOrDefault().Starships.FirstOrDefault().RequiredResupplies);
            mockBusiness.Verify(x => x.GetStarships(
                It.Is<int>(x => x == distance),
                It.Is<int>(x => x == pageNumber)));
        }

        [Fact]
        public void TestError_WithoutData_ShouldReturnOK()
        {
            // Arrange
            var mockBusiness = new Mock<IStarshipBusiness>();
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
