using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace MineSweeper
{
    /// <summary>
    /// A converter that takes in a Mine number and converts it to the assosiated Color
    /// </summary>
    public class BoolToColorConverter : BaseValueConverter<BoolToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? (SolidColorBrush)Application.Current.FindResource("NumberBlueBrush") : (SolidColorBrush)Application.Current.FindResource("NumberRedBrush");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
