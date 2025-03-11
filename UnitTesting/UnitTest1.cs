
using Xunit;
using HeatProductionSystem.Models;
using System.Collections.ObjectModel;

namespace HeatProductionSystem;


public class UnitTest_ProductionUnits
{

    [Fact]
    public void SetHeatOutput_SetCurrentHeatOutputToValidPercentageAndIsActiveToTrue()
    {
        // Arange - Instantiates a Gas- and Oilboiler
        var unit = new GasBoiler(){MaxHeatOutput = 4.0};    
        var unit2 = new OilBoiler() {MaxHeatOutput = 4.0};  


        // Act - SetHeatOutput to a valid percentage
        unit.SetHeatOutput(25);     
        unit2.SetHeatOutput(75);                            


        // Assert - Checks that CurrentHeatOutput is set correctly and IsActive is set to true
        Assert.Equal(1, unit.CurrentHeatOutput);    
        Assert.True(unit.IsActive);

        Assert.Equal(3, unit2.CurrentHeatOutput);
        Assert.True(unit2.IsActive);                         
    }

    [Fact]
    public void SetHeatOutput_InvalidPertencageShouldGiveArgumentOufOfRangeException()
    {
        // Arange - Instantiates two Gas- and two OilBoilers
        var unit = new GasBoiler(){MaxHeatOutput = 4.0};    
        var unit2 = new GasBoiler(){MaxHeatOutput = 4.0};

        var unit3 = new OilBoiler(){MaxHeatOutput = 4.0};
        var unit4 = new OilBoiler(){MaxHeatOutput = 4.0};
        

        // Act and Assert - Checks that both Boiler types throws an Exception at invalid percentages
        Assert.Throws<ArgumentOutOfRangeException>(() => unit.SetHeatOutput(-10));  
        Assert.Throws<ArgumentOutOfRangeException>(() => unit2.SetHeatOutput(110));

        Assert.Throws<ArgumentOutOfRangeException>(() => unit3.SetHeatOutput(-20));
        Assert.Throws<ArgumentOutOfRangeException>(() => unit4.SetHeatOutput(120));
    }

    [Fact]
    public void TurnOff_SetCurrentHeatOutputToZeroAndIsActiveToFalse()
    {
        // Arrange - Instantiates a Gas- and Oilboiler and turns them on
        var unit = new GasBoiler(){MaxHeatOutput = 4.0};    
        unit.SetHeatOutput(50);

        var unit2 = new OilBoiler(){MaxHeatOutput = 4.0};
        unit2.SetHeatOutput(75);
        

        // Act - Turns off the Gas- and Oilboiler
        unit.TurnOff();     
        unit2.TurnOff();


        // Assert - Checks that CurrentHeatOutput is set to 0 and IsActive is set to false
        Assert.Equal(0, unit.CurrentHeatOutput);    
        Assert.False(unit.IsActive);

        Assert.Equal(0, unit2.CurrentHeatOutput);
        Assert.False(unit2.IsActive);
    }

    [Fact]
    public void ObservableCollection_InstantiatesProductionUnitsCorrectly()
    {
        // Arange - Instantiates test units with the correct specifications provided for Scenario 1 and places them in an observable collection
        var unit = new GasBoiler { Name = "GB1", MaxHeatOutput = 4.0, ProductionCost = 520, CO2Emissions = 175, GasConsumption = 0.9 };  
        var unit2 = new GasBoiler { Name = "GB2", MaxHeatOutput = 3.0, ProductionCost = 560, CO2Emissions = 130, GasConsumption = 0.7 };
        var unit3 = new OilBoiler { Name = "OB1", MaxHeatOutput = 4.0, ProductionCost = 670, CO2Emissions = 330, OilConsumption = 1.5 };

        var test = new ObservableCollection<ProductionUnit>() {unit, unit2, unit3 }; 

        // Act - Instantiates our production units 
        var ProductionUnits = ProductionUnitLoader.ProductionUnitLoadData();

        // Assert - Compares our production units with the test units to check that their specifications are correct
        Assert.Equal(test.Select(x => x.Name), ProductionUnits.Select(x => x.Name));                       
        Assert.Equal(test.Select(x => x.MaxHeatOutput), ProductionUnits.Select(x => x.MaxHeatOutput));   
        Assert.Equal(test.Select(x => x.ProductionCost), ProductionUnits.Select(x => x.ProductionCost));
        Assert.Equal(test.Select(x => x.CO2Emissions), ProductionUnits.Select(x => x.CO2Emissions)); 

        Assert.Equal(test.OfType<OilBoiler>().Select(x => x.OilConsumption), ProductionUnits.OfType<OilBoiler>().Select(x => x.OilConsumption));
        Assert.Equal(test.OfType<GasBoiler>().Select(x => x.GasConsumption), ProductionUnits.OfType<GasBoiler>().Select(x => x.GasConsumption));
    }
}
