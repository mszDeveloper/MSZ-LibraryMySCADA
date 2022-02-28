//using MSZ_WATER;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using nsClassModBus;
using wpfLibMszControl;

using static nsClassModBus.ClassModBus;
namespace LibraryMySCADA.Capacity
{
    

    public partial class ucCapacity : UserControl
    {
        
        bool c;//флаг что уровень достигал максимума
        enum MODE
        {
            A,
            B,
            C,
        }
        MODE _MODE;

        bool reset = false;
        [Category("Setting")]
        public string valCategory { get; set; } = null;

        //----------------------------------------------------------------------------------------------------------
        [Description("sliderMax")]
        [Category("Setting")]
        public double radX
        {
            get { return (double)GetValue(radX_Property); }
            set { SetValue(radX_Property, value); }
        }
        public static readonly DependencyProperty radX_Property =
                   DependencyProperty.Register("radX", typeof(double),
                   typeof(ucCapacity), new PropertyMetadata((double)1));

         //----------------------------------------------------------------------------------------------------------


        //----------------------------------------------------------------------------------------------------------
        public CornerRadius CornerRadius { get { return border.CornerRadius; } set { border.CornerRadius = value; } }

        [Category("Setting")]
        [Description("Вход  - уровень житкости от 0 до 100 ")]
        public string valLevel
        {
            get { return (string)GetValue(valLevel_Property); }
            set { SetValue(valLevel_Property, value); }
        }
        public static readonly DependencyProperty valLevel_Property =
                   DependencyProperty.Register("valLevel", typeof(string),
                   typeof(ucCapacity), new PropertyMetadata(null));

        //----------------------------------------------------------------------------------------------------------
        //[Category("Setting_ModBus")]
        //[Description("Категория к какой принадлежат переменные в классе ModBUS. При не установленном " +
        //    "данном значении будут ипользоваться первые по списку переменные игнорируя категории.")]
        //public string valCategory { get; set; } = null;

        //----------------------------------------------------------------------------------------------------------
        [Description("sliderMax")]
        [Category("Setting")]
        public double sliderMax
        {
            get { return (double)GetValue(sliderMax_Property); }
            set { SetValue(sliderMax_Property, value); }
        }
        public static readonly DependencyProperty sliderMax_Property =
                   DependencyProperty.Register("sliderMax", typeof(double),
                   typeof(ucCapacity), new PropertyMetadata((double)10));
        //----------------------------------------------------------------------------------------------------------

        [Description("sliderMin")]
        [Category("Setting")]
        public double sliderMin
        {
            get { return (double)GetValue(sliderMin_Property); }
            set { SetValue(sliderMin_Property, value); }
        }
        public static readonly DependencyProperty sliderMin_Property =
                   DependencyProperty.Register("sliderMin", typeof(double),
                   typeof(ucCapacity), new PropertyMetadata((double)1));
        //----------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------
        [Description("Градиентная кисть заполнения емкости")]
        [Category("Setting")]
        public Brush CapacityFillBrush
        {
            get { return (Brush)GetValue(CapacityFillBrush_Property); }
            set { SetValue(CapacityFillBrush_Property, value); }
        }
        public static readonly DependencyProperty CapacityFillBrush_Property =
                   DependencyProperty.Register("CapacityFillBrush", typeof(Brush),
                   typeof(ucCapacity));
        //----------------------------------------------------------------------------------------------------------
        [Description("толщина бордюра")]
        [Category("Setting")]
        public double borderThickness
        {
            get { return (double)GetValue(borderThickness_Property); }
            set { SetValue(borderThickness_Property, value); }
        }
        public static readonly DependencyProperty borderThickness_Property =
                   DependencyProperty.Register("borderThickness", typeof(double),
                   typeof(ucCapacity), new PropertyMetadata((double)1));
        //----------------------------------------------------------------------------------------------------------
        [Description("Присутствие уровней")]
        [Category("Setting")]
        public bool UseLevels
        {
            get { return (bool)GetValue(UseLevels_Property); }
            set { SetValue(UseLevels_Property, value); }
        }
        public static readonly DependencyProperty UseLevels_Property =
                   DependencyProperty.Register("UseLevels", typeof(bool),
                   typeof(ucCapacity),new PropertyMetadata(true));

