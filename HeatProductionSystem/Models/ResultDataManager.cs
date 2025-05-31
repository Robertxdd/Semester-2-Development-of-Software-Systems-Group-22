using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace HeatProductionSystem.Models;

// Result Data objects class (properties shown on the UI)
public class UnitResults
{
    internal string UnitName { get; set; }
    internal double HeatProduced { get; set; }
    internal double Cost { get; set; }
    internal double FuelConsumed { get; set; }
    internal double CO2Emissions { get; set; }
    internal double ElectricityProduced { get; set; }
}

// Result Data Manager logic
public class ResultDataManager
{
    internal static readonly Dictionary<string, List<UnitResults>> resultDataByTime = new();

    // Creates the objects for the Result Data Manager and adds them to a list
    public static void CreateResultData(string timestamp, string unitName, double heatProduced, double cost, double fuelConsumed, double co2Emissions, double electricityProduced)
    {
        if (!resultDataByTime.ContainsKey(timestamp))
        {
            resultDataByTime[timestamp] = new List<UnitResults>();
        }

        resultDataByTime[timestamp].Add(new UnitResults
        {
            UnitName = unitName,
            HeatProduced = heatProduced,
            Cost = cost,
            FuelConsumed = fuelConsumed,
            CO2Emissions = co2Emissions,
            ElectricityProduced = electricityProduced
        });
    }

    //  Writes the resultDataByTime to the csv file
    public static void SaveToCSV()
    {
        string filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "HeatProductionSystem", "Assets", "ProductionUnitResults", "Test.csv");

        using var writer = new StreamWriter(filePath);

        writer.WriteLine("Timestamp, UnitName, HeatProduced (MWh(th)), Cost (DKK), GasConsumption (MWh(Fuel)), CO2Emission (Kg), ElectricityProduced (MW)");

        foreach (var instance in resultDataByTime)
        {
            foreach (var unit in instance.Value)
            {
                writer.WriteLine($"{instance.Key}, {unit.UnitName}, {unit.HeatProduced}, {unit.Cost}, {unit.FuelConsumed}, {unit.CO2Emissions}, {unit.ElectricityProduced}");
            }
            writer.WriteLine("");
        }
    }

    public static void ClearResults()
    {
        resultDataByTime.Clear();
    }

}