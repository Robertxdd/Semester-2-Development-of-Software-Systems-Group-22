using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HeatProductionSystem
{
    public class ResultDataManager
    {

        private List<OptimizationResult> optimizationResults = new List<OptimizationResult>();

        public void AddResult(string unitName, double heatProduced, double cost, double fuelConsumed)
        {

            var result = new OptimizationResult
            {
                
                UnitName = unitName, // ex GB1
                HeatProduced = heatProduced, // the used heat
                Cost = cost, // total cost
                FuelConsumed = fuelConsumed //fuel consumed
            };

            optimizationResults.Add(result);
        }


        public void SaveResultsToCsv(string unitName)
{
  
    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

    string relativeDirectory = Path.Combine(baseDirectory, @"..\..\..\Assets\ProductionUnitResults");

    // Resolve the absolute path
    string resolvedDirectory = Path.GetFullPath(relativeDirectory);

    string filePath = unitName switch
    {
        "GB1" => Path.Combine(resolvedDirectory, "GB1_Results.csv"),
        "GB2" => Path.Combine(resolvedDirectory, "GB2_Results.csv"),
        "OB1" => Path.Combine(resolvedDirectory, "OB1_Results.csv"),
        _ => throw new Exception($"Unknown unit name: {unitName}")
    };
   
    var resultsForUnit = optimizationResults.Where(r => r.UnitName == unitName).ToList();

    // Writing to CSV
    if (File.Exists(filePath)) 
    {
        using (var writer = new StreamWriter(filePath, append: true)) 
        {
            
            foreach (var result in resultsForUnit)
            {
                writer.WriteLine($"{result.UnitName},{result.HeatProduced},{result.Cost},{result.FuelConsumed}");
            }
        }

        
    }
    else
    {
        Console.WriteLine($"File does not exist: {filePath}. File cannot be saved.");
    }
}
    }
}

