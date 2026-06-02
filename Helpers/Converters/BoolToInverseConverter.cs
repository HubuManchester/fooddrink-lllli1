using System.Globalization;

namespace FoodLens.Helpers.Converters;

/// <summary>
/// Converts a boolean value to its inverse (true becomes false, false becomes true).
/// </summary>
public class BoolToInverseConverter : IValueConverter
{
    /// <summary>
    /// Returns the inverse of the boolean value.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool b && !b;
    }

    /// <summary>
    /// Returns the inverse of the boolean value (same logic as Convert).
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool b && !b;
    }
}
