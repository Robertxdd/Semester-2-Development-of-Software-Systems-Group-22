using Avalonia.Controls;
using Avalonia.Interactivity;
using HeatProductionSystem.ViewModels;


namespace HeatProductionSystem.Views;

public partial class ProductionUnitsView : ContentControl
{
    public ProductionUnitsView()
    {
        InitializeComponent();
    }

    private void RadioButton_Checked(object? sender, RoutedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.Tag is string value && DataContext is ProductionUnitsViewModel vm)
        {  
            vm.SelectedScenario = value;    
        }
    }
}