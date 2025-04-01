using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using HeatProductionSystem.Models;

namespace HeatProductionSystem.ViewModels;

public partial class ProductionUnitsViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<ProductionUnits> productionUnits;

    public ProductionUnitsViewModel()
    {
        this.productionUnits = ProductionUnitsData.ProductionUnitsCollection();
    }
}