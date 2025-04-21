using Xunit;
using System.Collections.Generic;
using HeatProductionSystem;
using HeatProductionSystem.Models;
using Avalonia.Remote.Protocol;

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
        var optimizer = new Optimizer();

        //act
        optimizer.ReadInformation();
        var actualHeatData = optimizer.HeatData;
        var actualUnitsData = optimizer.ProductionUnits;

        //assert
        Assert.Equal(originalHeatData.Count, actualHeatData.Count);
        for (int i = 0; i < originalHeatData.Count; i++)//testing HeatData information
            {
                Assert.Equal(originalHeatData[i].TimeFromW, actualHeatData[i].TimeFromW);
                Assert.Equal(originalHeatData[i].TimeToW, actualHeatData[i].TimeToW);
                Assert.Equal(originalHeatData[i].HeatDemandW, actualHeatData[i].HeatDemandW);
                Assert.Equal(originalHeatData[i].ElPriceW, actualHeatData[i].ElPriceW);
                Assert.Equal(originalHeatData[i].TimeFromS, actualHeatData[i].TimeFromS);
                Assert.Equal(originalHeatData[i].TimeToS, actualHeatData[i].TimeToS);
                Assert.Equal(originalHeatData[i].HeatDemandS, actualHeatData[i].HeatDemandS);
                Assert.Equal(originalHeatData[i].ElPriceS, actualHeatData[i].ElPriceS);
            }

        Assert.Equal(originalUnitsData.Select(x => x.Name), actualUnitsData.Select(x => x.Name));//testing ProductionUnits information                      
        Assert.Equal(originalUnitsData.Select(x => x.MaxHeatOutput), actualUnitsData.Select(x => x.MaxHeatOutput));   
        Assert.Equal(originalUnitsData.Select(x => x.ProductionCost), actualUnitsData.Select(x => x.ProductionCost));
        Assert.Equal(originalUnitsData.Select(x => x.CO2Emissions), actualUnitsData.Select(x => x.CO2Emissions)); 
        Assert.Equal(originalUnitsData.Select(x => x.FuelConsumption),actualUnitsData.Select(x => x.FuelConsumption)); 
    }
}