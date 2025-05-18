using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore;

namespace HeatProductionSystem.Models;

public class ElectricityPriceChart : Chart
{
    public ObservableCollection<double> ElectricityPrice { get; } = new();
    public ElectricityPriceChart()
    {
        // Initialize the chart with default values
        Series = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = ElectricityPrice,
                Fill = null,
                Stroke = new SolidColorPaint(SKColors.Blue, 2),
                GeometryFill = null,
                GeometrySize = 0,
            }
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
                Name = "Electricity Price",
            }
        };
    }

    public void Update(double elprice)
    {
        foreach (var time in ResultDataManager.resultDataByTime.Keys)
        {
            TimeLabels.Add(time);
        }

        // Update the chart with new data
        ElectricityPrice.Add(elprice);
    }
}