using LibraryMySCADA.Virt;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace LibraryMySCADA.Blower
{
    public partial class Blower : ClassVirtualAdd
    {
        Storyboard sb;

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public bool Auto
        {
            get { return (bool)GetValue(AutoProperty); }
            set { SetValue(AutoProperty, value); }
        }
        public static readonly DependencyProperty AutoProperty =
            DependencyProperty.Register("Auto", typeof(bool), typeof(Blower), new PropertyMetadata(false));

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public int MirorBlower
        {
            get { return (int)GetValue(MirorBlowerProperty); }
            set { SetValue(MirorBlowerProperty, value); }
        }
        public static readonly DependencyProperty MirorBlowerProperty =
            DependencyProperty.Register("MirorBlower", typeof(int), typeof(Blower), new PropertyMetadata(1));


        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public bool OnOff
        {
            get { return (bool)GetValue(OnOffProperty); }
            set { SetValue(OnOffProperty, value); }
        }
        public static readonly DependencyProperty OnOffProperty =
            DependencyProperty.Register("OnOff", typeof(bool), typeof(Blower), new PropertyMetadata(false, ChangeOnOff));

        private static void ChangeOnOff(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Blower).ChangeOnOff((bool)e.NewValue);
            
        }

        private void ChangeOnOff(bool newValue)
        {
            if (newValue)
            {
                
                sb.Begin();
                sb.SetSpeedRatio(Speed);
            }
            else
            {
                sb.Stop();
            }
        }

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public double Speed
        {
            get { return (double)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        public bool isUseSaved { get; set; }

        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register("Speed", typeof(double), typeof(Blower), new PropertyMetadata(0d, ChangeSpeed));

        private static void ChangeSpeed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Blower).ChangeSpeed((double)e.NewValue);
        }

        private void ChangeSpeed(double newValue)
        {
            try{ sb.SetSpeedRatio(newValue); } catch { };
        }

        //--------------------------------------------------------------------------------------------------------
        public Blower()
        {
            InitializeComponent();
            sb = (Storyboard)FindResource("AnimBlower");
        }

        private void Viewbox_Loaded(object sender, RoutedEventArgs e)
        {
           // Driver.SettingPanel.
        }

        //---------------------------------------------------------------------------------------------------------------
        public struct SaveData
        {
            public double speedBlower;
            public bool OnOff;
        }
        //-------------------------------------------
        public override DataSave GetSaveData()
        {
            if (!isUseSaved) return null;
            SaveData sd = new SaveData();
            //далее инициализация свойств
            sd.speedBlower = Speed;
            sd.OnOff = OnOff;
            return ToStroke<SaveData>(sd, GetType(), Name);
        }

        //----------------------------------------------
        public override bool SetSaveData(DataSave data)
        {
            if (!isUseSaved) return false;
            if (data == null || data.typ != GetType() || data.NameDataId != Name) return false;
            SaveData sd;
            sd = DataSaveSCADA.SerializableStringToObject<SaveData>(data.data);
            Speed = sd.speedBlower;
            //OnOff = sd.OnOff;
            return true;
        }

        //---------------------------------------------------------------------------------------------------------------

    }
}
