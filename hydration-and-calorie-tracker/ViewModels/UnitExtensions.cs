using System;

namespace hydration_and_calorie_tracker.Models;

public static class UnitExtensions
{
    public static string ToDisplayString(this Unit unit) => unit switch
    {
        Unit.HundredMilliliters => "ml",
        Unit.HundredGrams => "g",
        Unit.Pieces => "pcs",
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
    };
}