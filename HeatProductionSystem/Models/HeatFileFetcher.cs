using System;
using System.Collections.Generic;
using System.IO;

namespace HeatProductionSystem;

public class HeatFetcher
{
    public static List<HeatData> FetchHeatData() // fetching from the file and storing them in heatdata list
    {
        string heatFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "HeatProductionSystem", "Assets", "2025 Heat Production Optimization - Danfoss Deliveries - Source Data Manager(SDM).csv");

        List<HeatData> heatDataList = new List<HeatData>();

        if (!File.Exists(heatFilePath)) // added error handling 
        {
            Console.WriteLine("Error: The file can't be found.");
            return heatDataList;  
        }

        using (var reader = new StreamReader(heatFilePath))
        {
            reader.ReadLine(); // for skipping the header test 
            reader.ReadLine(); 
            reader.ReadLine(); 

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');


                var heatData = new HeatData
                {
                    TimeFromW = values[0],
                    TimeToW = values[1],
                    HeatDemandW = double.TryParse(values[2], out var heatDemandW) ? heatDemandW : 0, // the tryparse is trying to convert them if true it stores them in the variable if false they return as 0
                    ElPriceW = double.TryParse(values[3], out var elPriceW) ? elPriceW : 0,
                    // we jump value 4 because for some reason there is a double comma in the csv file and it makes it an empty value
                    TimeFromS = values[5],
                    TimeToS = values[6],
                    HeatDemandS = double.TryParse(values[7], out var heatDemandS) ? heatDemandS : 0,
                    ElPriceS = double.TryParse(values[8], out var elPriceS) ? elPriceS : 0
                };

                heatDataList.Add(heatData);
            }
        }

        return heatDataList;
    }
}