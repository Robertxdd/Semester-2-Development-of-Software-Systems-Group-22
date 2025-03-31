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

        using (var reader = new StreamReader(heatFilePath))
        {
            reader.ReadLine(); // for skipping the header
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
                    HeatDemandW = double.TryParse(values[2], out var heatDemandW) ? heatDemandW : 0,
                    ElPriceW = double.TryParse(values[3], out var elPriceW) ? elPriceW : 0,
                    TimeFromS = values[4],
                    TimeToS = values[5],
                    HeatDemandS = double.TryParse(values[6], out var heatDemandS) ? heatDemandS : 0,
                    ElPriceS = double.TryParse(values[7], out var elPriceS) ? elPriceS : 0
                };

                heatDataList.Add(heatData);
            }
        }

        return heatDataList;
    }
}