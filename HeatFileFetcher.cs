using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;

namespace Semester2ProjectGroup22;

public class HeatFetcher
{
    public static void FetchHeatData() // fetching from the file using dictionary and storing them in heatdata file
    {
        string heatFilePath = @"..\..\..\Assets\2025 Heat Production Optimization - Danfoss Deliveries - Source Data Manager(SDM).csv";

        Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

        using (var reader = new StreamReader(heatFilePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                List<HeatData> heatDataList = new List<HeatData>();



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
    }
}