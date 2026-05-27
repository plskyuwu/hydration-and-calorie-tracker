using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using hydration_and_calorie_tracker.Helpers;
using hydration_and_calorie_tracker.Models.Database;

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
    private async Task DeleteAllBrokenEntries()
    {
        var result = await DialogHelper.ShowWarningDialog(
            "Are you sure you want to delete all broken entries?\n\nThis action cannot be undone.");

        if (result != DialogOption.Yes) return;

        var count = _trackingService.DeleteAllBrokenEntries();

        await DialogHelper.ShowInfoDialog($"Deleted {count} entries.");
    }

    [RelayCommand]
    private async Task DeleteAllEntries()
    {
        var result = await DialogHelper.ShowWarningDialog(
            "Are you sure you want to delete all entries?\n\nThis action cannot be undone.");

        if (result != DialogOption.Yes) return;

        var count = _trackingService.DeleteAllEntries();

        await DialogHelper.ShowInfoDialog($"Deleted {count} entries.");
    }

    [RelayCommand]
    private async Task DeleteAllCustomItems()
    {
        var result = await DialogHelper.ShowWarningDialog(
            "Are you sure you want to delete all custom items?\n\nThis action cannot be undone.");

        if (result != DialogOption.Yes) return;

        var deleteEntries = await DialogHelper.ShowWarningDialog(
            "Do you also want to delete all broken entries?\n\nThis action cannot be undone.");

        var count =
            _trackingService.DeleteAllItems(deleteEntries == DialogOption.Yes);

        await DialogHelper.ShowInfoDialog(
            $"Deleted {count.items} items and {count.entries} entries.");
    }

    [RelayCommand]
    private async Task DeleteAllData()
    {
        var result = await DialogHelper.ShowWarningDialog(
            "Are you sure you want to delete all data?\n\nThis action cannot be undone.");

        if (result != DialogOption.Yes) return;

        var count = _trackingService.DeleteAllData();

        await DialogHelper.ShowInfoDialog(
            $"Deleted {count.items} items and {count.entries} entries.");
    }
}