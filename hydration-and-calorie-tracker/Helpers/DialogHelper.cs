using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;

namespace hydration_and_calorie_tracker.Helpers;

/// <summary>
/// A wrapper class around MessageBox simplifying the usage
/// </summary>
public static class DialogHelper
{
    /// <summary>
    /// Shows an info dialog and returns <c>DialogOption.Ok</c> when user closes the dialog
    /// </summary>
    /// <param name="message">the message inside the dialog window</param>
    /// <param name="minWidth">minimal width of the dialog window</param>
    /// <param name="minHeight">minimal height of the dialog window</param>
    /// <returns><c>DialogOption.Ok</c></returns>
    public static async Task<DialogOption> ShowInfoDialog(string message,
        double minWidth = 400, double minHeight = 100)
    {
        var box = CreateDialog("Info", message, minWidth, minHeight, false);

        await box.ShowAsync();

        return DialogOption.Ok;
    }

    /// <summary>
    /// Shows a warning dialog and returns a <c>DialogOption</c> when user closes the dialog, depending on user's selection
    /// </summary>
    /// <param name="message">the message inside the dialog window</param>
    /// <param name="minWidth">minimal width of the dialog window</param>
    /// <param name="minHeight">minimal height of the dialog window</param>
    /// <returns><c>DialogOption.Yes</c> or <c>DialogOption.No</c></returns>
    public static async Task<DialogOption> ShowWarningDialog(string message,
        double minWidth = 400, double minHeight = 100)
    {
        var box = CreateDialog("Warning", message, minWidth, minHeight);

        var result = await box.ShowAsync();

        return DialogOptionFromString(result);
    }

    /// <summary>
    /// Shows an error dialog and returns <c>DialogOption.Ok</c> when user closes the dialog
    /// </summary>
    /// <param name="message">the message inside the dialog window</param>
    /// <param name="minWidth">minimal width of the dialog window</param>
    /// <param name="minHeight">minimal height of the dialog window</param>
    /// <returns><c>DialogOption.Ok</c></returns>
    public static async Task<DialogOption> ShowErrorDialog(string message,
        double minWidth = 400, double minHeight = 100)
    {
        var box = CreateDialog("Error", message, minWidth, minHeight, false);

        await box.ShowAsync();

        return DialogOption.Ok;
    }

    private static DialogOption DialogOptionFromString(string s) =>
        s switch
        {
            nameof(DialogOption.Yes) => DialogOption.Yes,
            nameof(DialogOption.Ok) => DialogOption.Ok,
            _ => DialogOption.No
        };

    private static IMsBox<string> CreateDialog(string title, string message,
        double minWidth, double minHeight, bool confirm = true)
    {
        var buttonDefinitions = confirm
            ? new[]
            {
                new ButtonDefinition
                {
                    Name = nameof(DialogOption.Yes), IsDefault = false,
                    IsCancel = false
                },
                new ButtonDefinition
                {
                    Name = nameof(DialogOption.No), IsDefault = true,
                    IsCancel = true
                }
            }
            : new[]
            {
                new ButtonDefinition
                {
                    Name = nameof(DialogOption.Ok), IsDefault = true,
                    IsCancel = true
                }
            };

        return MessageBoxManager.GetMessageBoxCustom(
            new MessageBoxCustomParams
            {
                ContentTitle = title,
                ContentMessage = message,
                ButtonDefinitions = buttonDefinitions,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                MinWidth = minWidth,
                MinHeight = minHeight
            });
    }
}