using Xunit;
using HeatProductionSystem.Models;
using System.IO;
using System.Linq;
using System;

namespace HeatProductionSystem
{
    public class UnitTest_ResultDataManager
    {
        [Fact]
        public void SaveResultsToCsv_ReturnsFirstRowCorrectly()
        {
            // Arrange
            ResultDataManager.ClearResults();

            var expectedFirstRow = "2024-01-01 00:00, GB1, 4, 2080, 3.6, 0, 0";

            ResultDataManager.CreateResultData("2024-01-01 00:00", "GB1", 4, 2080, 3.6, 0, 0);
            ResultDataManager.CreateResultData("2024-01-01 00:00", "GB2", 2.62, 1467.2, 1.834, 0, 0);
            ResultDataManager.CreateResultData("2024-01-01 00:00", "OB1", 0.04, 26.8, 0.06, 0, 0);

            // Act
            ResultDataManager.SaveToCSV();

            string filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "HeatProductionSystem", "Assets", "ProductionUnitResults", "Test.csv");
            var allLines = File.ReadAllLines(filePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToList();

            // Assert
            Assert.True(allLines.Count >= 2, "CSV should contain at least one data row.");
            Assert.Equal(expectedFirstRow, allLines[1].Trim());
        }
    }
}
