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






namespace HeatProductionSystem.ViewModels;

public partial class OptimizerViewModel : ViewModelBase
{       
    // Event that informs Result Data Manager a new optimization has been completed and to update
    public static event Action OptimizationEvent;
    
    public ISeries[] Series { get; set; }
 
    [ObservableProperty]
    private string selectedChart;

    [ObservableProperty]
    public ISeries[] selectedSeries;

    [ObservableProperty]
    public Axis[] selectedAxis;
    
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
    private bool textBoxVisibility = false;

    public HeatScheduleChart HeatScheduleChart = new();

    public ISeries[] HeatScheduleSeries => HeatScheduleChart.Series;
    public Axis[] HeatScheduleXAxis => HeatScheduleChart.XAxis;
    public Axis[] HeatScheduleYAxis => HeatScheduleChart.YAxis;


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
                        Unit = unit ,
                        ArrowPosition = CalculateArrowPosition(unit) ,
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


                HeatScheduleChart.Update(hour);



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
            if (SelectedLiveAction == "No")
                delay = 0;

            SimulationRunning = true;
            TotalCost = TotalCO2Emissions = TotalFuelConsumption = 0;

            HeatScheduleChart.TotalHeatDemand.Clear();
            HeatScheduleChart.TimeLabels.Clear();



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

    partial void OnSelectedChartChanged(string? oldValue, string newValue)
    {
        if (newValue == oldValue) return;

        //Update the current ISeries & Axis to the newly selected chart
        switch (newValue)
        {
            case "ElectricityPrice":
                SelectedSeries = ElectricitySeries;
                SelectedAxis = ElectricityPriceTimeXAxes;
                break;
            case "HeatDemand":
                Console.WriteLine("Switched to HeatDemand Chart");
                break;
            case "HeatSchedule":
                SelectedSeries = HeatDemandSeries; // If you later separate them, use the dedicated schedule series
                SelectedAxis = TimeXAxis;
                break;
            default:
                break;
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
                foreach (var unit in new ProductionUnits[] 
                {
                    new GasBoiler { Name = "GB1"} ,
                    new GasBoiler { Name = "GB2"} ,
                    new OilBoiler { Name = "OB1"} ,
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
            else
            {
                foreach (var unit in new ProductionUnits[] 
                {
                    new GasBoiler { Name = "GB1"} ,
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
        }
    }

    // I don't think it's being used, but I'm not sure
    // private void LoadScenario(string? scenario)
    // {
    //     if (scenario == "Scenario 1")
    //     {
    //         Series = new ISeries[]
    //         {
    //             new LineSeries<double>
    //             {
    //                 Values = new double[] { 2, 4, 6, 8 },
    //                 Stroke = new SolidColorPaint(SKColors.Blue, 2)
    //             },
    //             new LineSeries<double>
    //             {
    //                 Values = new double[] { 1, 3, 5, 7 },
    //                 Stroke = new SolidColorPaint(SKColors.Red, 2)
    //             }
    //         };

    //         var HeatDataSeries = HeatFetcher.WinterData();
    //         var ElectricityPriceSeries = HeatDataSeries.Select(x => x.ElPriceW).ToArray();
    //         var ElectricityTimeSeries = HeatDataSeries.Select(x => x.TimeFromW).ToArray();

    //         electricitySeries = new ISeries[]
    //         {
    //             new ColumnSeries<double>
    //             {
    //                 Name = "DKK / Mwh(el)",
    //                 Values = ElectricityPriceSeries,
    //                 Stroke = new SolidColorPaint(SKColors.LightGreen, 2),
    //             }
    //         };

    //         electricityPriceTimeXAxes = new Axis[]
    //         {
    //             new Axis
    //             {
    //                 Name = "Date",
    //                 Labels = ElectricityTimeSeries,
    //             }
    //         };

    //         var HeatDemandValues = new double[]
    //         {
    //             4.1, 4.1, 4.2, 4.1, 4.2, 4.6, 4.8, 5.2, 5.5, 5.3, 4.7, 4.5,
    //             4.4, 4.4, 4.5, 4.5, 4.6, 4.7, 4.6, 4.5, 4.5, 4.6, 4.5, 4.7
    //         };

    //         heatDemandSeries = new ISeries[]
    //         {
    //             new LineSeries<double>
    //             {
    //                 Values = HeatDemandValues,
    //                 Stroke = new SolidColorPaint(SKColors.OrangeRed, 2),
    //                 Fill = null
    //             }
    //         };

    //         timeXAxis = new Axis[]
    //         {
    //             new Axis
    //             {
    //                 Name = "Hour",
    //                 Labels = Enumerable.Range(0, 24).Select(x => x.ToString("00")).ToArray()
    //             }
    //         };

    //         heatYAxis = new Axis[]
    //         {
    //             new Axis
    //             {
    //                 Name = "MW"
    //             }
    //         };
    //     }
    //     else if (scenario == "Scenario 2")
    //     {
            
    //     }
    // }
}
