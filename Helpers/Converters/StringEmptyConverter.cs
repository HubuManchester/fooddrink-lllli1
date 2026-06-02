using System.Globalization;

namespace FoodLens.Helpers.Converters;

/// <summary>
/// Converts a string value to true if it is null or empty.
/// </summary>
public class StringEmptyConverter : IValueConverter
{
    /// <summary>
    /// Returns true if the value is null or an empty string.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return !(value is string s && !string.IsNullOrEmpty(s));
    }

    /// <summary>
    /// Not implemented. Converts from target back to source.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
