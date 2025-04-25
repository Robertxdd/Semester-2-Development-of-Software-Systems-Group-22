
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;



namespace HeatProductionSystem.Models;

public abstract class ProductionUnits
{
    public string Name { get; set; } = "Unnamed Unit";
    public Bitmap Image { get; set; }

    public double MaxHeatOutput { get; set; }  // Also refered to as Max heat in provided documents (measured in MW)
    public double CurrentHeatOutput { get; set; } = 0; // Default to 0 since unit is OFF (measured in MWh)

    public double FuelConsumption { get; set; }
    public double CO2Emissions { get; set; }  // kg/MWh
    public bool IsActive { get; set; } = false;  // Default is set to OFF
    public virtual double ProductionCost { get; set; }  // DKK/MWh
    public double NetProductionCost { get; set; } = 0;

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
    private double _productionCost;
    
    public override double ProductionCost 
    {
        get => _productionCost;
        set
        {
            _productionCost = value;
            NetProductionCost = value;
        }
    }

    public GasBoiler()
    {
        if (!AppEnvironment.IsTestMode) // Needed because the Bitmap doesnt work in unit testing
        {
            Image = ImageHelper.LoadFromResource(new Uri("avares://Semester-2-Development-of-Software-Systems-Group-22/Assets/GasBoiler.png"));
        }
    }
}

public class OilBoiler : ProductionUnits
{   
    private double _productionCost;

    public override double ProductionCost 
    {
        get => _productionCost;
        set
        {
            _productionCost = value;
            NetProductionCost = value;
        }
    }

    public OilBoiler()
    {
        if (!AppEnvironment.IsTestMode) // Needed because the Bitmap doesnt work in unit testing
        {
            Image = ImageHelper.LoadFromResource(new Uri("avares://Semester-2-Development-of-Software-Systems-Group-22/Assets/OilBoiler.png"));
        }
    }
}




public class ProductionUnitsData
{   
     public static ObservableCollection<ProductionUnits> Scenario1Units()
    {
        return LoadProductionUnits().Scenario1Units;
    }

    public static ObservableCollection<ProductionUnits> Scenario2Units()
    {
        return LoadProductionUnits().Scenario2Units;
    
    }


    public ObservableCollection<ProductionUnits> Units { get; set; }

    public static (ObservableCollection<ProductionUnits> Scenario1Units, ObservableCollection<ProductionUnits> Scenario2Units) LoadProductionUnits()
    {                                            
        string unitsFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "HeatProductionSystem", "Assets", "ProductionUnitsSpecifications.csv");
        
        var Scenario1Units = new ObservableCollection<ProductionUnits>();
        var Scenario2Units = new ObservableCollection<ProductionUnits>();

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
                                    
                                            
                        Scenario1Units.Add(gasBoiler);
                        break;

                    case "Oil Boiler":
                        var oilBoiler = new OilBoiler { 
                                    Name = lineSplits[1] , 
                                    MaxHeatOutput = Convert.ToDouble(lineSplits[2]) , 
                                    ProductionCost = Convert.ToDouble(lineSplits[4]) , 
                                    CO2Emissions = Convert.ToDouble(lineSplits[5]) ,
                                    FuelConsumption = Convert.ToDouble(lineSplits[6]) };
                        
                        Scenario1Units.Add(oilBoiler);
                        break;
                }
            }
            return (Scenario1Units, Scenario1Units);
        }
    }
}
    