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

namespace HeatProductionSystem.Models;

public abstract class Chart
{   
    public ObservableCollection<string> TimeLabels { get; } = new();
    
    public ISeries[] Series { get; set; }

    public Axis[] XAxis { get; set; }

    public Axis[] YAxis { get; set; }

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