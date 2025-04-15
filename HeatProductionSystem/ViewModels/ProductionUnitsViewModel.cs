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
        else if (scenario == "Scenario 2")
        {
            ProductionUnitList = new ObservableCollection<ProductionUnits>(); // empty collection
        }
    }
}