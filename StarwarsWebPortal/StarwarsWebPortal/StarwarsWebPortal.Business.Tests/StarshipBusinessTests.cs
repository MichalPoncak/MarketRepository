using Moq;
using StarwarsWebPortal.Data;
using StarwarsWebPortal.Models;
using StarwarsWebPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace StarwarsWebPortal.Business.Tests
{
    public class StarshipBusinessTests
    {
        [Fact]
        public void TestGetStarship_WithDataInput_ShouldCallDataAccessWithData()
        {
            // Arrange
            var mockDataAccess = new Mock<IStarshipData>();
            int distance = 1000000;
            int pageNumber = 1;
            var starshipOutput = new WebAPIOutputModel<StarshipViewModel>();
            starshipOutput.DataList = new List<StarshipViewModel>() {
                new StarshipViewModel
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
                }
            };

            mockDataAccess.Setup(x => x.GetStarships(pageNumber)).Returns(starshipOutput);
            var business = new StarshipBusiness(mockDataAccess.Object);

            // Act
            var result = business.GetStarships(distance, pageNumber);

            // Assert
            mockDataAccess.Verify(x => x.GetStarships(pageNumber));
            Assert.Equal(0, result.DataList.FirstOrDefault().PreviousPageNumber);
            Assert.Equal(1, result.DataList.FirstOrDefault().CurrentPageNumber);
            Assert.Equal(2, result.DataList.FirstOrDefault().NextPageNumber);
            Assert.Equal(11, result.DataList.FirstOrDefault().Starships[0].RequiredResupplies);
            Assert.Equal(66666, result.DataList.FirstOrDefault().Starships[1].RequiredResupplies);
            Assert.Equal(0, result.DataList.FirstOrDefault().Starships[2].RequiredResupplies);
        }
    }
}
