using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraryMySCADA
{
    /// <summary>
    /// Логика взаимодействия для ucNumberOfUpDown.xaml
    /// </summary>
    public partial class ucNumberOfUpDown : UserControl
    {
        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Максимум")]
        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(double), typeof(ucNumberOfUpDown), new PropertyMetadata((double)100));

        //--------------------------------------------------------------------------------------------------------
        [Category("Кисть")]
        public Brush BackFont
        {
            get { return (Brush)GetValue(BackFontProperty); }
            set { SetValue(BackFontProperty, value); }
        }
        public static readonly DependencyProperty BackFontProperty =
            DependencyProperty.Register("BackFont", typeof(Brush), typeof(ucNumberOfUpDown), new PropertyMetadata(Brushes.White));

        //--------------------------------------------------------------------------------------------------------
        [Category("Кисть")]
        public Brush BackFontDisable
        {
            get { return (Brush)GetValue(BackFontDisableProperty); }
            set { SetValue(BackFontDisableProperty, value); }
        }
        public static readonly DependencyProperty BackFontDisableProperty =
            DependencyProperty.Register("BackFontDisable", typeof(Brush), typeof(ucNumberOfUpDown), new PropertyMetadata(Brushes.Silver));

        //--------------------------------------------------------------------------------------------------------
        [Category("Кисть")]
        public Brush ButtunColor
        {
            get { return (Brush)GetValue(ButtunColorProperty); }
            set { SetValue(ButtunColorProperty, value); }
        }
        public static readonly DependencyProperty ButtunColorProperty =
            DependencyProperty.Register("ButtunColor", typeof(Brush), typeof(ucNumberOfUpDown), new PropertyMetadata(Brushes.Silver));

        //--------------------------------------------------------------------------------------------------------
        [Category("Кисть")]
        public Brush FontColor
        {
            get { return (Brush)GetValue(FontColorProperty); }
            set { SetValue(FontColorProperty, value); }
        }
        public static readonly DependencyProperty FontColorProperty =
            DependencyProperty.Register("FontColor", typeof(Brush), typeof(ucNumberOfUpDown), new PropertyMetadata(Brushes.Black));

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Минимум")]
        public double Min
        {
            get { return (double)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }
        public static readonly DependencyProperty MinProperty =
                   DependencyProperty.Register("Min", typeof(double),
                   typeof(ucNumberOfUpDown), new PropertyMetadata((double)0));

        //--------------------------------------------------------------------------------------------------------
        [Description("Шаг")]
        [Category("Setting")]
        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }
        public static readonly DependencyProperty StepProperty =
                   DependencyProperty.Register("Step", typeof(double),
                   typeof(ucNumberOfUpDown), new PropertyMetadata((double)1));

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Значение")]
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
                   DependencyProperty.Register("Value", typeof(double),
                   typeof(ucNumberOfUpDown), new PropertyMetadata(0d, changedValue, CorrectValue));

        private static object CorrectValue(DependencyObject d, object baseValue)
        {
            ucNumberOfUpDown ob = d as ucNumberOfUpDown;
            double val = (double)baseValue;
            if (val > ob.Max) return ob.Max;
            if (val < ob.Min) return ob.Min;
            return val;

        }
        
        private static void changedValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d as ucNumberOfUpDown).IsLoaded) return;
            RoutedEventArgs b = new RoutedEventArgs(Value_Event, (double)e.NewValue);
            (d as ucNumberOfUpDown).RaiseEvent(b);
        }

        public static readonly RoutedEvent Value_Event = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(ucNumberOfUpDown));

        public event RoutedEventHandler Value_Changed
        {
            add { AddHandler(Value_Event, value); }
            remove { RemoveHandler(Value_Event, value); }
        }

        //--------------------------------------------------------------------------------------------------------
        public ucNumberOfUpDown()
        {
            InitializeComponent();
        }

        //--------------------------------------------------------------------------------------------------------
        private void textBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double rezult = Math.Round(Value + e.Delta / 10 * Step, GetBits(Step));
            Value = rezult <= Max && rezult >=Min ? rezult : Value;
        }

        //--------------------------------------------------------------------------------------------------------
        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            double rezult = Math.Round(Value + Step, GetBits(Step));
            Value = rezult <= Max ? rezult : Value;
        }

        //--------------------------------------------------------------------------------------------------------
        private void downButton_Click(object sender, RoutedEventArgs e)
        {
            double rezult = Math.Round(Value - Step, GetBits(Step));
            Value = rezult >= Min ? rezult : Value;// Math.Round(Value - Step, GetBits(Step));
           
        }

        //--------------------------------------------------------------------------------------------------------
        private int GetBits(double val)
        {
            return val.ToString().Substring(val.ToString().IndexOf(".") + 1).Length;
        }

        //--------------------------------------------------------------------------------------------------------
        private void NumberOfUpDown_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsEnabled) border.Background = BackFont;
            else border.Background = BackFontDisable;

        }

        //--------------------------------------------------------------------------------------------------------
        private void textBox_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            double val;
            string tx = textBox.Text.Replace('.', ',');
            try { val = double.Parse(tx, System.Globalization.NumberStyles.AllowDecimalPoint); } catch { textBox.Text = Value.ToString(); return; }
            Value = Math.Round(val, GetBits(Step));

        }
        //--------------------------------------------------------------------------------------------------------
    }
}
