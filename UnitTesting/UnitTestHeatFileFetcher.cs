using Xunit;
using HeatProductionSystem;
using System.Linq;

namespace HeatProductionSystem.Tests
{
    public class UnitTest_SourceDataManagerTest
    {
        [Fact]
        public void FetchHeatData_ReturnsFirstRowCorrectly()
        {
            // Arrange
            var expectedWinter = new HeatData
            {
                TimeFromW = "3/1/2024 0:00",
                TimeToW = "3/1/2024 1:00",
                HeatDemandW = 6.62,
                ElPriceW = 1190.9400000000001
            };

            var expectedSummer = new HeatData
            {
                TimeFromS = "8/11/2024 0:00",
                TimeToS = "8/11/2024 1:00",
                HeatDemandS = 1.79,
                ElPriceS = 752.02999999999997
            };

            // Act
        
            var firstWinter = SourceDataManager.WinterData.First();
            var firstSummer = SourceDataManager.SummerData.First();
           

            // Assert
            Assert.True(SourceDataManager.WinterData.Count == 336, "WinterData should contain at least one row.");
            Assert.True(SourceDataManager.SummerData.Count == 336, "SummerData should contain at least one row.");

            Assert.Equal(expectedWinter.TimeFromW, firstWinter.TimeFromW);
            Assert.Equal(expectedWinter.TimeToW, firstWinter.TimeToW);
            Assert.Equal(expectedWinter.HeatDemandW, firstWinter.HeatDemandW);
            Assert.Equal(expectedWinter.ElPriceW, firstWinter.ElPriceW);

            Assert.Equal(expectedSummer.TimeFromS, firstSummer.TimeFromS);
            Assert.Equal(expectedSummer.TimeToS, firstSummer.TimeToS);
            Assert.Equal(expectedSummer.HeatDemandS, firstSummer.HeatDemandS);
            Assert.Equal(expectedSummer.ElPriceS, firstSummer.ElPriceS);
        }
    }
}
