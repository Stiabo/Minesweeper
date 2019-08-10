using System;
using System.Globalization;
using System.Windows;

namespace MineSweeper
{
    /// <summary>
    /// A converter that takes in a boolean and returns a <see cref="Visibility"/>
    /// </summary>
    public class NumberToVisiblityConverter : BaseValueConverter<NumberToVisiblityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                if ((int)value == -1)
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            else
            {
                if ((int)value == 0 || (int)value == -1)
                    return Visibility.Hidden;
                else
                    return Visibility.Visible;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
