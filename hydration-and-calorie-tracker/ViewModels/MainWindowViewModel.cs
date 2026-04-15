using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using hydration_and_calorie_tracker.Models;

namespace hydration_and_calorie_tracker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";

    public ICommand ClickCommand { get; }

    private readonly AppDatabase _db;

    public MainWindowViewModel(AppDatabase db)
    {
        _db = db;

        ClickCommand = new RelayCommand(OnClick);
    }

    private void OnClick()
    {
        Console.WriteLine("Button clicked");
    }
}