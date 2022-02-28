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

namespace LibraryMySCADA.Valve_1
{
     public partial class Valve1 : UserControl
    {
        [Category("Setting")]
        public bool OpenedClosed
        {
            get { return (bool)GetValue(OpenedClosedProperty); }
            set { SetValue(OpenedClosedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpenedClosed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenedClosedProperty =
            DependencyProperty.Register("OpenedClosed", typeof(bool), typeof(Valve1), new PropertyMetadata(false));

        public Valve1()
        {
            InitializeComponent();
        }
    }
}
