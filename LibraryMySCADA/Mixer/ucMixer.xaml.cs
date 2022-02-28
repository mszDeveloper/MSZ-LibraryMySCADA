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

namespace LibraryMySCADA.Mixer
{
    public partial class ucMixer : UserControl
    {
        private Storyboard sb;

        [Description("Включение выключение миксера")]
        [Category("Setting")]
        public bool OnOffMixer
        {
            get { return (bool)GetValue(OnOffMixer_Property); }
            set { SetValue(OnOffMixer_Property, value); }
        }
        public static readonly DependencyProperty OnOffMixer_Property =
                   DependencyProperty.Register("OnOffMixer", typeof(bool),
                   typeof(ucMixer), new PropertyMetadata(false, changeOnOffMixer));

        private static void changeOnOffMixer(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sb = (d as ucMixer).FindResource("Moved");
            if ((bool)e.NewValue)
            {
               
                (sb as Storyboard).Begin();
            }
            else
            {
                (sb as Storyboard).Stop();
            }
        }



        public double Speed
        {
            get { return (double)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Speed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register("Speed", typeof(double), typeof(ucMixer), new PropertyMetadata(2d, ChangeSpeed));

        private static void ChangeSpeed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double speed = (double)e.NewValue;
            if (speed > 100) speed = 100;
            if (speed < 0) speed = 0;
            (d as ucMixer).sb.SetSpeedRatio(speed / 10);
        }


        //----------------------------------------------------------------------------------------------------------


        public ucMixer()
        {
            InitializeComponent();
            //Move.Storyboard.AccelerationRatio = 0.05d;
            sb = (Storyboard)FindResource("Moved");
            //Move.Storyboard.AccelerationRatio
        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Move.Storyboard.SetSpeedRatio(rootGrid, 20);
        }
    }
}
