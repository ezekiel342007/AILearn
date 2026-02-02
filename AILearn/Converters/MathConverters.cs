using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AILearn.Converters;

public class PercentToAngleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double or int or float)
        {
            // Convert 0-100 to 0-360
            return System.Convert.ToDouble(value) * 3.6;
        }
        return 0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}