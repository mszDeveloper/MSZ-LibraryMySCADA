using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static nsClassModBus.ClassModBus;
using LibraryMySCADA;
using LibraryMySCADA.Virt;

namespace LibraryMySCADA.Driver
{
    public partial class ucDriver : ClassVirtualAdd
    {
        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Название двигателя")]
        public string ucName
        {
            get { return (string)GetValue(ucName_Property); }
            set { SetValue(ucName_Property, value); }
        }
        public static readonly DependencyProperty ucName_Property =
                   DependencyProperty.Register("ucName", typeof(string),
                   typeof(ucDriver), new PropertyMetadata("Driver"));
        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Включение выбора режима работы")]
        public bool enebleSelectMode
        {
            get { return (bool)GetValue(enebleSelectMode_Property); }
            set { SetValue(enebleSelectMode_Property, value); }
        }
        public static readonly DependencyProperty enebleSelectMode_Property =
                   DependencyProperty.Register("enebleSelectMode", typeof(bool),
                   typeof(ucDriver), new PropertyMetadata(false, changedEneble));

        private static void changedEneble(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucDriver).comboBox.IsEnabled = (bool)e.NewValue;
        }

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
                   typeof(ucDriver), new PropertyMetadata(null));

        //--------------------------------------------------------------------------------------------------------

        [Category("Setting")]
        [Description("Присутствие PID регулятора")]
        public bool IsPresentPID
        {
            get { return (bool)GetValue(IsPresentPID_Property); }
            set { SetValue(IsPresentPID_Property, value); }
        }
        public static readonly DependencyProperty IsPresentPID_Property =
                   DependencyProperty.Register("IsPresentPID", typeof(bool),
                   typeof(ucDriver), new PropertyMetadata(false, changedIsPresentPID));

        private static void changedIsPresentPID(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue) { (d as ucDriver).isPCHT = true; (d as ucDriver).arrowFRQ.useSettingValue = false; }

        }

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Работа двигателя")]
        public bool IsWorker
        {
            get { return (bool)GetValue(IsWorkerProperty); }
            set { SetValue(IsWorkerProperty, value); }
        }
        public static readonly DependencyProperty IsWorkerProperty =
                   DependencyProperty.Register("IsWorker", typeof(bool),
                   typeof(ucDriver), new PropertyMetadata(false, changedIsWorker));

        private static void changedIsWorker(DependencyObject d, DependencyPropertyChangedEventArgs e)
        { (d as ucDriver).ChangedIsWorker(d, e); }

        private void ChangedIsWorker(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ucUGO_DRIVERControl.Worker = (bool)e.NewValue;

            if ((bool)e.NewValue)
            {
                if (timeOnControl != new TimeSpan(0, 0, 0))
                {
                    timer.Start();
                }
                if (IsPresentPID)
                {
                    ucPid.ResetPid();
                    ucPid.StartPID(timeOutStartedForPID);
                }
            }
            else
            {
                if (timer.IsEnabled) timer.Stop();
                if (IsPresentPID)
                {
                    ucPid.ResetPid();
                    ucPid.StopPID(timeOutStartedForPID);
                }
            }

            try { SetValueModBus(valSetOnOff, (bool)e.NewValue, valCategoryMB); } catch { }
            isOutLiquidPesence = (bool)e.NewValue;
        }

        //--------------------------------------------------------------------------------------------------------
        private void ChengeOnDriver(object sender, EventArgs e)
        {
            Error = true;
            timer.Stop();
        }


        //--------------------------------------------------------------------------------------------------------
        [Category("Setting_ModBus")]
        [Description("Категория к какой принадлежат переменные в классе ModBUS. При не установленном " +
            "данном значении будут ипользоваться первые по списку переменные игнорируя категории.")]
        public string valCategoryMB { get; set; } = null;

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting_ModBus")]
        [Description("Выход  - Включения/Выключения ")]
        public string valSetOnOff { get; set; } = null;

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting_ModBus")]
        [Description("вход значения тока двигателя")]
        public string valAmperage { get; set; } = null;

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting_ModBus")]
        [Description("вход обратной связи для ПИД регулятора")]
        public string valOsPid { get; set; } = null;

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting_ModBus")]
        [Description("вход внешней ошибки, true-ошибка")]
        public string valExternalError { get; set; } = null;

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting_ModBus")]
        [Description("вход контроля включения, true-ОК")]
        public string valControlON { get; set; } = null;

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting_ModBus")]
        [Description("выход установки частоты,от 0 до 1")]
        public string valOutFRQ { get; set; } = null;
        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Отключить включить панель управления")]
        public bool OnOffPanel
        {
            get { return GoToStateActionPanelVisible.IsEnabled; }
            set { GoToStateActionPanelVisible.IsEnabled = value; }
        }

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("задать режим работы 0 - auto 1 - manual")]
        public int setMode
        {
            get { return comboBox.SelectedIndex; }
            set { comboBox.SelectedIndex = value; }
        }

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Наличие частотного преобразователя true-есть, false-нет")]
        public bool isPCHT 
        {
            get { return (bool)GetValue(isPCHT_Property); }
            set { SetValue(isPCHT_Property, value); }
        }
        public static readonly DependencyProperty isPCHT_Property =
                   DependencyProperty.Register("isPCHT", typeof(bool),
                   typeof(ucDriver), new PropertyMetadata(false, changIsPCHT));

        private static void changIsPCHT(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucDriver).arrowFRQ.useSettingValue = (bool)e.NewValue;
        }

        //--------------------------------------------------------------------------------------------------------




    }
}
