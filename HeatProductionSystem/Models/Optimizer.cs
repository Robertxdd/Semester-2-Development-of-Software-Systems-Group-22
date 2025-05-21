using System;
using HeatProductionSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using HeatProductionSystem.Models;
using HeatProductionSystem.ViewModels;
using HeatProductionSystem.Views;

namespace HeatProductionSystem;

public class Optimizer
{
    public double TotalCost { get; private set; }
    public double TotalFuelConsumption { get; private set; }
    public double TotalCO2Emissions { get; private set; }
    
    public List<double> electricityPrices = new List<double>();



    
    public List<List<ProductionUnits>> Optimize(string scenario, string period, string preference)
    {
        ResultDataManager.ClearResults();

        var optimizationResults = new List<List<ProductionUnits>>();




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

            electricityPrices.Add(heatDemandElPrice);

            string heatDemandTimestamp = period == "Summer"
                ? heatDemand.TimeFromS + " — " + heatDemand.TimeToS
                : heatDemand.TimeFromW + " — " + heatDemand.TimeToW;

            if (scenario == "Scenario 2")  // Calculate Net Production Cost for GasMotor and HeatPump
            {
                var GM1 = (GasMotor)productionUnits.First(unit => unit.Name == "GM1"); // GasMotor and HeatPump class has not yet been defined, which is why it doesnt work
                GM1.NetProductionCost = GM1.ProductionCost - (heatDemandElPrice * (GM1.MaxElectricityOutput / GM1.MaxHeatOutput));

                var HP1 = (HeatPump)productionUnits.First(unit => unit.Name == "HP1");
                HP1.NetProductionCost = HP1.ProductionCost + (heatDemandElPrice * (GM1.MaxElectricityOutput / GM1.MaxHeatOutput));
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

                    heatNeeded -= usedHeat;

                    TotalCost += unit.CurrentHeatOutput * unit.NetProductionCost;
                    TotalFuelConsumption += unit.CurrentHeatOutput * unit.FuelConsumption;
                    TotalCO2Emissions += unit.CurrentHeatOutput * unit.CO2Emissions;


                    ResultDataManager.CreateResultData(heatDemandTimestamp, unit.Name, usedHeat, usedHeat * unit.NetProductionCost, unit.FuelConsumption * usedHeat, unit.CO2Emissions * unit.CurrentHeatOutput);
                    // Store the result data for unit
                    //resultDataManager.AddResult(unit.Name, usedHeat, usedHeat * unit.NetProductionCost, usedHeat * unit.FuelConsumption);
                    //usedUnits.Add(unit.Name);

                }

            }

            optimizationResults.Add(productionUnits);




        }

        ResultDataManager.SaveToCSV();



        Console.WriteLine("Optimization has finished..");

        return optimizationResults;
    }
}

