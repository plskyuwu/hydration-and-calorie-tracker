using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using hydration_and_calorie_tracker.Models;
using hydration_and_calorie_tracker.Models.Database;
using hydration_and_calorie_tracker.ViewModels.Dialogs;
using hydration_and_calorie_tracker.Views.Dialogs;

namespace hydration_and_calorie_tracker.ViewModels;

public partial class HistoryPageViewModel : ViewModelBase
{
    public string PageTitle { get; } = "History";

    private readonly TrackingService _trackingService;

    public ObservableCollection<EntryRow> Entries { get; } = [];

    [ObservableProperty] private EntryRow? _selectedEntry;

    public string[] ShowOptions { get; } = ["Both", "Drinks", "Foods"];

    [ObservableProperty] private string _show = "Both";

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnShowChanged(string value) => RefreshEntries();

    public HistoryPageViewModel()
    {
        _trackingService = null!;
    }

    public HistoryPageViewModel(TrackingService trackingService)
    {
        _trackingService = trackingService;
        RefreshEntries();
    }

    private void RefreshEntries()
    {
        var entries = Show switch
        {
            "Drinks" => _trackingService.GetAllDrinkEntries(),
            "Foods" => _trackingService.GetAllFoodEntries(),
            _ => _trackingService.GetAllEntries()
        };

        Entries.Clear();
        foreach (var entry in entries)
        {
            var itemName = _trackingService.GetItem(entry.ItemId)?.Name ??
                           "[Deleted]";
            Entries.Add(new EntryRow(entry, itemName));
        }
    }

    [RelayCommand]
    private async Task OpenEntryDetails(EntryRow entryRow)
    {
        var viewModel =
            new EntryDetailsDialogViewModel(entryRow, _trackingService);
        var dialog = new EntryDetailsDialog { DataContext = viewModel };

        var mainWindow =
            (Application.Current!.ApplicationLifetime as
                IClassicDesktopStyleApplicationLifetime)!.MainWindow!;
        var delete = await dialog.ShowDialog<bool>(mainWindow);

        if (!delete)
        {
            RefreshEntries();
            return;
        }

        _trackingService.DeleteEntry(entryRow.Entry.Id);

        RefreshEntries();
    }
}