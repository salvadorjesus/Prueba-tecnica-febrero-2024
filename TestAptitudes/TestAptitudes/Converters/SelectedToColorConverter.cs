using System;
using Xamarin.Forms;

namespace TestAptitudes.Converters
{
    public class SelectedToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isSelected && isSelected)
                return Application.Current.Resources["SelectedColor"];
            else
                return Application.Current.Resources["NotSelectedColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}