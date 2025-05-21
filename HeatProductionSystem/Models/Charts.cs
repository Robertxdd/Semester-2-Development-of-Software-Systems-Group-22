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


public class ElectricityPriceChart : Chart
{
    public ObservableCollection<double> ElectricityPrice { get; } = new();

    public void Update(List<double> elprice, int count)
    {
        TimeStamps(count);

        // Update the chart with new data
        ElectricityPrice.Add(elprice[count]);
    }

    public ElectricityPriceChart()
    {
        // Initialize the chart with default values
        Series = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = ElectricityPrice,
                Stroke = new SolidColorPaint(SKColors.Black, 2),
                GeometryStroke = null,
                GeometrySize = 0,
            }
        };

        XAxis = new Axis[]
        {
            new Axis
            {
                Labels = TimeLabels,
                LabelsRotation = 90,
                LabelsDensity = 0,
                NameTextSize = 10,
            }
        };

        YAxis = new Axis[]
        {
            new Axis
            {
                Name = "Electricity Price",
            }
        };
    }

    public override void Clear()
    {
        base.Clear();
        ElectricityPrice.Clear();
    }

}


public class HeatScheduleChart : Chart
{

    public List<double> TotalHeatDemand { get; set; } = new();
    public Dictionary<string, StackedColumnSeries<double>> UnitMap { get; set; } = new();


    public void Update(List<ProductionUnits> unitList, int count)
    {

        TimeStamps(count);

        var UnitColors = new Dictionary<string, SKColor>
        {
            { "GB1", new SKColor(189, 138, 0) },
            { "GB2", new SKColor(122, 62, 0) },
            { "OB1", new SKColor(192, 192, 192) },
            { "GM1", new SKColor(0, 192, 192) },
            { "HP1", new SKColor(0, 0, 192) }
        };

        double totalHeatDemand = 0;

        foreach (var unit in unitList)
        {
            totalHeatDemand += unit.CurrentHeatOutput;

            var color = UnitColors.ContainsKey(unit.Name) ? UnitColors[unit.Name] : SKColors.Gray;

            if (!UnitMap.ContainsKey(unit.Name))
            {
                var newSeries = new StackedColumnSeries<double>
                {
                    Values = new ObservableCollection<double>(),
                    Name = unit.Name,
                    Fill = new SolidColorPaint(color)
                };

                UnitMap[unit.Name] = newSeries;

                // Add it to the existing Series array
                var updatedSeries = Series.ToList();
                updatedSeries.Add(newSeries);
                Series = updatedSeries.ToArray();
            }

            ((ObservableCollection<double>)UnitMap[unit.Name].Values).Add(unit.CurrentHeatOutput);

        }

        TotalHeatDemand.Add(totalHeatDemand);
    }

    public HeatScheduleChart()
    {
        Series = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = TotalHeatDemand,
                Name = "Total Heat Demand",
                Stroke = new SolidColorPaint(SKColors.Black, 2),
                GeometrySize = 0,
                Fill = null,
                GeometryStroke = null

            }
        };

        XAxis = new Axis[]
        {
            new Axis
            {
                Labels = TimeLabels,
                LabelsRotation = 90,
                LabelsDensity = 0,
                NameTextSize = 10,
            }
        };

        YAxis = new Axis[]
        {
            new Axis
            {
               MinLimit = 0,
               MaxLimit = 10,
               MinStep = 1,
               ForceStepToMin = true,


                Name = "Heat Schedule"
            }
        };
    }

    public override void Clear()
    {
        base.Clear();
        TotalHeatDemand.Clear();


        foreach (var series in UnitMap.Values)
        {
            ((ObservableCollection<double>)series.Values).Clear();
        }


        UnitMap.Clear();

        

    }


}


public class HeatDemandChart : Chart
{
    public ObservableCollection<double> TotalHeatDemand { get; } = new();

    public void Update(List<ProductionUnits> unitList, int count)
    {
        TimeStamps(count);

        double heatDemand = 0;

        foreach (var unit in unitList)
        {
            //CO2 per MW * generated heat(MW)
            heatDemand += unit.CurrentHeatOutput;
        }

        TotalHeatDemand.Add(heatDemand);
    }

