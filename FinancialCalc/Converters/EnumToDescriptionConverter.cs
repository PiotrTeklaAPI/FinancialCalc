using FinancialCalc.Attributes;
using FinancialCalc.BaseClasses;
using System;
using System.Globalization;
using System.Windows.Data;

namespace FinancialCalc.Converters
{
    public class EnumToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string description && string.IsNullOrEmpty(description))
            {
                return value;
            }

            string result = string.Empty;
            if(value is Enum enumValue && enumValue.HasAttribute<Description>())
            {
                result = enumValue.ToDescription();
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
