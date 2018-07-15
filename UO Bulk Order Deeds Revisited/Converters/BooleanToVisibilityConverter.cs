using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UO_Bulk_Order_Deeds.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public Visibility TrueVisibility { get; set; } = Visibility.Visible;
        public Visibility FalseVisibility { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var convertedValue = false;

            try
            {
                if (value != null)
                {
                    convertedValue = (bool)value;
                }
            }
            catch
            {
            }

            return convertedValue ? TrueVisibility : FalseVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
