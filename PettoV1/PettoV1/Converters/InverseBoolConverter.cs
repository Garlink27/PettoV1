using System.Globalization;

namespace PettoV1.Converters
{
    /// <summary>
    /// Invierte un valor bool: true → false, false → true.
    /// Usado en Chat.xaml para mostrar la burbuja del usuario
    /// cuando EsRespuestaIA es false.
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is bool b && !b;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is bool b && !b;
    }
}
