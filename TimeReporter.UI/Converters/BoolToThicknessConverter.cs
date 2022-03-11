using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TimeReporter.UI.Converters
{
    class BoolToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolean)
            {
                return boolean ? new Thickness(1) : new Thickness(0);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
