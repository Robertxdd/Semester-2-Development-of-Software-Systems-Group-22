using Xunit;
using HeatProductionSystem;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace HeatProductionSystem
{
    public class UnitTest_HeatFetcherTest
    {
        [Fact]
        public void FetchHeatData_ReturnsFirstRowCorrectly()
        {
            // Arrange
            var expectedFirstRow = new HeatData
            {
                TimeFromW = "3/1/2024 0:00",
                TimeToW = "3/1/2024 1:00",
                HeatDemandW = 6.62, 
                ElPriceW = 1190.9400000000001,
                TimeFromS = "8/11/2024 0:00",
                TimeToS = "8/11/2024 1:00",
                HeatDemandS = 1.79,
                ElPriceS = 752.02999999999997
            };

            // Act
            var actualData = HeatFetcher.FetchHeatData();

            // Assert
            Assert.NotNull(actualData);
            Assert.True(actualData.Count > 0, "The fetched data should contain at least one row.");

            var firstRow = actualData.First(); // Get the first element

            Assert.Equal(expectedFirstRow.TimeFromW, firstRow.TimeFromW);
            Assert.Equal(expectedFirstRow.TimeToW, firstRow.TimeToW);
            Assert.Equal(expectedFirstRow.HeatDemandW, firstRow.HeatDemandW);
            Assert.Equal(expectedFirstRow.ElPriceW, firstRow.ElPriceW);
            Assert.Equal(expectedFirstRow.TimeFromS, firstRow.TimeFromS);
            Assert.Equal(expectedFirstRow.TimeToS, firstRow.TimeToS);
            Assert.Equal(expectedFirstRow.HeatDemandS, firstRow.HeatDemandS);
            Assert.Equal(expectedFirstRow.ElPriceS, firstRow.ElPriceS);
        }
    }
}
