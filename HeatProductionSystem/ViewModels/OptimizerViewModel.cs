using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using HeatProductionSystem.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace HeatProductionSystem.ViewModels;

public partial class OptimizerViewModel : ViewModelBase
{
    public ISeries[] Series { get; set; }
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

    public OptimizerViewModel()
    {
        SelectedScenario = "Scenario 1";  // Sets default Scenario in UI to Scenario 1
    }
    

    private void LoadScenario(string? scenario)
    {
        if (scenario == "Scenario 1")
        {
         Series = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = new double[] { 2, 4, 6, 8 },
                    Stroke = new SolidColorPaint(SKColors.Blue, 2)
                },
                new LineSeries<double>
                {
                    Values = new double[] { 1, 3, 5, 7 },
                    Stroke = new SolidColorPaint(SKColors.Red, 2)
                }
            };
            
        }
        else if (scenario == "Scenario 2")
        {
          
        }
    }
}