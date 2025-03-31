using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HeatProductionSystem.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]    // Creates the eaxct same object, that is then capitalized, and must then be referenced for UI updating
    private ViewModelBase selectedView;
    private ViewModelBase dataView;
    private ViewModelBase DashboardView;
    private ViewModelBase optimizerView;
    private ViewModelBase SettingsView;
    private ViewModelBase SimulationsView;
    public MainWindowViewModel()
    {
        dataView = new DataViewViewModel();
        DashboardView = new GridAssetViewViewModel();
        optimizerView = new OptimizerViewViewModel();
        SettingsView = new SettingsViewViewModel();
        
        SimulationsView = new SimulationsViewViewModel();

        selectedView = dataView; 
    }
    
    [RelayCommand]
    public void ChangeView(string viewID)
    {
        int.TryParse(viewID, out int _viewID);
        
        switch (_viewID)
        {
            case 0:
                SelectedView = DashboardView;
                break;
            case 1:
                SelectedView = SettingsView;
                break;
            case 2:
                SelectedView = optimizerView;
                break;
            case 3:
                SelectedView = dataView;
                break;
            case 4:
                SelectedView = SimulationsView;
                break;
            case 5:
                //Maybe to be implemented later: 
                //A function that properly shuts down the program
                Environment.Exit(0);
                break;
            default:
                break;
        }
    }
}