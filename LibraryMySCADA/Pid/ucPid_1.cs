using System;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using wpfLibMszControl.Pid;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Windows;
using System.Collections.Specialized;
using System.ComponentModel;
using LibraryMySCADA;
using System.Windows.Threading;
using System.Windows.Documents;

namespace wpfMszControl.Pid
{

    public enum PidModeCalc
    {
        toTime,
        toChangedOS
    }


    //------------------------------------------------------------------------------------------------------
    public partial class ucPid:LibraryMySCADA.Virt.ClassVirtualAdd
    {
        //------------------------------------------------------------------------------------------------------
        [Category("Variable")]
        public PidModeCalc modeCalc { get; set; } = PidModeCalc.toTime;

        bool WorkPidIsChangedOSMode = false;
        //------------------------------------------------------------------------------------------------------
        [Category("Variable")]
        public double MaxPID
        {
            get { return (double)GetValue(MaxPIDProperty); }
            set { SetValue(MaxPIDProperty, value); }
        }
        public static readonly DependencyProperty MaxPIDProperty =
            DependencyProperty.Register("MaxPID", typeof(double), typeof(ucPid), new PropertyMetadata(ChangeMaxPID));

        private static void ChangeMaxPID(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //(d as ucPid).PidArrow.MaxValue = (double)e.NewValue;
            (d as ucPid).pid.MaxPID = (double)e.NewValue;
        }

        //------------------------------------------------------------------------------------------------------
        [Category("Variable")]
        public double MinPID
        {
            get { return (double)GetValue(MinPIDProperty); }
            set { SetValue(MinPIDProperty, value); }
        }
        public static readonly DependencyProperty MinPIDProperty =
            DependencyProperty.Register("MinPID", typeof(double), typeof(ucPid), new PropertyMetadata(ChangeMinPID));

        private static void ChangeMinPID(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           // (d as ucPid).PidArrow.MinValue = (double)e.NewValue;
            (d as ucPid).pid.MinPID = (double)e.NewValue;
        }


        //------------------------------------------------------------------------------------------------------
        [Category("Variable")]
        [Description("Смещения выхода  PID")]
        public double OffsetPID
        {
            get { return (double)GetValue(OffsetPIDProperty); }
            set { SetValue(OffsetPIDProperty, value); }
        }
        public static readonly DependencyProperty OffsetPIDProperty =
            DependencyProperty.Register("OffsetPID", typeof(double), typeof(ucPid), new PropertyMetadata(0d));
        //------------------------------------------------------------------------------------------------------

        [Description("флаг указывающий что была рассчитана начальная точка ПИД")]
        public bool flagCalcStartPoint;

        [Category("Variable")]
        public bool PausePid
        {
            get { return (bool)GetValue(PausePidProperty); }
            set { SetValue(PausePidProperty, value); }
        }
        public static readonly DependencyProperty PausePidProperty =
            DependencyProperty.Register("PausePid", typeof(bool), typeof(ucPid), new PropertyMetadata(false, ChangePausePid));

        private static void ChangePausePid(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ucPid obj = d as ucPid;
            if((bool)e.NewValue && obj.timerWorkPID.IsEnabled)
            {
                obj.AddLoger("ПИД-ПАУЗА");
                obj.timerWorkPID.Stop();
            }
            else if(obj.PidWork && !obj.timerWorkPID.IsEnabled)
            {
                obj.AddLoger("ПИД-СНЯТИЕ С ПАУЗЫ");
                obj.timerWorkPID.Start();
            }
        }

        //------------------------------------------------------------------------------------------------------
        [Category("Variable")]
        [Description("В каком режиме работать регулятору (false - нагрев, true - охлаждение)")]
        public bool CoolingHeat
        {
            get { return (bool)GetValue(CoolingHeatProperty); }
            set { SetValue(CoolingHeatProperty, value); }
        }
        public static readonly DependencyProperty CoolingHeatProperty =
            DependencyProperty.Register("CoolingHeat", typeof(bool), typeof(ucPid), new PropertyMetadata(false));

