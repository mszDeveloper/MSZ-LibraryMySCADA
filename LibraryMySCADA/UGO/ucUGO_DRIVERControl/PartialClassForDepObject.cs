using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LibraryMySCADA
{
    public partial class ucUGO_DRIVERControl : UserControl
    {
        private bool _alarm, _worke, _onoff;
        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Описание")]
        public string ToolTipText
        {
            get { return (string)GetValue(ToolTipText_Property); }
            set { SetValue(ToolTipText_Property, value); }
        }
        public static readonly DependencyProperty ToolTipText_Property =
                   DependencyProperty.Register("ToolTipText", typeof(string),
                   typeof(ucUGO_DRIVERControl), new PropertyMetadata(null));

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Авария")]
        public bool Alarm
        {
            get { return (bool)GetValue(Alarm_Property); }
            set { SetValue(Alarm_Property, value); }
        }
        public static readonly DependencyProperty Alarm_Property =
                   DependencyProperty.Register("Alarm", typeof(bool),
                   typeof(ucUGO_DRIVERControl), new PropertyMetadata(false,null));

        private static object correctAlarmValue(DependencyObject d, object baseValue)
        {
            if ((d as ucUGO_DRIVERControl).OnOff) return  (d as ucUGO_DRIVERControl).Alarm;
                else return baseValue;
        }

 
        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Работа")]
        public bool Worker
        {
            get { return (bool)GetValue(Worker_Property); }
            set { SetValue(Worker_Property, value); _alarm = value; }
        }
        public static readonly DependencyProperty Worker_Property =
                   DependencyProperty.Register("Worker", typeof(bool),
                   typeof(ucUGO_DRIVERControl), new PropertyMetadata(false, null));// correctWorkerValue));

        //private static object correctWorkerValue(DependencyObject d, object baseValue)
        //{
        //    //if ((d as ucUGO_DRIVERControl).OnOff) return (d as ucUGO_DRIVERControl).Worker;
        //    //    else return baseValue;
        //}

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Включен - выключен")]
        public bool OnOff
        {
            get { return (bool)GetValue(OnOff_Property); }
            set { SetValue(OnOff_Property, value); _onoff = value; }
        }
        public static readonly DependencyProperty OnOff_Property =
                   DependencyProperty.Register("OnOff", typeof(bool),
                   typeof(ucUGO_DRIVERControl), new PropertyMetadata(false, changeOnOff, null));

        private static void changeOnOff(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ucUGO_DRIVERControl obj = d as ucUGO_DRIVERControl;
            if ((bool)e.NewValue)
            {
                obj.ClearValue(Alarm_Property);
                obj.ClearValue(Worker_Property);
               // obj.ClearValue(OnOff_Property);
                obj.Alarm = obj._alarm;
                //obj.OnOff = obj._onoff;
                obj.Worker = obj._worke;

            }
        }

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Рабочий режим - Auto/Manual")]
        public bool AutoManual
        {
            get { return (bool)GetValue(AutoManual_Property); }
            set { SetValue(AutoManual_Property, value); }
        }
        public static readonly DependencyProperty AutoManual_Property =
                   DependencyProperty.Register("AutoManual", typeof(bool),
                   typeof(ucUGO_DRIVERControl), new PropertyMetadata(false));

        //--------------------------------------------------------------------------------------------------------


    }
}
