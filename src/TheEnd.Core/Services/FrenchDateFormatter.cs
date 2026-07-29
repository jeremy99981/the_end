using System.Globalization;

namespace TheEnd.Core.Services;

public static class FrenchDateFormatter
{
    public static string Format(DateTime date)
    {
        var formatted = date.ToString("dddd d MMMM yyyy", CultureInfo.GetCultureInfo("fr-FR"));
        return char.ToUpperInvariant(formatted[0]) + formatted[1..];
    }
}
