using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using hydration_and_calorie_tracker.Models.Database;

namespace hydration_and_calorie_tracker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";

    private readonly TrackingService _trackingService;

    [ObservableProperty] private ViewModelBase _currentViewModel;

    public MainWindowViewModel() : this(null!)
    {
    }

    public MainWindowViewModel(TrackingService trackingService)
    {
        _trackingService = trackingService;
        CurrentViewModel = new HomePageViewModel(_trackingService);
    }

    [RelayCommand]
    private void NavigateToHomePage()
    {
        if (CurrentViewModel is HomePageViewModel) return;

        CurrentViewModel = new HomePageViewModel(_trackingService);
    }

    [RelayCommand]
    private void NavigateToHistoryPage()
    {
        if (CurrentViewModel is HistoryPageViewModel) return;

        CurrentViewModel = new HistoryPageViewModel();
    }

    [RelayCommand]
    private void NavigateToItemsPage()
    {
        if (CurrentViewModel is ItemsPageViewModel) return;

        CurrentViewModel = new ItemsPageViewModel();
    }

    [RelayCommand]
    private void NavigateToSettingsPage()
    {
        if (CurrentViewModel is SettingsPageViewModel) return;

        CurrentViewModel = new SettingsPageViewModel();
    }
}