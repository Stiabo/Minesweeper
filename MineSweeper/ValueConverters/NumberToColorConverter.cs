using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace MineSweeper
{
    /// <summary>
    /// A converter that takes in a boolean and returns a <see cref="Visibility"/>
    /// </summary>
    public class NumberToColorConverter : BaseValueConverter<NumberToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                //Get Number
                int number = (int)value;
                var brush = new SolidColorBrush();
                switch (number)
                {
                    case 1:
                        brush = (SolidColorBrush)Application.Current.FindResource("NumberBlueBrush");
                        break;
                    case 2:
                        brush = (SolidColorBrush)Application.Current.FindResource("NumberGreenBrush");
                        break;
                    case 3:
                        brush = (SolidColorBrush)Application.Current.FindResource("NumberRedBrush");
                        break;
                    case 4:
                        brush = (SolidColorBrush)Application.Current.FindResource("NumberPurpleBrush");
                        break;
                    case 5:
                        brush = (SolidColorBrush)Application.Current.FindResource("NumberDarkRedBrush");
                        break;
                    case 6:
                        brush = (SolidColorBrush)Application.Current.FindResource("NumberCyanBrush");
                        break;
                    case 7:
                        brush = (SolidColorBrush)Application.Current.FindResource("NumberBlackBrush");
                        break;
                    case 8:
                        brush = (SolidColorBrush)Application.Current.FindResource("NumberGrayBrush");
                        break;
                    default:
                        brush = (SolidColorBrush)Application.Current.FindResource("NumberBlueBrush");
                        break;

                }
                return brush;

            }
            else
                return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
