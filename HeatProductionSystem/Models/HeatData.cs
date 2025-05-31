using System;
using System.Collections.ObjectModel;

namespace HeatProductionSystem;
public class HeatData 
{
internal string TimeFromW { get; set; }
internal string TimeToW { get; set; }
internal double HeatDemandW { get; set; }
internal double ElPriceW { get; set; }
internal string TimeFromS { get; set; }
internal string TimeToS { get; set; }
internal double HeatDemandS { get; set; }
internal double ElPriceS { get; set; }

}