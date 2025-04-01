
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;
using Avalonia.Controls;



namespace HeatProductionSystem.Models;

public abstract class ProductionUnits
{
    public string Name { get; set; } = "Unnamed Unit";
    public string ImagePath { get; set; }

    public double MaxHeatOutput { get; set; }  // Also refered to as Max heat in provided documents (measured in MW)
    public double CurrentHeatOutput { get; set; } = 0; // Default to 0 since unit is OFF (measured in MWh)

    public double FuelConsumption { get; set; }
    public double ProductionCost { get; set; }  // DKK/MWh
    public double CO2Emissions { get; set; }  // kg/MWh
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


public class GasBoiler : ProductionUnits
{
    public GasBoiler()
    {
        ImagePath = Path.Combine(AppContext.BaseDirectory, "HeatProductionSystem", "Assets", "GasBoiler.png");
    }
}

public class OilBoiler : ProductionUnits
{
    public OilBoiler()
    {
        ImagePath = Path.Combine(AppContext.BaseDirectory, "HeatProductionSystem", "Assets", "OilBoiler.png");
    }
}




public class ProductionUnitsData
{
    public ObservableCollection<ProductionUnits> Units { get; set; }

    public static ObservableCollection<ProductionUnits> ProductionUnitsCollection()
    {                                            
        string unitsFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "HeatProductionSystem", "Assets", "ProductionUnitsSpecifications.csv");
        
        var ProductionUnits = new ObservableCollection<ProductionUnits>();

        using (var reader = new StreamReader(unitsFilePath))
        {
            while (!reader.EndOfStream)
            {
                var lineSplits = reader.ReadLine().Split(','); 

                switch (lineSplits[0])
                {
                    case "Gas Boiler":
                        var gasBoiler = new GasBoiler { 
                                    Name = lineSplits[1] , 
                                    MaxHeatOutput = Convert.ToDouble(lineSplits[2]) , 
                                    ProductionCost = Convert.ToDouble(lineSplits[4]) , 
                                    CO2Emissions = Convert.ToDouble(lineSplits[5]) ,
                                    FuelConsumption = Convert.ToDouble(lineSplits[6])};
                                            
                        ProductionUnits.Add(gasBoiler);
                        break;

                    case "Oil Boiler":
                        var oilBoiler = new OilBoiler { 
                                    Name = lineSplits[1] , 
                                    MaxHeatOutput = Convert.ToDouble(lineSplits[2]) , 
                                    ProductionCost = Convert.ToDouble(lineSplits[4]) , 
                                    CO2Emissions = Convert.ToDouble(lineSplits[5]) ,
                                    FuelConsumption = Convert.ToDouble(lineSplits[6]) };
                        
                        ProductionUnits.Add(oilBoiler);
                        break;
                }
            }
            return ProductionUnits;
        }
    }
}
