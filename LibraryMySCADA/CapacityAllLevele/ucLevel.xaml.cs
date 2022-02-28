using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryMySCADA.CapacityAllLevele
{
    [DesignTimeVisible(false)]
    public partial class ucLevel : UserControl
    {
        public string NameLevel { get; set; } = "DefaultName";
        internal int index;
        internal bool modeFull;//флаг перехода уровня в режим наполнения( уровень достиг наполнения)

        public bool LevelIsFill
        {
            get { return (bool)GetValue(LevelIsFillProperty); }
            set { SetValue(LevelIsFillProperty, value); }
        }
        public static readonly DependencyProperty LevelIsFillProperty =
            DependencyProperty.Register("LevelIsFill", typeof(bool), typeof(ucLevel), new PropertyMetadata(false, changeState));

        private static void changeState(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RoutedEventArgs b = new RoutedEventArgs(LevelIsState_Event, (d as ucLevel).index);
            (d as ucLevel).RaiseEvent(b);

        }

        //------------------------------------------------------------------------------------------------------------------------
        public void CheckValueLevel(double val)
        {
            if (val < levelValue) LevelIsFill = false;
            else LevelIsFill = true;
        }
        //------------------------------------------------------------------------------------------------------------------------
        public static readonly RoutedEvent LevelIsState_Event = EventManager.RegisterRoutedEvent("LevelIsStateChange", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(ucLevel));

        public event RoutedEventHandler LevelIsStateChange
        {
            add { AddHandler(LevelIsState_Event, value); }
            remove { RemoveHandler(LevelIsState_Event, value); }
        }

        //------------------------------------------------------------------------------------------------------------------------
        public ucLevel()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------------------------------------------------
        public Brush arrowColor
        {
            get { return (Brush)GetValue(arrowColorProperty); }
            set { SetValue(arrowColorProperty, value); }
        }
        public static readonly DependencyProperty arrowColorProperty =
            DependencyProperty.Register("arrowColor", typeof(Brush), typeof(ucLevel), new PropertyMetadata(Brushes.Blue));

        //------------------------------------------------------------------------------------------------------------------------
        public double levelValue
        {
            get { return (double)GetValue(levelValueProperty); }
            set { SetValue(levelValueProperty, value); }
        }
        public static readonly DependencyProperty levelValueProperty =
            DependencyProperty.Register("levelValue", typeof(double), typeof(ucLevel), new PropertyMetadata(0d,changeValue));

        private static void changeValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ucLevel obj = d as ucLevel;

            double w = obj.RenderSize.Height - 2.5d;
            var g = (w / 100) * obj.sliderA.Value;
            obj.pathA.Margin = new Thickness(0, 0, 0, g);

            RoutedEventArgs b = new RoutedEventArgs(LevelValueChanged_Event, (double)e.NewValue);
            (d as ucLevel).RaiseEvent(b);
        }
        //------------------------------------------------------------------------------------------------------------------------
        private void userControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = RenderSize.Height - 2.5d;
            var f = (w / 100) * sliderA.Value;
            pathA.Margin = new Thickness(0, 0, 0, f);
        }

        //------------------------------------------------------------------------------------------------------------------------
        public static readonly RoutedEvent LevelValueChanged_Event = EventManager.RegisterRoutedEvent("LevelValueChanged", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(ucLevel));
        

        public event RoutedEventHandler LevelValueChanged
        {
            add { AddHandler(LevelValueChanged_Event, value); }
            remove { RemoveHandler(LevelValueChanged_Event, value); }
        }
        //------------------------------------------------------------------------------------------------------------------------
    }
}
