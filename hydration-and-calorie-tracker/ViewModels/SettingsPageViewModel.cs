using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using hydration_and_calorie_tracker.Models.Database;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;

namespace hydration_and_calorie_tracker.ViewModels;

public partial class SettingsPageViewModel : ViewModelBase
{
    public string PageTitle { get; } = "Settings";

    private readonly TrackingService _trackingService;

    [ObservableProperty] private decimal? _hydrationGoal;

    [ObservableProperty] private decimal? _calorieGoal;

    public SettingsPageViewModel()
    {
        _trackingService = null!;
    }

    public SettingsPageViewModel(TrackingService trackingService)
    {
        _trackingService = trackingService;
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        HydrationGoal = _trackingService.HydrationGoal;
        CalorieGoal = _trackingService.CalorieGoal;
    }

    partial void OnHydrationGoalChanged(decimal? value)
    {
        if (value.HasValue) _trackingService.HydrationGoal = value.Value;
    }

    partial void OnCalorieGoalChanged(decimal? value)
    {
        if (value.HasValue) _trackingService.CalorieGoal = value.Value;
    }

    [RelayCommand]
    private async Task DeleteAllEntries()
    {
        var box = CreateWarningDialog(
            "Are you sure you want to delete all entries?\n\nThis action cannot be undone.");

        var result = await box.ShowAsync();

        if (result == "Yes")
        {
            _trackingService.DeleteAllEntries();
        }
    }

    [RelayCommand]
    private async Task DeleteAllCustomItems()
    {
        var box = CreateWarningDialog(
            "Are you sure you want to delete all custom items?\n\nThis action cannot be undone.");

        var result = await box.ShowAsync();

        if (result == "Yes")
        {
            _trackingService.DeleteAllItems();
        }
    }

    private static IMsBox<string> CreateWarningDialog(string contentMessage)
    {
        return MessageBoxManager.GetMessageBoxCustom(
            new MessageBoxCustomParams
            {
                ContentTitle = "Warning",
                ContentMessage = contentMessage,
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Yes", IsDefault = false },
                    new ButtonDefinition { Name = "No", IsDefault = true }
                ],
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                MinHeight = 100,
                MinWidth = 400
            });
    }
}