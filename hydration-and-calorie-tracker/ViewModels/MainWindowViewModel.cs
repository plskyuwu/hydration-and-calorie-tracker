using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using hydration_and_calorie_tracker.Models.Database;

namespace hydration_and_calorie_tracker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";

    public ICommand AddEntryClickCommand { get; }

    private readonly TrackingService _trackingService;

    [ObservableProperty] private ViewModelBase _currentViewModel;

    public MainWindowViewModel() : this(null!)
    {
    }

    public MainWindowViewModel(TrackingService trackingService)
    {
        _trackingService = trackingService;
        CurrentViewModel = new HomePageViewModel();

        AddEntryClickCommand = new RelayCommand(OnClick);
    }

    private void OnClick()
    {
        Console.WriteLine("Add Entry button clicked");
    }

    [RelayCommand]
    private void NavigateToHomePage()
    {
        if (CurrentViewModel is HomePageViewModel) return;

        CurrentViewModel = new HomePageViewModel();
        Console.WriteLine("Switching to Home Page");
    }

    [RelayCommand]
    private void NavigateToHistoryPage()
    {
        if (CurrentViewModel is HistoryPageViewModel) return;

        CurrentViewModel = new HistoryPageViewModel();
        Console.WriteLine("Switching to History Page");
    }

    [RelayCommand]
    private void NavigateToItemsPage()
    {
        if (CurrentViewModel is ItemsPageViewModel) return;

        CurrentViewModel = new ItemsPageViewModel();
        Console.WriteLine("Switching to Items Page");
    }

    [RelayCommand]
    private void NavigateToSettingsPage()
    {
        if (CurrentViewModel is SettingsPageViewModel) return;

        CurrentViewModel = new SettingsPageViewModel();
        Console.WriteLine("Switching to Settigns Page");
    }
}