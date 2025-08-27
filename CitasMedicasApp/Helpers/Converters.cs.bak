using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace CitasMedicasApp.Helpers
{
    public class BoolToConnectionColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isConnected)
            {
                return isConnected ? Microsoft.Maui.Graphics.Colors.Green : Microsoft.Maui.Graphics.Colors.Red;
            }
            return Microsoft.Maui.Graphics.Colors.Gray;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToConnectionTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isConnected)
            {
                return isConnected ? "Servidor conectado" : "Servidor desconectado";
            }
            return "Estado desconocido";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InvertedBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return true;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }
    }

    public class StringIsNotNullOrEmptyConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty(value as string);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EstadoCitaToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string estado)
            {
                return estado.ToLower() switch
                {
                    "programada" => Microsoft.Maui.Graphics.Colors.Blue,
                    "confirmada" => Microsoft.Maui.Graphics.Colors.Green,
                    "en_proceso" => Microsoft.Maui.Graphics.Colors.Orange,
                    "completada" => Microsoft.Maui.Graphics.Colors.DarkGreen,
                    "cancelada" => Microsoft.Maui.Graphics.Colors.Red,
                    "no_asistio" => Microsoft.Maui.Graphics.Colors.Gray,
                    _ => Microsoft.Maui.Graphics.Colors.Black
                };
            }
            return Microsoft.Maui.Graphics.Colors.Black;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}