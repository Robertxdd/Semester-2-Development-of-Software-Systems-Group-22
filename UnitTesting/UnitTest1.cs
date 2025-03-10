
using Xunit;
using HeatProductionSystem.Models;

namespace HeatProductionSystem;


public class UnitTest_ProductionUnits
{

    [Fact]
    public void SetHeatOutput_SetCurrentHeatOutputToValidPercentageAndIsActiveToFalse()
    {
        // Arange
        var unit = new GasBoiler(){MaxHeatOutput = 4.0};

        // Act
        unit.SetHeatOutput(50);

        // Assert
        Assert.Equal(2, unit.CurrentHeatOutput);
        Assert.True(unit.IsActive);
    }

    [Fact]
    public void SetHeatOutput_InvalidPertencageShouldGiveArgumentOufOfRangeException()
    {
        // Arange
        var unit = new GasBoiler(){MaxHeatOutput = 4.0};
        var unit2 = new GasBoiler(){MaxHeatOutput = 4.0};
        

        // Act and Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => unit.SetHeatOutput(-10));
        Assert.Throws<ArgumentOutOfRangeException>(() => unit2.SetHeatOutput(110));
    }


    [Fact]
    public void TurnOff_SetCurrentHeatOutputToZeroAndIsActiveToOff()
    {
        // Arrange
        var unit = new GasBoiler(){MaxHeatOutput = 4.0};
        unit.SetHeatOutput(50);
        
        // Act
        unit.TurnOff();

        // Assert
        Assert.Equal(0, unit.CurrentHeatOutput);
        Assert.False(unit.IsActive);
    }

}
