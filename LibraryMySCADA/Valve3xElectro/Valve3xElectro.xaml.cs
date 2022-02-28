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

using static nsClassModBus.ClassModBus;

namespace LibraryMySCADA.Valve3xElectro
{
    public partial class Valve3xElectro : UserControl
    {

        [Category("Setting_ModBus")]
        [Description("Категория к какой принадлежат переменные в классе ModBUS. При не установленном " +
                    "данном значении будут ипользоваться первые по списку переменные игнорируя категории.")]
        public string Category { get; set; } = null;

        //--------------------------------------------------------------------------------------------------------
        [Category("Setting_ModBus")]
        [Description("Выход  - Открытия/Закрытия ")]
        public string valSetOpenClose { get; set; }
        //--------------------------------------------------------------------------------------------------------

        [Category("Setting")]
        public Visibility VisibleArrows
        {
            get { return ArrowLeft.Visibility; }
            set { ArrowLeft.Visibility = value; ArrowRight.Visibility = value; ArrowLeftEnd.Visibility = value; ArrowRightEnd.Visibility = value; }
        }
        //------------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public bool OpenClose
        {
            get { return (bool)GetValue(OpenCloseProperty); }
            set { SetValue(OpenCloseProperty, value); }
        }
        public static readonly DependencyProperty OpenCloseProperty =
            DependencyProperty.Register("OpenClose", typeof(bool), typeof(Valve3xElectro), 
                new PropertyMetadata(false, changedOpenClose));

        private static void changedOpenClose(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try { SetValueModBus((d as Valve3xElectro).valSetOpenClose, (bool)e.NewValue, (d as Valve3xElectro).Category); } catch { }
        }

        //------------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public Brush onBrush
        {
            get { return (Brush)GetValue(onBrushProperty); }
            set { SetValue(onBrushProperty, value); }
        }
        public static readonly DependencyProperty onBrushProperty =
            DependencyProperty.Register("onBrush", typeof(Brush), typeof(Valve3xElectro), new PropertyMetadata(Brushes.Blue));

        //------------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public Brush offBrush
        {
            get { return (Brush)GetValue(offBrushProperty); }
            set { SetValue(offBrushProperty, value); }
        }
        public static readonly DependencyProperty offBrushProperty =
            DependencyProperty.Register("offBrush", typeof(Brush), typeof(Valve3xElectro), new PropertyMetadata(Brushes.Silver));

        //------------------------------------------------------------------------------------------------------------------


        public Valve3xElectro()
        {
            InitializeComponent();
        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
