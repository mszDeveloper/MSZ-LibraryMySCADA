using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace LibraryMySCADA.CounterNumericAnim
{
    public partial class CounterNumericAnim : UserControl
    {
        [Category("Кисть")]
        public Brush backColorNumeric
        {
            get { return (Brush)GetValue(backColorNumericProperty); }
            set { SetValue(backColorNumericProperty, value); }
        }
        public static readonly DependencyProperty backColorNumericProperty =
            DependencyProperty.Register("backColorNumeric", typeof(Brush), typeof(CounterNumericAnim), new PropertyMetadata(null));

        //-------------------------------------------------------------------------------------------------------------------
        [Category("Макет")]
        public int BorderRadius
        {
            get { return (int)GetValue(BorderRadiusProperty); }
            set { SetValue(BorderRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderRadiusProperty =
            DependencyProperty.Register("BorderRadius", typeof(int), typeof(CounterNumericAnim), new PropertyMetadata(4));

        //-------------------------------------------------------------------------------------------------------------------
        [Category("Макет")]
        public Thickness NumericBorderThickness
        {
            get { return (Thickness)GetValue(NumericBorderThicknessProperty); }
            set { SetValue(NumericBorderThicknessProperty, value); }
        }
        public static readonly DependencyProperty NumericBorderThicknessProperty =
            DependencyProperty.Register("NumericBorderThickness", typeof(Thickness), typeof(CounterNumericAnim), new PropertyMetadata(new Thickness(1, 1, 1, 1)));

        //-------------------------------------------------------------------------------------------------------------------
        [Category("Макет")]
        public Thickness textThickness
        {
            get { return (Thickness)GetValue(textThicknessProperty); }
            set { SetValue(textThicknessProperty, value); }
        }
        public static readonly DependencyProperty textThicknessProperty =
            DependencyProperty.Register("textThickness", typeof(Thickness), typeof(CounterNumericAnim), new PropertyMetadata(new Thickness(0, 0, 0, 2)));

        //-------------------------------------------------------------------------------------------------------------------
        public double ValueCounter
        {
            get { return (double)GetValue(ValueCounterProperty); }
            set { SetValue(ValueCounterProperty, value); }
        }
        public static readonly DependencyProperty ValueCounterProperty =
            DependencyProperty.Register("ValueCounter", typeof(double), typeof(CounterNumericAnim), new PropertyMetadata(0d, ChangeValueCounter));

        private static void ChangeValueCounter(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CounterNumericAnim obj = d as CounterNumericAnim;
            string du = e.NewValue.ToString();

            du = du.PadLeft(obj.NumbersCout, '0');
            du = du.Remove(0, du.Length - obj.NumbersCout);

            int ind = 0;
            foreach (var item in obj.numerics)
            {
                item.newChar = du[ind++];
            }
        }

        //-------------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public int NumbersCout { get; set; } = 5;

        List<_NumericAnimChange> numerics = new List<_NumericAnimChange>();

        //-------------------------------------------------------------------------------------------------------------------
        public CounterNumericAnim()
        {
            InitializeComponent();
         }

        private void Couter_Loaded(object sender, RoutedEventArgs e)
        {
            var v = Name;
            if (numerics.Count > 0) return;
            for (int i = 0; i < NumbersCout; i++)
            {
                _NumericAnimChange numb = new _NumericAnimChange();
                numb.textThickness = textThickness;
                numb.NumericBorderThickness = NumericBorderThickness;
                numb.BorderRadius = BorderRadius;
                numb.backColorNumeric = backColorNumeric;
                numerics.Add(numb);
            }
            foreach (var item in numerics)
            {
              rootGrid.Children.Add(item);
            }

        }
    }
}