        //------------------------------------------------------------------------------------------------------
        [Category("Variable")]
        [Description("Сигнал включения ограничения ПИД по масимуму")]
        public bool LimitPidOnMax
        {
            get { return (bool)GetValue(LimitPidOnMaxProperty); }
            set { SetValue(LimitPidOnMaxProperty, value); }
        }
        public static readonly DependencyProperty LimitPidOnMaxProperty =
            DependencyProperty.Register("LimitPidOnMax", typeof(bool), typeof(ucPid), new PropertyMetadata(false, ChangedLimitPidOnMax));

        private static void ChangedLimitPidOnMax(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RoutedEventArgs b = new RoutedEventArgs(LimitPidOnMax_Event, (bool)e.NewValue);
            (d as ucPid).RaiseEvent(b);

        }

        public static readonly RoutedEvent LimitPidOnMax_Event = EventManager.RegisterRoutedEvent("LimitPidOnMaxChanged", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(ucPid));

        public event RoutedEventHandler LimitPidOnMax_Changed
        {
            add { AddHandler(LimitPidOnMax_Event, value); }
            remove { RemoveHandler(LimitPidOnMax_Event, value); }
        }

        //------------------------------------------------------------------------------------------------------
        [Category("Variable")]
        [Description("Сигнал включения ограничения ПИД по минимуму")]
        public bool LimitPidOnMin
        {
            get { return (bool)GetValue(LimitPidOnMinProperty); }
            set { SetValue(LimitPidOnMinProperty, value); }
        }
        public static readonly DependencyProperty LimitPidOnMinProperty =
            DependencyProperty.Register("LimitPidOnMin", typeof(bool), typeof(ucPid), new PropertyMetadata(false, ChangedLimitPidOnMin));

        private static void ChangedLimitPidOnMin(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RoutedEventArgs b = new RoutedEventArgs(LimitPidOnMin_Event, (bool)e.NewValue);
            (d as ucPid).RaiseEvent(b);

        }

        public static readonly RoutedEvent LimitPidOnMin_Event = EventManager.RegisterRoutedEvent("LimitPidOnMinChanged", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(ucPid));

        public event RoutedEventHandler LimitPidOnMin_Changed
        {
            add { AddHandler(LimitPidOnMin_Event, value); }
            remove { RemoveHandler(LimitPidOnMin_Event, value); }
        }

        //------------------------------------------------------------------------------------------------------
        [Category("Variable")]
        public double StarPoint
        {
            get { return (double)GetValue(StarPointProperty); }
            set { SetValue(StarPointProperty, value); }
        }
        public static readonly DependencyProperty StarPointProperty =
            DependencyProperty.Register("StarPoint", typeof(double), typeof(ucPid), new PropertyMetadata(0d, ChangedStarPoint));

        private static void ChangedStarPoint(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucPid).pid.StartPoint = (double)e.NewValue;
        }

        //------------------------------------------------------------------------------------------------------
        double pidOrigin;

        public enum PID_PRESENCE { Yes, No }

        public PID pid;

        bool flagPid;
        DispatcherTimer timerOn, timerOff, timerWorkPID;
        //------------------------------------------------------------------------------------------------------
        public ucPid()
        {
            InitializeComponent();
            DataContext = this;

            timerOn = new DispatcherTimer();
            timerOn.Tick += timeTickON;//таймер запуска

            timerOff = new DispatcherTimer();
            timerOff.Tick += timeTickOFF;//таймер остановки

            timerWorkPID = new DispatcherTimer();
            timerWorkPID.Tick += timePidTick;//таймер работы (вычисления пид)
            pid = (PID)FindResource("_Pid");
        }

        //------------------------------------------------------------------------------------------------------
        private void timeTickON(object sender, EventArgs e)// Запуск вычесления ПИД
        {
            timerOn.Stop();
            //if(StarPoint>0) SetStartPoinForIntegral(StarPoint);
            timerWorkPID.Interval = new TimeSpan((long)(TimeSpan.TicksPerSecond * pid.TimePid));
            if (!flagCalcStartPoint) { SetStartPoinForIntegral(StarPoint); CalcStartPoint(pid.StartPoint); }
            AddLoger("ПИД-Работа");
            flagPid = false;
            timerWorkPID.Start();
            PidWork = true;
            flagCalcStartPoint = false;
        }

