using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using HeatProductionSystem.Models;
using System;
using CommunityToolkit.Mvvm.Input;
using System.Linq;

namespace HeatProductionSystem.ViewModels;

public partial class ProductionUnitsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string selectedScenario;

    [ObservableProperty] private bool scenarioIsChecked = true;

    [ObservableProperty]
    private ObservableCollection<ProductionUnits> productionUnitList = new();

    public ProductionUnitsViewModel()
    {
        // Instantiates the production units in the scenarios

        SelectedScenario = "Scenario 1";
    }

    partial void OnSelectedScenarioChanged(string value)
    {
        if (value == "Scenario 1")
        {
            ProductionUnitList = AssetDataManager.scenario1Units;
            UpdateAddUnitList(SelectedScenario);
        }

        else if (value == "Scenario 2")
        {
            ProductionUnitList = AssetDataManager.scenario2Units;
            UpdateAddUnitList(SelectedScenario);
        }

    }



    // ————————————————————————————————————————————————————————————————————————————————————


    [ObservableProperty]
    private bool addUnitPopupIsOpen;

    [RelayCommand]
    public void AddUnitPopupButtonPressed()
    {
        AddUnitPopupIsOpen = !AddUnitPopupIsOpen;
    }

    [ObservableProperty]
    private bool removeUnitPopupIsOpen;

    [RelayCommand]
    public void RemoveUnitPopupButtonPressed()
    {
        RemoveUnitPopupIsOpen = !RemoveUnitPopupIsOpen;
    }




    [ObservableProperty]
    public ObservableCollection<string> addProductionUnitsList = new();

    [ObservableProperty]
    public ObservableCollection<string> removeProductionUnitsList = new();

    public ObservableCollection<string> allUnitNames { get; } = new()
    {
        "GB1",
        "GB2",
        "OB1",
        "GM1",
        "HP1",
    };

    [ObservableProperty]
    private string selectedAddProductionUnit;

    [ObservableProperty]
    private string selectedRemoveProductionUnit;


    [RelayCommand]
    public void AddUnitButton()
    {
        if (SelectedScenario == "Scenario 1")
        {
            AssetDataManager.LoadProductionUnits(AssetDataManager.scenario1Units, SelectedAddProductionUnit);
        }

        else if (SelectedScenario == "Scenario 2")
        {
            AssetDataManager.LoadProductionUnits(AssetDataManager.scenario2Units, SelectedAddProductionUnit);
        }

        UpdateAddUnitList(SelectedScenario);
        SelectedAddProductionUnit = AddProductionUnitsList.FirstOrDefault();

    }

    public void RemoveUnitButton()
    {

        if (SelectedScenario == "Scenario 1")
        {
            var unitToRemove = AssetDataManager.scenario1Units.FirstOrDefault(unit => unit.Name == SelectedRemoveProductionUnit);

            AssetDataManager.scenario1Units.Remove(unitToRemove);

        }

        else if (SelectedScenario == "Scenario 2")
        {
            var unitToRemove = AssetDataManager.scenario2Units.FirstOrDefault(unit => unit.Name == SelectedRemoveProductionUnit);

            AssetDataManager.scenario2Units.Remove(unitToRemove);
        }

        UpdateAddUnitList(SelectedScenario);
        SelectedAddProductionUnit = AddProductionUnitsList.FirstOrDefault();
    }



    public void UpdateAddUnitList(string scenario)
    {
        AddProductionUnitsList.Clear();
        RemoveProductionUnitsList.Clear();

        switch (scenario)
        {
            case "Scenario 1":
                foreach (string name in allUnitNames)
                {
                    if (!AssetDataManager.scenario1Units.Any(unit => unit.Name == name))
                    {
                        AddProductionUnitsList.Add(name);
                    }
                }

                foreach (var unit in AssetDataManager.scenario1Units)
                    RemoveProductionUnitsList.Add(unit.Name);

                break;

            case "Scenario 2":
                foreach (string name in allUnitNames)
                {
                    if (!AssetDataManager.scenario2Units.Any(unit => unit.Name == name))
                    {
                        AddProductionUnitsList.Add(name);
                    }
                }

                foreach (var unit in AssetDataManager.scenario2Units)
                    RemoveProductionUnitsList.Add(unit.Name);

                break;
        }

        SelectedAddProductionUnit = AddProductionUnitsList.FirstOrDefault();
        SelectedRemoveProductionUnit = RemoveProductionUnitsList.FirstOrDefault();
    }



    [ObservableProperty]
    private bool editIsVisible = false;

    [RelayCommand]
    public void EditButtonPressed()
    {
        EditIsVisible = !EditIsVisible;
    }
    
    
    
}