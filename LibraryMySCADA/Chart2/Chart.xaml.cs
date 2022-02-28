using System;
using System.Collections.Generic;
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

namespace LibraryMySCADA.Chart
{
    /// <summary>
    /// Логика взаимодействия для Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {

        //--------------------------------------------------------------------------------------------------------------------------
        public double XLenght
        {
            get { return (double)GetValue(XLenghtProperty); }
            set { SetValue(XLenghtProperty, value); }
        }
        public static readonly DependencyProperty XLenghtProperty =
            DependencyProperty.Register("XLenght", typeof(double), typeof(Chart), new PropertyMetadata(15d));

        //--------------------------------------------------------------------------------------------------------------------------
        public double YLenght
        {
            get { return (double)GetValue(YLenghtProperty); }
            set { SetValue(YLenghtProperty, value); }
        }
        public static readonly DependencyProperty YLenghtProperty =
            DependencyProperty.Register("YLenght", typeof(double), typeof(Chart), new PropertyMetadata(5d));

        //--------------------------------------------------------------------------------------------------------------------------
        public double XStep
        {
            get { return (double)GetValue(XStepProperty); }
            set { SetValue(XStepProperty, value); }
        }
        public static readonly DependencyProperty XStepProperty =
            DependencyProperty.Register("XStep", typeof(double), typeof(Chart), new PropertyMetadata(1d));

        //--------------------------------------------------------------------------------------------------------------------------
        public double YStep
        {
            get { return (double)GetValue(YStepProperty); }
            set { SetValue(YStepProperty, value); }
        }
        public static readonly DependencyProperty YStepProperty =
            DependencyProperty.Register("YStep", typeof(double), typeof(Chart), new PropertyMetadata(1d));


        //--------------------------------------------------------------------------------------------------------------------------
        public Chart()
        {
            InitializeComponent();
        }

        //--------------------------------------------------------------------------------------------------------------------------
    }
}
