using LibraryMySCADA.Virt;
using nsClassModBus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Threading;
using System.Xml.Serialization;
using static nsClassModBus.ClassModBus;

namespace LibraryMySCADA.Driver
{
    /// <summary>
    /// Логика взаимодействия для ucDriver.xaml
    /// </summary>
    


    public partial class ucDriver : ClassVirtualAdd
    {
        [Serializable]
        public struct SaveData
        {
            public int mode;
            public int min;
            public int max;
            public int maxAmperage;

        }

        Transform myTrans, myTransPid;
        DispatcherTimer timer = null;

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public double AmperageInit { get { return arrowAmperage.MaxValue; } set { arrowAmperage.MaxValue = value; } }

        [Category("Setting")]
        public bool timeOutStoppedForPID { get; set; } = false;

        [Category("Setting")]
        public bool timeOutStartedForPID { get; set; } = false;

        [Category("Setting")]
        public bool Worker
        {
            get { return IsWorker; }
            set { if (comboBox.Text == "Auto" && !Error) IsWorker = value; }
        }

        [Category("Setting")]
        public TimeSpan timeOnControl { get; set; } = new TimeSpan(0, 0, 0);


        [Category("Setting")]
        [Description("Масштаб выходного сигнала частоты")]
        public double scaleOutFRQ { get; set; } = 0.01;

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Обратная связь для ПИД")]
        public double osPid
       {
            get { return (double)GetValue(osPid_Property); }
            set { SetValue(osPid_Property, value); }
        }
        public static readonly DependencyProperty osPid_Property =
                   DependencyProperty.Register("osPid", typeof(double),
                   typeof(ucDriver), new PropertyMetadata((double)0, changedOSpid));

        private static void changedOSpid(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucDriver).ucPid.PidOS = (double)e.NewValue;
        }

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Error")]
        public bool Error
        {
            get { return (bool)GetValue(Error_Property); }
            set { SetValue(Error_Property, value); }
        }
        public static readonly DependencyProperty Error_Property =
                   DependencyProperty.Register("Error", typeof(bool),
                   typeof(ucDriver), new PropertyMetadata(false,changError));

        private static void changError(DependencyObject d, DependencyPropertyChangedEventArgs e)
        { (d as ucDriver).ChangError(d, e); }

        private void ChangError(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if((bool)e.NewValue) IsWorker = false;
            ucUGO_DRIVERControl.Alarm = (bool)e.NewValue;
        }

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Смещение показа панели настроек")]
        public Thickness offsetPanel
        {
            get { return (Thickness)GetValue(offsetPanel_Property); }
            set { SetValue(offsetPanel_Property, value); }
        }
        public static readonly DependencyProperty offsetPanel_Property =
                   DependencyProperty.Register("offsetPanel", typeof(Thickness),
                   typeof(ucDriver), new PropertyMetadata(new Thickness(0, 0, 0, 0), changeThickness));

