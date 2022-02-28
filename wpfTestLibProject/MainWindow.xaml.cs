using LibraryMySCADA;
using LibraryMySCADA.CapacityAllLevele;
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
using System.Windows.Shapes;

namespace wpfTestLibProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public bool Error
        {
            get { return (bool)GetValue(ErrorProperty); }
            set { SetValue(ErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Error.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorProperty =
            DependencyProperty.Register("Error", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));




        static Window wndBase;
        nsClassModBus.ClassModBus mbClass;
        public int Zind = 10;

        public MainWindow()
        {
            wndBase = this;
            mbClass = new nsClassModBus.ClassModBus();
            InitializeComponent();
            //directionPanel.arrowFRQ.MinValue = 20;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataSaveSCADA.FindAndLoadUserControl(this);
            //CapacityAnyAmountLevels.ChangeLevelState += CapacityAnyAmountLevels_ChangeLevelState;
        }

 
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           DataSaveSCADA.FindAndSaveUserControl(this);
            mbClass.StopModBusthread();
        }
        
        public static void SavedAllData()
        {
            //DataSaveSCADA.FindAndSaveUserControl(wndBase);
            //wndBase.Dispatcher.Invoke(new Action(() => { }));
        }

        private void buttonAuto_Click(object sender, RoutedEventArgs e)
        {
            //fume.isAuto = !fume.isAuto;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mbClass.ShowWinModBusClass();
        }

        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
           // ucDirect.HideVisiblePanel = !ucDirect.HideVisiblePanel;
        }

        private void ButtonAutoOnOff_Click(object sender, RoutedEventArgs e)
        {
            //dddd.OnOffMixer = !dddd.OnOffMixer;//mix.OnOffMixer = !mix.OnOffMixer;
        }
        //Char ss = '0';
        private void buttonSendError_Click(object sender, RoutedEventArgs e)
        {
            //counterNumericAnim.ValueCounter++;
        }

        private void ucUGO_DRIVERControl_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void buttonWorker_Click(object sender, RoutedEventArgs e)
        {
        }

        private void buttonOnOff_Click(object sender, RoutedEventArgs e)
        {
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            //sMyPanel.HideVisiblePanel = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Signal.Visibility = Signal.Visibility == Visibility.Collapsed? Visibility.Visible : Visibility.Collapsed ;
            //PidTest.StartPID(false);
        }

        public void ZindexCheck(object sender, RoutedEventArgs e)
        {
            var obj = ((ContentPresenter)sender).TemplatedParent as UserControl;
            int curIndex = Panel.GetZIndex(obj);
            if (Panel.GetZIndex(obj) == Zind) return;
            Panel.SetZIndex(obj, ++Zind);
        }

 
        private void CapacityAnyAmountLevels_ChangeLevelState_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
