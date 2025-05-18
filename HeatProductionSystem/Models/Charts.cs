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
            string[] splitTimeFrom = timeFrom.Split('/',' ');

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
    public ObservableCollection<List<ProductionUnits>> UsedUnits { get; } = new();
    public List<List<double>> UnitsList { get; } = new();
    
    
    public  void Update(List<ProductionUnits> unitList)
    {   
        List<ProductionUnits> ChartUnitsPerHour = new();

        List<double> UnitsDouble = new();


        foreach(var time in ResultDataManager.resultDataByTime.Keys)
        {
            TimeLabels.Add(time);
        }

        double totalHeatDemand = 0;

        // Console.WriteLine($"List length: {unitList.Count}");

        foreach (var unit in unitList)
        {   

            Console.WriteLine($"unit name: {unit.Name} - CurrentheatOutput: {unit.CurrentHeatOutput}");

            totalHeatDemand += unit.CurrentHeatOutput;

            UnitsDouble.Add(unit.CurrentHeatOutput);
            
            // ChartUnitsPerHour.Add(unit);
            
        }
        Console.WriteLine($"TotalHeatDemand: {totalHeatDemand}");
        TotalHeatDemand.Add(totalHeatDemand);
        // UsedUnits.Add(ChartUnitsPerHour);

        UnitsList.Add(UnitsDouble);

        
        
    }

    public HeatScheduleChart()
    {
        Series = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = TotalHeatDemand,
                Name = "Total",
                Stroke = new SolidColorPaint(SKColors.Black, 2),
                GeometrySize = 0,
                GeometryStroke = null,
                Fill = null
            },
            
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
                MinStep = 2 ,
                ForceStepToMin = true,  

                 
                Name = "Heat Demand (MW)",
            }
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