using System.Collections.ObjectModel;
using Avalonia.Controls.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using HeatProductionSystem.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Linq;

namespace HeatProductionSystem.ViewModels;

public partial class OptimizerViewModel : ViewModelBase
{
    public ISeries[] Series { get; set; }

    [ObservableProperty]
    private ISeries[] electricitySeries;

    [ObservableProperty]
    private Axis[] electricityPriceTimeXAxes;

    public ObservableCollection<string> AvailableScenarios { get; } = new()
    {
        "Scenario 1",
        "Scenario 2"
    };
    
    [ObservableProperty]
    private string selectedScenario;

    partial void OnSelectedScenarioChanged(string value)
    {
        LoadScenario(value);
        ButtonExpanded = false;
    }

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

            var HeatDataSeries = HeatFetcher.FetchHeatData();
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
