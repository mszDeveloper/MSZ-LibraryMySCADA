using System;
using System.Collections.Generic;
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

namespace LibraryMySCADA.Logics
{
    /// <summary>
    /// Логика взаимодействия для NOT.xaml
    /// </summary>
    public partial class NOT : UserControl
    {


        public bool isVisibleNot
        {
            get { return (bool)GetValue(isVisibleNotProperty); }
            set { SetValue(isVisibleNotProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isVisibleNot.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isVisibleNotProperty =
            DependencyProperty.Register("isVisibleNot", typeof(bool), typeof(NOT), new PropertyMetadata(false));



        //----------------------------------------------------------------------------------------------------
        public bool inPin
        {
            get { return (bool)GetValue(inPinProperty); }
            set { SetValue(inPinProperty, value); }
        }
        public static readonly DependencyProperty inPinProperty =
            DependencyProperty.Register("inPin", typeof(bool), typeof(NOT), new PropertyMetadata(false, changedIn));

        private static void changedIn(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NOT).outPin = !(bool)e.NewValue;
        }

        //----------------------------------------------------------------------------------------------------
        public bool outPin
        {
            get { return (bool)GetValue(outPinProperty); }
            set { SetValue(outPinProperty, value); }
        }
        public static readonly DependencyProperty outPinProperty =
            DependencyProperty.Register("outPin", typeof(bool), typeof(NOT), new PropertyMetadata(true));

        //----------------------------------------------------------------------------------------------------
        public NOT()
        {
            InitializeComponent();
        }
    }
}
