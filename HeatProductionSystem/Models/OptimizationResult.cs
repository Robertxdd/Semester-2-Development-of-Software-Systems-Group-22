using System;
using System.Collections.ObjectModel;

namespace HeatProductionSystem;

public class OptimizationResult
{
    public required string UnitName { get; set; }
    public double HeatProduced { get; set; }
    public double Cost { get; set; }
    public double FuelConsumed { get; set; }
}