using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace HeatProductionSystem;

public class SourceDataManager
{

    public static ObservableCollection<HeatData> WinterData { get; private set; } = new();
    public static ObservableCollection<HeatData> SummerData { get; private set; } = new();


    static SourceDataManager()
    {
        FetchHeatData();
    }


    public static void FetchHeatData() // fetching from the file and storing them in heatdata list
    {
        string heatFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "HeatProductionSystem", "Assets", "2025 Heat Production Optimization - Danfoss Deliveries - Source Data Manager(SDM).csv");



        if (!File.Exists(heatFilePath)) // added error handling 
        {
            Console.WriteLine("Error: The file can't be found.");
            throw new FileNotFoundException("The specified heat data file was not found.", heatFilePath);
        }

        using (var reader = new StreamReader(heatFilePath))
        {
            reader.ReadLine(); // for skipping the header
            reader.ReadLine();
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');


                var winterData = new HeatData
                {
                    TimeFromW = values[0],
                    TimeToW = values[1],
                    HeatDemandW = Convert.ToDouble(values[2]),
                    ElPriceW = Convert.ToDouble(values[3])

                };

                var summerData = new HeatData
                {
                    // we exclude value 4 because for there is a double comma in the csv file and it makes it an empty value
                    TimeFromS = values[5],
                    TimeToS = values[6],
                    HeatDemandS = Convert.ToDouble(values[7]),
                    ElPriceS = Convert.ToDouble(values[8])
                };

                WinterData.Add(winterData);
                SummerData.Add(summerData);
            }
        }
    }
}