
using System;
using System.Collections.ObjectModel;
using System.IO;



namespace HeatProductionSystem.Models;

public abstract class ProductionUnit
{
    public string Name { get; set; } = "Unnamed Unit";

    public double MaxHeatOutput { get; set; }  // Also refered to as Max heat in provided documents (measured in MW)
    public double CurrentHeatOutput { get; set; } = 0; // Default to 0 since unit is OFF (measured in MWh)

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


public class GasBoiler : ProductionUnit
{
    public double GasConsumption { get; set; }
}

public class OilBoiler : ProductionUnit
{
    public double OilConsumption { get; set; }
}


public class ProductionUnitLoader
{
    public ObservableCollection<ProductionUnit> Units { get; set; }

    public static ObservableCollection<ProductionUnit> ProductionUnitLoadData()
    {                                                                                   
        //string unitsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "ProductionUnitsSpecifications.csv"); // the path doesn't work for some reason and needs to be fixed
        string unitsFilePath = Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName; // the path doesn't work for some reason and needs to be fixed
        Console.WriteLine(unitsFilePath);

        var ProductionUnits = new ObservableCollection<ProductionUnit>();

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
                                    GasConsumption = Convert.ToDouble(lineSplits[6]) };
                                            
                        ProductionUnits.Add(gasBoiler);
                        break;

                    case "Oil Boiler":
                        var oilBoiler = new OilBoiler { 
                                    Name = lineSplits[1] , 
                                    MaxHeatOutput = Convert.ToDouble(lineSplits[2]) , 
                                    ProductionCost = Convert.ToDouble(lineSplits[4]) , 
                                    CO2Emissions = Convert.ToDouble(lineSplits[5]) ,
                                    OilConsumption = Convert.ToDouble(lineSplits[6]) };
                        
                        ProductionUnits.Add(oilBoiler);
                        break;
                }
            }
            return ProductionUnits;
        }
    }
}
