using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;


namespace HeatProductionSystem.Models;

// UI Binding purpose (ResultView)
public class TimestampGroup
{
    internal string Timestamp { get; set; }
    internal ObservableCollection<UnitResults> Units { get; set; }
}

// UI Binding purpose (OptimizerView) -- NEDS REFACTORING
public class UnitWithArrow
{
    internal ProductionUnits Unit { get; set; }
    internal double ArrowPosition { get; set; }
    internal double BarHeight { get; set; }
    internal double HeatOutput { get; set; }

    public UnitWithArrow(ProductionUnits unit)
    {
        HeatOutput = unit.CurrentHeatOutput / unit.MaxHeatOutput * 100;
    }

    public UnitWithArrow()
    {}
}