using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;


namespace HeatProductionSystem.Models;

public abstract class ProductionUnits
{
    public virtual string UnitType => GetType().Name;
    public string Name { get; set; } = "Unnamed Unit";
    public Bitmap Image { get; set; }
    public bool IsActive { get; set; } = false;  // Default is set to OFF

    public virtual double ProductionCost { get; set; }  // DKK/MWh
    public double NetProductionCost { get; set; } = 0;

    internal double MaxHeatOutput { get; set; }  // Also refered to as Max heat in provided documents (measured in MW)
    internal double CurrentHeatOutput { get; set; } = 0; // Default to 0 since unit is OFF (measured in MWh)
    internal double MaxElectricityOutput {get; set; } = 0;

    internal double FuelConsumption { get; set; }
    internal double CO2Emissions { get; set; }  // kg/MWh
   
    

    public void SetHeatOutput(double percentage)
    {
        if (percentage < 0 || percentage > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 0 and 100.");
        }

        CurrentHeatOutput = (MaxHeatOutput * percentage) / 100;
        IsActive = CurrentHeatOutput > 0;
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
    
    public GasMotor()   
    {
        if (!AppEnvironment.IsTestMode) // Needed because the Bitmap doesnt work in unit testing
        {
            Image = ImageHelper.LoadFromResource(new Uri("avares://Semester-2-Development-of-Software-Systems-Group-22/Assets/GasMotor.png"));
        }
    }
}

public class HeatPump : ProductionUnits
{
    
    public HeatPump()  
    {
        if (!AppEnvironment.IsTestMode) // Needed because the Bitmap doesnt work in unit testing
        {
            Image = ImageHelper.LoadFromResource(new Uri("avares://Semester-2-Development-of-Software-Systems-Group-22/Assets/HeatPump.png"));
        }
    }
}



public class AssetDataManager
{

    internal static ObservableCollection<ProductionUnits> baseScenario1Units { get; private set; } = new();
    internal static ObservableCollection<ProductionUnits> baseScenario2Units { get; private set; } = new();

    internal static ObservableCollection<ProductionUnits> scenario1Units { get; private set; } = new();
    internal static ObservableCollection<ProductionUnits> scenario2Units { get; private set; } = new();



    static AssetDataManager()
    {
        LoadProductionUnits(scenario1Units, "GB1", "GB2", "OB1");
        LoadProductionUnits(scenario2Units, "GB1", "OB1", "GM1", "HP1");
    }


    public static void LoadProductionUnits(ObservableCollection<ProductionUnits> targetCollection, params string[] unitNames)
    {
        string unitsFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "HeatProductionSystem", "Assets", "ProductionUnitsSpecifications.csv");

        using (var reader = new StreamReader(unitsFilePath))
        {
            while (!reader.EndOfStream)
            {
                var lineSplits = reader.ReadLine().Split(',');

                if (unitNames.Contains(lineSplits[1]))
                {
                    switch (lineSplits[1])
                    {
                        case "GB1":
                            var gasBoiler1 = new GasBoiler
                            {
                                Name = lineSplits[1],
                                MaxHeatOutput = Convert.ToDouble(lineSplits[2]),
                                ProductionCost = Convert.ToDouble(lineSplits[4]),
                                CO2Emissions = Convert.ToDouble(lineSplits[5]),
                                FuelConsumption = Convert.ToDouble(lineSplits[6])
                            };

                            targetCollection.Add(gasBoiler1);

                            if (!baseScenario1Units.Any(unit => unit.Name == "GB1"))
                                baseScenario1Units.Add(gasBoiler1);

                            if (!baseScenario2Units.Any(unit => unit.Name == "GB1"))
                                baseScenario2Units.Add(gasBoiler1);

                            break;

                        case "GB2":
                            var gasBoiler2 = new GasBoiler
                            {
                                Name = lineSplits[1],
                                MaxHeatOutput = Convert.ToDouble(lineSplits[2]),
                                ProductionCost = Convert.ToDouble(lineSplits[4]),
                                CO2Emissions = Convert.ToDouble(lineSplits[5]),
                                FuelConsumption = Convert.ToDouble(lineSplits[6])
                            };

                            targetCollection.Add(gasBoiler2);

                            if (!baseScenario1Units.Any(unit => unit.Name == "GB2"))
                                baseScenario1Units.Add(gasBoiler2);
                            break;

                        case "OB1":
                            var oilBoiler = new OilBoiler
                            {
                                Name = lineSplits[1],
                                MaxHeatOutput = Convert.ToDouble(lineSplits[2]),
                                ProductionCost = Convert.ToDouble(lineSplits[4]),
                                CO2Emissions = Convert.ToDouble(lineSplits[5]),
                                FuelConsumption = Convert.ToDouble(lineSplits[6])
                            };

                            targetCollection.Add(oilBoiler);

                            if (!baseScenario1Units.Any(unit => unit.Name == "OB1"))
                                baseScenario1Units.Add(oilBoiler);

                            if (!baseScenario2Units.Any(unit => unit.Name == "OB1"))
                                baseScenario2Units.Add(oilBoiler);
                            break;

                        case "GM1":
                            var gasMotor = new GasMotor
                            {
                                Name = lineSplits[1],
                                MaxHeatOutput = Convert.ToDouble(lineSplits[2]),
                                MaxElectricityOutput = Convert.ToDouble(lineSplits[3]),
                                ProductionCost = Convert.ToDouble(lineSplits[4]),
                                CO2Emissions = Convert.ToDouble(lineSplits[5]),
                                FuelConsumption = Convert.ToDouble(lineSplits[6])
                            };

                            targetCollection.Add(gasMotor);

                            if (!baseScenario2Units.Any(unit => unit.Name == "GM1"))
                                baseScenario2Units.Add(gasMotor);
                            break;

                        case "HP1":
                            var heatPump = new HeatPump
                            {
                                Name = lineSplits[1],
                                MaxHeatOutput = Convert.ToDouble(lineSplits[2]),
                                MaxElectricityOutput = Convert.ToDouble(lineSplits[3]),
                                ProductionCost = Convert.ToDouble(lineSplits[4])
                            };

                            targetCollection.Add(heatPump);

                            if (!baseScenario2Units.Any(unit => unit.Name == "HP1"))
                                baseScenario2Units.Add(heatPump);
                            break;
                    }
                }
            }
        }
    }
}



    