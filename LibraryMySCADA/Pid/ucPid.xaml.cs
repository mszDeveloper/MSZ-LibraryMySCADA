using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System;
using System.Windows.Controls;
using wpfLibMszControl.Pid;
using System.Windows.Documents;

namespace wpfMszControl.Pid
{

    public partial class ucPid : LibraryMySCADA.Virt.ClassVirtualAdd
    {

        //    public class PID
        //    {
        //        public double StartPoint;
        //        public double ValuePID;
        //        public double DifK;
        //        public double IntegralK;
        //        public double PropK;
        //        public double Dif_Work;
        //        public double Integral_Work;
        //        public double Prop_Work;
        //        public double S;//составляющая интегрального регулятора
        //        public double En; //составляющая дифференциального звена
        //        public double e; //e - рассоглосования (T_UST - OS_PID)
        //        public double TimeOutPid; //Задержка вклчения в секундах
        //        public double TimeOutPidOff; //Задержка выключения в секундах    
        //        public double TimePid;
        //        public double MaxPID;
        //        public double MinPID;
        //        public bool LimitCalc; //ограничивать пид (True - да)
        //        public bool AlwaysPositive; //всегда положительный пид (True - да)
        //        public double T_UST;//уставка
        //        public double OS_PID;
        //        public bool on_DifK;
        //        public bool on_IntegK;
        //        public bool CoolingHeat;
        //        public bool on_PropK;
        //    }



        [Category("Setting")]
        public Grid obj
        {
            get { return (Grid)GetValue(objProperty); }
            set { SetValue(objProperty, value); }
        }
        public static readonly DependencyProperty objProperty =
            DependencyProperty.Register("obj", typeof(Grid), typeof(ucPid), new PropertyMetadata(null));


        [Category("Setting")]
        public bool StartStopPID
        {
            get { return (bool)GetValue(StartStopPIDProperty); }
            set { SetValue(StartStopPIDProperty, value); }
        }
        public static readonly DependencyProperty StartStopPIDProperty =
            DependencyProperty.Register("StartStopPID", typeof(bool), typeof(ucPid), new PropertyMetadata(false, changStartStopPID));

        private static void changStartStopPID(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ucPid obg = d as ucPid;
            if ((bool)e.NewValue) obg.StartPID(false);
            else obg.StopPID(false);
        }


        //-----------------------------------------------------------------------------------------------------------------
        [Category("Внешний вид")]
        public bool VisiblePID
        {
            get { return (bool)GetValue(VisiblePIDProperty); }
            set { SetValue(VisiblePIDProperty, value); }
        }
        public static readonly DependencyProperty VisiblePIDProperty =
            DependencyProperty.Register("VisiblePID", typeof(bool), typeof(ucPid), new PropertyMetadata(false));
        //-----------------------------------------------------------------------------------------------------------------


        [Description("Показать регулятор")]
        [Category("Внешний вид")]
        public bool VisibleOnOff { get { return _VisibleOnOff; } set { _VisibleOnOff = value; Visibility = value ? Visibility.Visible : Visibility.Collapsed; } } 
        bool _VisibleOnOff = true;

        //-----------------------------------------------------------------------------------------------------------------
        private void button_Click(object sender, RoutedEventArgs e)
        {
            //SaveDataPID();
            RoutedEventArgs b = new RoutedEventArgs(HideEvent_Event, true);
            base.RaiseEvent(b);
            VisiblePID = false;
        }

        public static readonly RoutedEvent HideEvent_Event = EventManager.RegisterRoutedEvent("Hide_Event", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(ucPid));

        public event RoutedEventHandler Hide_Event
        {
            add { AddHandler(HideEvent_Event, value); }
            remove { RemoveHandler(HideEvent_Event, value); }
        }
        //-----------------------------------------------------------------------------------------------------------------

        [Description("Выходное значение ПИД")]
        [Category("Setting")]
        public double PidValue
        {
            get { return (double)GetValue(PidValue_Property); }
            set { SetValue(PidValue_Property, value); }
        }
        public static readonly DependencyProperty PidValue_Property = 
                   DependencyProperty.Register("PidValue", typeof(double),
                   typeof(ucPid), new PropertyMetadata((double)1, null, pidValueChanged));

        private static object pidValueChanged(DependencyObject d, object baseValue)
        {
            RoutedEventArgs b = new RoutedEventArgs(PidValue_Event, (double)baseValue);
            (d as ucPid).RaiseEvent(b);
            return baseValue;
        }

        private static void pidValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RoutedEventArgs b = new RoutedEventArgs(PidValue_Event, (double)e.NewValue);
            (d as ucPid).RaiseEvent(b);
        }

        public static readonly RoutedEvent PidValue_Event = EventManager.RegisterRoutedEvent("PidValueChanged", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(ucPid));

