using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Cet.SmartClient.Client
{
    public class InvertedNullableBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool?))
                throw new InvalidOperationException("The target must be a boolean.");

            bool? booleanValue = (bool?)value;
            if (booleanValue.HasValue)
                booleanValue = !booleanValue;

            return booleanValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? booleanValue = (bool?)value;
            if (booleanValue.HasValue)
                booleanValue = !booleanValue;

            return booleanValue;
        }
    }

}
