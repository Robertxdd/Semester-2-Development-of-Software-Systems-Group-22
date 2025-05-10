using CommunityToolkit.Mvvm.ComponentModel;

namespace HeatProductionSystem.Models;


public class UnitWithArrow : ObservableObject
{
    public ProductionUnits Unit { get; set; }
    public double ArrowPosition { get; set; }
    public double BarHeight { get; set; }
}