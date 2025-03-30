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
        public void FetchHeatData_ReturnsListOfHeatData()
        {
            // Arrange
            string heatFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Assets", "2025 Heat Production Optimization - Danfoss Deliveries - Source Data Manager(SDM).csv");

           
            var testData = new List<HeatData>
            {
                new HeatData
                {
                    TimeFromW = "3/1/2024 0:00",
                    TimeToW = "3/1/2024 1:00",
                    HeatDemandW = "6.62", 
                    ElPriceW = "50",
                    TimeFromS = "2025-03-01 00:00:00",
                    TimeToS = "2025-03-01 01:00:00",
                    HeatDemandS = "100",
                    ElPriceS = "60"
                }
            };

            // Act
            var actualData = HeatFetcher.FetchHeatData();

            // Assert
            Assert.NotNull(actualData); // Checking if the result is null
            Assert.IsType<List<HeatData>>(actualData); // Checking if it returns as a list
            Assert.True(actualData.Count > 0); // Check if the list contains at least one element

            // Comparing the test data with actual csv data
            Assert.Equal(testData.Select(x => x.TimeFromW), actualData.Select(x => x.TimeFromW));
            Assert.Equal(testData.Select(x => x.HeatDemandW), actualData.Select(x => x.HeatDemandW));
            Assert.Equal(testData.Select(x => x.ElPriceW), actualData.Select(x => x.ElPriceW));
            Assert.Equal(testData.Select(x => x.TimeFromS), actualData.Select(x => x.TimeFromS));
            Assert.Equal(testData.Select(x => x.TimeToS), actualData.Select(x => x.TimeToS));
            Assert.Equal(testData.Select(x => x.HeatDemandS), actualData.Select(x => x.HeatDemandS));
            Assert.Equal(testData.Select(x => x.ElPriceS), actualData.Select(x => x.ElPriceS));
        }
    }
}
