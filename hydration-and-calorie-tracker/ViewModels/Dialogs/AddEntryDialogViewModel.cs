using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using hydration_and_calorie_tracker.Models;
using hydration_and_calorie_tracker.Models.Database;

namespace hydration_and_calorie_tracker.ViewModels.Dialogs;

public partial class AddEntryDialogViewModel : ViewModelBase
{
    private readonly TrackingService _trackingService;

    public ObservableCollection<Item> FilteredItems { get; } = [];

    [ObservableProperty] private string _unit = "";

    [ObservableProperty] private Item? _selectedItem;

    [ObservableProperty] private bool _isDrinkSelected = true;

    public bool IsFoodSelected
    {
        get => !IsDrinkSelected;
        set => IsDrinkSelected = !value;
    }

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnIsDrinkSelectedChanged(bool value) => RefreshFilteredItems();

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private decimal? _amount = 100;

    [ObservableProperty] private bool _useCurrentTime = true;

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnUseCurrentTimeChanged(bool value) =>
        OnPropertyChanged(nameof(IsPickingTime));

    public bool IsPickingTime => !UseCurrentTime;

    [ObservableProperty] private DateTime? _pickedDate = DateTime.Now;

    [ObservableProperty] private TimeSpan _pickedTime = DateTime.Now.TimeOfDay;

    /// <summary>
    /// A helper variable for form input validation
    /// </summary>
    private bool CanConfirm => SelectedItem is not null && Amount > 0;

    public AddEntryDialogViewModel()
    {
        _trackingService = null!;
    }

    public AddEntryDialogViewModel(TrackingService trackingService)
    {
        _trackingService = trackingService;
        RefreshFilteredItems();
    }

    private void RefreshFilteredItems()
    {
        var category = IsDrinkSelected ? Category.Drink : Category.Food;

        var items = category switch
        {
            Category.Drink => _trackingService.GetAllDrinks(),
            Category.Food => _trackingService.GetAllFoods(),
            _ => throw new ArgumentOutOfRangeException()
        };

        FilteredItems.Clear();

        foreach (var item in items) FilteredItems.Add(item);

        SelectedItem = FilteredItems.FirstOrDefault();

        Unit = SelectedItem is not null
            ? SelectedItem.Unit.ToDisplayString()
            : "";
    }

    [RelayCommand(CanExecute = nameof(CanConfirm))]
    private void Confirm(Window dialog)
    {
        if (SelectedItem is null) return;

        var timestamp = UseCurrentTime
            ? DateTime.Now
            : (PickedDate ?? DateTime.Today).Date + PickedTime;

        var entry = new Entry
        {
            Timestamp = timestamp,
            ItemId = SelectedItem.Id,
            Amount = Amount!.Value
        };

        dialog.Close(entry);
    }

    [RelayCommand]
    private void Cancel(Window dialog) => dialog.Close(null);
}