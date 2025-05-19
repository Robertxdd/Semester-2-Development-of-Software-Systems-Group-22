using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Linq;

namespace HeatProductionSystem.Models;

public abstract class Chart
{
    public ObservableCollection<string> TimeLabels { get; set; } = new();

    public ISeries[] Series { get; set; }
    public Axis[] XAxis { get; set; }
    public Axis[] YAxis { get; set; }

    public virtual void Clear()
    {
        TimeLabels.Clear();
    }

    protected void TimeStamps(int count)
    {
        var AllTimestamps = ResultDataManager.resultDataByTime.Keys.ToList();

        if (count < AllTimestamps.Count)
        {
            string timeFrom = AllTimestamps[count].Split('—')[0];
            string[] splitTimeFrom = timeFrom.Split('/', ' ');

            if (splitTimeFrom.Length == 5)
            {
                string timestamp = splitTimeFrom[0] + "/" + splitTimeFrom[1] + " " + splitTimeFrom[3];
                TimeLabels.Add(timestamp);
            }

            else
            {
                Console.WriteLine($"Malformed timestamp: {timeFrom}");
            }
        }
    }
}





public class HeatScheduleChart : Chart
{
    public ObservableCollection<double> TotalHeatDemand { get; } = new();

    private Dictionary<string, ObservableCollection<double>> unitHeatOutputs = new();
    private List<StackedAreaSeries<double>> stackedSeries = new();

    public HeatScheduleChart()
    {
        Series = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = TotalHeatDemand,
                Name = "Heat Demand",
                Stroke = new SolidColorPaint(SKColors.Black, 2),
                GeometrySize = 0
            }
        };

        XAxis = new Axis[]
        {
            new Axis { Labels = TimeLabels }
        };

        YAxis = new Axis[]
        {
            new Axis { Name = "Heat Output (MW)" }
        };
    }

    public void Update(List<ProductionUnits> unitList, int count)
    {
        TimeStamps(count);

        double totalHeatDemand = 0;

        foreach (var unit in unitList)
        {
            totalHeatDemand += unit.CurrentHeatOutput;

            if (!unitHeatOutputs.ContainsKey(unit.Name))
            {
                unitHeatOutputs[unit.Name] = new ObservableCollection<double>();

                var color = GetColorForUnit(unit.Name);

                var newArea = new StackedAreaSeries<double>
                {
                    Values = unitHeatOutputs[unit.Name],
                    Name = unit.Name,
                    Fill = new SolidColorPaint(color),
                    Stroke = null,
                    GeometrySize = 0
                };

                stackedSeries.Add(newArea);

                // Rebuild all series: stacked first, then demand line
                var allSeries = new List<ISeries>();
                allSeries.AddRange(stackedSeries);
                allSeries.Add(new LineSeries<double>
                {
                    Values = TotalHeatDemand,
                    Name = "Heat Demand",
                    Stroke = new SolidColorPaint(SKColors.Black, 2),
                    GeometrySize = 0
                });

                Series = allSeries.ToArray();
            }

            unitHeatOutputs[unit.Name].Add(unit.CurrentHeatOutput);
        }

        TotalHeatDemand.Add(totalHeatDemand);

        // Pad missing data with 0 for consistency
        foreach (var unitName in unitHeatOutputs.Keys)
        {
            if (!unitList.Any(u => u.Name == unitName))
                unitHeatOutputs[unitName].Add(0);
        }
    }

    private SKColor GetColorForUnit(string unitName)
    {
        return unitName switch
        {
            "GB1" => new SKColor(189, 138, 0),    // Mustard Yellow
            "GB2" => new SKColor(122, 62, 0),     // Dark Brown
            "OB1" => new SKColor(192, 192, 192),  // Gray
            "HP1" => new SKColor(0, 192, 192),
            "GM1" => new SKColor(0, 0, 192),
            _ => SKColors.LightGray
        };
    }
}




public class CO2EmissionsChart : Chart
{
    public ObservableCollection<double> TotalCO2Emissions { get; } = new();

    public void Update(List<ProductionUnits> unitList, int count)
    {
        TimeStamps(count);

        double totalCO2Emissions = 0;

        foreach (var unit in unitList)
        {
            //CO2 per MW * generated heat(MW)
            double CO2Emission = unit.CO2Emissions * unit.CurrentHeatOutput;
            totalCO2Emissions += CO2Emission;
        }

        TotalCO2Emissions.Add(totalCO2Emissions);
    }

    public CO2EmissionsChart()
    {
        Series = new ISeries[]
        {
                new LineSeries<double>
                {
                    Values = TotalCO2Emissions,
                    Name = "Total CO2 Emissions",
                    Stroke = new SolidColorPaint(SKColors.Black, 2),
                    GeometrySize = 0,
                    GeometryStroke = null
                }
        };

        XAxis = new Axis[]
        {
                new Axis
                {
                    Labels = TimeLabels
                }
        };

        YAxis = new Axis[]
        {
                new Axis
                {
                    Name = "CO2 Emissions"
                }
        };
    }
}