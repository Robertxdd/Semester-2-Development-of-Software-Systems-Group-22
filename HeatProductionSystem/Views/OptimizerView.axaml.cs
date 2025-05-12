using System;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using HeatProductionSystem.ViewModels;


namespace HeatProductionSystem.Views;

public partial class OptimizerView : ContentControl
{
    public OptimizerView()
    {
        InitializeComponent();
    }

    private void RadioButton_Checked(object? sender, RoutedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.Tag is string value && DataContext is OptimizerViewModel vm)
        {
            switch (radioButton.GroupName)
            {
                case "ScenarioGroup":
                vm.SelectedScenario = value;
                break;
            case "PeriodGroup":
                vm.SelectedPeriod = value;
                break;
            case "PreferenceGroup":
                vm.SelectedPreference = value;
                break;
            case "LiveActionGroup":
                vm.SelectedLiveAction = value;
                break;
            }
        }
    }
    
}