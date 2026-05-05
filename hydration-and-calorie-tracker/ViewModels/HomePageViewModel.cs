using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using hydration_and_calorie_tracker.Models.Database;

namespace hydration_and_calorie_tracker.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    public string PageTitle { get; } = "Home";
    
    public string HydrationTitle { get; } = "Hydration";
    
    public string CalorieTitle { get; } = "Calories";

    [ObservableProperty] private TrackingService _trackingService;

    [ObservableProperty] private decimal _totalHydrationToday;

    [ObservableProperty] private decimal _totalCaloriesToday;

    [ObservableProperty] private decimal _hydrationGoal;

    [ObservableProperty] private decimal _calorieGoal;

    /// <summary>
    /// Parameterless constructor for Avalonia designer preview.
    /// </summary>
    public HomePageViewModel()
    {
        _trackingService = null!;
    }

    /// <summary>
    /// Initializes the view model with the given tracking service.
    /// </summary>
    /// <param name="trackingService">The service used to add and retrieve entries and goal.</param>
    public HomePageViewModel(TrackingService trackingService)
    {
        _trackingService = trackingService;
        RefreshProperties();
    }

    /// <summary>
    /// Refreshes all observable properties using data from the tracking service.
    /// </summary>
    public void RefreshProperties()
    {
        TotalHydrationToday = 1000;
        TotalCaloriesToday = TrackingService.TotalCaloriesToday;

        HydrationGoal = TrackingService.HydrationGoal;
        CalorieGoal = TrackingService.CalorieGoal;
    }

    [RelayCommand]
    private async Task AddEntry()
    {
        Console.WriteLine("Add Entry");
        RefreshProperties();
    }
}