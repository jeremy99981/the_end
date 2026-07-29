using System.Globalization;

namespace TheEnd.Core.Services;

public static class FrenchDateFormatter
{
    public static string Format(DateTime date) => date.ToString("dddd d MMMM yyyy", CultureInfo.GetCultureInfo("fr-FR"));
}
