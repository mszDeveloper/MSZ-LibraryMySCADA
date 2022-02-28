using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LibraryMySCADA.SignalPanel
{
    /// <summary>
    /// Логика взаимодействия для ucSignalPanel.xaml
    /// </summary>
    /// 
    public enum _STATE
    {
        OK,
        ERROR,
        WORK,
        ALARM,
        ON,
        OFF,
        AUTO_ON,
        AUTO_OFF,
        MANUAL_ON,
        MANUAL_OFF,
        Pipel,
        Flooding,
        _3,
        _4,
        LOCAL,
        REMOTE,
        COLLAPSED
    }
   
    public partial class ucSignalPanel : UserControl
    {

        DispatcherTimer timer = null;

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting:Image")]
        public ImageSource OK { get; set; } = new BitmapImage(new Uri(@"Images/ok.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource ERROR { get; set; } = new BitmapImage(new Uri(@"Images/error.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource WORK { get; set; } = new BitmapImage(new Uri(@"Images/none.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource ALARM { get; set; } = new BitmapImage(new Uri(@"Images/alarm.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource ON { get; set; } = new BitmapImage(new Uri(@"Images/on.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource OFF { get; set; } = new BitmapImage(new Uri(@"Images/off.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource AUTO_ON { get; set; } = new BitmapImage(new Uri(@"Images/avto_on_1.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource AUTO_OFF { get; set; } = new BitmapImage(new Uri(@"Images/avto_off_1.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource MANUAL_ON { get; set; } = new BitmapImage(new Uri(@"Images/manual.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource MANUAL_OFF { get; set; } = new BitmapImage(new Uri(@"Images/avto_off_1.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource REMOTE { get; set; } = new BitmapImage(new Uri(@"Images/computerlearning.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource Pipel { get; set; } = new BitmapImage(new Uri(@"Images/light-bulb.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource Flooding { get; set; } = new BitmapImage(new Uri(@"Images/Flooding.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource _3 { get; set; } = new BitmapImage(new Uri(@"Images/none.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource _4 { get; set; } = new BitmapImage(new Uri(@"Images/none.png", UriKind.RelativeOrAbsolute));
        [Category("Setting:Image")]
        public ImageSource _LOCAL { get; set; } = new BitmapImage(new Uri(@"Images/cropped-Letter-M-icon.png", UriKind.RelativeOrAbsolute));
        BitmapImage NONE = new BitmapImage(new Uri(@"Images/none.png", UriKind.RelativeOrAbsolute));
        //-----------------------------------------------------------------------------------------------------------------

        [Description("Состояние")]
        [Category("Setting")]
        public _STATE STATE
        {
            get { return (_STATE)GetValue(STATE_Property); }
            set { SetValue(STATE_Property, value); }
        }
        public static readonly DependencyProperty STATE_Property =
                   DependencyProperty.Register("STATE", typeof(_STATE),
                   typeof(ucSignalPanel), new PropertyMetadata((_STATE)_STATE.OK,changedState));

        private static void changedState(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucSignalPanel).ChangeImageState((_STATE)e.NewValue);
        }

        //-----------------------------------------------------------------------------------------------------------------
        public void ChangeImageState(_STATE _STATE)
        {
            //if (_STATE == _STATE.COLLAPSED) { image.Source = null; Visibility = Visibility.Collapsed; }
            //    else Visibility = Visibility.Visible;

            switch (_STATE)
            {
                case _STATE.COLLAPSED: image.Source = null; break;
                case _STATE.OK: image.Source = OK;  break;
                case _STATE.ERROR: image.Source = ERROR;  break;
                case _STATE.WORK: image.Source = WORK;  break;
                case _STATE.ALARM: image.Source = ALARM;  break;
                case _STATE.ON: image.Source = ON;  break;
                case _STATE.OFF: image.Source = OFF;  break;
                case _STATE.AUTO_ON: image.Source = AUTO_ON; break;
                case _STATE.AUTO_OFF: image.Source = AUTO_OFF;  break;
                case _STATE.MANUAL_ON: image.Source = MANUAL_ON;  break;
                case _STATE.MANUAL_OFF: image.Source = MANUAL_OFF;  break;
                case _STATE.Pipel: image.Source = Pipel;   break;
                case _STATE.Flooding: image.Source = Flooding; break;
                case _STATE._3: image.Source = _3;  break;
                case _STATE._4: image.Source = _4;  break;
                case _STATE.LOCAL: image.Source = _LOCAL; break;
                case _STATE.REMOTE: image.Source = REMOTE;  break;
                default: image.Source = NONE; break;
            }

        }
        //-----------------------------------------------------------------------------------------------------------------
        [Description("Моргание")]
        [Category("Setting")]
        public bool Flash
        {
            get { return (bool)GetValue(Flash_Property); }
            set { SetValue(Flash_Property, value); }
        }
        public static readonly DependencyProperty Flash_Property =
                   DependencyProperty.Register("Flash", typeof(bool),
                   typeof(ucSignalPanel), new PropertyMetadata((bool)false, changedStateFlash));

        private static void changedStateFlash(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucSignalPanel).changedStateFlash(e);
        }

        private void changedStateFlash(DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue && timer == null)
            {
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Tick += new EventHandler(timerTick);
                timer.Start();
            }
            else if ((bool)e.NewValue && timer != null)
            {
                timer.Start();
            }
            else if (!(bool) e.NewValue && timer != null)
            {
                timer.Stop();
                Opacity = 1;
            }

        }

        private void timerTick(object sender, EventArgs e)
        {
            if (Opacity == 1) Opacity = 0.20d;
            else Opacity = 1;
        }

        //-----------------------------------------------------------------------------------------------------------------
        [Description("Поля")]
        [Category("Setting")]
        public Thickness ThickBorder
        {
            get { return (Thickness)GetValue(ThickBorder_Property); }
            set { SetValue(ThickBorder_Property, value); }
        }
        public static readonly DependencyProperty ThickBorder_Property =
                   DependencyProperty.Register("ThickBorder", typeof(Thickness),
                   typeof(ucSignalPanel), new PropertyMetadata(new Thickness()));
        //-----------------------------------------------------------------------------------------------------------------
        [Description("Радиус Поля")]
        [Category("Setting")]
        public CornerRadius RadiusBorder
        {
            get { return (CornerRadius)GetValue(RadiusBorder_Property); }
            set { SetValue(RadiusBorder_Property, value); }
        }
        public static readonly DependencyProperty RadiusBorder_Property =
                   DependencyProperty.Register("RadiusBorder", typeof(CornerRadius),
                   typeof(ucSignalPanel), new PropertyMetadata(new CornerRadius()));
        //-----------------------------------------------------------------------------------------------------------------


        public ucSignalPanel()
        {
            InitializeComponent();
           
            //image.Source = OK;
        }

        private void ucSignalControl_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeImageState(STATE);
        }

        private void ucSignalControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                Flash = !Flash;
                Flash = !Flash;
            }
            else
            {
                timer.Stop();
            }
        }
    }//CornerRadius

    //-----------------------------------------------------------------------------------------------------------------
    public class boolToAutoValueConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            try { if ((bool)value) return _STATE.AUTO_ON; else return _STATE.AUTO_OFF; } catch { return _STATE.AUTO_OFF; }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((_STATE)value == _STATE.AUTO_ON) return true;
            else return false;
        }

    }
    //-----------------------------------------------------------------------------------------------------------------
    public class boolToOnOffValueConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            if ((bool)value) return _STATE.ON; else return _STATE.OFF;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((_STATE)value == _STATE.ON) return true;
            else return false;
        }
    }
    //-----------------------------------------------------------------------------------------------------------------
    public class boolToOnOffDriverConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return _STATE.ON; else return _STATE.OFF;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((_STATE)value == _STATE.ON) return true;
            else return false;
        }
    }
    //-----------------------------------------------------------------------------------------------------------------
    //параметр - Normal, Invert
    public class boolToOKandAlarmConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string par = parameter as string;
            if (par == "Invert")
                return (bool)value ? _STATE.ALARM : _STATE.OK;
            else
                return (bool)value ? _STATE.OK : _STATE.ALARM;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((_STATE)value == _STATE.OK) return true;
            else return false;
        }
    }
    //-----------------------------------------------------------------------------------------------------------------
    //параметр - состояние 
    public class boolToCollapsedAndSelectState : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool)value) return _STATE.COLLAPSED;
            else
            {
                string par = parameter as string;
                switch (par)
                {
                    case "COLLAPSED":
                    case "OK": return _STATE.OK;
                    case "ERROR": return _STATE.ERROR;
                    case "WORK": return _STATE.WORK;
                    case "ALARM": return _STATE.ALARM;
                    case "ON": return _STATE.ON;
                    case "OFF": return _STATE.OFF;
                    case "AUTO_ON":return _STATE.AUTO_ON;
                    case "AUTO_OFF":return _STATE.AUTO_OFF;
                    case "MANUAL_ON":return _STATE.MANUAL_ON;
                    case "MANUAL_OFF":return _STATE.MANUAL_OFF;
                    case "Pipel":return _STATE.Pipel;
                    case "Flooding":return _STATE.Flooding;
                    case "_3":return _STATE._3;
                    case "_4":return _STATE._4;
                    case "LOCAL":return _STATE.LOCAL;
                    case "_REMOTE":return _STATE.REMOTE;
                    default: return _STATE.OK;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((_STATE)value == _STATE.OK) return true;
            else return false;
        }
    }
    //-----------------------------------------------------------------------------------------------------------------

}