        //----------------------------------------------------------------------------------------------------------
        public double redYValue
        {
            get { return (double)GetValue(redYValue_Property); }
            private set { SetValue(redYValue_Property, value); }
        }
        public static readonly DependencyProperty redYValue_Property =
                   DependencyProperty.Register("redYValue", typeof(double),
                   typeof(ucCapacity));
        //----------------------------------------------------------------------------------------------------------
        public double blueYValue
        {
            get { return (double)GetValue(blueYValue_Property); }
            private set { SetValue(blueYValue_Property, value); }
        }
        public static readonly DependencyProperty blueYValue_Property =
                   DependencyProperty.Register("blueYValue", typeof(double),
                   typeof(ucCapacity));
        //----------------------------------------------------------------------------------------------------------
        [Description("Максимальное значение уровня")]
        [Category("Setting")]
        public double maxValue
        {
            get { return (double)GetValue(maxValue_Property); }
            set { SetValue(maxValue_Property, value); }
        }
        public static readonly DependencyProperty maxValue_Property =
                   DependencyProperty.Register("maxValue", typeof(double),
                   typeof(ucCapacity), new PropertyMetadata((double)0, chengMaxValue));

        private static void chengMaxValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucCapacity).label_maxValue.Content = e.NewValue.ToString() + "m";
        }

        //----------------------------------------------------------------------------------------------------------
        [Description("Текущее значение уровня")]
        [Category("Setting")]
        public double levelValue
        {   
            get  { return (double)GetValue(levelValue_Property); }
            set { SetValue(levelValue_Property, value); }
        }
        public static readonly DependencyProperty levelValue_Property =
                   DependencyProperty.Register("levelValue", typeof(double),
                   typeof(ucCapacity), new PropertyMetadata((double)0, changeLevelWater));

        private static void changeLevelWater(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucCapacity).label_currentLevel.Content = Math.Round(((d as ucCapacity).maxValue / 100) * (double)e.NewValue, 2).ToString()+"m";
            bool f = (d as ucCapacity).СheckOnOff();
            (d as ucCapacity).FillRect();
            (d as ucCapacity).onOff = f;

            RoutedEventArgs b = new RoutedEventArgs(OnOff_Event, f);
            (d as ucCapacity).RaiseEvent(b);

        }

        //----------------------------------------------------------------------------------------------------------
        [Description("Значение красного уровня")]
        [Category("Setting")]
        public double levelStopValue
        {
            get { return (double)GetValue(levelStopValue_Property); }
            set { SetValue(levelStopValue_Property, value); }
        }
        public static readonly DependencyProperty levelStopValue_Property =
                   DependencyProperty.Register("levelStopValue", typeof(double),
                   typeof(ucCapacity));

        //----------------------------------------------------------------------------------------------------------
        [Description("Значение синего уровня")]
        [Category("Setting")]
        public double levelStartValue
        {
            get { return (double)GetValue(levelStartValue_Property); }
            set { SetValue(levelStartValue_Property, value); }
        }
        public static readonly DependencyProperty levelStartValue_Property =
                   DependencyProperty.Register("levelStartValue", typeof(double),
                   typeof(ucCapacity));

        //----------------------------------------------------------------------------------------------------------
        [Description("Значение onOff")]
        [Category("Setting")]
        public bool onOff
        {
            get { return (bool)GetValue(onOff_Property); }
            set { SetValue(onOff_Property, value); }
        }
        public static readonly DependencyProperty onOff_Property =
                   DependencyProperty.Register("onOff", typeof(bool),
                   typeof(ucCapacity), new PropertyMetadata(false,changedOnOff,dddd));

        private static object dddd(DependencyObject d, object baseValue)
        {
            if ((d as ucCapacity).reset)
                (d as ucCapacity).createEventOnOff(new DependencyPropertyChangedEventArgs(onOff_Property,false, (d as ucCapacity).onOff));
            (d as ucCapacity).reset = false;
            return baseValue;
        }

        private static void changedOnOff(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucCapacity).createEventOnOff(e);
        }


        void createEventOnOff(DependencyPropertyChangedEventArgs e)
        {
            RoutedEventArgs b = new RoutedEventArgs(OnOff_Event, (bool)e.NewValue);
            RaiseEvent(b);

        }
        public static readonly RoutedEvent OnOff_Event = EventManager.RegisterRoutedEvent("OnOffChanged", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(ucCapacity));

        public event RoutedEventHandler OnOff_Changed
        {
            add { AddHandler(OnOff_Event, value); }
            remove { RemoveHandler(OnOff_Event, value); }
        }
        //----------------------------------------------------------------------------------------------------------
        public ucCapacity()
        {
            InitializeComponent();
            CapacityFillBrush = (Brush)Resources["BrushFillCapacity"];


        }

        //----------------------------------------------------------------------------------------------------------
        private void slider_max_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try {
                if (e.NewValue <= slider_min.Value) { slider_max.Value = slider_min.Value + 1; return; }
                redYValue = (gridEmcost.ActualHeight / 100) * e.NewValue * -1;
                levelStopValue = Math.Round((maxValue / 100) * e.NewValue, 1);
                label_MaxLevel.Content = levelStopValue.ToString() + "м";
            }
            catch { }
        }

        //----------------------------------------------------------------------------------------------------------
        private void slider_min_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            try
            {
                if (e.NewValue >= slider_max.Value && IsLoaded) { slider_min.Value = slider_max.Value-1; return; }
                blueYValue = (gridEmcost.ActualHeight / 100) * e.NewValue * -1;
                levelStartValue = Math.Round((maxValue / 100) * e.NewValue, 1);
                label_MinLevel.Content = levelStartValue.ToString() + "м";
            }
            catch { }            
        }

        //----------------------------------------------------------------------------------------------------------
        private void slider_min_Loaded(object sender, RoutedEventArgs e)
        {
            
            redYValue = (recWater.ActualHeight / 100) * slider_max.Value * -1;
            blueYValue = (recWater.ActualHeight / 100) * slider_min.Value * -1;
            levelStartValue = Math.Round((maxValue / 100) * slider_min.Value, 2);
            levelStopValue = Math.Round((maxValue / 100) * slider_max.Value, 2);
            label_MaxLevel.Content = Math.Round((maxValue / 100) * slider_max.Value, 2).ToString() + "м";
            label_MinLevel.Content = Math.Round((maxValue / 100) * slider_min.Value, 2).ToString() + "м";
        }
        //----------------------------------------------------------------------------------------------------------

        private bool СheckOnOff()
        {
            bool on = false;
            _MODE = MODE.A;
            if (levelValue >= slider_min.Value) _MODE = MODE.B;
            if (levelValue >= slider_max.Value) _MODE = MODE.C;

            if (_MODE == MODE.A) { on = true; c = false; }
            else if (_MODE == MODE.B & !c) on = true;
            else if (_MODE == MODE.C) { c = true; on = false; }//выключение



            return on;
        }

        //----------------------------------------------------------------------------------------------------------
        private void FillRect()
        {
            double y = (levelValue / 50 - 1) * -1;
            Resources["Point1"] = new Point(0, y);
        }

        //----------------------------------------------------------------------------------------------------------
         //----------------------------------------------------------------------------------------------------------

        public void Reset() { c = false; reset = true; }

        private void Capacity_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (valLevel != null) SetCallBackForModBusValue(ChengeLevel, valLevel, valCategory);
            }
            catch { }

        }

        private void ChengeLevel(ModBasValue val)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    levelValue = double.Parse(val.Value);
                }
                catch { }
            }
            ));
        }
        //----------------------------------------------------------------------------------------------------------
    }

    public class ConverterSub : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (double)values[0] - ((double)values[1] * 2);// = (double)parameter;
            }
            catch { return values; }

        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}
