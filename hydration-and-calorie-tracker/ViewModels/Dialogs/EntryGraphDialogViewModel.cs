using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using hydration_and_calorie_tracker.Models;
using hydration_and_calorie_tracker.Models.Database;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace hydration_and_calorie_tracker.ViewModels.Dialogs;

public partial class EntryGraphDialogViewModel : ViewModelBase
{
    private readonly TrackingService _trackingService;
    private readonly List<Entry> _entries;

    public ISeries[] Series { get; private set; } = [];

    public string[] MetricOptions { get; } =
        ["Amount", "Calories", "Hydration"];

    [ObservableProperty] private string _metric = "Amount";

    [ObservableProperty] private DateTime? _from = DateTime.Now.AddMonths(-1);

    [ObservableProperty] private DateTime? _to = DateTime.Now;

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnFromChanged(DateTime? value) => BuildSeries();

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnToChanged(DateTime? value) => BuildSeries();

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnMetricChanged(string value) => BuildSeries();

    public EntryGraphDialogViewModel()
    {
        _trackingService = null!;
        _entries = null!;
    }

    public EntryGraphDialogViewModel(TrackingService trackingService,
        string category)
    {
        _trackingService = trackingService;

        _entries = category switch
        {
            "Drinks" => _trackingService.GetAllDrinkEntries(),
            "Foods" => _trackingService.GetAllFoodEntries(),
            _ => _trackingService.GetAllEntries()
        };

        BuildSeries();
    }

    private void BuildSeries()
    {
        var filtered = _entries.Where(e =>
            (From is null || e.Timestamp >= From.Value) &&
            (To is null || e.Timestamp <= To.Value)).ToList();

        Series = filtered
            .GroupBy(e => e.ItemId)
            .Select(g =>
            {
                var item = _trackingService.GetItem(g.Key);
                var name = item?.Name ?? "Unknown";
                var divisor = item?.Unit == Unit.Pieces ? 1 : 100;
                var value = Metric switch
                {
                    "Calories" => g.Sum(e =>
                        e.Amount * (item?.Calories ?? 0) / divisor),
                    "Hydration" => g.Sum(e =>
                        e.Amount * (item?.WaterContent ?? 0) / divisor),
                    _ => g.Sum(e => e.Amount)
                };
                return new PieSeries<decimal>
                {
                    Name = name,
                    Values = [value]
                };
            })
            .ToArray<ISeries>();

        OnPropertyChanged(nameof(Series));
    }
}