using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace TD.WPF.Converters;

public class EnumDescriptionConverter : IValueConverter
{
    private static string GetEnumDescription(Enum enumObj)
    {
        var fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
        if (fieldInfo == null)
            return enumObj.ToString();

        var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute != null)
        {
            if (displayAttribute.ResourceType != null && !string.IsNullOrWhiteSpace(displayAttribute.Name))
            {
                var resource = displayAttribute.ResourceType
                    .GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                    .FirstOrDefault(p => p.PropertyType == typeof(System.Resources.ResourceManager));

                if (resource != null)
                {
                    var resourceManager = resource.GetValue(null) as System.Resources.ResourceManager;
                    var value = resourceManager?.GetString(displayAttribute.Name);
                    if (!string.IsNullOrWhiteSpace(value))
                        return value;
                }
            }

            if (!string.IsNullOrWhiteSpace(displayAttribute.Name))
                return displayAttribute.Name;
        }

        var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
        if (descriptionAttribute != null && !string.IsNullOrWhiteSpace(descriptionAttribute.Description))
            return descriptionAttribute.Description;

        return enumObj.ToString();
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return string.Empty;

        if (value is string text)
            return text;

        if (value is Enum enumValue)
            return GetEnumDescription(enumValue);

        return value.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return Binding.DoNothing;

        if (!targetType.IsEnum)
            return Binding.DoNothing;

        var text = value as string;
        if (string.IsNullOrWhiteSpace(text))
            return Binding.DoNothing;

        foreach (var enumValue in Enum.GetValues(targetType).Cast<Enum>())
        {
            if (string.Equals(GetEnumDescription(enumValue), text, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(enumValue.ToString(), text, StringComparison.OrdinalIgnoreCase))
            {
                return enumValue;
            }
        }

        return Binding.DoNothing;
    }
}
