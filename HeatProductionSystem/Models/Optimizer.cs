using System;
using HeatProductionSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace HeatProductionSystem;

public class Optimizer
{
    public void ReadInformation()
    {
        var heatData = HeatFetcher.FetchHeatData();
        var productionData = ProductionUnitsData.ProductionUnitsCollection();
        
        double totalCost = 0;
        double totalHeatProduced = 0;

        foreach (var heatDemand in heatData)
        {
            double heatNeeded = heatDemand.HeatDemandW;
            double costForHour = 0;
            double heatProduced = 0;

            List<ProductionUnits> selectedUnits = new List<ProductionUnits>();

            // Start by using the cheapest unit first (GB1, then GB2, then OB1)
            var gb1 = productionData.FirstOrDefault(unit => unit.Name == "GB1");
            var gb2 = productionData.FirstOrDefault(unit => unit.Name == "GB2");
            var ob1 = productionData.FirstOrDefault(unit => unit.Name == "OB1");

            if (gb1 != null && heatNeeded > 0)
            {
                double usedHeat = Math.Min(gb1.MaxHeatOutput, heatNeeded);
                costForHour += usedHeat * gb1.ProductionCost;
                heatProduced += usedHeat;
                heatNeeded -= usedHeat;
                selectedUnits.Add(gb1);
            }

            if (gb2 != null && heatNeeded > 0)
            {
                double usedHeat = Math.Min(gb2.MaxHeatOutput, heatNeeded);
                costForHour += usedHeat * gb2.ProductionCost;
                heatProduced += usedHeat;
                heatNeeded -= usedHeat;
                selectedUnits.Add(gb2);
            }

            if (ob1 != null && heatNeeded > 0)
            {
                double usedHeat = Math.Min(ob1.MaxHeatOutput, heatNeeded);
                costForHour += usedHeat * ob1.ProductionCost;
                heatProduced += usedHeat;
                heatNeeded -= usedHeat;
                selectedUnits.Add(ob1);
            }

            totalCost += costForHour;
            totalHeatProduced += heatProduced;
        }
    }
}