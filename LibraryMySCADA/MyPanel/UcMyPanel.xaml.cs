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

namespace LibraryMySCADA.MyPanel
{
    public partial class UcMyPanel : UserControl
    {

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public Visibility TitleVisibility
        {
            get { return (Visibility)GetValue(TitleVisibilityProperty); }
            set { SetValue(TitleVisibilityProperty, value); }
        }
        public static readonly DependencyProperty TitleVisibilityProperty =
            DependencyProperty.Register("TitleVisibility", typeof(Visibility), typeof(UcMyPanel), new PropertyMetadata(Visibility.Visible));

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public Visibility ButtonCloseVisibility
        {
            get { return (Visibility)GetValue(ButtonCloseVisibilityProperty); }
            set { SetValue(ButtonCloseVisibilityProperty, value); }
        }
        public static readonly DependencyProperty ButtonCloseVisibilityProperty =
            DependencyProperty.Register("ButtonCloseVisibility", typeof(Visibility), typeof(UcMyPanel), new PropertyMetadata(Visibility.Visible));

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public Brush BrushTitle
        {
            get { return (Brush)GetValue(BrushTitleProperty); }
            set { SetValue(BrushTitleProperty, value); }
        }
        public static readonly DependencyProperty BrushTitleProperty =
            DependencyProperty.Register("BrushTitle", typeof(Brush), typeof(UcMyPanel), new PropertyMetadata(Brushes.Silver));


        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public Brush BrushButtonClose   
        {
            get { return (Brush)GetValue(BrushButtonCloseProperty); }
            set { SetValue(BrushButtonCloseProperty, value); }
        }
        public static readonly DependencyProperty BrushButtonCloseProperty =
            DependencyProperty.Register("BrushButtonCloseTitle", typeof(Brush), typeof(UcMyPanel), new PropertyMetadata(Brushes.Silver));


        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public bool HideVisiblePanel
        {
            get { return (bool)GetValue(HideVisiblePanelProperty); }
            set { SetValue(HideVisiblePanelProperty, value); }
        }
        public static readonly DependencyProperty HideVisiblePanelProperty =
            DependencyProperty.Register("HideVisiblePanel", typeof(bool), typeof(UcMyPanel), new PropertyMetadata(true));

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public Grid ParentGrid
        {
            get { return (Grid)GetValue(ParentGridProperty); }
            set { SetValue(ParentGridProperty, value); }
        }
        public static readonly DependencyProperty ParentGridProperty =
            DependencyProperty.Register("ParentGrid", typeof(Grid), typeof(UcMyPanel), new PropertyMetadata(null));

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(UcMyPanel), new PropertyMetadata(0d));

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public string Tittle
        {
            get { return (string)GetValue(TittleProperty); }
            set { SetValue(TittleProperty, value); }
        }
        public static readonly DependencyProperty TittleProperty =
            DependencyProperty.Register("Tittle", typeof(string), typeof(UcMyPanel), new PropertyMetadata("Tittle"));

        //--------------------------------------------------------------------------------------------------------------------------
         public UcMyPanel()
        {
           
            InitializeComponent();
        }

        //--------------------------------------------------------------------------------------------------------------------------
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            if (ParentGrid != null)
            {
                var asd = TranslatePoint(new Point(0, 0), ParentGrid);
                (Parent as Panel).Children.Remove(this);
                ParentGrid.Children.Add(this);
                RenderTransform = new TranslateTransform(asd.X, asd.Y);
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public void ResetPosithion()
        {
            RenderTransform = new TranslateTransform(0, 0);
        }
        //--------------------------------------------------------------------------------------------------------------------------
        //private void  ButtonCloseClick(object sender, RoutedEventArgs e)
        //{

        //}
        //--------------------------------------------------------------------------------------------------------------------------
    }
}