    public HeatDemandChart()
    {
        Series = new ISeries[]
        {
                new LineSeries<double>
                {
                    Values = TotalHeatDemand,
                    Name = "Total Heat Demand",
                    Stroke = new SolidColorPaint(SKColors.Black, 2),
                    GeometrySize = 0,
                    GeometryStroke = null
                }
        };

        XAxis = new Axis[]
        {
                new Axis
                {
                    Labels = TimeLabels,
                    LabelsRotation = 90,
                    LabelsDensity = 0,
                    NameTextSize = 10,
                }
        };

        YAxis = new Axis[]
        {
                new Axis
                {
                    Name = "Heat Demand"
                }
        };
    }

    public override void Clear()
    {
        base.Clear();
        TotalHeatDemand.Clear();
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
            totalCO2Emissions += unit.CO2Emissions * unit.CurrentHeatOutput;
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
                    Labels = TimeLabels,
                    LabelsRotation = 90,
                    LabelsDensity = 0,
                    NameTextSize = 10,
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

    public override void Clear()
    {
        base.Clear();
        TotalCO2Emissions.Clear();
    }
}



// public class HeatScheduleChart : Chart
// {
//     public ObservableCollection<double> TotalHeatDemand { get; } = new();

//     private Dictionary<string, ObservableCollection<double>> unitHeatOutputs = new();
//     private List<StackedAreaSeries<double>> stackedSeries = new();

//     public HeatScheduleChart()
//     {
//         Series = new ISeries[]
//         {
//             new LineSeries<double>
//             {
//                 Values = TotalHeatDemand,
//                 Name = "Heat Demand",
//                 Stroke = new SolidColorPaint(SKColors.Black, 2),
//                 GeometrySize = 0,
//                 GeometryStroke = null

//             }
//         };

//         XAxis = new Axis[]
//         {
//             new Axis
//             {
//                 Labels = TimeLabels,
//                 LabelsRotation = 90,
//                 LabelsDensity = 0,
//                 NameTextSize = 10,
//             }
//         };

//         YAxis = new Axis[]
//         {
//             new Axis
//             {
//                 Name = "Heat Output (MW)"
//             }
//         };
//     }

//     public void Update(List<ProductionUnits> unitList, int count)
//     {
//         TimeStamps(count);

//         double totalHeatDemand = 0;

//         foreach (var unit in unitList)
//         {
//             totalHeatDemand += unit.CurrentHeatOutput;

//             if (!unitHeatOutputs.ContainsKey(unit.Name))
//             {
//                 unitHeatOutputs[unit.Name] = new ObservableCollection<double>();

//                 var color = GetColorForUnit(unit.Name);

//                 var newArea = new StackedAreaSeries<double>
//                 {
//                     Values = unitHeatOutputs[unit.Name],
//                     Name = unit.Name,
//                     Fill = new SolidColorPaint(color),
//                     Stroke = null,
//                     GeometrySize = 0
//                 };

//                 stackedSeries.Add(newArea);

//                 // Rebuild all series: stacked first, then demand line
//                 var allSeries = new List<ISeries>();
//                 allSeries.AddRange(stackedSeries);
//                 allSeries.Add(new LineSeries<double>
//                 {
//                     Values = TotalHeatDemand,
//                     Name = "Heat Demand",
//                     Stroke = new SolidColorPaint(SKColors.Black, 2),
//                     GeometrySize = 0
//                 });

//                 Series = allSeries.ToArray();
//             }

//             unitHeatOutputs[unit.Name].Add(unit.CurrentHeatOutput);
//         }

//         TotalHeatDemand.Add(totalHeatDemand);

//         // Pad missing data with 0 for consistency
//         foreach (var unitName in unitHeatOutputs.Keys)
//         {
//             if (!unitList.Any(u => u.Name == unitName))
//                 unitHeatOutputs[unitName].Add(0);
//         }
//     }

//     private SKColor GetColorForUnit(string unitName)
//     {
//         return unitName switch
//         {
//             "GB1" => new SKColor(189, 138, 0),    // Mustard Yellow
//             "GB2" => new SKColor(122, 62, 0),     // Dark Brown
//             "OB1" => new SKColor(192, 192, 192),  // Gray
//             "HP1" => new SKColor(0, 192, 192),
//             "GM1" => new SKColor(0, 0, 192),
//             _ => SKColors.LightGray
//         };
//     }
// }