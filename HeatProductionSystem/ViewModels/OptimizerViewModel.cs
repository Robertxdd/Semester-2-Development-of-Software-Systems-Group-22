using System.Collections.ObjectModel;
using Avalonia.Controls.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using HeatProductionSystem.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Linq;
using System.Threading.Tasks;
using System;
using CommunityToolkit.Mvvm.Input;






namespace HeatProductionSystem.ViewModels;

public partial class OptimizerViewModel : ViewModelBase
{
    public ISeries[] Series { get; set; }

    [ObservableProperty]
    private ISeries[] electricitySeries;

    [ObservableProperty]
    private Axis[] electricityPriceTimeXAxes;

    [ObservableProperty]
    private bool buttonExpanded = false;

    [ObservableProperty]
    private ObservableCollection<ProductionUnits> productionUnitList;

    [ObservableProperty]
    private ISeries[] heatDemandSeries;

    [ObservableProperty]
    private Axis[] timeXAxis;

    [ObservableProperty]
    private Axis[] heatYAxis;

    public OptimizerViewModel()
    {
        SelectedScenario = "Scenario 1";
        SelectedPeriod = "Winter";
        SelectedPreference = "Price";
        SelectedLiveAction = "No";

        Placeholder();
    }


    [ObservableProperty]
    public double delayInSeconds;

    [ObservableProperty]
    public double totalFuelConsumption;

    [ObservableProperty]
    public double totalCost;

    [ObservableProperty]
    public double totalCO2Emissions;
    
    [ObservableProperty]
    private string selectedScenario;

    [ObservableProperty]
    private string selectedPeriod;

    [ObservableProperty]
    private string selectedPreference;

    [ObservableProperty]
    private string selectedLiveAction;

    public ObservableCollection<ProductionUnits>  CurrentHourUnits { get; } = new(); // A collection of units to show CurrentHeatOutput on the UI

    public bool SimulationRunning = false;

    public async Task OptimizerSimulation(double delayInSeconds)
    {   
        Console.WriteLine("Simulation started");
        
        try
        {
            var optimizer = new Optimizer();
            var optimizedData = optimizer.Optimize(SelectedScenario, SelectedPeriod, SelectedPreference);

            Console.WriteLine("Optimizer created");

            foreach (var hour in optimizedData)
            {   
                if (!SimulationRunning) break; // Enables you to later be able to stop the simulation

                 CurrentHourUnits.Clear();

                foreach (var unit in hour)
                {   
                    // Recalculating CurrentHeatOutput into a percentage based on MaxHeatOuptup
                    unit.CurrentHeatOutput = unit.CurrentHeatOutput / unit.MaxHeatOutput * 100;

                    CurrentHourUnits.Add(unit);

                    // UI statistics
                    TotalFuelConsumption += optimizer.TotalFuelConsumption;
                    TotalCost += optimizer.TotalCost;
                    TotalCO2Emissions += optimizer.TotalCO2Emissions;
                }

                // Add charts here so they can update dynamically throughout the simulation


                await Task.Yield();
                await Task.Delay((int)(delayInSeconds * 1000));
            }

            Console.WriteLine("Outside loop");
        }
        
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Optimizer failed: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task Optimize()
    {
        SimulationRunning = true;
        
        Console.WriteLine("Button pressed!");
        Console.WriteLine($"SelectedPeriod = {SelectedScenario}");
        Console.WriteLine($"SelectedPeriod = {SelectedPeriod}");
        Console.WriteLine($"SelectedPeriod = {SelectedPreference}");
        
        await OptimizerSimulation(DelayInSeconds);
    }

    private void Placeholder() // Simply a placeholder for the units' CurrentHeatOutput in the UI
    {
        foreach (var unit in new ProductionUnits[] 
        {
            new GasBoiler { Name = "GB1"} ,
            new OilBoiler { Name = "OB1"} ,
            new GasMotor { Name = "GM1"} ,
            new HeatPump { Name = "HP1"} ,
        })

            CurrentHourUnits.Add(unit);   
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

            var HeatDataSeries = HeatFetcher.WinterData();
            var ElectricityPriceSeries = HeatDataSeries.Select(x => x.ElPriceW).ToArray();
            var ElectricityTimeSeries = HeatDataSeries.Select(x => x.TimeFromW).ToArray();

            electricitySeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Name = "DKK / Mwh(el)",
                    Values = ElectricityPriceSeries,
                    Stroke = new SolidColorPaint(SKColors.LightGreen, 2),
                }
            };

            electricityPriceTimeXAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Date",
                    Labels = ElectricityTimeSeries,
                }
            };

            var HeatDemandValues = new double[]
            {
                4.1, 4.1, 4.2, 4.1, 4.2, 4.6, 4.8, 5.2, 5.5, 5.3, 4.7, 4.5,
                4.4, 4.4, 4.5, 4.5, 4.6, 4.7, 4.6, 4.5, 4.5, 4.6, 4.5, 4.7
            };

            heatDemandSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = HeatDemandValues,
                    Stroke = new SolidColorPaint(SKColors.OrangeRed, 2),
                    Fill = null
                }
            };

            timeXAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Hour",
                    Labels = Enumerable.Range(0, 24).Select(x => x.ToString("00")).ToArray()
                }
            };

            heatYAxis = new Axis[]
            {
                new Axis
                {
                    Name = "MW"
                }
            };
        }
        else if (scenario == "Scenario 2")
        {
            
        }
    }
}
