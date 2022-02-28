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
using nsClassModBus;
using static nsClassModBus.ClassModBus;

namespace LibraryMySCADA.Sensor
{
    /// <summary>
    /// Логика взаимодействия для ucSensor.xaml
    /// </summary>
    public partial class ucSensor : Virt.ClassVirtualAdd
    {

        List<ImageSource> imageList = new List<ImageSource>();

        [Category("Переменные")]
        public bool isUseSaved { get; set; } = true;

        //--------------------------------------------------------------------------------------------------------------


        [Category("Setting")]
        public bool? isAdmin
        {
            get { return (bool?)GetValue(isAdminProperty); }
            set { SetValue(isAdminProperty, value); }
        }
        public static readonly DependencyProperty isAdminProperty =
            DependencyProperty.Register("isAdmin", typeof(bool?), typeof(ucSensor), new PropertyMetadata(null));

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public int imageIndex { get { return _imageIndex; } set { _imageIndex = value; ImageSensor.Source = imageList[value]; } } 
        int _imageIndex=0;

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public Brush backBrush
        {
            get { return (Brush)GetValue(backBrushProperty); }
            set { SetValue(backBrushProperty, value); }
        }
        public static readonly DependencyProperty backBrushProperty =
            DependencyProperty.Register("backBrush", typeof(Brush), typeof(ucSensor), new PropertyMetadata(null));


        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Угол поворота")]
        public double angleTurn
        {
            get { return (double)GetValue(angleTurn_Property); }
            set { SetValue(angleTurn_Property, value); }
        }
        public static readonly DependencyProperty angleTurn_Property =
                   DependencyProperty.Register("angleTurn", typeof(double),
                   typeof(ucSensor), new PropertyMetadata((double)0));

        //-----------------------------------------------------------------------------------------------------------------
        private static bool changeValueSens = false;

        [Category("Setting")]
        [Description("Аналоговое значение датчика")]
        public double valSensorAnalog
        {
            get { return (double)GetValue(valSensorAnalog_Property); }
            set { SetValue(valSensorAnalog_Property, value); }
        }
        public static readonly DependencyProperty valSensorAnalog_Property =
                   DependencyProperty.Register("valSensorAnalog", typeof(double),
                   typeof(ucSensor), new PropertyMetadata(0d, ChangeAnalougeSens));

        private static void ChangeAnalougeSens(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (changeValueSens) { changeValueSens = false; return; }
            ucSensor sens = d as ucSensor;
            var valSensorAnalog = (double)e.NewValue * sens.KAngle.Value;//угловой К
            valSensorAnalog += sens.OffsetLine.Value;//смещение
            valSensorAnalog *= sens.ScaleLIne.Value;//масштабирование

            valSensorAnalog = Math.Round(valSensorAnalog, (int)sens.DecCount.Value);//округление
            if (sens.valSensorAnalog != valSensorAnalog) changeValueSens = true;
            sens.valSensorAnalog = valSensorAnalog;
        }

        //--------------------------------------------------------------------------------------------------------
        //[Category("Setting")]
        //[Description("Дискретное значение датчика")]
        //public bool valSensorDiscrete
        //{
        //    get { return (bool)GetValue(valSensorDiscrete_Property); }
        //    set { SetValue(valSensorDiscrete_Property, value); }
        //}
        //public static readonly DependencyProperty valSensorDiscrete_Property =
        //           DependencyProperty.Register("valSensorDiscrete", typeof(bool),
        //           typeof(ucSensor), new PropertyMetadata(false));

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Установка ошибки датчика")]
        public bool sensorError
        {
            get { return (bool)GetValue(sensorError_Property); }
            set { SetValue(sensorError_Property, value); }
        }

        public static readonly DependencyProperty sensorError_Property =
                   DependencyProperty.Register("sensorError", typeof(bool),
                   typeof(ucSensor), new PropertyMetadata(false));

        //--------------------------------------------------------------------------------------------------------        
        [Category("Setting")]
        [Description("Смещение показа панели информации")]
        public Thickness offsetPanelInfo
        {
            get { return (Thickness)GetValue(offsetPanelInfo_Property); }
            set { SetValue(offsetPanelInfo_Property, value); }
        }
        public static readonly DependencyProperty offsetPanelInfo_Property =
                   DependencyProperty.Register("offsetPanelInfo", typeof(Thickness),
                   typeof(ucSensor), new PropertyMetadata(new Thickness(0, 0, 0, 0), changeThickness));

        private static void changeThickness(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucSensor).CanvasInfo.Margin = (Thickness)e.NewValue;
        }

        //--------------------------------------------------------------------------------------------------------        
        [Category("Setting")]
        [Description("Смещение показа панели настроек")]
        public Thickness offsetPanelSetting     
        {
            get { return (Thickness)GetValue(offsetPanelSetting_Property); }
            set { SetValue(offsetPanelSetting_Property, value); }
        }
        public static readonly DependencyProperty offsetPanelSetting_Property =
                   DependencyProperty.Register("offsetPanelSetting", typeof(Thickness),
                   typeof(ucSensor), new PropertyMetadata(new Thickness(-50, 0, 0, 0), changeThicknessSetting));

