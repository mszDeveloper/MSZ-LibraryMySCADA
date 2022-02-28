using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace LibraryMySCADA.Flowmeter
{
    public partial class FlowMeterControl : UserControl
    {

        //-------------------------------------------------------------------------------------------------------------------
        public double DailyCounter
        {
            get { return (double)GetValue(DailyCounterProperty); }
            set { SetValue(DailyCounterProperty, value); }
        }
        public static readonly DependencyProperty DailyCounterProperty =
            DependencyProperty.Register("DailyCounter", typeof(double), typeof(FlowMeterControl), new PropertyMetadata(0d, changed));

        private static void changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlowMeterControl obj = d as FlowMeterControl;
            obj.CommonFlowMeter.ValueCounter = obj.AllCounter + (double)e.NewValue;
            RoutedEventArgs b = new RoutedEventArgs(NewValue_Event, e.NewValue);
            obj.RaiseEvent(b);

        }

        //-------------------------------------------------------------------------------------------------------------------
        public double AllCounter
        {
            get { return (double)GetValue(AllCounterProperty); }
            set { SetValue(AllCounterProperty, value); }
        }
        public static readonly DependencyProperty AllCounterProperty =
            DependencyProperty.Register("AllCounter", typeof(double), typeof(FlowMeterControl), new PropertyMetadata(0d, changeAllCounter));

        private static void changeAllCounter(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            // FlowMeterControl obj = d as FlowMeterControl;
            // obj.AllCounter = (double)e.NewValue;
        }


        //-------------------------------------------------------------------------------------------------------------------
        [Category("Variable")]
        public bool VisiblePanel
        {
            get { return (bool)GetValue(VisiblePanelProperty); }
            set { SetValue(VisiblePanelProperty, value); }
        }
        public static readonly DependencyProperty VisiblePanelProperty =
            DependencyProperty.Register("VisiblePanel", typeof(bool), typeof(FlowMeterControl), new PropertyMetadata(true));
       
        //-------------------------------------------------------------------------------------------------------------------


        public FlowMeterControl()
        {
            InitializeComponent();
            //textBoxEnterValue.bu
        }

        private void buttonHide_Click(object sender, RoutedEventArgs e)
        {
            VisiblePanel = false;
        }

        private void rButton_Click(object sender, RoutedEventArgs e)
        {
            textBoxEnterValue.Visibility = Visibility.Visible;
            textBoxEnterValue.Text = CommonFlowMeter.ValueCounter.ToString();
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            textBoxEnterValue.Visibility = Visibility.Collapsed;
            int val= -1;
            try { val = int.Parse(textBoxEnterValue.Text); } catch { textBoxEnterValue.Text = null; val = -1; return; }
            var rez = MessageBox.Show("Новое значение счетчика - " + val.ToString() + "\nПрезаписать, вы уверены?", "Новое значение", MessageBoxButton.OKCancel);
            if (rez == MessageBoxResult.Cancel) { val = -1; return; }
            
            RoutedEventArgs b = new RoutedEventArgs(NewValueFlowMeter_Event, val);
            RaiseEvent(b);

        }

        //-------------------------------------------------------------------------------------------------------------------
        //сообщение о записи нового значения в счетчик
        public static readonly RoutedEvent NewValueFlowMeter_Event = EventManager.RegisterRoutedEvent("NewValueFlowMeter", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(FlowMeterControl));
        public event RoutedEventHandler NewValueFlowMeter_Changed
        {
            add { AddHandler(NewValueFlowMeter_Event, value); }
            remove { RemoveHandler(NewValueFlowMeter_Event, value); }
        }

        //-------------------------------------------------------------------------------------------------------------------
        //сообщение о изменении значения счетчика
        public static readonly RoutedEvent NewValue_Event = EventManager.RegisterRoutedEvent("SetNewValue", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(FlowMeterControl));
        public event RoutedEventHandler SetNewValue
        {
            add { AddHandler(NewValue_Event, value); }
            remove { RemoveHandler(NewValue_Event, value); }
        }
        //RoutedEventArgs b = new RoutedEventArgs(NewValue_Event, val);
        //RaiseEvent(b);

        //-------------------------------------------------------------------------------------------------------------------
        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            //CommonFlowMeter.ValueCounter = AllCounter;
        }

    }
}
