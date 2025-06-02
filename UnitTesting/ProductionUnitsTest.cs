
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
    public void ObservableCollection_InstantiatesScenario1ProductionUnitsCorrectly()
    {
        AppEnvironment.IsTestMode = true; // Ensures the Bitmap Image does not load (it does not work in unit testing)
        
        // Arange - Instantiates test units with the correct specifications provided for Scenario 1 and places them in an observable collection
        var unit1 = new GasBoiler { Name = "GB1", MaxHeatOutput = 4.0, ProductionCost = 520, CO2Emissions = 175, FuelConsumption = 0.9 };  
        var unit2 = new GasBoiler { Name = "GB2", MaxHeatOutput = 3.0, ProductionCost = 560, CO2Emissions = 130, FuelConsumption = 0.7 };
        var unit3 = new OilBoiler { Name = "OB1", MaxHeatOutput = 4.0, ProductionCost = 670, CO2Emissions = 330, FuelConsumption = 1.5 };

        var test = new ObservableCollection<ProductionUnits>() {unit1, unit2, unit3 };

        // Act - Instantiates our production units 
        
        // The production units are added already since they are static

        // Assert - Compares our production units with the test units to check that their specifications are correct
        Assert.Equal(test.Select(x => x.Name), AssetManager.scenario1Units.Select(x => x.Name));                       
        Assert.Equal(test.Select(x => x.MaxHeatOutput), AssetManager.scenario1Units.Select(x => x.MaxHeatOutput));   
        Assert.Equal(test.Select(x => x.ProductionCost), AssetManager.scenario1Units.Select(x => x.ProductionCost));
        Assert.Equal(test.Select(x => x.CO2Emissions), AssetManager.scenario1Units.Select(x => x.CO2Emissions)); 
        Assert.Equal(test.Select(x => x.FuelConsumption), AssetManager.scenario1Units.Select(x => x.FuelConsumption)); 

    }

}
