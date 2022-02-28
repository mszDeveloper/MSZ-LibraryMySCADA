using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interactivity;

namespace wpfLibMszControl.ArrowPointer
{
    /// <summary>
    /// Логика взаимодействия для ucArrowPointer.xaml
    /// </summary>
    public partial class ucArrowPointer : UserControl
    {
        [Category("Setting")]
        public bool isMinOut { get; set; } = false;
        //************************************************************************************************************
        [Category("Setting")]
        [Description("Присутствие установки")]
        public bool useSettingValue
        {
            get { return (bool)GetValue(useSettingValue_Property); }
            set { SetValue(useSettingValue_Property, value); }
        }
        public static readonly DependencyProperty useSettingValue_Property = DependencyProperty.Register("useSettingValue", typeof(bool), typeof(ucArrowPointer), new PropertyMetadata(false));


        //************************************************************************************************************

        [Category("Setting")]
        [Description("Видимость Max и Min")]
        public bool visualMinMax
        {
            get { return (bool)GetValue(visualMinMax_Property); }
            set { SetValue(visualMinMax_Property, value); }
        }
        public static readonly DependencyProperty visualMinMax_Property = DependencyProperty.Register("visualMinMax", typeof(bool), typeof(ucArrowPointer), new PropertyMetadata(false));


        //************************************************************************************************************
        [Category("Setting")]
        [Description("Заливка полосы указателя")]
        public Brush LinebBrush
        {
            get { return (Brush)GetValue(LinebBrush_Property); }
            set { SetValue(LinebBrush_Property, value); }
        }
        public static readonly DependencyProperty LinebBrush_Property = DependencyProperty.Register("LinebBrush", typeof(Brush), typeof(ucArrowPointer), new PropertyMetadata(Brushes.Blue, changedValueMaxMin));

        //************************************************************************************************************
        [Category("Setting")]
        public double MaxValue
        {
            get { return (double)GetValue(MaxValue_Property); }
            set { SetValue(MaxValue_Property, value); }
        }
        public static readonly DependencyProperty MaxValue_Property = DependencyProperty.Register("MaxValue", typeof(double), typeof(ucArrowPointer),new PropertyMetadata(100d, changedValueMax));

        private static void changedValueMax(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if((double)e.NewValue<=(d as ucArrowPointer).MinValue) (d as ucArrowPointer).MaxValue = (double)e.OldValue ;
        }

        //************************************************************************************************************
        [Category("Setting")]
        public double MinValue
        {
            get {
                return (double)GetValue(MinValue_Property); }
            set { SetValue(MinValue_Property, value); }
        }
        public static readonly DependencyProperty MinValue_Property = DependencyProperty.Register("MinValue", typeof(double), typeof(ucArrowPointer), new PropertyMetadata(0d, changedValueMin));

        private static void changedValueMin(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((double)e.NewValue >= (d as ucArrowPointer).MaxValue) (d as ucArrowPointer).MinValue = (double)e.OldValue;
        }

        private static void changedValueMaxMin(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucArrowPointer).changedValue(null);
        }

        //************************************************************************************************************
        [Category("Setting")]
        [Description("Надпись - единица измерения")]
        public string ValuePr
        {
            get { return (string)GetValue(ValuePr_Property); }
            set { SetValue(ValuePr_Property, value); }
        }
        public static readonly DependencyProperty ValuePr_Property = DependencyProperty.Register("ValuePr", typeof(string), typeof(ucArrowPointer), new PropertyMetadata("Hz"));

        //************************************************************************************************************
        [Category("Setting")]
        public double Value
        {
            get { return (double)GetValue(Value_Property); }
            set { SetValue(Value_Property, value); }
        }
        public static readonly DependencyProperty Value_Property = DependencyProperty.Register("Value", typeof(double), typeof(ucArrowPointer), new PropertyMetadata(0d,changedValue));

        private static void changedValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucArrowPointer).changedValue((double)e.NewValue);
        }

        private void changedValue(double? e)
        {
            if (e == null) e = Value;
            if (e > MaxValue) Value = MaxValue;
            if (e <= MinValue) Value = MinValue;// isMinOut ? MinValue:0;
        }

        //private void Invalidate(object sender, RoutedEventArgs e)
        //{
        //    BindingExpression be;
        //    be = BindingOperations.GetBindingExpression(this, Value_Property);
        //    //ClearValue(be.TargetProperty);
        //    be.UpdateTarget();

        //}


        public void InvalidateValue()
        {
            double temp = Value;
            BindingExpression be = BindingOperations.GetBindingExpression(this, Value_Property);
            //double temp = Value;
            ClearValue(Value_Property);
            //Value = temp;
            SetBinding(Value_Property, be.ParentBindingBase);
            Value = temp;
            //target.InvalidateProperty(Value_Property);
            //Value++;            //try
            //{
            //    BindingExpression be;
            //    be = BindingOperations.GetBindingExpression(this, Value_Property);
            //    //ClearValue(be.TargetProperty);
            //    be.UpdateTarget();
            //    be.UpdateSource();


            //}
            //catch { }
            //be.UpdateTarget();
            //b//e.UpdateSource();
        }


        //************************************************************************************************************
        public ucArrowPointer()
        {
            InitializeComponent();
        }

        //************************************************************************************************************
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RoutedEventArgs b = new RoutedEventArgs(ChangedSetManual_Event, e.NewValue);
            base.RaiseEvent(b);
        }

        public static readonly RoutedEvent ChangedSetManual_Event = EventManager.RegisterRoutedEvent("ChangedSetManual_Event", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(ucArrowPointer));

        public event RoutedEventHandler ChangedValue_Event
        {
            add { AddHandler(ChangedSetManual_Event, value); }
            remove { RemoveHandler(ChangedSetManual_Event, value); }
        }
        //************************************************************************************************************
        private void ArrowPointer_Loaded(object sender, RoutedEventArgs e)
        {
            //Value = MinValue;
        }

        //************************************************************************************************************
    }


    //************************************************************************************************************
    [ValueConversion(typeof(double), typeof(double))]
    public class ValueToAngleArrow : DependencyObject, IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double Max = (double)values[1];
                double Min = (double)values[2];
                double Value = (double)values[0];
                var s = (double)((Value - Min) / ((Max - Min) / 90)) - 45;
                if (s < -45) s = -45;
                if (s > 45) s = 45;
                return s;

            }catch { return 0; }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
