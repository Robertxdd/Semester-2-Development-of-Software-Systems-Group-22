using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using HeatProductionSystem.Models;

namespace HeatProductionSystem.ViewModels;

public partial class ProductionUnitsViewModel : ViewModelBase
{
    public ObservableCollection<string> AvailableScenarios { get; } = new()
    {
        "Scenario 1",
        "Scenario 2"
    };
    
    [ObservableProperty]
    private string selectedScenario;
    partial void OnSelectedScenarioChanged(string value) // Automatically runs every time SelectedScenario is changed
    {
        LoadScenario(value);
        ButtonExpanded = false;
    }

    [ObservableProperty]
    private bool buttonExpanded = false; // The Expander button in the UI

    [ObservableProperty]
    private ObservableCollection<ProductionUnits> productionUnitList;

    public ProductionUnitsViewModel()
    {
        SelectedScenario = "Scenario 1";  // Sets default Scenario in UI to Scenario 1
    }
    

    private void LoadScenario(string? scenario)
    {
        if (scenario == "Scenario 1")
        {
            ProductionUnitList = ProductionUnitsData.ProductionUnitsCollection();
            
        }
        else if (scenario == "Scenario 2")  //Scenario 2 misses a method for object initialization, which is why theyre for now being hardcoded
        {                                   
            //Also, Creating a new Collection of ProductionUnits seems unnecessary: Make 1 collection for each scenario initially 
            //Hold references to the scenarios, and then switch between them:

            ProductionUnitList = new ObservableCollection<ProductionUnits>()
            {
                new GasBoiler(){Name = "GB1", MaxHeatOutput = 4, ProductionCost = 520, CO2Emissions = 175, FuelConsumption = 0.9f},
                new OilBoiler(){Name = "OB1", MaxHeatOutput = 4, ProductionCost = 670, CO2Emissions = 330, FuelConsumption = 1.5f},
                new GasMotor(){Name = "GM1", MaxHeatOutput = 3.5f, ProductionCost = 990, CO2Emissions = 650, FuelConsumption = 1.8f, MaxElectricity = 2.6f},
                new HeatPump(){Name = "HP1", MaxHeatOutput = 6, ProductionCost = 60, MaxElectricity = -6}
            };
        }
    }
}