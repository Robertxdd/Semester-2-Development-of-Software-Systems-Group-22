using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using HeatProductionSystem.Models;
using System;

namespace HeatProductionSystem.ViewModels;

public partial class ProductionUnitsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string selectedScenario;

    [ObservableProperty]
    private ObservableCollection<ProductionUnits> productionUnitList = new ObservableCollection<ProductionUnits>();

    public ProductionUnitsViewModel()
    {
        SelectedScenario = "Scenario 1"; 
    }

    partial void OnSelectedScenarioChanged(string value)
    {
        if (value == "Scenario 1")
        {
            ProductionUnitList = ProductionUnitsData.Scenario1Units();
            
            foreach (var unit in ProductionUnitList)
            {
                Console.WriteLine($"Unit Name: {unit.Name}");
            }

            Console.WriteLine("");

        }

        else if (value == "Scenario 2")
        {
            ProductionUnitList = ProductionUnitsData.Scenario2Units();

            foreach (var unit in ProductionUnitList)
            {
                Console.WriteLine($"Unit Name: {unit.Name}");
            }

            Console.WriteLine("");
        }
    }

    
    
    
    
    
    
    
    
    
    
}