        private static void changeThicknessSetting(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucSensor).CanvasInfoSetting.Margin = (Thickness)e.NewValue;
        }

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Масштаб панели информации")]
        public double scalePanelInfo
        {
            get { return (double)GetValue(scalePanelInfo_Property); }
            set { SetValue(scalePanelInfo_Property, value); }
        }
        public static readonly DependencyProperty scalePanelInfo_Property =
                   DependencyProperty.Register("scalePanelInfo", typeof(double),
                   typeof(ucSensor), new PropertyMetadata((double)1));

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Масштаб панели настроек")]
        public double scalePanelSetting 
        {
            get { return (double)GetValue(scalePanelSetting_Property); }
            set { SetValue(scalePanelSetting_Property, value); }
        }
        public static readonly DependencyProperty scalePanelSetting_Property =
                   DependencyProperty.Register("scalePanelSetting", typeof(double),
                   typeof(ucSensor), new PropertyMetadata(0.5d));

        //-----------------------------------------------------------------------------------------------------------------

        [Category("Setting")]
        public Brush backSensColor
        {
            get { return (Brush)GetValue(backSensColorProperty); }
            set { SetValue(backSensColorProperty, value); }
        }
        public static readonly DependencyProperty backSensColorProperty =
            DependencyProperty.Register("backSensColor", typeof(Brush), typeof(ucSensor), new PropertyMetadata(null));

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public bool errorSensor
        {
            get { return (bool)GetValue(errorSensorPropertyProperty); }
            set { SetValue(errorSensorPropertyProperty, value); }
        }
        public static readonly DependencyProperty errorSensorPropertyProperty =
            DependencyProperty.Register("errorSensor", typeof(bool), typeof(ucSensor), new PropertyMetadata(false));

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public Visibility VisualSetting
        {
            get { return (Visibility)GetValue(VisualSettingProperty); }
            set { SetValue(VisualSettingProperty, value); }
        }
        public static readonly DependencyProperty VisualSettingProperty =
            DependencyProperty.Register("VisualSetting", typeof(Visibility), typeof(ucSensor), new PropertyMetadata(Visibility.Collapsed, ChangeVisual));

        private static void ChangeVisual(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ucSensor sens = d as ucSensor;
            sens.CanvasInfoSetting.RenderTransform  = new TranslateTransform(0, 0);
        }

        //-----------------------------------------------------------------------------------------------------------------
        public ucSensor()
        {
            InitializeComponent();
            imageList.Add(new BitmapImage(new Uri(@"Images/датчик1.png", UriKind.RelativeOrAbsolute)));
            imageList.Add(new BitmapImage(new Uri(@"Images/датчик2.png", UriKind.RelativeOrAbsolute)));
            imageList.Add(new BitmapImage(new Uri(@"Images/датчик3.png", UriKind.RelativeOrAbsolute)));
            imageList.Add(new BitmapImage(new Uri(@"Images/датчик4.png", UriKind.RelativeOrAbsolute)));
        }

        //--------------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ImageSensor.Source = imageList[imageIndex];
        }

        //--------------------------------------------------------------------------------------------------------
        public void SetValSensor(double val) { valSensorAnalog = val; }

        //--------------------------------------------------------------------------------------------------------
        private void button_Click(object sender, RoutedEventArgs e)
        {
            VisualSetting = Visibility.Collapsed;
        }

        //--------------------------------------------------------------------------------------------------------
        private void userControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isAdmin == null || isAdmin == true)
            {
                VisualSetting = VisualSetting == Visibility.Visible ? Visibility.Collapsed:Visibility.Visible;
            }
        }

        //--------------------------------------------------------------------------------------------------------
        [Serializable]
        public class SD
        {
            public double LineK;
            public double LineOffset;
            public double ScaleValue;
            public double DecimalCount;
        }

        public override DataSave GetSaveData()
        {
            if (!isUseSaved) return null;
            SD sd = new SD();
            sd.LineK = KAngle.Value;
            sd.LineOffset = OffsetLine.Value;
            sd.ScaleValue = ScaleLIne.Value;
            sd.DecimalCount = DecCount.Value;

            DataSave ds = new DataSave();
            ds.NameDataId = Name;
            ds.typ = GetType();
            ds.data = DataSaveSCADA.SerializableToString<SD>(sd);
            return ds;
        }


        public override bool SetSaveData(DataSave data)
        {
            if (!isUseSaved) return false;
            if (data == null || data.typ != GetType() || data.NameDataId != Name) return false;
            SD sd = new SD();
            sd = DataSaveSCADA.SerializableStringToObject<SD>(data.data);
            KAngle.Value = sd.LineK;
            OffsetLine.Value = sd.LineOffset;
            ScaleLIne.Value  = sd.ScaleValue;
            DecCount.Value   = sd.DecimalCount;
            return true;
        }

        //--------------------------------------------------------------------------------------------------------


        //--------------------------------------------------------------------------------------------------------
    }
}
