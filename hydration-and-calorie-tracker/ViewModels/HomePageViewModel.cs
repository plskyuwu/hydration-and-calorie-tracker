using CommunityToolkit.Mvvm.ComponentModel;
using hydration_and_calorie_tracker.Models.Database;

namespace hydration_and_calorie_tracker.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    public string PageTitle { get; } = "Home";

    [ObservableProperty] private TrackingService _trackingService;

    public HomePageViewModel() : this(null!)
    {
    }

    public HomePageViewModel(TrackingService trackingService)
    {
        _trackingService = trackingService;
    }
}