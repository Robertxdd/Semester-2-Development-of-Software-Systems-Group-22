using System;
using System.Collections.ObjectModel;

namespace HeatProductionSystem;
public class HeatData 
{
public string TimeFromW { get; set; }
public string TimeToW { get; set; }
public double HeatDemandW { get; set; }
public double ElPriceW { get; set; }
public string TimeFromS { get; set; }
public string TimeToS { get; set; }
public double HeatDemandS { get; set; }
public double ElPriceS { get; set; }

}