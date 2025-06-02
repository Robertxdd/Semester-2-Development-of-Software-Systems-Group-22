using System;
using HeatProductionSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using HeatProductionSystem.ViewModels;
using HeatProductionSystem.Views;

namespace HeatProductionSystem;

public class Optimizer
{
    public List<double> electricityPrices = new List<double>();

    public List<List<ProductionUnits>> Optimize(string scenario, string period, string preference)
    {
        ResultDataManager.ClearResults();

        var optimizationResults = new List<List<ProductionUnits>>();

        var heatData = period == "Summer"
            ? SourceDataManager.SummerData
            : SourceDataManager.WinterData;

        // Clone the base units list to avoid modifying static collections during optimization
        var baseProductionUnits = scenario == "Scenario 1"
            ? AssetManager.scenario1Units.Select(u => u.Clone()).ToList()
            : AssetManager.scenario2Units.Select(u => u.Clone()).ToList();

        foreach (var heatDemand in heatData)
        {
            // Clone again for each heat demand to avoid mutation across iterations
            var productionUnits = baseProductionUnits.Select(unit => unit.Clone()).ToList();

            double heatNeeded = period == "Summer"
                ? heatDemand.HeatDemandS
                : heatDemand.HeatDemandW;

            double heatDemandElPrice = period == "Summer"
                ? heatDemand.ElPriceS
                : heatDemand.ElPriceW;

            electricityPrices.Add(heatDemandElPrice);

            string heatDemandTimestamp = period == "Summer"
                ? heatDemand.TimeFromS + " — " + heatDemand.TimeToS
                : heatDemand.TimeFromW + " — " + heatDemand.TimeToW;

            if (productionUnits.Any(unit => unit.Name == "GM1"))  // Calculate Net Production Cost for GasMotor
            {
                var GM1 = (GasMotor)productionUnits.First(unit => unit.Name == "GM1");
                GM1.NetProductionCost = GM1.ProductionCost - (heatDemandElPrice * (GM1.MaxElectricityOutput / GM1.MaxHeatOutput));
            }

            if (productionUnits.Any(unit => unit.Name == "HP1")) // Calculate Net Production Cost for HeatPump
            {
                var HP1 = (HeatPump)productionUnits.First(unit => unit.Name == "HP1");
                HP1.NetProductionCost = HP1.ProductionCost + Math.Abs(heatDemandElPrice * (HP1.MaxElectricityOutput / HP1.MaxHeatOutput));
            }

            if (preference == "Price")
                productionUnits = productionUnits.OrderBy(unit => unit.NetProductionCost).ToList();
            else
                productionUnits = productionUnits.OrderBy(unit => unit.CO2Emissions).ToList();

            foreach (var unit in productionUnits)
            {
                if (unit != null && heatNeeded > 0)
                {
                    double usedHeat = Math.Min(unit.MaxHeatOutput, heatNeeded);
                    double heatPercentage = (usedHeat / unit.MaxHeatOutput) * 100;
                    unit.SetHeatOutput(heatPercentage);

                    var electricityProduced = unit.CurrentHeatOutput * (unit.MaxElectricityOutput / unit.MaxHeatOutput);

                    heatNeeded -= usedHeat;

                    ResultDataManager.CreateResultData(
                        heatDemandTimestamp,
                        unit.Name,
                        usedHeat,
                        usedHeat * unit.NetProductionCost,
                        unit.FuelConsumption * usedHeat,
                        unit.CO2Emissions * unit.CurrentHeatOutput,
                        electricityProduced);
                }
            }

            optimizationResults.Add(productionUnits);
        }

        ResultDataManager.SaveToCSV();

        Console.WriteLine("Optimization has finished..");

        return optimizationResults;
    }
}
