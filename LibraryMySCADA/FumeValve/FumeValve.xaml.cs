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
using LibraryMySCADA.Virt;

namespace LibraryMySCADA.FumeValve
{
    /// <summary>
    /// Логика взаимодействия для FumeValve.xaml
    /// </summary>
    public partial class FumeValve :  ClassVirtualAdd
    {

        float maxMargin = 196;
        float minMargin = 140;

        //---------------------------------------------------------------------------------------------
        [Category("Setting")]
        public bool isVisiblePIDbutton
        {
            get { return (bool)GetValue(isVisiblePIDbuttonProperty); }
            set { SetValue(isVisiblePIDbuttonProperty, value); }
        }
        public static readonly DependencyProperty isVisiblePIDbuttonProperty =
            DependencyProperty.Register("isVisiblePIDbutton", typeof(bool), typeof(FumeValve), new PropertyMetadata(true));

        //---------------------------------------------------------------------------------------------
        [Category("Setting")]
        public bool isWorker
        {
            get { return (bool)GetValue(isWorkerProperty); }
            set { SetValue(isWorkerProperty, value); }
        }
        public static readonly DependencyProperty isWorkerProperty =
            DependencyProperty.Register("isWorker", typeof(bool), typeof(FumeValve), new PropertyMetadata(false));

        //---------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Проценнт открытия клапана ( от 0 до 100 )")]
        public float valueOpened
        {  
            get { return (float)GetValue(valueOpenedProperty); }
            set { SetValue(valueOpenedProperty, value); }
        }
        public static readonly DependencyProperty valueOpenedProperty =
            DependencyProperty.Register("valueOpened", typeof(float), typeof(FumeValve), 
                new PropertyMetadata((float)0, changedValue));

