using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;

namespace HeatProductionSystem;

public class HeatFetcher
{
    public static List<HeatData> FetchHeatData() // fetching from the file and storing them in heatdata file
    {
        string heatFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "HeatProductionSystem", "Assets", "2025 Heat Production Optimization - Danfoss Deliveries - Source Data Manager(SDM).csv");

        List<HeatData> heatDataList = new List<HeatData>();

        using (var reader = new StreamReader(heatFilePath))
        {

            reader.ReadLine(); // read one line to skip header
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
                    HeatDemandW = values[2],
                    ElPriceW = values[3],
                    TimeFromS = values[4],
                    TimeToS = values[5],
                    HeatDemandS = values[6],
                    ElPriceS = values[7]
                };

                heatDataList.Add(heatData);
               
            }
           
        }

       
        return heatDataList; 
    }
}