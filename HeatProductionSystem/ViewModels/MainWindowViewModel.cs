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
    private ViewModelBase gridAssetView;
    private ViewModelBase optimizerView;
    private ViewModelBase SettingsView;
    public MainWindowViewModel()
    {
        dataView = new DataViewViewModel();
        gridAssetView = new GridAssetViewViewModel();
        optimizerView = new OptimizerViewViewModel();
        SettingsView = new SettingsViewViewModel();

        selectedView = dataView; 
    }
    
    [RelayCommand]
    public void ChangeView(string viewID)
    {
        Console.WriteLine("CLicked");
        //Convert the input parameter to an int
        int.TryParse(viewID, out int _viewID);
        
        switch (_viewID)
        {
            case 0:
                SelectedView = dataView;
                break;
            case 1:
                SelectedView = gridAssetView;
                break;
            case 2:
                SelectedView = optimizerView;
                break;
            case 3:
                SelectedView = SettingsView;
                break;
            default:
                break;
        }
    }
}