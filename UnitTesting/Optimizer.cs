using System;
using HeatProductionSystem.Models;

namespace HeatProductionSystem;

public class Optimizer
{
    private HeatFetcher heatFetcher;
    private ProductionUnitsData productionUnitsData;

    HeatFetcher heat_Fetcher = new HeatFetcher();
    ProductionUnitsData production_Units_Data = new ProductionUnitsData();

    public void ReadInformation()
    {
        var _heatData = HeatFetcher.FetchHeatData();
        var _productionUnitsData = ProductionUnitsData.ProductionUnitsCollection();
    }
}
