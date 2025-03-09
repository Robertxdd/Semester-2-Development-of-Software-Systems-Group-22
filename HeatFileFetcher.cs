using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;

namespace Semester2ProjectGroup22;

public class HeatFetcher
{
    static void Main(string[] args)
    {
        string heatFilePath = @"..\..\..\Assets\2025 Heat Production Optimization - Danfoss Deliveries - Source Data Manager(SDM).csv";

        Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

        using (var reader = new StreamReader(heatFilePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');


                var heatData = new HeatData
                {
                    TimeFromW = values[0].Trim(),
                    TimeToW = values[1].Trim(),
                    HeatDemandW = values[2].Trim(),
                    ElPriceW = values[3].Trim(),
                    TimeFromS = values[4].Trim(),
                    TimeToS = values[5].Trim(),
                    HeatDemandS = values[6].Trim(),
                    ElPriceS = values[7].Trim()
                };

               
                heatDataList.Add(heatData);

                foreach (var data in heatDataList)
            {
                Console.WriteLine($"Time From (W): {data.TimeFromW}, Time To (W): {data.TimeToW}, Heat Demand (W): {data.HeatDemandW}");
                Console.WriteLine($"Time From (S): {data.TimeFromS}, Time To (S): {data.TimeToS}, Heat Demand (S): {data.HeatDemandS}");
                Console.WriteLine($"El Price (W): {data.ElPriceW}, El Price (S): {data.ElPriceS}");
            }

            }
        }
    }
}