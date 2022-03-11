using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TimeReporter.Model;

namespace TimeReporter.UI.Converters
{
    class DayTypeToBrushConverter : IValueConverter
    {
        private Dictionary<DayType, Brush> _map = new Dictionary<DayType, Brush>()
        {
            [DayType.Work] = new SolidColorBrush(Color.FromRgb(255, 230, 255)),
            [DayType.Weekend] = new SolidColorBrush(Color.FromRgb(242, 242, 242)),
            [DayType.NationalHoliday] = new SolidColorBrush(Color.FromRgb(255, 230, 230)),
            [DayType.DayOff] = new SolidColorBrush(Color.FromRgb(230, 255, 230)),
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DayType type)
            {
                return _map[type];
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
