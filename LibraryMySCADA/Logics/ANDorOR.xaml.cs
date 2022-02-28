using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryMySCADA.Logics
{
    //-----------------------------------------------------------------------------------------------------------------------
    public partial class ANDorOR : UserControl
    {
        public enum Logic
        {
            OR,
            AND
        }

        [Category("Setting")]
        public string info { get; set; }


        [Category("Setting")]
        public Logic logic { get; set; } = Logic.AND;

        [Category("Setting")]
        public bool visiblePanel
        {
            get { return ViewBox.Visibility == Visibility.Visible ? true : false; }
            set { ViewBox.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }


    [Category("Setting")]
        public bool? in1
        {
            get { return (bool?)GetValue(in1Property); }
            set { SetValue(in1Property, value); }
        }
        public static readonly DependencyProperty in1Property =
            DependencyProperty.Register("in1", typeof(bool?), typeof(ANDorOR), new PropertyMetadata(null,changeOn1));

        private static void changeOn1(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            (d as ANDorOR).CheckOut(e);

        }

        private void CheckOut(DependencyPropertyChangedEventArgs e)
        {
            if (logic == Logic.AND)
            {
                if ((in1 == null || in1 == true) && (in2 == null || in2 == true) && (in3 == null || in3 == true)) outPin = true;
                else outPin = false;

            }
            else
            {
                //var d = in1 | in2 | in3;
                if ((in1 == true) || (in2 == true) || (in3 == true)) outPin = true;
                else outPin = false;
            }

        }

        //--------------------------------------------------------------------------
        [Category("Setting")]
        public bool? in2
        {
            get { return (bool?)GetValue(in2Property); }
            set { SetValue(in2Property, value); }
        }
        public static readonly DependencyProperty in2Property =
            DependencyProperty.Register("in2", typeof(bool?), typeof(ANDorOR), new PropertyMetadata(null, changeOn2));

        private static void changeOn2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ANDorOR).CheckOut(e);
        }

        //--------------------------------------------------------------------------
        [Category("Setting")]
        public bool? in3
        {
            get { return (bool?)GetValue(in3Property); }
            set { SetValue(in3Property, value); }
        }
        public static readonly DependencyProperty in3Property =
            DependencyProperty.Register("in3", typeof(bool?), typeof(ANDorOR), new PropertyMetadata(null, changeOn3));

        private static void changeOn3(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ANDorOR).CheckOut(e);
        }

        //--------------------------------------------------------------------------
        [Category("Setting")]
        public bool outPin
        {
            get { return (bool)GetValue(outPinProperty); }
            set { SetValue(outPinProperty, value); }
        }
        public static readonly DependencyProperty outPinProperty =
            DependencyProperty.Register("outPin", typeof(bool), typeof(ANDorOR), new PropertyMetadata(false, changeoutPin));

        private static void changeoutPin(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        //--------------------------------------------------------------------------
        public ANDorOR()
        {
            InitializeComponent();
         }

        private void cheng(object sender, NotifyCollectionChangedEventArgs e)
        {
            
        }

        private void UserControl_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            Label.Content = logic.ToString();
            ViewBox.ToolTip = info;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
}
