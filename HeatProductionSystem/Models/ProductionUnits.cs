using System;
using System.Collections.ObjectModel;
using System.IO;
using Avalonia.Media.Imaging;


namespace HeatProductionSystem.Models;

public abstract class ProductionUnits
{
    public virtual string UnitType => GetType().Name;
    public string Name { get; set; } = "Unnamed Unit";
    public Bitmap Image { get; set; }

    public double MaxHeatOutput { get; set; }  // Also refered to as Max heat in provided documents (measured in MW)
    public double CurrentHeatOutput { get; set; } = 0; // Default to 0 since unit is OFF (measured in MWh)
    public double MaxElectricityOutput {get; set; } = 0;

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

    // Used for cloning in the optimizer so we don't have to recreate the units all the time
    public virtual ProductionUnits Clone() 
    {
        return (ProductionUnits)this.MemberwiseClone();
    }
}


public class GasBoiler : ProductionUnits
{   
    private double _productionCost; // This is because ProductionCost and NetProductionCost is the same for this boiler
    
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
    private double _productionCost; // This is because ProductionCost and NetProductionCost is the same for this boiler

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

public class GasMotor : ProductionUnits
{
    
    public GasMotor()   //Actual Image must still be created for the Gasmotor
    {
        if (!AppEnvironment.IsTestMode) // Needed because the Bitmap doesnt work in unit testing
        {
            Image = ImageHelper.LoadFromResource(new Uri("avares://Semester-2-Development-of-Software-Systems-Group-22/Assets/GasMotor.png"));
        }
    }
}

public class HeatPump : ProductionUnits
{
    
    public HeatPump()   //Actual Image must still be created for the Heatpump
    {
        if (!AppEnvironment.IsTestMode) // Needed because the Bitmap doesnt work in unit testing
        {
            Image = ImageHelper.LoadFromResource(new Uri("avares://Semester-2-Development-of-Software-Systems-Group-22/Assets/HeatPump.png"));
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

                switch (lineSplits[1])
                {
                    case "GB1":
                        var gasBoiler1 = new GasBoiler { 
                                    Name = lineSplits[1] , 
                                    MaxHeatOutput = Convert.ToDouble(lineSplits[2]) , 
                                    ProductionCost = Convert.ToDouble(lineSplits[4]) , 
                                    CO2Emissions = Convert.ToDouble(lineSplits[5]) ,
                                    FuelConsumption = Convert.ToDouble(lineSplits[6])};
                                    
                                            
                        Scenario1Units.Add(gasBoiler1);
                        Scenario2Units.Add(gasBoiler1);
                        break;
                    
                    case "GB2":
                        var gasBoiler2 = new GasBoiler { 
                                    Name = lineSplits[1] , 
                                    MaxHeatOutput = Convert.ToDouble(lineSplits[2]) , 
                                    ProductionCost = Convert.ToDouble(lineSplits[4]) , 
                                    CO2Emissions = Convert.ToDouble(lineSplits[5]) ,
                                    FuelConsumption = Convert.ToDouble(lineSplits[6])};
                                    
                                            
                        Scenario1Units.Add(gasBoiler2);
                        break;

                    case "OB1":
                        var oilBoiler = new OilBoiler { 
                                    Name = lineSplits[1] , 
                                    MaxHeatOutput = Convert.ToDouble(lineSplits[2]) , 
                                    ProductionCost = Convert.ToDouble(lineSplits[4]) , 
                                    CO2Emissions = Convert.ToDouble(lineSplits[5]) ,
                                    FuelConsumption = Convert.ToDouble(lineSplits[6]) };
                        
                        Scenario1Units.Add(oilBoiler);
                        Scenario2Units.Add(oilBoiler);
                        break;
                    
                    case "GM1":
                        var gasMotor = new GasMotor { 
                                    Name = lineSplits[1] , 
                                    MaxHeatOutput = Convert.ToDouble(lineSplits[2]) , 
                                    MaxElectricityOutput = Convert.ToDouble(lineSplits[3]) , 
                                    ProductionCost = Convert.ToDouble(lineSplits[4]) , 
                                    CO2Emissions = Convert.ToDouble(lineSplits[5]) ,
                                    FuelConsumption = Convert.ToDouble(lineSplits[6]) };
                        
                        Scenario2Units.Add(gasMotor);
                        break;
                
                    case "HP1":
                        var heatPump = new HeatPump { 
                                    Name = lineSplits[1] , 
                                    MaxHeatOutput = Convert.ToDouble(lineSplits[2]) , 
                                    MaxElectricityOutput = Convert.ToDouble(lineSplits[3]) ,
                                    ProductionCost = Convert.ToDouble(lineSplits[4]) };
                        
                        Scenario2Units.Add(heatPump);
                        break;
                }
            }
            return (Scenario1Units, Scenario2Units);
        }
    }
}



    