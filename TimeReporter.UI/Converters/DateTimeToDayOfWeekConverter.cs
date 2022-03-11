using System;
using System.Globalization;
using System.Windows.Data;

namespace TimeReporter.UI.Converters
{
    class DateTimeToDayOfWeekConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                var date = (DateTime)value;
                var result = (int)date.DayOfWeek - 1;
                if (result < 0)
                {
                    result += 7;
                }

                return result;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
