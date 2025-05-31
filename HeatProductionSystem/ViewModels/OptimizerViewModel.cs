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
using System.Diagnostics.Metrics;


namespace HeatProductionSystem.ViewModels;

public partial class OptimizerViewModel : ViewModelBase
{
    // Event that informs Result Data Manager a new optimization has been completed and to update
    public static event Action OptimizationEvent;


    [ObservableProperty]
    private bool buttonExpanded = false;


    // Setting ScenarioIsChecked to true in constructor instead allows On..Changed() method to immediately run, which is a placeholder for units
    [ObservableProperty] private bool scenarioIsChecked;
    [ObservableProperty] private bool periodIsChecked = true;
    [ObservableProperty] private bool preferenceIsChecked = true;
    [ObservableProperty] private bool liveActionIsChecked = true;


    public OptimizerViewModel()
    {
        SelectedScenario = "Scenario 1";
        SelectedPeriod = "Winter";
        SelectedPreference = "Price";
        SelectedLiveAction = "No";

        ScenarioIsChecked = true;

        SelectedSeries = HeatDemandChart.Series;
        SelectedXAxis = HeatDemandChart.XAxis;
        SelectedYAxis = HeatDemandChart.YAxis;

    }


    [ObservableProperty] public string delayInSeconds;

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
    private bool textBoxVisibility = false;


    [ObservableProperty]
    public ISeries[] selectedSeries;

    [ObservableProperty]
    public Axis[] selectedXAxis;

    [ObservableProperty]
    public Axis[] selectedYAxis;

    [ObservableProperty]
    private string selectedChart;




    public HeatDemandChart HeatDemandChart = new();
    public HeatScheduleChart HeatScheduleChart = new();
    public ElectricityPriceChart ElectricityPriceChart = new();
    public CO2EmissionsChart CO2EmissionsChart = new();


    public ISeries[] HeatDemandSeries => HeatDemandChart.Series;
    public Axis[] HeatDemandXAxis => HeatDemandChart.XAxis;
    public Axis[] HeatDemandYAxis => HeatDemandChart.YAxis;

    public ISeries[] HeatScheduleSeries => HeatScheduleChart.Series;
    public Axis[] HeatScheduleXAxis => HeatScheduleChart.XAxis;
    public Axis[] HeatScheduleYAxis => HeatScheduleChart.YAxis;

    public ISeries[] CO2EmissionsSeries => CO2EmissionsChart.Series;
    public Axis[] CO2EmissionsXAxis => CO2EmissionsChart.XAxis;
    public Axis[] CO2EmissionsYAxis => CO2EmissionsChart.YAxis;

    public ISeries[] ElectricitySeries => ElectricityPriceChart.Series;
    public Axis[] ElectricityXAxis => ElectricityPriceChart.XAxis;
    public Axis[] ElectricityYAxis => ElectricityPriceChart.YAxis;



    // A collection of units to show CurrentHeatOutput on the UI
    public ObservableCollection<UnitWithArrow> CurrentUnitsWithArrow { get; } = new();

    [ObservableProperty]
    public bool simulationRunning = false;