        //------------------------------------------------------------------------------------------------------
        private void timeTickOFF(object sender, EventArgs e)// Остановка вычесления ПИД
        {
            timerOff.Stop();
            timerWorkPID.Stop();
            AddLoger("ПИД-Остановлен");
            PidWork = false;
            PidValue = 0;
        }

        //------------------------------------------------------------------------------------------------------
        private void timePidTick(object sender, EventArgs e) // Вычесление ПИД
        {
            if (!isEneble) return;
            double prK = pid.PropK * (CoolingHeat ? -1 : 1);
            double intK = pid.IntegralK * (CoolingHeat ? -1 : 1);
            double difK= pid.DifK * (CoolingHeat ? -1 : 1);

            pid.e = (PidSetpoint - PidOS);//e - рассоглосования
            //Пропорциональный регулятор 
            pid.Prop_Work = prK * pid.e;//пропорцианальный выход
            //Алгоритм интегрального регулятора. 
            pid.S += pid.e;   // к переменной S прибавляется E ошибка рассогласования 
            pid.Integral_Work = intK * pid.S;  // K_INTEGRAL, коэффициент интегрирования умножается на переменную S (pid.S += pid.e)
            //Алгоритм дифференциального звена
            pid.Dif_Work = difK * (pid.e - pid.En);   // Kd, коэффициент дифференцирования умножается на разность e и En. 

            pid.En = pid.e;

            pidOrigin = (pid.on_PropK ? pid.Prop_Work:0) + (pid.on_IntegK ? pid.Integral_Work:0) + (pid.on_DifK ? pid.Dif_Work:0);

            if (pidOrigin > (pid.MaxPID) || (pidOrigin < (pid.MinPID)))//ограничивает пид - если больше установленного то устанавливается предыдущее состояние пид
            {
                if(pidOrigin > (pid.MaxPID)) LimitPidOnMax = true;
                else if (pidOrigin < (pid.MinPID)) LimitPidOnMin = true;

                if (pidOrigin > (pid.MaxPID * 2) && !pid.LimitCalc && flagPid)//принудительное ограничение пида
                {
                    CalcStartPoint(pid.MaxPID);
                    
                }
                else if (pidOrigin < (pid.MinPID * 2) && !pid.LimitCalc && flagPid)//принудительное ограничение пида
                {
                    CalcStartPoint(pid.MinPID);
                }
                else if (pid.LimitCalc) 
                {
                    if (pidOrigin < pid.MinPID && flagPid)
                    {
                        CalcStartPoint(pid.MinPID);
                    }
                    else if (pidOrigin > pid.MinPID && flagPid)
                    {
                        CalcStartPoint(pid.MaxPID);
                    }
                }
                else pid.En = pid.e;

                if (pidOrigin > pid.MaxPID) pid.ValuePID = pid.MaxPID;
                else pid.ValuePID = pid.MinPID;

            }
            else
            {
                LimitPidOnMax = false;
                LimitPidOnMin = false;


                pid.En = pid.e;
                pid.ValuePID = pidOrigin;
            }

            if (pidOrigin > pid.MinPID || pidOrigin < pid.MaxPID) flagPid = true;

            if (pid.AlwaysPositive)//если пид всегда положительный
            {
                pid.ValuePID = Math.Abs(pid.ValuePID);

            }

            PidArrow.Value = pid.ValuePID;
            PidValue = pid.ValuePID + OffsetPID;
            
        }

        //------------------------------------------------------------------------------------------------------
        public void SetStartPoinForIntegral(double point)
        {
            
            StarPoint = point;
            ResetPid();
        }

        //------------------------------------------------------------------------------------------------------
        void CalcStartPoint(double point)
        {
            try
            {
                double prK = pid.PropK * (CoolingHeat ? -1 : 1);
                double intK = pid.IntegralK * (CoolingHeat ? -1 : 1);
                //double difK = pid.DifK * (CoolingHeat ? -1 : 1);


                if (pid.StartPoint < pid.MinPID) pid.StartPoint = pid.MinPID;
                if (pid.StartPoint > pid.MaxPID) pid.StartPoint = pid.MaxPID;
               // if (point == 0) return;
                double e = (PidSetpoint - PidOS);
                pid.Prop_Work = prK * e;
                pid.En = e;
                pid.Dif_Work = 0;// pid.DifK * e;
                pid.S = (point - pid.Prop_Work - pid.Dif_Work) / (intK == 0 ? 1: intK);
                pid.S -= e;
                flagCalcStartPoint = true;
            }
            catch { }
        }

