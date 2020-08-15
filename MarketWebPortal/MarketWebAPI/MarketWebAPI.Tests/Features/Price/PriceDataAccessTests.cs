using MarketWebAPI.Common;
using MarketWebAPI.Features.Price;
using MarketWebAPI.Tests.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Text;
using Xunit;

namespace MarketWebAPI.Tests.Features.Price
{
    public class PriceDataAccessTests
    {
        [Fact]
        public void TestGetPrices_With2Records_ShouldReturn2Fees()
        {
            // Arrange
            var dataAccessHelper = new Mock<IDataAccessHelper>();
            var pricesDummyDBResultSet = new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>()
                {
                    { "pric_PriceID", 4 },
                    { "pric_Date", DateTime.Now },
                    { "pric_MarketPrice", 66.98999786m },
                    { "pric_CreatedTimeStamp", DateTime.MinValue },
                    { "pric_CreatedBy", "Michal" },
                    { "pric_UpdatedTimeStamp", DateTime.MaxValue },
                    { "pric_UpdatedBy", "John" }
                },
                new Dictionary<string, object>()
                {
                    { "pric_PriceID", 5 },
                    { "pric_Date", DateTime.MinValue },
                    { "pric_MarketPrice", 99.98999789m },
                    { "pric_CreatedTimeStamp", DateTime.Now },
                    { "pric_CreatedBy", "Michal" },
                    { "pric_UpdatedTimeStamp", DateTime.MaxValue },
                    { "pric_UpdatedBy", "John" }
                }
            };

            var mockReader = new MockDataReader(pricesDummyDBResultSet);
            dataAccessHelper.Setup(x => x.GetDataReader(
                It.IsAny<string>(),
                It.IsAny<List<DbParameter>>(),
                It.IsAny<CommandType>())).Returns(mockReader);

            var priceDataAccess = new PriceDataAccess(dataAccessHelper.Object);

            // Act
            var result = priceDataAccess.GetPrices();

            // Assert
            Assert.Equal(pricesDummyDBResultSet.Count, result.Count);
            Assert.Equal(pricesDummyDBResultSet[0]["pric_PriceID"], result[0].PriceID);
            Assert.Equal(pricesDummyDBResultSet[0]["pric_Date"], result[0].Date);
            Assert.Equal(pricesDummyDBResultSet[0]["pric_MarketPrice"], result[0].MarketPrice);
            Assert.Equal(pricesDummyDBResultSet[0]["pric_CreatedTimeStamp"], result[0].CreatedTimeStamp);
            Assert.Equal(pricesDummyDBResultSet[0]["pric_CreatedBy"], result[0].CreatedBy);
            Assert.Equal(pricesDummyDBResultSet[0]["pric_UpdatedTimeStamp"], result[0].UpdatedTimeStamp);
            Assert.Equal(pricesDummyDBResultSet[0]["pric_UpdatedBy"], result[0].UpdatedBy);

            Assert.Equal(pricesDummyDBResultSet[1]["pric_PriceID"], result[1].PriceID);
            Assert.Equal(pricesDummyDBResultSet[1]["pric_Date"], result[1].Date);
            Assert.Equal(pricesDummyDBResultSet[1]["pric_MarketPrice"], result[1].MarketPrice);
            Assert.Equal(pricesDummyDBResultSet[1]["pric_CreatedTimeStamp"], result[1].CreatedTimeStamp);
            Assert.Equal(pricesDummyDBResultSet[1]["pric_CreatedBy"], result[1].CreatedBy);
            Assert.Equal(pricesDummyDBResultSet[1]["pric_UpdatedTimeStamp"], result[1].UpdatedTimeStamp);
            Assert.Equal(pricesDummyDBResultSet[1]["pric_UpdatedBy"], result[1].UpdatedBy);
        }

        [Fact]
        public void TestInsertPrice_WithValidData_ShouldCallStoredProcedure()
        {
            // Arrange
            string createdBy = "Michal";
            var dataAccessHelper = new Mock<IDataAccessHelper>();
            var priceDataAccess = new PriceDataAccess(dataAccessHelper.Object);
            var priceInput = new Collection<PriceInputDto>()
            {
                new PriceInputDto { PriceDate = DateTime.Now, MarketPrice = 66.98999786m },
                new PriceInputDto { PriceDate = DateTime.MaxValue, MarketPrice = 99.98999789m }
            };

            // Act
            priceDataAccess.InsertPrices(createdBy, createdBy, priceInput);

            // Assert
            dataAccessHelper.Verify(x => x.ExecuteNonQuery(
                It.Is<string>(y => y == "spPriceBulkInsert"),
                It.Is<List<DbParameter>>(y => y.Count == 4 && (string)y[0].Value == createdBy),
                It.IsAny<CommandType>()));
        }
    }
}