    public async Task OptimizerSimulation(double Delay_InSeconds)
    {
        Console.WriteLine("Simulation started");

        try
        {
            var optimizer = new Optimizer();
            var optimizedData = optimizer.Optimize(SelectedScenario, SelectedPeriod, SelectedPreference);

            int ChartCount = 0;

            foreach (var hour in optimizedData)
            {
                // Enables you to later be able to stop the simulation
                if (!SimulationRunning) break;

                CurrentUnitsWithArrow.Clear();

                foreach (var unit in hour)
                {
                    // An ObservableCollection for showing CurrentHeatOutput in the UI
                    var unitsWithUIElements = new UnitWithArrow(unit)
                    {
                        Unit = unit,
                        ArrowPosition = CalculateArrowPosition(unit),
                        BarHeight = 164 - CalculateArrowPosition(unit)
                    };

                    CurrentUnitsWithArrow.Add(unitsWithUIElements);


                    // UI statistics
                    // TotalCost += unit.CurrentHeatOutput * unit.NetProductionCost;
                    TotalFuelConsumption += unit.CurrentHeatOutput * unit.FuelConsumption;
                    TotalCO2Emissions += unit.CurrentHeatOutput * unit.CO2Emissions;
                    TotalCost += unit.CurrentHeatOutput * unit.NetProductionCost;

                }

                // Add charts here so they can update dynamically throughout the simulation


                HeatDemandChart.Update(hour, ChartCount);
                HeatScheduleChart.Update(hour, ChartCount);
                CO2EmissionsChart.Update(hour, ChartCount);
                ElectricityPriceChart.Update(optimizer.electricityPrices, ChartCount);

                ChartCount++;


                if (SelectedChart == "HeatSchedule")
                {
                    SelectedSeries = null;

                    SelectedSeries = HeatScheduleChart.Series;
                    SelectedXAxis = HeatScheduleChart.XAxis;
                    SelectedYAxis = HeatScheduleChart.YAxis;
                }




                await Task.Yield();
                await Task.Delay((int)(Delay_InSeconds * 1000));

            }

            Console.WriteLine("Outside loop");
            SimulationRunning = false;

            // Invokes event 
            OptimizationEvent?.Invoke();
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
            InvalidInput = false;

            if (SelectedLiveAction == "No")
                delay = 0;

            SimulationRunning = true;
            TotalCost = TotalCO2Emissions = TotalFuelConsumption = 0;

            HeatScheduleChart.Clear();
            HeatDemandChart.Clear();
            CO2EmissionsChart.Clear();
            ElectricityPriceChart.Clear();








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

            InvalidInput = true;

            // Possibly make a popup that tells you the input is invalid
        }
    }

    [RelayCommand]
    public async Task StopOptimize()
    {
        SimulationRunning = false;
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

    partial void OnSelectedChartChanged(string? oldValue, string newValue)
    {
        if (newValue == oldValue) return;

        switch (newValue)
        {
            case "HeatSchedule":
                SelectedSeries = HeatScheduleChart.Series;
                SelectedXAxis = HeatScheduleChart.XAxis;
                SelectedYAxis = HeatScheduleChart.YAxis;
                break;

            case "HeatDemand":
                SelectedSeries = HeatDemandChart.Series;
                SelectedXAxis = HeatDemandChart.XAxis;
                SelectedYAxis = HeatDemandChart.YAxis;
                break;

            case "CO2Emissions":
                SelectedSeries = CO2EmissionsChart.Series;
                SelectedXAxis = CO2EmissionsChart.XAxis;
                SelectedYAxis = CO2EmissionsChart.YAxis;
                break;

            case "ElectricityPrice":
                SelectedSeries = ElectricityPriceChart.Series;
                SelectedXAxis = ElectricityPriceChart.XAxis;
                SelectedYAxis = ElectricityPriceChart.YAxis;
                break;

            default:
                break;

                //Add more charts here for switching
        }
    }

    partial void OnSelectedLiveActionChanged(string value)
    {
        if (value == "Yes")
            TextBoxVisibility = true;

        else
            TextBoxVisibility = false;
    }

    // Simply a placeholder for the units' CurrentHeatOutput in the UI
    partial void OnScenarioIsCheckedChanged(bool value)
    {
        if (SimulationRunning == false)
        {
            CurrentUnitsWithArrow.Clear();

            if (value == true)
            {

                foreach (var unit in AssetDataManager.scenario1Units)
                {
                    var placeholder = new UnitWithArrow
                        {
                            Unit = unit,
                            ArrowPosition = 164
                        };

                        CurrentUnitsWithArrow.Add(placeholder);
                }
            }
            else
            {
                foreach (var unit in AssetDataManager.scenario2Units)
                {
                    var placeholder = new UnitWithArrow
                        {
                            Unit = unit,
                            ArrowPosition = 164
                        };

                        CurrentUnitsWithArrow.Add(placeholder);
                }
                
            }
        }
    }


    [ObservableProperty]
    private bool chartListIsOpen;

    [RelayCommand]
    public void ChartButtonPressed()
    {
        ChartListIsOpen = !ChartListIsOpen;
    }


    [ObservableProperty]
    private bool invalidInput;

}

