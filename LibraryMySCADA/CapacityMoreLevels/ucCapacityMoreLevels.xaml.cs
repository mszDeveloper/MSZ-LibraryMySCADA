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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryMySCADA.CapacityMoreLevels
{
    //[Serializable]
    //public struct DIR_MODE
    //{
    //    public double p_skv1;
    //    public double p_skv2;
    //    public double p_skv3;
    //    public bool ON_skv1;
    //    public bool ON_skv2;
    //    public bool ON_skv3;
    //    public bool pp_60;
    //    public bool pp_40;
    //    public string name;
    //}
    //-----------------------------------------------
    public enum MODE { A, B, C, D,NULL }

    public partial class ucCapacityMoreLevels : ClassVirtualAdd
    {

        MODE _MODE;
        //-----------------------------------------------

        bool A, B, C, D;

        //-------------------------------------------------------------------------------------------------------------

        //[Category("Setting")]
        //public bool visualOnLevelA
        //{
        //    get { return pathA.Visibility == Visibility.Visible; }
        //    set
        //    {
        //        Visibility vs = value ? Visibility.Visible : Visibility.Collapsed;
        //        pathA.Visibility = vs;
        //        Viewbox_A.Visibility = vs;
        //        sliderA.Visibility = vs;
        //        if(vs == Visibility.Collapsed )sliderA = null;
        //    }
        //}
        //[Category("Setting")]
        //public bool visualOnLevelB
        //{
        //    get { return pathB.Visibility == Visibility.Visible; }
        //    set
        //    {
        //        Visibility vs = value ? Visibility.Visible : Visibility.Collapsed;
        //        pathB.Visibility = vs;
        //        Viewbox_B.Visibility = vs;
        //        sliderB.Visibility = vs;
        //        if (vs == Visibility.Collapsed) sliderB = null;
        //    }
        //}
        //[Category("Setting")]
        //public bool visualOnLevelC
        //{
        //    get { return pathC.Visibility == Visibility.Visible; }
        //    set
        //    {
        //        Visibility vs = value ? Visibility.Visible : Visibility.Collapsed;
        //        pathC.Visibility = vs;
        //        Viewbox_C.Visibility = vs;
        //        sliderC.Visibility = vs;
        //        if (vs == Visibility.Collapsed) sliderC = null;
        //    }
        //}
        //[Category("Setting")]
        //public bool visualOnLevelD
        //{
        //    get { return pathD.Visibility == Visibility.Visible; }
        //    set
        //    {
        //        Visibility vs = value ? Visibility.Visible : Visibility.Collapsed;
        //        pathD.Visibility = vs;
        //        Viewbox_D.Visibility = vs;
        //        sliderD.Visibility = vs;
        //        if (vs == Visibility.Collapsed) sliderD = null;
        //    }
        //}

        [Description("Флаг изменения данных" )]
        [Category("Setting")]
        public bool wrFlag
        {
            get { return (bool)GetValue(wrFlag_Property); }
            set { SetValue(wrFlag_Property, value); }
        }
        public static readonly DependencyProperty wrFlag_Property =
                   DependencyProperty.Register("wrFlag", typeof(bool),
                   typeof(ucCapacityMoreLevels), new PropertyMetadata(false));
        //-------------------------------------------------------------------------------------------------------------
        [Description("Уровень А")]
        [Category("Setting")]
        public double LevelA
        {
            get { return (double)GetValue(LevelA_Property); }
            set { SetValue(LevelA_Property, value); }
        }
        public static readonly DependencyProperty LevelA_Property =
                   DependencyProperty.Register("LevelA", typeof(double),
                   typeof(ucCapacityMoreLevels), new PropertyMetadata((double)10));
        //-------------------------------------------------------------------------------------------------------------
        [Description("Уровень B")]
        [Category("Setting")]
        public double LevelB
        {
            get { return (double)GetValue(LevelB_Property); }
            set { SetValue(LevelB_Property, value); }
        }
        public static readonly DependencyProperty LevelB_Property =
                   DependencyProperty.Register("LevelB", typeof(double),
                   typeof(ucCapacityMoreLevels),new PropertyMetadata((double)30));
        //-------------------------------------------------------------------------------------------------------------
        [Description("Уровень C")]
        [Category("Setting")]
        public double LevelC
        {
            get { return (double)GetValue(LevelC_Property); }
            set { SetValue(LevelC_Property, value); }
        }
        public static readonly DependencyProperty LevelC_Property =
                   DependencyProperty.Register("LevelC", typeof(double),
                   typeof(ucCapacityMoreLevels), new PropertyMetadata((double)50));
        //-------------------------------------------------------------------------------------------------------------
        [Description("Уровень D")]
        [Category("Setting")]
        public double LevelD
        {
            get { return (double)GetValue(LevelD_Property); }
            set { SetValue(LevelD_Property, value); }
        }
        public static readonly DependencyProperty LevelD_Property =
                   DependencyProperty.Register("LevelD", typeof(double),
                   typeof(ucCapacityMoreLevels), new PropertyMetadata((double)70));
        //-------------------------------------------------------------------------------------------------------------


        [Description("Максимальный уровень в метрах")]
        [Category("Setting")]
        public double LevelMax { get { return _LevelMax; } set { _LevelMax = value; } }
        double _LevelMax;
        //-------------------------------------------------------------------------------------------------------------
        [Description("Градиентная кисть заполнения емкости")]
        [Category("Setting")]
        public Brush CapacityFillBrush
        {
            get { return (Brush)GetValue(CapacityFillBrush_Property); }
            set { SetValue(CapacityFillBrush_Property, value); }
        }
        public static readonly DependencyProperty CapacityFillBrush_Property =
                   DependencyProperty.Register("CapacityFillBrush", typeof(Brush),
                   typeof(ucCapacityMoreLevels));
        //-------------------------------------------------------------------------------------------------------------
        [Description("Уровень (в процентах 0 - 100)")]
        [Category("Setting")]
        public double level
        {
            get { return (double)GetValue(level_Property); }
            set { SetValue(level_Property, value); }
        }
        public static readonly DependencyProperty level_Property =
                   DependencyProperty.Register("level", typeof(double),
                   typeof(ucCapacityMoreLevels), new PropertyMetadata((double)50, changedLevel));

        private static void changedLevel(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucCapacityMoreLevels).FillRect();
           (d as ucCapacityMoreLevels).mode = (d as ucCapacityMoreLevels).GetMode();
        }

        //-------------------------------------------------------------------------------------------------------------
        [Description("Режим  - А,B,C или D")]
        [Category("Setting")]
        public MODE mode
        {
            get { return (MODE)GetValue(mode_Property); }
            set { SetValue(mode_Property, value); }
        }
        public static readonly DependencyProperty mode_Property =
                   DependencyProperty.Register("mode", typeof(MODE),
                   typeof(ucCapacityMoreLevels), new PropertyMetadata(MODE.A,null,changedMode));

        private static object changedMode(DependencyObject d, object baseValue)
        {
            (d as ucCapacityMoreLevels).CreateModeEvent((MODE)baseValue);
            return baseValue;
        }

        //private static void changedMode(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as ucCapacityMoreLevels).CreateModeEvent((MODE)e.NewValue);
        //}


        //-------------------------------------------------------------------------------------------------------------
        public ucCapacityMoreLevels()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------
        private void sliderA_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            
            var d = rectWater.ActualHeight - (rectWater.ActualHeight / 100 * sliderA.Value);
            pathA.Margin = new Thickness(5, d, 5, 0);
            if (label_A != null)
            {
                label_A.Content = Math.Round((LevelMax / 100) * sliderA.Value, 2).ToString() + "м";
                Viewbox_A.Margin = new Thickness(Viewbox_A.Margin.Left, d - label_A.ActualHeight, Viewbox_A.Margin.Right, Viewbox_A.Margin.Bottom);
            }
            if (wrFlag) return;
            if (sliderB == null) return;
            if (e.NewValue >= sliderB.Value) sliderA.Value = sliderB.Value - 1;

        }

        //-------------------------------------------------------------------------------------------------------------
        private void sliderB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            
            var d = rectWater.ActualHeight - (rectWater.ActualHeight / 100 * sliderB.Value);
            pathB.Margin = new Thickness(5, d, 5, 0);
            if (label_B != null)
            {
                label_B.Content = Math.Round((LevelMax / 100) * sliderB.Value, 2).ToString() + "м";
                Viewbox_B.Margin = new Thickness(Viewbox_B.Margin.Left, d - label_B.ActualHeight, Viewbox_B.Margin.Right, Viewbox_B.Margin.Bottom);
            }
            if (wrFlag) return;
            if (sliderC == null || sliderA == null) return;
            if (e.NewValue >= sliderC.Value) sliderB.Value = sliderC.Value - 1;
            else if (e.NewValue <= sliderA.Value) sliderB.Value = sliderA.Value + 1;

        }

        //-------------------------------------------------------------------------------------------------------------
        private void sliderC_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
           

            var d = rectWater.ActualHeight - (rectWater.ActualHeight / 100 * sliderC.Value);
            pathC.Margin = new Thickness(5, d, 5, 0);
            if (label_C != null)
            {
                label_C.Content = Math.Round((LevelMax / 100) * sliderC.Value, 2).ToString() + "м";
                Viewbox_C.Margin = new Thickness(Viewbox_C.Margin.Left, d - label_C.ActualHeight, Viewbox_C.Margin.Right, Viewbox_C.Margin.Bottom);
            }
            if (wrFlag) return;
            if (sliderB == null || sliderD == null) return;
            if (e.NewValue >= sliderD.Value) sliderC.Value = sliderD.Value - 1;
            else if (e.NewValue <= sliderB.Value) sliderC.Value = sliderB.Value + 1;
        }

        //-------------------------------------------------------------------------------------------------------------
        private void sliderD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
           
            var d = rectWater.ActualHeight - (rectWater.ActualHeight / 100 * sliderD.Value);
            pathD.Margin = new Thickness(5, d, 5, 0);
            if (label_D != null)
            {
                label_D.Content = Math.Round((LevelMax / 100) * sliderD.Value, 2).ToString() + "м";
                Viewbox_D.Margin = new Thickness(Viewbox_D.Margin.Left, d - label_D.ActualHeight, Viewbox_D.Margin.Right, Viewbox_D.Margin.Bottom);
            }
            if (wrFlag) return;
            if (sliderC == null) return;
            if (e.NewValue <= sliderC.Value) sliderD.Value = sliderC.Value + 1;

        }

        //-------------------------------------------------------------------------------------------------------------
        private void CapacityMoreLevels_Loaded(object sender, RoutedEventArgs e) { RecalcLine(); }
        //-------------------------------------------------------------------------------------------------------------
        private void RecalcLine()
        {
            if(sliderA != null)//.Visibility != Visibility.Collapsed)
            {
                var d = rectWater.ActualHeight - (rectWater.ActualHeight / 100 * sliderA.Value);
                pathA.Margin = new Thickness(5, d, 5, 0);
                Viewbox_A.Margin = new Thickness(Viewbox_A.Margin.Left, d - label_A.ActualHeight, Viewbox_A.Margin.Right, Viewbox_A.Margin.Bottom);
                label_A.Content = Math.Round((LevelMax / 100) * sliderA.Value, 2).ToString() + "м";
            }


            if (sliderB != null)//.Visibility != Visibility.Collapsed)
            {
                var d = rectWater.ActualHeight - (rectWater.ActualHeight / 100 * sliderB.Value);
                pathB.Margin = new Thickness(5, d, 5, 0);
                Viewbox_B.Margin = new Thickness(Viewbox_B.Margin.Left, d - label_B.ActualHeight, Viewbox_B.Margin.Right, Viewbox_B.Margin.Bottom);
                label_B.Content = Math.Round((LevelMax / 100) * sliderB.Value, 2).ToString() + "м";
            }


            if (sliderC != null)//.Visibility != Visibility.Collapsed)
            {
                var d = rectWater.ActualHeight - (rectWater.ActualHeight / 100 * sliderC.Value);
                pathC.Margin = new Thickness(5, d, 5, 0);
                Viewbox_C.Margin = new Thickness(Viewbox_C.Margin.Left, d - label_C.ActualHeight, Viewbox_C.Margin.Right, Viewbox_C.Margin.Bottom);
                label_C.Content = Math.Round((LevelMax / 100) * sliderC.Value, 2).ToString() + "м";
            }

            if (sliderD != null)//.Visibility != Visibility.Collapsed)
            {
                var d = rectWater.ActualHeight - (rectWater.ActualHeight / 100 * sliderD.Value);
                pathD.Margin = new Thickness(5, d, 5, 0);
                Viewbox_D.Margin = new Thickness(Viewbox_D.Margin.Left, d - label_D.ActualHeight, Viewbox_D.Margin.Right, Viewbox_D.Margin.Bottom);
                label_D.Content = Math.Round((LevelMax / 100) * sliderD.Value, 2).ToString() + "м";
            }

            label_maxValue.Content = LevelMax.ToString() + "m";
        }

        private void CapacityMoreLevels_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RecalcLine();
        }

        //-------------------------------------------------------------------------------------------------------------
        public void ResetDirectMode()
        {
            A = B = C = D = false;
            mode = MODE.NULL;
        }

        //-------------------------------------------------------------------------------------------------------------

        internal MODE GetMode()
        {
            _MODE = MODE.NULL;
            if (level >= sliderA.Value) _MODE = MODE.A;
            if (level >= sliderB.Value) _MODE = MODE.B;
            if (level >= sliderC.Value) _MODE = MODE.C;
            if (level >= sliderD.Value) _MODE = MODE.D;

            if (_MODE == MODE.NULL) { A = B = C = D = false; return MODE.A; }

            if (_MODE == MODE.A & !A) { A = true; return MODE.A; }
            else if (_MODE == MODE.B & !B) { B = true; return MODE.B; }
            else if (_MODE == MODE.C & !C) { C = true; return MODE.C; }
            else if (_MODE == MODE.D) { D = true; return MODE.D; }

            else if (_MODE == MODE.B & D)   { D = false; return MODE.C; }
            else if (_MODE == MODE.A & C)   { C = false; return MODE.B; }
            else if (_MODE == MODE.NULL & B) { B = false; return MODE.A; }
            
            return mode;
        }

        //-------------------------------------------------------------------------------------------------------------
        public void CreateModeEvent(MODE mode)
        {
            RoutedEventArgs b = new RoutedEventArgs(ChangeMode_Event, mode);
            RaiseEvent(b);
        }

        public static readonly RoutedEvent ChangeMode_Event = EventManager.RegisterRoutedEvent("ChangeMode", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucCapacityMoreLevels));

        public event RoutedEventHandler ChangeMode
        {
            add { AddHandler(ChangeMode_Event, value); }
            remove { RemoveHandler(ChangeMode_Event, value); }
        }

        //----------------------------------------------------------------------------------------------------------
        private void FillRect()
        {
            double y = (level / 50 - 1) * -1;
            Resources["Point1"] = new Point(0, y);
        }

        public void SetsAdminPermission(bool admin)
        {
            sliderA.IsEnabled = admin;
            sliderB.IsEnabled = admin;
            sliderC.IsEnabled = admin;
            sliderD.IsEnabled = admin;
        }
        //----------------------------------------------------------------------------------------------------------
        public void ResetMode() { A = B = C = D = false; mode = MODE.NULL; }



        //----------------------------------------------------------------------------------------------------------
        [Serializable]
        public struct SaveData
        {
            public double LevelA;
            public double LevelB;
            public double LevelC;
            public double LevelD;
        }

        public override DataSave GetSaveData()
        {
            SaveData sd = new SaveData();
            sd.LevelA = LevelA;
            sd.LevelB = LevelB;
            sd.LevelC = LevelC;
            sd.LevelD = LevelD;


            DataSave ds = new DataSave();
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

            LevelA= sd.LevelA;
            LevelB  = sd.LevelB;
            LevelC  = sd.LevelC;
            LevelD  = sd.LevelD;

            return true;

        }
    }
}
