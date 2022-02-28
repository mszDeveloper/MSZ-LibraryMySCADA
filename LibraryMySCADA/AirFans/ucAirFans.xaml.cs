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

namespace LibraryMySCADA.AirFans
{
    /// <summary>
    /// Логика взаимодействия для ucAirFans.xaml
    /// </summary>
    public partial class ucAirFans : UserControl
    {



        public bool? OnOff
        {
            get { return (bool?)GetValue(OnOffProperty); }
            set { SetValue(OnOffProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnOff.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnOffProperty =
            DependencyProperty.Register("OnOff", typeof(bool?), typeof(ucAirFans), new PropertyMetadata(null));



        public int FunsCount { get; set; } = 4;

        public ucAirFans()
        {
            InitializeComponent();
 

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < FunsCount; i++)
            {
                StackForFans.Children.Add(new oneFanControl());
            }


        }
    }
}