        //------------------------------------------------------------------------------------------------------
        public void OneCalcPID(int count=1) { CalcStartPoint(StarPoint);  for (int r = 0; r<count; r++)  timePidTick(null, null); }
        //------------------------------------------------------------------------------------------------------
        public void StartPID(bool TimeOut)//запуск регулятора
        {
            if(modeCalc== PidModeCalc.toChangedOS)
            {
                WorkPidIsChangedOSMode = true;
                return;
            }

            if ((timerOn.IsEnabled && TimeOut) || timerWorkPID.IsEnabled || !IsLoaded) return;
            //if (StartPoint > 0) SetStartPoinForIntegral((double)StartPoint);
            //else if(StarPoint>0) SetStartPoinForIntegral(StarPoint);

            if (timerOff.IsEnabled) { timerOff.Stop(); AddLoger("Отмена остановки ПИД"); }

            if (TimeOut)
            {
                timerOn.Interval = new TimeSpan(TimeSpan.TicksPerSecond * (long)pid.TimeOutPid);
                timerOn.Start();
                if (pid.TimeOutPid > 0) AddLoger("Запуск ПИД с задержкой " + numTimeOut.Value + "сек.");
                return;
            }
            timerOff.Stop();
            timerOn.Stop();
            timeTickON(null, null);
        }
        //------------------------------------------------------------------------------------------------------
        public void StopPID(bool TimeOut)//остановка регулятора
        {
            if(modeCalc == PidModeCalc.toChangedOS)
            {
                WorkPidIsChangedOSMode = false;
                return;
            }

            if (timerOff.IsEnabled && TimeOut) return;//) || !timerWorkPID.IsEnabled
            if (timerOn.IsEnabled) { timerOn.Stop(); AddLoger("Отмена запуска ПИД"); }

            if (TimeOut)
            {
                timerOff.Interval = new TimeSpan(TimeSpan.TicksPerSecond * (long)pid.TimeOutPidOff);
                timerOff.Start();
                if (pid.TimeOutPidOff > 0) AddLoger("Остановка ПИД с задержкой " + numTimeOut.Value + "сек.");
                return;
            }
            timerOff.Stop();
            timerOn.Stop();
            timeTickOFF(null, null);
        }

        //------------------------------------------------------------------------------------------------------
         private void AddLoger(string log)
        {
            string Text = DateTime.Now.ToString("HH:mm:ss") + " - " + log+"\n";
            flowDocument.Document.Blocks.LastBlock.ContentStart.InsertTextInRun(Text); 
        }

        //-----------------------------------------------------------------------------------------------------
        public bool LoadDataPID(string name = null)
        {
            if (name == null) name = "Setting\\" + Name + ".xmlpid";
            try
            {
                
                Stream stream;
                XmlSerializer serializerSetting = new XmlSerializer(typeof(PID));
                using (stream = File.OpenRead(name))
                {
                    PID s = (PID)serializerSetting.Deserialize(stream);
                    PID t = (PID)Resources["_Pid"];

                    t.AlwaysPositive = s.AlwaysPositive;
                    t.LimitCalc = s.LimitCalc;
                    t.PropK = s.PropK;
                    t.DifK = s.DifK;
                    t.IntegralK = s.IntegralK;
                    t.StartPoint = s.StartPoint;
                    StarPoint = t.StartPoint;
                    t.TimeOutPidOff = s.TimeOutPidOff;
                    t.MaxPID = s.MaxPID;
                    t.MinPID = s.MinPID;
                    
                    t.TimeOutPid = s.TimeOutPid;
                    t.TimePid = s.TimePid;

                    t.on_DifK = s.on_DifK;
                    t.on_IntegK = s.on_IntegK;
                    t.on_PropK = s.on_PropK;

                    CoolingHeat = s.CoolingHeat;
                }
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        //-----------------------------------------------------------------------------------------------------
        void SaveDataPID(string name = null)
        {
            if (name == null) name = "Setting\\" + Name + ".xmlpid";
            Directory.CreateDirectory("Setting");
            try
            {

                PID pid = (PID)FindResource("_Pid");
                Directory.CreateDirectory("Setting");
                XmlSerializer serializerSetting = new XmlSerializer(typeof(PID));
                using (Stream s = File.Create(name))
                {
                    serializerSetting.Serialize(s, pid);
                    s.Close();
                }
            }
            catch { }
        }

        //-----------------------------------------------------------------------------------------------------
        private void button_File_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.DefaultExt = "xmlpid";
            openDialog.Filter = "Данные PID|*.xmlpid";
            openDialog.InitialDirectory = "Setting\\";
            if (openDialog.ShowDialog().Value)
            {
                LoadDataPID(openDialog.FileName);
            }
        }
        //-----------------------------------------------------------------------------------------------------

        private void button_File_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xmlpid";
            saveDialog.Filter = "Данные PID|*.xmlpid";
            saveDialog.InitialDirectory = "Setting\\";
            if (saveDialog.ShowDialog().Value)
            {
                SaveDataPID(saveDialog.FileName);
            }   
        }

