using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using WPFLocalizeExtension.Extensions;

namespace Cet.SmartClient.Client
{
    public class StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is LocTextExtension)
            {
                string outString;
                ((LocTextExtension)value).ResolveLocalizedValue(out outString);

                return outString;
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is LocTextExtension)
            {
                string outString;
                ((LocTextExtension)value).ResolveLocalizedValue(out outString);

                return outString;
            }
            return value;
        }
    }
}
