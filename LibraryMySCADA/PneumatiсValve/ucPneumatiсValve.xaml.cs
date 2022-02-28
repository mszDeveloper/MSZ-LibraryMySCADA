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



namespace LibraryMySCADA.PneumatiсValve
{
    /// <summary>
    /// Логика взаимодействия для ucPneumatiсValve.xaml
    /// </summary>
    

    public partial class ucPneumatiсValve : UserControl
    {

        ////-----------------------------------------------------------------------------------------------------------------
        //[Category("Setting_ModBus")]
        //[Description("Категория к какой принадлежат переменные в классе ModBUS. При не установленном " +
        //    "данном значении будут ипользоваться первые по списку переменные игнорируя категории.")]
        //public string MB_Category { get; set; } = null;


        //[Category("Setting_ModBus")]
        //[Description("Имя переменной ModBus(in bool) - контроля открытия привода, ( null - не используеться.)")]
        //public string MB_OpenControl { get; set; } = null;

        //[Category("Setting_ModBus")]
        //[Description("Имя переменной ModBus(out) - управлением привода, ( null - не используеться.)")]
        //public string MB_Open { get; set; } = null;

        //--------------------------------------------------------------------------------------------------------
        [Category("Variable")]
        public bool isInWater
        {
            get { return (bool)GetValue(isInWaterProperty); }
            set { SetValue(isInWaterProperty, value); }
        }
        public static readonly DependencyProperty isInWaterProperty =
            DependencyProperty.Register("isInWater", typeof(bool), typeof(ucPneumatiсValve), new PropertyMetadata(false, changedInWalue));

        private static void changedInWalue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ucPneumatiсValve obj = d as ucPneumatiсValve;
            if ((bool)e.NewValue && obj.openValve) obj.isOutWater = true;
            else obj.isOutWater = false;

        }

        //--------------------------------------------------------------------------------------------------------
        [Category("Variable")]
        public bool isOutWater
        {
            get { return (bool)GetValue(isOutWaterProperty); }
            set { SetValue(isOutWaterProperty, value); }
        }
        public static readonly DependencyProperty isOutWaterProperty =
            DependencyProperty.Register("isOutWater", typeof(bool), typeof(ucPneumatiсValve), new PropertyMetadata(false));

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Открытие/Закрытие привода")]
        public bool openValve
        {
            get { return (bool)GetValue(openValve_Property); }
            set { SetValue(openValve_Property, value); }
        }
        public static readonly DependencyProperty openValve_Property =
                   DependencyProperty.Register("openValve", typeof(bool),
                   typeof(ucPneumatiсValve), new PropertyMetadata(false, changedOpenValve));

        private static void changedOpenValve(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ucPneumatiсValve obj = d as ucPneumatiсValve;
            obj.isOpen = (bool)e.NewValue;
            if ((bool)e.NewValue && obj.isInWater) obj.isOutWater = true;
            else obj.isOutWater = false;
            // (d as ucPneumatiсValve).OutModBusOpenedValue((bool)e.NewValue);

        }

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Состояние привода")]
        public bool isOpen
        {
            get { return (bool)GetValue(isOpen_Property); }
            set { SetValue(isOpen_Property, value); }
        }
        public static readonly DependencyProperty isOpen_Property =
                   DependencyProperty.Register("isOpen", typeof(bool),
                   typeof(ucPneumatiсValve), new PropertyMetadata(false, changedIsOpen));

        private static void changedIsOpen(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucPneumatiсValve).isOutLiquidPesence = (bool)e.NewValue;
        }

        //--------------------------------------------------------------------------------------------------------

        [Category("Setting")]
        [Description("Наличие жидкости на входе привода.")]
        public bool isInLiquidPesence
        {
            get { return (bool)GetValue(isLiquidPesence_Property); }
            set { SetValue(isLiquidPesence_Property, value); }
        }
        public static readonly DependencyProperty isLiquidPesence_Property =
                   DependencyProperty.Register("isInLiquidPesence", typeof(bool),
                   typeof(ucPneumatiсValve), new PropertyMetadata(false, changedIsInLiquidPesence));

        private static void changedIsInLiquidPesence(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucPneumatiсValve).isOutLiquidPesence = (bool)e.NewValue;
        }


        //--------------------------------------------------------------------------------------------------------

        [Category("Setting")]
        [Description("Наличие жидкости на выходе привода.")]
        public bool isOutLiquidPesence
        {
            get { return (bool)GetValue(isOutLiquidPesence_Property); }
            set { SetValue(isOutLiquidPesence_Property, value); }
        }
        public static readonly DependencyProperty isOutLiquidPesence_Property =
                   DependencyProperty.Register("isOutLiquidPesence", typeof(bool),
                   typeof(ucPneumatiсValve), new PropertyMetadata(false, chackIsOutLiquidPesence));

        private static void chackIsOutLiquidPesence(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d as ucPneumatiсValve).isOpen || !(d as ucPneumatiсValve).isInLiquidPesence) (d as ucPneumatiсValve).isOutLiquidPesence = false;
        }

        public void SetValve(bool set)
        {
            openValve = set;
        }
        //--------------------------------------------------------------------------------------------------------

        public ucPneumatiсValve()
        {
            InitializeComponent();
        }

        //--------------------------------------------------------------------------------------------------------
        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (MB_OpenControl != null) SetCallBackForBitValue(ChengeMB_OpenControl, MB_OpenControl, MB_Category);

            //}
            //catch { }
        }

        //--------------------------------------------------------------------------------------------------------
        //private void ChengeMB_OpenControl(Bit val)
        //{
        //    //Dispatcher.Invoke(new Action(() => {  try { isOpen = val.Val; } catch { }  }));

        //}
        ////--------------------------------------------------------------------------------------------------------

        //private void OutModBusOpenedValue(bool val)
        //{
        //    SetValueModBus(MB_Open, val, MB_Category);
        //}
        //--------------------------------------------------------------------------------------------------------


    }
}