        //-----------------------------------------------------------------------------------------------------
        private void numTime_PID_Value_Changed(object sender, RoutedEventArgs e)
        {
            pid.TimePid = (double)e.OriginalSource;
                if (timerWorkPID.IsEnabled)
                {
                    timerWorkPID.Stop();
                    var d = TimeSpan.TicksPerSecond * pid.TimePid;
                    timerWorkPID.Interval = new TimeSpan((long)d);
                    timerWorkPID.Start();
                }
        }

        //-----------------------------------------------------------------------------------------------------
        public void ResetPid()
        {
            button_ResetPid_Click(null, null);
        }
        //-----------------------------------------------------------------------------------------------------
        private void button_ResetPid_Click(object sender, RoutedEventArgs e)
        {
            PID pid = (PID)FindResource("_Pid");
            pid.Integral_Work = 0;
            pid.Dif_Work = 0;
            pid.Prop_Work = 0;
            pid.e = 0;
            pid.En = 0;
            pid.S = 0;
            pid.ValuePID = 0;
            PidValue = 0;
            pidOrigin = 0;
            pid.StartPoint = numStartPoint.Value;
            //flagPid = false;
            CalcStartPoint(pid.StartPoint);
            PausePid = false;
        }

        //-----------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------
        [Serializable]
        public struct SaveData
        {
            public PID PID;
            public bool CoolingHeat;
        }
        //-----------------------------------------------------------------------------------------------------------------
        public override DataSave GetSaveData()
        {
            SaveData sd = new SaveData();
            sd.PID = pid;
            sd.CoolingHeat = CoolingHeat;
            return ToStroke<SaveData>(sd, GetType(), Name);
        }

        //-----------------------------------------------------------------------------------------------------------------
        public override bool SetSaveData(DataSave data)
        {
            if (data == null || data.typ != GetType() || data.NameDataId != Name) return false;
            SaveData sd;
            sd = DataSaveSCADA.SerializableStringToObject<SaveData>(data.data);
            PID t = (PID)Resources["_Pid"];

            t.AlwaysPositive = sd.PID.AlwaysPositive;
            t.LimitCalc = sd.PID.LimitCalc;

            t.PropK = sd.PID.PropK;
            t.DifK = sd.PID.DifK;
            t.IntegralK = sd.PID.IntegralK;
            t.StartPoint = sd.PID.StartPoint;
            StarPoint = sd.PID.StartPoint;

            t.MaxPID = sd.PID.MaxPID;
            t.MinPID = sd.PID.MinPID;

            t.TimeOutPid = sd.PID.TimeOutPid;
            t.TimeOutPidOff = sd.PID.TimeOutPidOff;
            t.TimePid = sd.PID.TimePid;

            t.on_DifK = sd.PID.on_DifK;
            t.on_IntegK = sd.PID.on_IntegK;
            t.on_PropK = sd.PID.on_PropK;

            CoolingHeat = sd.CoolingHeat;
            return true;
        }
    }



    
}
