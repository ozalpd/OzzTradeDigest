using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TD.WPF.Converters;

internal class BoolToVisibility : IValueConverter
{
    /// <summary>
    /// Converts a boolean value to a Visibility value.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <param name="targetType">The target type of the conversion.</param>
    /// <param name="parameter">An optional parameter; pass "Invert" to reverse the mapping.</param>
    /// <param name="culture">The culture information for the conversion.</param>
    /// <returns>Visible for true, Collapsed for false (reversed when parameter is "Invert").</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool flag = (value is bool b && b) || (value is bool? && (((bool?)value) ?? false));
        if (parameter is string s && s == "Invert")
        {
            flag = !flag;
        }
        return flag ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
