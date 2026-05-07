using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using hydration_and_calorie_tracker.Models;

namespace hydration_and_calorie_tracker.ViewModels.Dialogs;

public partial class AddItemDialogViewModel : ViewModelBase
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private string _name = "";

    [ObservableProperty] private bool _isDrinkSelected = true;

    public bool IsFoodSelected
    {
        get => !IsDrinkSelected;
        set => IsDrinkSelected = !value;
    }

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private decimal? _waterContent;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private decimal? _calories;

    [ObservableProperty] private decimal? _totalFats;
    [ObservableProperty] private decimal? _saturatedFats;
    [ObservableProperty] private decimal? _totalCarbohydrates;
    [ObservableProperty] private decimal? _sugar;
    [ObservableProperty] private decimal? _fiber;
    [ObservableProperty] private decimal? _protein;
    [ObservableProperty] private decimal? _salt;

    [ObservableProperty] private bool _isMillilitersSelected = true;
    [ObservableProperty] private bool _isGramsSelected;
    [ObservableProperty] private bool _isPiecesSelected;

    private bool CanConfirm => !string.IsNullOrWhiteSpace(Name)
                               && WaterContent.HasValue && Calories.HasValue;

    [RelayCommand(CanExecute = nameof(CanConfirm))]
    private void Confirm(Window dialog)
    {
        var unit = IsMillilitersSelected ? Unit.HundredMilliliters
            : IsGramsSelected ? Unit.HundredGrams : Unit.Pieces;

        var item = new Item
        {
            Name = Name,
            Category = IsDrinkSelected ? Category.Drink : Category.Food,
            Calories = Calories!.Value,
            WaterContent = WaterContent!.Value,
            TotalFats = TotalFats ?? 0,
            SaturatedFats = SaturatedFats ?? 0,
            TotalCarbohydrates = TotalCarbohydrates ?? 0,
            Sugar = Sugar ?? 0,
            Protein = Protein ?? 0,
            Salt = Salt ?? 0,
            Unit = unit
        };

        dialog.Close(item);
    }

    [RelayCommand]
    private void Cancel(Window dialog) => dialog.Close(null);
}