        public event RoutedEventHandler PidValue_Changed
        {
            add { AddHandler(PidValue_Event, value); }
            remove { RemoveHandler(PidValue_Event, value); }
        }

        //-----------------------------------------------------------------------------------------------------------------
        [Description("(вход) Значение обратной связи ПИД")]
        [Category("Setting")]
        public double PidOS
        {
            get { return (double)GetValue(PidOSProperty); }
            set { SetValue(PidOSProperty, value); }
        }
        public static readonly DependencyProperty PidOSProperty =
                   DependencyProperty.Register("PidOS", typeof(double),
                   typeof(ucPid), new PropertyMetadata(0d, ChangedPidOS));

        private static void ChangedPidOS(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ucPid pid = d as ucPid;
            if (pid.modeCalc == PidModeCalc.toChangedOS) pid.timePidTick(null, null);
        }

        //-----------------------------------------------------------------------------------------------------------------
        [Description("контроль работы ПИД (true - работа)")]
        [Category("Setting")]
        public bool PidWork
        {
            get { return (bool)GetValue(PidWork_Property); }
            set { SetValue(PidWork_Property, value); }
        }
        public static readonly DependencyProperty PidWork_Property =
                   DependencyProperty.Register("PidWork", typeof(bool),
                   typeof(ucPid), new PropertyMetadata(false));

        //-----------------------------------------------------------------------------------------------------------------
        [Description("Таймоут остановки ПИД (время такоеже как и запуска )")]
        [Category("Setting")]
        public bool PidStopTimeOut
        {
            get { return (bool)GetValue(PidStopTimeOut_Property); }
            set { SetValue(PidStopTimeOut_Property, value); }
        }
        public static readonly DependencyProperty PidStopTimeOut_Property =
                   DependencyProperty.Register("PidStopTimeOut", typeof(bool),
                   typeof(ucPid), new PropertyMetadata(false));

        //-----------------------------------------------------------------------------------------------------------------
        [Description("Задействован пид или не (true - задействован)")]
        [Category("Setting")]
        public PID_PRESENCE PidState
        {
            get { return (PID_PRESENCE)GetValue(PidState_Property); }
            set { SetValue(PidState_Property, value); }
        }
        public static readonly DependencyProperty PidState_Property =
                   DependencyProperty.Register("PidState", typeof(PID_PRESENCE),
                   typeof(ucPid), new PropertyMetadata(PID_PRESENCE.Yes));

        //-----------------------------------------------------------------------------------------------------------------
        [Description("Уставка значения")]
        [Category("Setting")]
        public double PidSetpoint
        {
            get { return (double)GetValue(PidSetpoint_Property); }
            set { SetValue(PidSetpoint_Property, value); }
        }
        public static readonly DependencyProperty PidSetpoint_Property =
                   DependencyProperty.Register("PidSetpoint", typeof(double),
                   typeof(ucPid), new PropertyMetadata((double)0));
        

        //-----------------------------------------------------------------------------------------------------------------
        private void classPID_Loaded(object sender, RoutedEventArgs e)
        {
            textBox_Name.Text = Name;
            if (obj != null)
            {
                if (DesignerProperties.GetIsInDesignMode(this)) return;
                var asd = TranslatePoint(new Point(0, 0), obj);
                Grid par = Parent as Grid;
                if (par == null) return;
                par.Children.Remove(this);
                obj.Children.Add(this);
                //var asd = UcStathion.TranslatePoint(new Point(0, 0), MainWindow.win);
                Margin = new Thickness(asd.X, asd.Y, 0, 0);
            }

            //LoadDataPID();//setpoint
            pid = (PID)FindResource("_Pid");
        }

        //private void numStartPoint_Value_Changed(object sender, RoutedEventArgs e)
        //{
        //    StarPoint = (double)e.OriginalSource;
        //}

        //-----------------------------------------------------------------------------------------------------------------
        [Description("Включение или отключение рассчёта ПИД, сам регулятор остаеться в работе!")]
        [Category("Setting")]
        public bool isEneble
        {
            get { return (bool)GetValue(isEnebleProperty); }
            set { SetValue(isEnebleProperty, value); }
        }

        public static readonly DependencyProperty isEnebleProperty =
            DependencyProperty.Register("isEneble", typeof(bool), typeof(ucPid), new PropertyMetadata(true));

        //-----------------------------------------------------------------------------------------------------------------
        private void buttonHotIce_Click(object sender, RoutedEventArgs e)
        {
            CoolingHeat = !CoolingHeat;
        }

        //-----------------------------------------------------------------------------------------------------------------
        private void ButtonClearText_Click(object sender, RoutedEventArgs e)
        {
            flowDocument.Document.Blocks.Clear();
            flowDocument.Document.Blocks.Add(new Paragraph(new Run("PID...")));
        }


        //-----------------------------------------------------------------------------------------------------------------

    }
}
