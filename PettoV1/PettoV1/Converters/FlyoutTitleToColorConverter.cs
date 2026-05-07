using System.Globalization;

namespace PettoV1.Converters
{
    public class FlyoutTitleToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == "Home"
                ? Colors.Black
                : Color.FromArgb("#E0F7FA");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
