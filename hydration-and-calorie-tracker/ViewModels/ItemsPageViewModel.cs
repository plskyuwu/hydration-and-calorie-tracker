using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using hydration_and_calorie_tracker.Helpers;
using hydration_and_calorie_tracker.Models;
using hydration_and_calorie_tracker.Models.Database;
using hydration_and_calorie_tracker.ViewModels.Dialogs;
using hydration_and_calorie_tracker.Views.Dialogs;

namespace hydration_and_calorie_tracker.ViewModels;

public partial class ItemsPageViewModel : ViewModelBase
{
    public string PageTitle { get; } = "Items";

    private readonly TrackingService _trackingService;

    public ObservableCollection<Item> Items { get; } = [];

    [ObservableProperty] private Item? _selectedItem;

    public string[] SortOptions { get; } =
        ["Id", "Name", "Category", "Calories", "Hydration"];

    [ObservableProperty] private string _sortBy = "Id";

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnSortByChanged(string value) => RefreshItems();

    public ItemsPageViewModel()
    {
        _trackingService = null!;
    }

    public ItemsPageViewModel(TrackingService trackingService)
    {
        _trackingService = trackingService;
        RefreshItems();
    }

    private void RefreshItems()
    {
        var sorted = SortBy switch
        {
            "Name" => _trackingService.GetAllItems().OrderBy(i => i.Name),
            "Category" => _trackingService.GetAllItems()
                .OrderBy(i => i.Category),
            "Calories" => _trackingService.GetAllItems()
                .OrderByDescending(i => i.Calories),
            "Hydration" => _trackingService.GetAllItems()
                .OrderByDescending(i => i.WaterContent),
            _ => _trackingService.GetAllItems().OrderBy(i => i.Id)
        };

        Items.Clear();
        foreach (var item in sorted)
            Items.Add(item);
    }

    [RelayCommand]
    private async Task AddItem()
    {
        var viewModel = new AddItemDialogViewModel();
        var dialog = new AddItemDialog { DataContext = viewModel };

        var mainWindow =
            (Application.Current!.ApplicationLifetime as
                IClassicDesktopStyleApplicationLifetime)!.MainWindow!;
        var item = await dialog.ShowDialog<Item?>(mainWindow);

        if (item is null) return;

        _trackingService.AddItem(item);

        RefreshItems();
    }

    [RelayCommand]
    private async Task OpenItemDetails(Item item)
    {
        var viewModel = new ItemDetailsDialogViewModel(item);
        var dialog = new ItemDetailsDialog { DataContext = viewModel };

        var mainWindow =
            (Application.Current!.ApplicationLifetime as
                IClassicDesktopStyleApplicationLifetime)!.MainWindow!;
        var delete = await dialog.ShowDialog<bool>(mainWindow);

        if (!delete) return;

        var result = await DialogHelper.ShowWarningDialog(
            $"Are you sure you want to delete {item.Name}?\n\nThis action cannot be undone.");

        if (result != DialogOption.Yes) return;

        var deleteEntries = await DialogHelper.ShowWarningDialog(
            $"Do you also want to delete all entries that were using {item.Name}?\n\nThis action cannot be undone.");

        var count =
            _trackingService.DeleteItem(item.Id, deleteEntries == DialogOption.Yes);

        if (!count.deleted)
            await DialogHelper.ShowErrorDialog($"Failed to delete {item.Name}.");

        await DialogHelper.ShowInfoDialog(
            $"Deleted {item.Name} and {count.entries} entries.");

        RefreshItems();
    }
}