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

namespace LibraryMySCADA.Flowmeter
{
    public partial class ucFlowmeter : Virt.ClassVirtualAdd
    {
        [Category("Setting")]
        public nsClassModBus.ClassModBus cmb;

        [Category("Setting")]
        public bool isWorkIN
        {
            get { return (bool)GetValue(isWorkINProperty); }
            set { SetValue(isWorkINProperty, value); }
        }
        public static readonly DependencyProperty isWorkINProperty =
            DependencyProperty.Register("isWorkIN", typeof(bool), typeof(ucFlowmeter), new PropertyMetadata(false,changeWork));

        private static void changeWork(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            ucFlowmeter obj = d as ucFlowmeter;
            if((bool)e.NewValue) obj.start = true;
            obj.time = DateTime.Now;
            obj.FMControl.timeText.Text = obj.time.ToString(@"hh\:mm\:ss");
        }

        //-------------------------------------------------------------------------------------------------------------------
        public double GetValueFlowmeter()
        {
            return FMControl.DailyCounter + FMControl.AllCounter;
        }

        //-------------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public Grid PaneltoGrid
        {
            get { return (Grid)GetValue(PaneltoGridProperty); }
            set { SetValue(PaneltoGridProperty, value); }
        }
        public static readonly DependencyProperty PaneltoGridProperty =
            DependencyProperty.Register("PaneltoGrid", typeof(Grid), typeof(ucFlowmeter), new PropertyMetadata(null));

        //-------------------------------------------------------------------------------------------------------------------

        [Category("Setting")]
        public string CtegoryMB { get; set; } = "";

        [Category("Setting")]
        [Description("Имя переменной для чтения, ModBUS")]
        public string nameDataReadMB { get; set; } = "FlowMetrRead";

        [Category("Setting")]
        [Description("Имя переменной для записи, ModBUS")]
        public string nameDataWriteMB { get; set; } = "FlowMetrWrite";

        [Category("Setting")]
        public string Title { get { return (string)FMControl.LabelTitle.Content; } set { FMControl.LabelTitle.Content = value; } }


        //-------------------------------------------------------------------------------------------------------------------
        public bool visibleControlPanel {
            get { return FMControl.Visibility == Visibility.Hidden ? false:true; }
            set { FMControl.Visibility=  value? Visibility.Visible: Visibility.Hidden; } }

        private bool start;

        //-------------------------------------------------------------------------------------------------------------------


        DateTime time = new DateTime(0);
        public double fs;
        public double fh;

        public FlowMeterControl FMControl { get; set; } = new FlowMeterControl();

        //-------------------------------------------------------------------------------------------------------------------
        public ucFlowmeter()
        {
            InitializeComponent();
            canvas.Children.Add(FMControl);
            
            FMControl.VisiblePanel = false;
            FMControl.rButton.Click += RButton_Click;
            FMControl.NewValueFlowMeter_Changed += FMControl_NewValueFlowMeter_Changed;
        }

        //-------------------------------------------------------------------------------------------------------------------
        private void FMControl_NewValueFlowMeter_Changed(object sender, RoutedEventArgs e)
        {
            ClassModBus.SetOrUnsetIgnorableVariable(nameDataWriteMB, false);
            ClassModBus.SetValueModBus(nameDataWriteMB, (int)0, CtegoryMB, true);
            FMControl.AllCounter = (int)e.OriginalSource;
            FMControl.DailyCounter = 0;
            FMControl.CommonFlowMeter.ValueCounter = (int)e.OriginalSource;
        }


        //-------------------------------------------------------------------------------------------------------------------
        private void RButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        //-------------------------------------------------------------------------------------------------------------------
        private void FlowMeter_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FMControl.VisiblePanel = true;
        }

        //-------------------------------------------------------------------------------------------------------------------
        private void FlowMeter_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            if (PaneltoGrid != null)
            {
                canvas.Children.Remove(FMControl);
                PaneltoGrid.Children.Add(FMControl);
                Point point = this.TranslatePoint(new Point(0, 0), PaneltoGrid);
                FMControl.Margin = new Thickness(point.X, point.Y - ActualWidth - 50, 0, 0);//- FMControl.ActualHeight/2
            }
            ClassModBus.SetCallBackForModBusValue(readModBus, nameDataReadMB, CtegoryMB);
            FMControl.Name = Name;
        }

        //-------------------------------------------------------------------------------------------------------------------
        private void readModBus(ModBasValue value)
        {
                try
                {
                    int val = int.Parse(value.Value); 
                    FMControl.DailyCounter = val;
                }
                catch { }

                DateTime time2 = DateTime.Now;

                if (time2 == time) return;

                if(time.DayOfYear != time2.DayOfYear)
                {
                    UpdateDataInDevice();
                }

                if (start) { start = false; time = time2; return; }
                FMControl.timeText.Text = (time2 - time).ToString(@"hh\:mm\:ss");
                fs = Math.Round((1d / (time2 - time).TotalMinutes),2);
                fh = Math.Round((1d / (time2 - time).TotalHours),2);
                FMControl.secondCount.Text = fs.ToString();
                FMControl.hourCount.Text = fh.ToString();
                time = time2;
        }

        //-------------------------------------------------------------------------------------------------------------------
        void UpdateDataInDevice()
        {
            ClassModBus.SetOrUnsetIgnorableVariable(nameDataWriteMB, false, "CtegoryMB");
            FMControl.AllCounter = FMControl.AllCounter + FMControl.DailyCounter;
            ClassModBus.SetValueModBus(nameDataWriteMB, (int)0, CtegoryMB, true);
            //FMControl.AllCounter = FMControl.AllCounter + FMControl.DailyCounter;
            FMControl.DailyCounter = 0;
        }
        //-------------------------------------------------------------------------------------------------------------------

        [Serializable]
        public class SaveData
        {
            public DateTime dateTime;
            public double counterValue;
            public double dailyCounter;
        }

        public override DataSave GetSaveData()
        {
            SaveData sd = new SaveData();
            sd.counterValue = FMControl.AllCounter;
            sd.dateTime = DateTime.Now;
            sd.dailyCounter = FMControl.DailyCounter;

            DataSave ds = new DataSave();
            ds.NameDataId = Name;
            ds.typ = GetType();
            ds.data = DataSaveSCADA.SerializableToString<SaveData>(sd);
            return ds;

        }

        public override bool SetSaveData(DataSave data)
        {
            if (data == null || data.typ != GetType() || data.NameDataId != Name) return false;
            SaveData sd;
            sd = DataSaveSCADA.SerializableStringToObject<SaveData>(data.data);
            FMControl.AllCounter = sd.counterValue;
            FMControl.DailyCounter = sd.dailyCounter;
            time = sd.dateTime;
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
    }
}
