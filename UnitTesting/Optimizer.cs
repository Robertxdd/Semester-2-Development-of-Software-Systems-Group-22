using Xunit;
using HeatProductionSystem;
using System.Linq;

public class OptimizerTests
{
    [Fact]
    public void Optimize_Scenario1_Summer_Price_ReturnsValidResults()
    {
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
        Assert.Equal(568484.80000000005, totalCost, 4);
        Assert.Equal(983.91600000000005, totalFuelConsumption, 4);
        Assert.Equal(191317, totalCO2Emissions, 4);
        Assert.Equal(672, results.Count);
    }

    [Fact]
    public void Optimize_Scenario2_Winter_CO2_ReturnsValidResults()
    {
        // Arrange
        var optimizer = new Optimizer();
        string scenario = "Scenario 2";
        string period = "Winter";
        string preference = "CO2";

        // Act
        var results = optimizer.Optimize(scenario, period, preference);

        // Calculate totals from results
        double totalCost = results.Sum(unitList => unitList.Sum(unit => unit.CurrentHeatOutput * unit.NetProductionCost));
        double totalFuelConsumption = results.Sum(unitList => unitList.Sum(unit => unit.FuelConsumption * unit.CurrentHeatOutput));
        double totalCO2Emissions = results.Sum(unitList => unitList.Sum(unit => unit.CO2Emissions * unit.CurrentHeatOutput));

        // Assert
        Assert.Equal(4081753.6000000001, totalCost, 4);
        Assert.Equal(504.72000000000003, totalFuelConsumption, 4);
        Assert.Equal(98140, totalCO2Emissions, 4);
        Assert.Equal(672, results.Count);
    }
}
