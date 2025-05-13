using Xunit;
using HeatProductionSystem;

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

        // Assert
        //The data is not real needs to be changed with real one
        Assert.Equal(284242.4000000001, optimizer.TotalCost);
        Assert.Equal(12298.950000000006, optimizer.TotalFuelConsumption);
        Assert.Equal(2391462.5 , optimizer.TotalCO2Emissions);
        Assert.Equal(336, results.Count);

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

        // Assert
        //The data is not real needs to be changed with real one
        Assert.Equal(1584677.3942857152, optimizer.TotalCost);
        Assert.Equal(6309.000000000002, optimizer.TotalFuelConsumption);
        Assert.Equal(1226750, optimizer.TotalCO2Emissions);
        Assert.Equal(336, results.Count);
    }
}
