using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LibraryMySCADA.Convertors
{
   // [ValueConversion(typeof(bool), typeof(Brushes))]
    public class OnOffToBackgroundConverter : IValueConverter
    {
        public Brush OnBrush { get; set; } 
        public Brush OffBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter,System.Globalization.CultureInfo culture)
        {
            if(value == null) return System.Windows.Media.Brushes.Silver;
            if ((bool)value)
                return System.Windows.Media.Brushes.Blue;
            else if (!(bool)value)
                return System.Windows.Media.Brushes.Red;
            else return System.Windows.Media.Brushes.Silver;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class BoolToVisibleAndCollapsed : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string par;
            if(parameter == null) par = "null";
            else par = parameter.ToString();

            if ((bool?)value == true)
                return par=="invert" ? Visibility.Collapsed:Visibility.Visible;
            else
                return par == "invert" ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    //[Description("Параметр - масштаб скорости (double)")]
    public class doubleToSpeedBlowerConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double param;
            try { param = Double.Parse(parameter.ToString()); } catch { param = 1; }
            return (double)value * param;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    //**********************************************************************************************************
    public class DoubleRounding : IValueConverter //конвертор округления (параметр - количество десятичных разрядов)
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            int rou;
            try { rou = int.Parse(parameter as string); } catch { rou = 0; }
            return Math.Round(val, rou);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0d;
        }
    }
    //**********************************************************************************************************

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InvertableBooleanToVisibilityConverter : IValueConverter
    {
        enum Parameters
        {
            Normal, Inverted
        }

        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;
            var direction = (Parameters)Enum.Parse(typeof(Parameters), (string)parameter);

            if (direction == Parameters.Inverted)
                return !boolValue ? Visibility.Visible : Visibility.Collapsed;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
        //**********************************************************************************************************
    }

}
