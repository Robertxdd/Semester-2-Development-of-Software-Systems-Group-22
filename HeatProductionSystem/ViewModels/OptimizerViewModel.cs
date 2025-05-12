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
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace HeatProductionSystem.ViewModels
{
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
            SelectedLiveAction = "Yes";

            // Initialize charts directly here
            InitializeCharts();
            Placeholder();
        }

        [ObservableProperty]
        public string delayInSeconds;

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

        [ObservableProperty]
        private bool textBoxVisibility;

        // A collection of units to show CurrentHeatOutput on the UI
        public ObservableCollection<UnitWithArrow> CurrentUnitsWithArrow { get; } = new();

        public bool SimulationRunning = false;

        public async Task OptimizerSimulation(double Delay_InSeconds)
        {
            Console.WriteLine("Simulation started");

            try
            {
                var optimizer = new Optimizer();
                var optimizedData = optimizer.Optimize(SelectedScenario, SelectedPeriod, SelectedPreference);

                foreach (var hour in optimizedData)
                {
                    // Enables you to later be able to stop the simulation
                    if (!SimulationRunning) break;

                    CurrentUnitsWithArrow.Clear();

                    foreach (var unit in hour)
                    {
                        // An ObservableCollection for showing CurrentHeatOutput in the UI
                        var unitsWithUIElements = new UnitWithArrow
                        {
                            Unit = unit,
                            ArrowPosition = CalculateArrowPosition(unit),
                            BarHeight = 164 - CalculateArrowPosition(unit)
                        };

                        CurrentUnitsWithArrow.Add(unitsWithUIElements);

                        // Recalculating CurrentHeatOutput into a percentage based on MaxHeatOutput for the UI
                        unitsWithUIElements.Unit.CurrentHeatOutput = unit.CurrentHeatOutput / unit.MaxHeatOutput * 100;

                        // UI statistics
                        TotalFuelConsumption += optimizer.TotalFuelConsumption;
                        TotalCost += optimizer.TotalCost;
                        TotalCO2Emissions += optimizer.TotalCO2Emissions;
                    }

                    // Add charts here so they can update dynamically throughout the simulation

                    LoadScenario("Scenario 1");

                    await Task.Yield();
                    await Task.Delay((int)(Delay_InSeconds * 1000));
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
            if (double.TryParse(DelayInSeconds, out double delay) || SelectedLiveAction == "No")
            {
                if (SelectedLiveAction == "No")
                    delay = 0;

                SimulationRunning = true;

                Console.WriteLine($"Selected Scenario = {SelectedScenario}");
                Console.WriteLine($"Selected Period = {SelectedPeriod}");
                Console.WriteLine($"Selected Preference = {SelectedPreference}");
                Console.WriteLine($"Selected Live-Action = {SelectedLiveAction}");
                Console.WriteLine($"Selected Delay = {delay}");

                await OptimizerSimulation(delay);
            }
            else
            {
                Console.WriteLine("Invalid input");

                // Possibly make a popup that tells you the input is invalid
            }
        }

        private double CalculateArrowPosition(ProductionUnits unit)
        {
            // 164 is the bottom most position on the Canvas while 0 is the highest

            double CurrentHeatOutputPercentage;
            double CurrentHeatOutputArrowPosition;

            CurrentHeatOutputPercentage = unit.CurrentHeatOutput / unit.MaxHeatOutput * 100;
            CurrentHeatOutputArrowPosition = 164 - (164 * (CurrentHeatOutputPercentage / 100));

            return CurrentHeatOutputArrowPosition;
        }

        partial void OnSelectedLiveActionChanged(string value)
        {
            if (value == "Yes")
                TextBoxVisibility = true;
            else
                TextBoxVisibility = false;
        }

        // Simply a placeholder for the units' CurrentHeatOutput in the UI
        private void Placeholder()
        {
            foreach (var unit in new ProductionUnits[]
            {
                new GasBoiler { Name = "GB1"} ,
                new GasBoiler { Name = "GB2"} ,
                new OilBoiler { Name = "OB1"} ,
                new GasMotor { Name = "GM1"} ,
                new HeatPump { Name = "HP1"} ,
            })
            {
                var placeholder = new UnitWithArrow
                {
                    Unit = unit,
                    ArrowPosition = 164
                };

                CurrentUnitsWithArrow.Add(placeholder);
            }
        }
  // WIP CHART
     private void InitializeCharts()
{
    var HeatDataSeriesW = HeatFetcher.WinterData();

    var TimeFromW = HeatDataSeriesW.Select(x => x.TimeFromW).ToArray();
    var HeatDemandW = HeatDataSeriesW.Select(x => x.HeatDemandW).ToArray();

    heatDemandSeries = new ISeries[]
    {
        new ColumnSeries<double>
        {
            Name = "Heat Demand (MW)",
            Values = HeatDemandW,
            Stroke = new SolidColorPaint(SKColors.Black, 2),
            Fill = new SolidColorPaint(SKColors.Black.WithAlpha(100)) 
        }
    };

    timeXAxis = new Axis[]
    {
        new Axis
        {
            Name = "Time",
            Labels = TimeFromW
        }
    };

    heatYAxis = new Axis[]
    {
        new Axis
        {
            Name = "Heat Demand (MW)"
        }
    };
}
    


        private void LoadScenario(string? scenario)
        {
            if (scenario == "Scenario 1")
            {
                // Electricity chart
                var HeatDataSeries = HeatFetcher.WinterData();
                var ElectricityPriceSeries = HeatDataSeries.Select(x => x.ElPriceW).ToArray();
                var ElectricityTimeSeries = HeatDataSeries.Select(x => x.TimeFromW).ToArray();

                ElectricitySeries = new ISeries[]
                {
                    new ColumnSeries<double>
                    {
                        Name = "DKK / Mwh(el)",
                        Values = ElectricityPriceSeries,
                        Stroke = new SolidColorPaint(SKColors.LightGreen, 2),
                    }
                };

                ElectricityPriceTimeXAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Date",
                        Labels = ElectricityTimeSeries,
                    }   
                };
            }
            else if (scenario == "Scenario 2")
            {
                // Handle scenario 2
            }
        }
    }
}