        private static void changeThickness(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucDriver).CanvasSetting.Margin = (Thickness)e.NewValue;
            (d as ucDriver).myTrans = (d as ucDriver).CanvasSetting.RenderTransform;
        }

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Смещение показа панели ПИД")]
        public Thickness offsetPanelPID
        {
            get { return (Thickness)GetValue(offsetPanelPID_Property); }
            set { SetValue(offsetPanelPID_Property, value); }
        }
        public static readonly DependencyProperty offsetPanelPID_Property =
                   DependencyProperty.Register("offsetPanelPID", typeof(Thickness),
                   typeof(ucDriver), new PropertyMetadata(new Thickness(0, 0, 0, 0), changeThicknessPID));

        private static void changeThicknessPID(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucDriver).CanvasPID.Margin = (Thickness)e.NewValue;
            (d as ucDriver).myTransPid = (d as ucDriver).CanvasSetting.RenderTransform;
        }

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Поворот значка УГО")]
        public double angleUGO
        {
            get { return (double)GetValue(angleUGO_Property); }
            set { SetValue(angleUGO_Property, value); }
        }
        public static readonly DependencyProperty angleUGO_Property =
                   DependencyProperty.Register("angleUGO", typeof(double),
                   typeof(ucDriver), new PropertyMetadata((double)0));

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("вход внешней ошибки, true-ошибка")]
        public bool ModBusError
        {
            get { return (bool)GetValue(ModBusError_Property); }
            set { SetValue(ModBusError_Property, value); }
        }
        public static readonly DependencyProperty ModBusError_Property =
                   DependencyProperty.Register("ModBusError", typeof(bool),
                   typeof(ucDriver), new PropertyMetadata(false, changedModBusError));

        private static void changedModBusError(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucDriver).Error = (bool)e.NewValue;
        }

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("вход уставки - если задествован ПИД регулятор" +
            "то значение является уставкой для ПИД")]
        public double setValue
        {
            get { return (double)GetValue(setValue_Property); }
            set { SetValue(setValue_Property, value); }
        }
        public static readonly DependencyProperty setValue_Property =
                   DependencyProperty.Register("setValue", typeof(double),
                   typeof(ucDriver), new PropertyMetadata((double)0, changeValue));

        private static void changeValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucDriver).ucPid.PidSetpoint = (double)e.NewValue;
        }

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Для разработки")]
        public bool visibleSettigPanel
        {
            get { return (bool)GetValue(visibleSettigPanel_Property); }
            set { SetValue(visibleSettigPanel_Property, value); }
        }
        public static readonly DependencyProperty visibleSettigPanel_Property =
                   DependencyProperty.Register("visibleSettigPanel", typeof(bool),
                   typeof(ucDriver), new PropertyMetadata(false));

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Для разработки")]
        public bool visiblePidPanel
        {
            get { return (bool)GetValue(visiblePidPanel_Property); }
            set { SetValue(visiblePidPanel_Property, value); }
        }
        public static readonly DependencyProperty visiblePidPanel_Property =
                   DependencyProperty.Register("visiblePidPanel", typeof(bool),
                   typeof(ucDriver), new PropertyMetadata(false, changedPIDVisible));

        private static void changedPIDVisible(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucDriver).ucPid.Opacity = 100;
        }

        //-----------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Наличие жидкости на входе.")]
        public bool isInLiquidPesence
        {
            get { return (bool)GetValue(isLiquidPesence_Property); }
            set { SetValue(isLiquidPesence_Property, value); }
        }
        public static readonly DependencyProperty isLiquidPesence_Property =
                   DependencyProperty.Register("isInLiquidPesence", typeof(bool),
                   typeof(ucDriver), new PropertyMetadata(false, changedIsInLiquidPesence));

        private static void changedIsInLiquidPesence(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucDriver).isOutLiquidPesence = (bool)e.NewValue;
        }

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Наличие жидкости на выходе.")]
        public bool isOutLiquidPesence
        {
            get { return (bool)GetValue(isOutLiquidPesence_Property); }
            set { SetValue(isOutLiquidPesence_Property, value); }
        }
        public static readonly DependencyProperty isOutLiquidPesence_Property =
                   DependencyProperty.Register("isOutLiquidPesence", typeof(bool),
                   typeof(ucDriver), new PropertyMetadata(false, chackIsOutLiquidPesence));

        private static void chackIsOutLiquidPesence(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d as ucDriver).IsWorker || !(d as ucDriver).isInLiquidPesence) (d as ucDriver).isOutLiquidPesence = false;
        }

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Контроль включения")]
        public bool controlOn
        {
            get { return (bool)GetValue(controlOnProperty); }
            set { SetValue(controlOnProperty, value); }
        }
        public static readonly DependencyProperty controlOnProperty =
            DependencyProperty.Register("controlOn", typeof(bool), typeof(ucDriver), new PropertyMetadata(false, changedControlOn));

        private static void changedControlOn(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue && (d as ucDriver).timer.IsEnabled) (d as ucDriver).timer.Stop();
            else if ((bool)e.NewValue && !(d as ucDriver).Worker) (d as ucDriver).Error = true;
            else if (!(bool)e.NewValue && (d as ucDriver).Worker) (d as ucDriver).Error = true;
        }

        //--------------------------------------------------------------------------------------------------------
        public ucDriver()
        {
            InitializeComponent();
        }

        //-----------------------------------------------------------------------------------------------------------------
        private void buttonStop_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsWorker = true;
        }

        //-----------------------------------------------------------------------------------------------------------------
        private void buttonStart_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Error) return;
            IsWorker = true;
        }

        //-----------------------------------------------------------------------------------------------------------------
        public void ChengeAmperage(ModBasValue mv)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    arrowAmperage.Value = double.Parse(mv.Value);
                 }
                catch { }
            }
  
            ));
        }

        //-----------------------------------------------------------------------------------------------------------------
        private void ChengeOsPid(ModBasValue mv)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    ucPid.PidOS = double.Parse(mv.Value);
                }
                catch { }
            }

             ));
        }

        //-----------------------------------------------------------------------------------------------------------------
        private void ChengeExternalError(Bit mv)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    ModBusError = mv.Val;
                }
                catch { }
            }
            ));
        }

        //-----------------------------------------------------------------------------------------------------------------
        private void ChengeControlON(Bit value)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    //timer.Stop();
                    controlOn = value.Val;
                }
                catch { }
            }
            ));

        }

        //-----------------------------------------------------------------------------------------------------------------
        private void ucDriverDirection_Loaded(object sender, RoutedEventArgs e)
        {
            myTrans = CanvasSetting.RenderTransform;
            myTransPid = CanvasPID.RenderTransform;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += ChengeOnDriver;
            timer.Interval = timeOnControl;


            ucPid.Name = Name + "_PID";
            try
            {
                if (valAmperage != null) SetCallBackForModBusValue(ChengeAmperage, valAmperage, valCategoryMB);
                if (valOsPid != null) SetCallBackForModBusValue(ChengeOsPid, valOsPid, valCategoryMB);
                if (valExternalError != null) SetCallBackForBitValue(ChengeExternalError, valExternalError, valCategoryMB);
                if (valControlON != null) SetCallBackForBitValue(ChengeControlON, valControlON, valCategoryMB);

            }
            catch { }


            if (IsPresentPID)
            {
                ucPid.PidValue_Changed += UcPid_PidValue_Changed;
            }
        }

        ////-----------------------------------------------------------------------------------------------------------------
        private void UcPid_PidValue_Changed(object sender, RoutedEventArgs e)
        { 
            //SetValueModBus(valOutFRQ, Convert.ToSingle(e.OriginalSource ) * scaleOutFRQ);
            arrowFRQ.Value = (double)e.OriginalSource;
        }

        //-----------------------------------------------------------------------------------------------------------------
        private void ucPid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CanvasPID.RenderTransform = myTransPid;
        }

        //-----------------------------------------------------------------------------------------------------------------
        private void arrowFRQ_ChangedValue_Event(object sender, RoutedEventArgs e)
        {
            SetValueModBus(valOutFRQ, (Convert.ToSingle(e.OriginalSource) )* 2 * (Single)scaleOutFRQ);
            if (arrowFRQ.Value < arrowFRQ.MinValue)
                try {SetValueModBus(valSetOnOff, false); } catch { }
                else try { SetValueModBus(valSetOnOff, IsWorker); } catch { }
                //SetValue(IsWorker_Property, false);
            //else SetValue(IsWorker_Property, Worker);
        }

        //-----------------------------------------------------------------------------------------------------------------
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if(!Error)IsWorker = true;
        }

        //-----------------------------------------------------------------------------------------------------------------
        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            IsWorker = false;

        }

        //-----------------------------------------------------------------------------------------------------------------
        private void buttonReset__Click(object sender, RoutedEventArgs e)
        {
            Error = false;
            ClearValue(IsWorkerProperty);
            ClearValue(Error_Property);
            ClearValue(controlOnProperty);

        }

        //-----------------------------------------------------------------------------------------------------------------
        private void CanvasSetting_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CanvasSetting.RenderTransform = myTrans;
        }
        //-----------------------------------------------------------------------------------------------------------------

        public override DataSave GetSaveData()
        {
            DataSave ds = new DataSave();
    
            SaveData sd = new SaveData();
            sd.max = (int)arrowFRQ.MaxValue;
            sd.min = (int)arrowFRQ.MinValue;
            sd.maxAmperage = (int)maxAmperage.Value;
            sd.mode = comboBox.SelectedIndex;

            ds.NameDataId = Name;
            ds.typ = GetType();
            ds.data = DataSaveSCADA.SerializableToString<SaveData>(sd);
            return ds;
        }

        //private void ucDriverDirection_Loaded(object sender, RoutedEventArgs e)
        //{
            
        //}


        //-----------------------------------------------------------------------------------------------------------------
        public override bool SetSaveData(DataSave data)
        {
            if (data == null) return false;
            if (data.typ != GetType() || data.NameDataId != Name) return false;
            SaveData sd;
            sd = DataSaveSCADA.SerializableStringToObject<SaveData>(data.data);

            arrowFRQ.MaxValue = sd.max;
            arrowFRQ.MinValue = sd.min;
            maxAmperage.Value = sd.maxAmperage;
            comboBox.SelectedIndex = sd.mode;
            return true;

        }

        //-----------------------------------------------------------------------------------------------------------------
    }
}
