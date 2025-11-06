using System;
using System.Globalization;
using System.Windows.Data;

namespace Client;

public class BoolToEditHeaderConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isEditing)
        {
            return isEditing ? "编辑用户" : "用户详情";
        }
        return "用户详情";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}