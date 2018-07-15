using System;
using System.Globalization;
using System.Windows.Data;

namespace UO_Bulk_Order_Deeds.Converters
{
    public class DoubleToStringConverter : IValueConverter
    {
        public string Format { get; set; } = "0.00";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var convertedValue = String.Empty;

            try
            {
                if (value != null)
                {
                    convertedValue = ((double)value).ToString(Format, culture);
                }
            }
            catch
            {
            }

            return convertedValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