        private static void changedValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FumeValve).ChangedValue(e);
        }

        //---------------------------------------------------------------------------------------------
        private void ChangedValue(DependencyPropertyChangedEventArgs e)
        {
            float del = maxMargin - minMargin;
            float proc = (float)del / 100;
            float Y = minMargin + (proc * ( 100 - (float)e.NewValue));
            Hodunok.Margin = new Thickness(Hodunok.Margin.Left,Y, Hodunok.Margin.Right, Hodunok.Margin.Bottom);
            if (OutValveOpenValue != null)
            {
                ClassModBus.SetValueModBus(OutValveOpenValue, (float)e.NewValue, Category);
            }

            if ((float)e.NewValue > 0) isWorker = true;
                else isWorker = false;
        }

        //---------------------------------------------------------------------------------------------
        [Category("Setting")]
        public bool isAuto
        {
            get { return (bool)GetValue(isAutoProperty); }
            set { SetValue(isAutoProperty, value); }
        }
        public static readonly DependencyProperty isAutoProperty =
            DependencyProperty.Register("isAuto", typeof(bool), typeof(FumeValve), new PropertyMetadata(false, changedAuto));

        private static void changedAuto(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FumeValve).buttonCloseValve.Visibility = (bool)e.NewValue ? Visibility.Collapsed: Visibility.Visible;
            (d as FumeValve).ChangedAuto(e);
        }

        //---------------------------------------------------------------------------------------------
        private void ChangedAuto(DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (isPid) { ucPid.ResetPid(); ucPid.StartPID(false); }
                else valueOpened =  (float)slider.Value;
            }
            else
            {
                if (isPid) { ucPid.StopPID(false); valueOpened = 0; }
                else valueOpened = 0; 
            }
        }

        //---------------------------------------------------------------------------------------------
        [Category("Setting")]
        public float OSPID
        {
            get { return (float)GetValue(OSPIDProperty); }
            set { SetValue(OSPIDProperty, value); }
        }
        public static readonly DependencyProperty OSPIDProperty =
            DependencyProperty.Register("OSPID", typeof(float), typeof(FumeValve), new PropertyMetadata((float)0, changedOs));

        private static void changedOs(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FumeValve).ucPid.PidOS = (float)e.NewValue;
        }

        //---------------------------------------------------------------------------------------------
        [Category("Setting")]
        public double setTProduct
        {
            get { return (double)GetValue(setTProductProperty); }
            set { SetValue(setTProductProperty, value); }
        }
        public static readonly DependencyProperty setTProductProperty =
            DependencyProperty.Register("setTProduct", typeof(double), typeof(FumeValve), new PropertyMetadata((double)0));



        //---------------------------------------------------------------------------------------------
        [Category("Setting")]
        public bool isPid
        {
            get { return (bool)GetValue(isPidProperty); }
            set { SetValue(isPidProperty, value); }
        }
        public static readonly DependencyProperty isPidProperty =
            DependencyProperty.Register("isPid", typeof(bool), typeof(FumeValve), new PropertyMetadata(false));

 
        //---------------------------------------------------------------------------------------------
        [Category("Setting_ModBUS")]
        public string Category { get; set; } = null;

        [Category("Setting_ModBUS")]
        public string OutValveOpenValue { get; set; }

        [Category("Setting_ModBUS")]
        public string inValvePidOSValue { get; set; } = null;

        //---------------------------------------------------------------------------------------------
        [Category("Setting")]
        public string NameFume
        {
            get { return Name; }
            set { this.Name = value; }
        }
        //---------------------------------------------------------------------------------------------



        public FumeValve()
        {
            InitializeComponent();
        }
        //---------------------------------------------------------------------------------------------
        private void buttonOpenValve_Click(object sender, RoutedEventArgs e)
        {
            valueOpened = (float)slider.Value;
        }
        //---------------------------------------------------------------------------------------------
        private void buttonCloseValve_Click(object sender, RoutedEventArgs e)
        {
            valueOpened = 0;
        }
        //---------------------------------------------------------------------------------------------
        private void buttonPID_Click(object sender, RoutedEventArgs e)
        {

        }
        //---------------------------------------------------------------------------------------------
        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            ucPid.Name = Name + "_PID";
            try
            {
                if (inValvePidOSValue != null) SetCallBackForModBusValue(ChengeOsPid, inValvePidOSValue, Category);
            }
            catch {  }
            ucPid.PidSetpoint = numericOsPid.Value;
        }
        //---------------------------------------------------------------------------------------------
        private void ChengeOsPid(ModBasValue mv)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    ucPid.PidOS = float.Parse(mv.Value);
                }
                catch { }
            }

         ));
        }

        //---------------------------------------------------------------------------------------------
        private void ucPid_PidValue_Changed(object sender, RoutedEventArgs e)
        {
            var d = ucPid.PidValue;
            if(isAuto) valueOpened = (float)d;
        }
        //---------------------------------------------------------------------------------------------
        private void ucNumberOfUpDown_Value_Changed(object sender, RoutedEventArgs e)
        {
            if (ucPid == null) return;
            ucPid.PidSetpoint =  numericOsPid.Value;
        }
        //---------------------------------------------------------------------------------------------
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isAuto && !isPid) valueOpened = (float)e.NewValue;
        }

        //---------------------------------------------------------------------------------------------
        [Serializable]
        public struct SaveData
        {
            public double setT;
            public double sliderVAlue;
        }

        public override DataSave GetSaveData()
        {
            DataSave ds = new DataSave();

            SaveData sd = new SaveData();
            sd.setT = numericOsPid.Value;
            sd.sliderVAlue = slider.Value;

            ds.NameDataId = Name;
            ds.typ = GetType();
            ds.data = DataSaveSCADA.SerializableToString<SaveData>(sd);
            return ds;
        }

         //-----------------------------------------------------------------------------------------------------------------
        public override bool SetSaveData(DataSave data)
        {
            if (data == null) return false;
            if (data.typ != GetType() || data.NameDataId != Name) return false;
            SaveData sd;
            sd = DataSaveSCADA.SerializableStringToObject<SaveData>(data.data);

            numericOsPid.Value = sd.setT;
            slider.Value = sd.sliderVAlue;
            return true;

        }

        //-----------------------------------------------------------------------------------------------------------------


    }
}
