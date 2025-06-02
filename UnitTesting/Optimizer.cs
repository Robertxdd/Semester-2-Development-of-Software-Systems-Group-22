using Xunit;
using HeatProductionSystem;
using System.Linq;
using HeatProductionSystem.Models;

public class OptimizerTests
{
    [Fact]
    public void Optimizer_Scenario1SummerPriceReturnsValidResults()
    {
        AppEnvironment.IsTestMode = true; // Ensures the Bitmap Image does not load in the units (it does not work in unit testing)
       
        // Arrange
        var optimizer = new Optimizer();
        string scenario = "Scenario 1";
        string period = "Summer";
        string preference = "Price";

        // Act
        var results = optimizer.Optimize(scenario, period, preference);

        // Calculate totals from results
        double totalCost = results.Sum(unitList => unitList.Sum(unit => unit.CurrentHeatOutput * unit.NetProductionCost));
        double totalFuelConsumption = results.Sum(unitList => unitList.Sum(unit => unit.FuelConsumption * unit.CurrentHeatOutput));
        double totalCO2Emissions = results.Sum(unitList => unitList.Sum(unit => unit.CO2Emissions * unit.CurrentHeatOutput));

        // Assert - replace optimizer.Total* with calculated totals
        Assert.Equal(284242.4, totalCost, 4);
        Assert.Equal(491.958, totalFuelConsumption, 4);
        Assert.Equal(95658.5, totalCO2Emissions, 4);

    }

   

    [Fact]
    public void Optimizer_Scenario2WinterCO2ReturnsValidResults()
    {
        AppEnvironment.IsTestMode = true; // Ensures the Bitmap Image does not load in the units (it does not work in unit testing)

        // Arrange
            
        var optimizer = new Optimizer();
        string scenario = "Scenario 2";
        string period = "Winter";
        string preference = "CO2 Emissions";

        // Act
        var optimizedData = optimizer.Optimize(scenario, period, preference);

        // Calculate totals from results
        double totalCost = optimizedData.Sum(unitList => unitList.Sum(unit => unit.CurrentHeatOutput * unit.NetProductionCost));
        double totalFuelConsumption = optimizedData.Sum(unitList => unitList.Sum(unit => unit.FuelConsumption * unit.CurrentHeatOutput));
        double totalCO2Emissions = optimizedData.Sum(unitList => unitList.Sum(unit => unit.CO2Emissions * unit.CurrentHeatOutput));


        // Assert
        Assert.Equal(2040876.8, totalCost, 4);
        Assert.Equal(252.36, totalFuelConsumption, 4);
        Assert.Equal(49070, totalCO2Emissions, 4);
        Assert.Equal(336, optimizedData.Count);

    }
}
