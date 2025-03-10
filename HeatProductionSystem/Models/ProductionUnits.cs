
using System;
using System.Collections.ObjectModel;


namespace HeatProductionSystem.Models;

public abstract class ProductionUnit
{
    public string Name { get; set; } = "Unnamed Unit";

    public double MaxHeatOutput { get; set; }  // Also refered to as Max heat in provided documents (measured in MW)
    public double CurrentHeatOutput { get; set; }  = 0; // Default to 0 since unit is OFF (measured in MWh)

    public double ProductionCost { get; set; }  // DKK/MWh
    public double CO2Emissions { get; set;}  // kg/MWh
    public bool IsActive { get; set; } = false;  // Default is set to OFF

    public void SetHeatOutput(double percentage)
    {
        if (percentage < 0 || percentage > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 0 and 100.");
        }

        CurrentHeatOutput = (MaxHeatOutput * percentage) / 100;
        IsActive = CurrentHeatOutput > 0;
    }

    public void TurnOff()
    {
        CurrentHeatOutput = 0;
        IsActive = false;
    }
}


public class GasBoiler : ProductionUnit
{ 
    public double GasConsumption { get; set; }
}

public class OilBoiler : ProductionUnit
{
    public double OilConsumption { get; set; }
}


public class ProductionUnitViewModel
{
    public ObservableCollection<ProductionUnit> Units { get; set; }

    public ProductionUnitViewModel()
    {
        Units = new ObservableCollection<ProductionUnit>  // Instantializing the collection
        {
            new GasBoiler { Name = "GB1", MaxHeatOutput = 4.0, ProductionCost = 520, CO2Emissions = 175, GasConsumption = 0.9 } ,
            new GasBoiler { Name = "GB2", MaxHeatOutput = 3.0, ProductionCost = 560, CO2Emissions = 130, GasConsumption = 0.7 } ,
            new OilBoiler { Name = "OB1", MaxHeatOutput = 4.0, ProductionCost = 670, CO2Emissions = 330, OilConsumption = 1.5 }
        };
    }
}
