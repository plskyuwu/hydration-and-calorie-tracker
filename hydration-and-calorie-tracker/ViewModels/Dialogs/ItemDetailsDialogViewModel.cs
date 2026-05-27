using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using hydration_and_calorie_tracker.Helpers;
using hydration_and_calorie_tracker.Models;

namespace hydration_and_calorie_tracker.ViewModels.Dialogs;

public partial class ItemDetailsDialogViewModel : ViewModelBase
{
    private readonly Item? _item;

    public string Name => _item?.Name ?? "";
    public string Category => _item?.Category.ToString() ?? "";
    public int Id => _item?.Id ?? 0;
    public string Unit => _item?.Unit.ToDisplayString() ?? "";
    public decimal WaterContent => _item?.WaterContent ?? 0;
    public decimal Calories => _item?.Calories ?? 0;
    public decimal TotalFats => _item?.TotalFats ?? 0;
    public decimal SaturatedFats => _item?.SaturatedFats ?? 0;
    public decimal TotalCarbohydrates => _item?.TotalCarbohydrates ?? 0;
    public decimal Sugar => _item?.Sugar ?? 0;
    public decimal Fiber => _item?.Fiber ?? 0;
    public decimal Protein => _item?.Protein ?? 0;
    public decimal Salt => _item?.Salt ?? 0;
    public bool IsDefault => _item?.IsDefault ?? false;

    public ItemDetailsDialogViewModel()
    {
    }

    public ItemDetailsDialogViewModel(Item item)
    {
        _item = item;
    }

    [RelayCommand]
    private void Delete(Window dialog) => dialog.Close(true);

    [RelayCommand]
    private void Close(Window dialog) => dialog.Close(false);
}