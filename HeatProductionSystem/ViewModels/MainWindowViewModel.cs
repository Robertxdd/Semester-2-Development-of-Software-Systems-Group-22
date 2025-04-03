using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HeatProductionSystem.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]    // Creates the eaxct same object, that is then capitalized, and must then be referenced for UI updating
    private ViewModelBase selectedView;
    private ViewModelBase productionUnitsView;
    private ViewModelBase dashboardView;
    private ViewModelBase optimizerView;
 
    public MainWindowViewModel()
    {
        
        dashboardView = new DashboardViewModel();
        optimizerView = new OptimizerViewModel();
        productionUnitsView = new ProductionUnitsViewModel();
  

        selectedView = dashboardView; 
    }
    
    [RelayCommand]
    public void ChangeView(string viewID)
    {
        int.TryParse(viewID, out int _viewID);
        
        switch (_viewID)
        {
            case 0:
                SelectedView = dashboardView;
                break;
   
            case 1:
                SelectedView = optimizerView;
                break;
            case 2:
                SelectedView = productionUnitsView;
                break;

            case 3:
                //Maybe to be implemented later: 
                //A function that properly shuts down the program
                Environment.Exit(0);
                break;
            default:
                break;
        }
    }
}