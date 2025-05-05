using System;
using HeatProductionSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using HeatProductionSystem.Models;

namespace HeatProductionSystem;



public class Optimizer
{
    public double TotalCost { get; private set; }
    public double TotalFuelConsumption { get; private set; }
    public double TotalCO2Emissions { get; private set; }
    
    public List<List<ProductionUnits>> Optimize(string scenario, string period) 
    {
        var optimizationResults = new List<List<ProductionUnits>>();
        var resultDataManager = new ResultDataManager();
        var usedUnits = new List<string>(); // For storing unit Names for the resultDataManager
        
        var heatData = period == "Summer" 
            ? HeatFetcher.SummerData()
            : HeatFetcher.WinterData(); 
        
        var baseProductionUnits = scenario == "Scenario 1" 
                ? ProductionUnitsData.Scenario1Units().ToList() 
                : ProductionUnitsData.Scenario2Units().ToList();

        foreach (var heatDemand in heatData)
        {   
            var productionUnits = baseProductionUnits.Select(unit => unit.Clone()).ToList();

            double heatNeeded = period == "Summer" 
                ? heatDemand.HeatDemandS 
                : heatDemand.HeatDemandW;
            
            double heatDemandElPrice = period == "Summer" 
                ? heatDemand.ElPriceS
                : heatDemand.ElPriceW;

            if (scenario == "Scenario 2")  // Calculate Net Production Cost for GasMotor and HeatPump
            {
                var GM1 = (GasMotor)productionUnits.First(unit => unit.Name == "GM1"); // GasMotor and HeatPump class has not yet been defined, which is why it doesnt work
                GM1.NetProductionCost = GM1.ProductionCost - (heatDemandElPrice * (GM1.MaxElectricityOutput / GM1.MaxHeatOutput));

                var HP1 = (HeatPump)productionUnits.First(unit => unit.Name == "HP1");
                HP1.NetProductionCost = HP1.ProductionCost + (heatDemandElPrice * (GM1.MaxElectricityOutput / GM1.MaxElectricityOutput));
            }

            productionUnits.OrderBy(unit => unit.NetProductionCost).ToList();

            foreach (var unit in productionUnits)
            {
                if (unit != null && heatNeeded > 0)
                {
                    
                    double usedHeat = Math.Min(unit.MaxHeatOutput, heatNeeded);

                    double heatPercentage = (usedHeat / unit.MaxHeatOutput) * 100;
                    unit.SetHeatOutput(heatPercentage);

                    heatNeeded -= usedHeat;
                    TotalCost += unit.CurrentHeatOutput * unit.NetProductionCost;
                    TotalFuelConsumption += unit.CurrentHeatOutput * unit.FuelConsumption;
                    TotalCO2Emissions += unit.CurrentHeatOutput * unit.CO2Emissions;

                    
                    // Store the result data for unit
                    resultDataManager.AddResult(unit.Name, usedHeat, usedHeat * unit.NetProductionCost, usedHeat * unit.FuelConsumption);
                    usedUnits.Add(unit.Name);
                }
            }   
        }

        // After processing all heat demands, save the results to CSV for each unit
        foreach (var unitName in usedUnits) 
            resultDataManager.SaveResultsToCsv(unitName);
            

        return optimizationResults;
    }
}
