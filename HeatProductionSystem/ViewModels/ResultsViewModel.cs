using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HeatProductionSystem.ViewModels
{
    public partial class ResultsViewModel : ViewModelBase
    {
        
        public ObservableCollection<string> AvailableProductionUnits { get; } = new()
        {
            "Gas Boiler 1",
            "Oil Boiler",
            "Gas Boiler 2"
        };

        [ObservableProperty]
        private string selectedProductionUnit;

        [ObservableProperty]
        private ObservableCollection<string> productionUnitData = new();

        public ResultsViewModel()
        {
            
            SelectedProductionUnit = AvailableProductionUnits.FirstOrDefault();
        }

        partial void OnSelectedProductionUnitChanged(string value)
        {
            LoadDataForSelectedUnit(value);
        }

        private void LoadDataForSelectedUnit(string unit)
        {
            string baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string relativeDirectory = Path.Combine(baseDirectory, @"..\..\..\Assets\ProductionUnitResults");
            string resolvedDirectory = Path.GetFullPath(relativeDirectory);

            string filePath = unit switch
            {
                "Gas Boiler 1" => Path.Combine(resolvedDirectory, "GB1_Results.csv"),
                "Oil Boiler" => Path.Combine(resolvedDirectory, "OB1_Results.csv"),
                "Gas Boiler 2" => Path.Combine(resolvedDirectory, "GB2_Results.csv"),
                _ => string.Empty
            };

            ProductionUnitData.Clear();

            if (string.IsNullOrWhiteSpace(filePath))
            {
                ProductionUnitData.Add("Unknown unit selected: " + unit);
                return;
            }

            if (File.Exists(filePath))
{
    var lines = File.ReadAllLines(filePath).ToList();

    foreach (var line in lines)
    {
        ProductionUnitData.Add(line);
    }
}
else
{
    ProductionUnitData.Add("File not found: " + filePath);
}
        }
    }
}
