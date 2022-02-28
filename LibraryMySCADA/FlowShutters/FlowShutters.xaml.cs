using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryMySCADA.FlowShutters
{
     public partial class FlowShutters : Virt.ClassVirtualAdd
    {
        //-------------------------------------------------------------------------------------------------------------------
        public double openShutter
        {
            get { return (double)GetValue(openShutterProperty); }
            set { SetValue(openShutterProperty, value); }
        }
        public static readonly DependencyProperty openShutterProperty =
            DependencyProperty.Register("openShutter", typeof(double), typeof(FlowShutters), new PropertyMetadata(0d));

        //-------------------------------------------------------------------------------------------------------------------
        public FlowShutters()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------
    }

    //-------------------------------------------------------------------------------------------------------------------
    //-------CONVERTOR---------------------------------------------------------------------------------------------------
    public class OpenToAngleConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;
            double open = (double)value;
            var d =  open * 0.9;
            if (d > 90) d = 90;
            else if (d < 0) d = 0;
            return d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double open = (double)value;
            return (10/9) * open;
        }
    }

}
