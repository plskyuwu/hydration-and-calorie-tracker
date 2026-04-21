using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using hydration_and_calorie_tracker.Models.Database;

namespace hydration_and_calorie_tracker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";

    public ICommand ClickCommand { get; }

    private readonly TrackingService _trackingService;

    public MainWindowViewModel() : this(null!)
    {
    }

    public MainWindowViewModel(TrackingService trackingService)
    {
        _trackingService = trackingService;

        ClickCommand = new RelayCommand(OnClick);
    }

    private void OnClick()
    {
        Console.WriteLine("Button clicked");
    }
}