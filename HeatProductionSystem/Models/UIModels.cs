using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;


namespace HeatProductionSystem.Models;

// UI Binding purpose (ResultView)
public class TimestampGroup
{
    public string Timestamp { get; set; }
    public ObservableCollection<UnitResults> Units { get; set; }
}

// UI Binding purpose (OptimizerView) -- NEDS REFACTORING
public class UnitWithArrow
{
    public ProductionUnits Unit { get; set; }
    public double ArrowPosition { get; set; }
    public double BarHeight { get; set; }
    public double HeatOutput { get; set; }

    public UnitWithArrow(ProductionUnits unit)
    {
        HeatOutput = unit.CurrentHeatOutput / unit.MaxHeatOutput * 100;
    }

    public UnitWithArrow()
    {}
}