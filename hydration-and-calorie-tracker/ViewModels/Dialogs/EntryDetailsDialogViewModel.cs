using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using hydration_and_calorie_tracker.Models;
using hydration_and_calorie_tracker.Models.Database;
using hydration_and_calorie_tracker.Views.Dialogs;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;

namespace hydration_and_calorie_tracker.ViewModels.Dialogs;

public partial class EntryDetailsDialogViewModel : ViewModelBase
{
    private readonly EntryRow? _entryRow;
    private readonly TrackingService _trackingService;

    public int Id => _entryRow?.Entry.Id ?? 0;
    public int ItemId => _entryRow?.Entry.ItemId ?? -1;

    public DateTime Timestamp =>
        _entryRow?.Entry.Timestamp ?? DateTime.UnixEpoch;

    public decimal Amount => _entryRow?.Entry.Amount ?? 0;
    public string ItemName => _entryRow?.ItemName ?? "[Deleted]";

    public EntryDetailsDialogViewModel()
    {
        _trackingService = null!;
    }

    public EntryDetailsDialogViewModel(EntryRow entryRow,
        TrackingService trackingService)
    {
        _entryRow = entryRow;
        _trackingService = trackingService;
    }

    [RelayCommand]
    private void Delete(Window dialog) => dialog.Close(true);

    [RelayCommand]
    private void Close(Window dialog) => dialog.Close(false);

    [RelayCommand]
    private async Task OpenItemDetails(int itemId)
    {
        if (itemId < 0)
        {
            await ShowErrorDialog(
                "Cannot show item details; item id is not valid.");
            return;
        }

        var item = _trackingService.GetItem(itemId);

        if (item is null)
        {
            await ShowErrorDialog(
                "Cannot show item details; item was deleted. ");
            return;
        }

        var viewModel = new ItemDetailsDialogViewModel(item);
        var dialog = new ItemDetailsDialog { DataContext = viewModel };

        var mainWindow =
            (Application.Current!.ApplicationLifetime as
                IClassicDesktopStyleApplicationLifetime)!.MainWindow!;
        var delete = await dialog.ShowDialog<bool>(mainWindow);

        if (!delete) return;

        _trackingService.DeleteItem(item.Id);
    }

    private static async Task ShowErrorDialog(string contentMessage)
    {
        var box = CreateErrorDialog(contentMessage);

        await box.ShowAsync();
    }

    private static IMsBox<string> CreateErrorDialog(string contentMessage)
    {
        return MessageBoxManager.GetMessageBoxCustom(
            new MessageBoxCustomParams
            {
                ContentTitle = "Error",
                ContentMessage = contentMessage,
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Ok", IsDefault = true }
                ],
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                MinHeight = 100,
                MinWidth = 400
            });
    }
}