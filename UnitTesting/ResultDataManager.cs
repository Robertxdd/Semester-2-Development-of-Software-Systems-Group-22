using Xunit;
using HeatProductionSystem.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace HeatProductionSystem
{
    public class UnitTest_ResultDataManager
    {
        [Fact]
        public void SaveResultsToCsv_ReturnsFirstRowCorrectly()
        {
            // Arrange
            var expectedFirstRowGB1 = new OptimizationResult
            {
                UnitName = "GB1",
                HeatProduced = 4,
                Cost = 2080,
                FuelConsumed = 3.6
            };

            var expectedFirstRowGB2 = new OptimizationResult
            {
                UnitName = "GB2",
                HeatProduced = 2.62,
                Cost = 1467.2,
                FuelConsumed = 1.8339999999999999
            };

            var expectedFirstRowOB1 = new OptimizationResult
            {
                UnitName = "OB1",
                HeatProduced = 0.040000000000000036,
                Cost = 26.800000000000026,
                FuelConsumed = 0.06000000000000005
            };

            var manager = new ResultDataManager();

            manager.AddResult(expectedFirstRowGB1.UnitName, expectedFirstRowGB1.HeatProduced, expectedFirstRowGB1.Cost, expectedFirstRowGB1.FuelConsumed);
            manager.AddResult(expectedFirstRowGB2.UnitName, expectedFirstRowGB2.HeatProduced, expectedFirstRowGB2.Cost, expectedFirstRowGB2.FuelConsumed);
            manager.AddResult(expectedFirstRowOB1.UnitName, expectedFirstRowOB1.HeatProduced, expectedFirstRowOB1.Cost, expectedFirstRowOB1.FuelConsumed);

            // Act
            manager.SaveResultsToCsv("GB1");
            manager.SaveResultsToCsv("GB2");
            manager.SaveResultsToCsv("OB1");

            var actualRowGB1 = File.ReadAllLines("GB1.csv").FirstOrDefault();
            var actualRowGB2 = File.ReadAllLines("GB2.csv").FirstOrDefault();
            var actualRowOB1 = File.ReadAllLines("OB1.csv").FirstOrDefault();

            // Assert
            Assert.Equal($"{expectedFirstRowGB1.UnitName},{expectedFirstRowGB1.HeatProduced},{expectedFirstRowGB1.Cost},{expectedFirstRowGB1.FuelConsumed}", actualRowGB1);
            Assert.Equal($"{expectedFirstRowGB2.UnitName},{expectedFirstRowGB2.HeatProduced},{expectedFirstRowGB2.Cost},{expectedFirstRowGB2.FuelConsumed}", actualRowGB2);
            Assert.Equal($"{expectedFirstRowOB1.UnitName},{expectedFirstRowOB1.HeatProduced},{expectedFirstRowOB1.Cost},{expectedFirstRowOB1.FuelConsumed}", actualRowOB1);
        }
    }
}
