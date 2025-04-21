using Xunit;
using System.Collections.Generic;
using HeatProductionSystem;
using HeatProductionSystem.Models;
using Avalonia.Remote.Protocol;
using System.Collections.ObjectModel;


namespace HeatProductionSystem;

public class UnitTest_Optimizer
{
    [Fact]
    public void Optimizer_CorrectData()
    {
        AppEnvironment.IsTestMode = true;
        //arrange 
        var originalHeatData = HeatFetcher.FetchHeatData();
        var originalUnitsData = ProductionUnitsData.ProductionUnitsCollection();

        var expectedFirstRow = new HeatData
        {
            TimeFromW = "3/1/2024 0:00",
            TimeToW = "3/1/2024 1:00",
            HeatDemandW = 6.62, 
            ElPriceW = 1190.9400000000001,
            TimeFromS = "8/11/2024 0:00",
            TimeToS = "8/11/2024 1:00",
            HeatDemandS = 1.79,
            ElPriceS = 752.02999999999997
        };


        //assert
        Assert.NotNull(originalHeatData);
        Assert.True(originalHeatData.Count > 0, "The fetched data should contain at least one row.");

        var firstRow = originalHeatData.First(); // Get the first element

        Assert.Equal(expectedFirstRow.TimeFromW, firstRow.TimeFromW);
        Assert.Equal(expectedFirstRow.TimeToW, firstRow.TimeToW);
        Assert.Equal(expectedFirstRow.HeatDemandW, firstRow.HeatDemandW);
        Assert.Equal(expectedFirstRow.ElPriceW, firstRow.ElPriceW);
        Assert.Equal(expectedFirstRow.TimeFromS, firstRow.TimeFromS);
        Assert.Equal(expectedFirstRow.TimeToS, firstRow.TimeToS);
        Assert.Equal(expectedFirstRow.HeatDemandS, firstRow.HeatDemandS);
        Assert.Equal(expectedFirstRow.ElPriceS, firstRow.ElPriceS);

// Arange - Instantiates test units with the correct specifications provided for Scenario 1 and places them in an observable collection
        var unit1 = new GasBoiler { Name = "GB1", MaxHeatOutput = 4.0, ProductionCost = 520, CO2Emissions = 175, FuelConsumption = 0.9 };  
        var unit2 = new GasBoiler { Name = "GB2", MaxHeatOutput = 3.0, ProductionCost = 560, CO2Emissions = 130, FuelConsumption = 0.7 };
        var unit3 = new OilBoiler { Name = "OB1", MaxHeatOutput = 4.0, ProductionCost = 670, CO2Emissions = 330, FuelConsumption = 1.5 };

        var test = new ObservableCollection<ProductionUnits>() {unit1, unit2, unit3 }; 
 

        // Assert - Compares our production units with the test units to check that their specifications are correct
        Assert.Equal(test.Select(x => x.Name), originalUnitsData.Select(x => x.Name));                       
        Assert.Equal(test.Select(x => x.MaxHeatOutput), originalUnitsData.Select(x => x.MaxHeatOutput));   
        Assert.Equal(test.Select(x => x.ProductionCost), originalUnitsData.Select(x => x.ProductionCost));
        Assert.Equal(test.Select(x => x.CO2Emissions), originalUnitsData.Select(x => x.CO2Emissions));
        Assert.Equal(test.Select(x => x.FuelConsumption), originalUnitsData.Select(x => x.FuelConsumption));

    }

    [Fact]
    public void SetHeatOutput_ZeroAndFullPercentageShouldWorkCorrectly()
    {   
        var unit = new GasBoiler { MaxHeatOutput = 4.0 };
        var unit2 = new OilBoiler { MaxHeatOutput = 4.0 };

        unit.SetHeatOutput(0);
        unit2.SetHeatOutput(100);

        Assert.Equal(0, unit.CurrentHeatOutput);       // 0% of 4.0
        Assert.True(unit.IsActive);                    // Still considered "on"

        Assert.Equal(4, unit2.CurrentHeatOutput);      // 100% of 4.0
        Assert.True(unit2.IsActive);
    }
}    