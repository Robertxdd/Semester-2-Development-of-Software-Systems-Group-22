using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using HeatProductionSystem.Models;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;

namespace HeatProductionSystem.ViewModels;

public partial class ResultsViewModel : ViewModelBase
{   
    
    
    [ObservableProperty]
    private ObservableCollection<TimestampGroup> optimizationResults = new();
    
    public ResultsViewModel()
    {
        LoadResultsFromCSV();
        
        OptimizerViewModel.OptimizationEvent += LoadResultsFromCSV;
       
    }
    
    public void LoadResultsFromCSV()
    {   
        string filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "HeatProductionSystem", "Assets", "ProductionUnitResults", "Test.csv");
        
        OptimizationResults.Clear();

        if (!File.Exists(filePath))
            return;
        
        var lines = File.ReadAllLines(filePath).Skip(1);
        var groupedUnits = new Dictionary<string, TimestampGroup>();

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length >= 5)
            {
                
                
                // string unitTimestamp = parts[0];
                string[] unitTimestampParts = parts[0].Split('—');
                string unitTimestamp;
                if (unitTimestampParts.Length == 2) 
                    unitTimestamp = $"     Time from:\n{unitTimestampParts[0]}\n\n     Time To:\n{unitTimestampParts[1]} ";
                
                else
                    unitTimestamp = parts[0];
                 
                
                var unitData = new UnitResults
                {
                    UnitName = parts[1],
                    HeatProduced = Convert.ToDouble(parts[2]),
                    Cost = Convert.ToDouble(parts[3]),
                    FuelConsumed = Convert.ToDouble(parts[4])
                };
                
                if (!groupedUnits.ContainsKey(unitTimestamp))
                {
                    groupedUnits[unitTimestamp] = new TimestampGroup
                    {
                        Timestamp = unitTimestamp,
                        Units = new ObservableCollection<UnitResults>()
                    };
                }
                
                groupedUnits[unitTimestamp].Units.Add(unitData);   


            }
        }
        
        foreach (var group in groupedUnits.Values)
        {
            OptimizationResults.Add(group);
        }

    
    }
}







