using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace AILearn.Converters;

public class IntToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Check if the current selection matches the button's assigned number (parameter)
        if (value is int selectedIndex && parameter is string targetIndexStr)
        {
            return selectedIndex == int.Parse(targetIndexStr);
        }
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Only update the model if the radio button is CHECKED
        if (value is bool isChecked && isChecked && parameter is string targetIndexStr)
        {
            return int.Parse(targetIndexStr);
        }
        
         // If unchecked, do nothing (don't write -1 to the ViewModel)
         return BindingOperations.DoNothing;
    }
} 