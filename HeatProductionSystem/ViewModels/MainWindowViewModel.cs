using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HeatProductionSystem.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]    // Creates the eaxct same object, that is then capitalized, and must then be referenced for UI updating
    private ContentControl selectedView;
    private ContentControl dataView;
    private ContentControl gridAssetView;
    private ContentControl optimizerView;
    private ContentControl SettingsView;
    public MainWindowViewModel()
    {
        dataView = new Views.DataView();
        gridAssetView = new Views.GridAssetView();
        optimizerView = new Views.OptimizerView();
        SettingsView = new Views.SettingsView();

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