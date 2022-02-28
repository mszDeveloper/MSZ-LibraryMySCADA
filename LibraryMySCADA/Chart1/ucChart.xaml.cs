using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using static System.Net.Mime.MediaTypeNames;

namespace wpfLibMszControl.Chart
{
    public partial class ucChart : UserControl
    {
        public enum LineType
        {
            Line,
            PoliLine,
        }

        public struct PointChart
        {
            public PointChart(decimal xin, decimal yin) { x = xin; y = yin; }
            public decimal x;
            public decimal y;
        }

        private List<decimal> pointX = new List<decimal>();

        private List<PointChart> points = new List<PointChart>();

        private bool mouseRightDown = false;
        private double X1Select,Xselect, XselectEnd;
        //private Thickness SelectThicckiness;
        //----------------------------------------------------------------------------------------------------------
        [Description("На сколько увеличиваеться максимальное значение оси Y")]
        [Category("Setting")]
        public int ForChartYincrease { get; set; } = 1;

        //----------------------------------------------------------------------------------------------------------
        [Description("Среднее значение выделенного участка")]
        [Category("Setting")]
        public double AverageValueSelected
        {
            get { return (double)GetValue(AverageValueSelectedProperty); }
            set { SetValue(AverageValueSelectedProperty, value); }
        }
        public static readonly DependencyProperty AverageValueSelectedProperty =
            DependencyProperty.Register("AverageValueSelected", typeof(double), typeof(ucChart), new PropertyMetadata(0d));

        //----------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public double Xrezult
        {
            get { return (double)GetValue(XrezultProperty); }
            set { SetValue(XrezultProperty, value); }
        }
        public static readonly DependencyProperty XrezultProperty =
            DependencyProperty.Register("Xrezult", typeof(double), typeof(ucChart), new PropertyMetadata(0d));

        //----------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public double Yrezult
        {
            get { return (double)GetValue(YrezultProperty); }
            set { SetValue(YrezultProperty, value); }
        }
        public static readonly DependencyProperty YrezultProperty =
            DependencyProperty.Register("Yrezult", typeof(double), typeof(ucChart), new PropertyMetadata(0d));

        //----------------------------------------------------------------------------------------------------------
        [Description("Имя графика")]
        [Category("Setting")]
        public string nameChart
        {
            get { return (string)GetValue(nameChart_Property); }
            set { SetValue(nameChart_Property, value); }
        }

        public static readonly DependencyProperty nameChart_Property =
                   DependencyProperty.Register("nameChart", typeof(string),
                   typeof(ucChart), new PropertyMetadata("Default"));

        //----------------------------------------------------------------------------------------------------------
        [Description("Цвет фона")]
        [Category("Setting")]
        public Brush colorChart
        {
            get { return (Brush)GetValue(colorChart_Property); }
            set { SetValue(colorChart_Property, value); }
        }

        public static readonly DependencyProperty colorChart_Property =
                   DependencyProperty.Register("colorChart", typeof(Brush),
                   typeof(ucChart), new PropertyMetadata(Brushes.Silver));

        //----------------------------------------------------------------------------------------------------------
        [Description("Цвет выделения")]
        [Category("Setting")]
        public Brush colorSelected
        {
            get { return (Brush)GetValue(colorSelected_Property); }
            set { SetValue(colorSelected_Property, value); }
        }

        public static readonly DependencyProperty colorSelected_Property =
                   DependencyProperty.Register("colorSelected", typeof(Brush),
                   typeof(ucChart), new PropertyMetadata(Brushes.Yellow));

        //----------------------------------------------------------------------------------------------------------
        [Description("Видимость выделения")]
        [Category("Setting")]
        public Visibility VisibilitySelected
        {
            get { return (Visibility)GetValue(VisibilitySelectedProperty); }
            set { SetValue(VisibilitySelectedProperty, value); }
        }
        public static readonly DependencyProperty VisibilitySelectedProperty =
            DependencyProperty.Register("VisibilitySelected", typeof(Visibility), typeof(ucChart), new PropertyMetadata(Visibility.Collapsed));

        //----------------------------------------------------------------------------------------------------------
        [Description("Цвет линии")]
        [Category("Setting")]
        public Brush colorLineChart
        {
            get { return (Brush)GetValue(colorLineChart_Property); }
            set { SetValue(colorLineChart_Property, value); }
        }

        public static readonly DependencyProperty colorLineChart_Property =
                   DependencyProperty.Register("colorLineChart", typeof(Brush),
                   typeof(ucChart), new PropertyMetadata(Brushes.Blue));

        //----------------------------------------------------------------------------------------------------------
        [Description("Цвет осей")]
        [Category("Setting")]
        public Brush colorAxes
        {
            get { return (Brush)GetValue(colorAxes_Property); }
            set { SetValue(colorAxes_Property, value); }
        }

        public static readonly DependencyProperty colorAxes_Property =
                   DependencyProperty.Register("colorAxes", typeof(Brush),
                   typeof(ucChart), new PropertyMetadata(Brushes.Black));

        //----------------------------------------------------------------------------------------------------------
        [Description("Толщина линий осей")]
        [Category("Setting")]
        public int lineAxesWight
        {
            get { return (int)GetValue(lineAxesWight_Property); }
            set { SetValue(lineAxesWight_Property, value); }
        }

        public static readonly DependencyProperty lineAxesWight_Property =
                   DependencyProperty.Register("lineAxesWight", typeof(int),
                   typeof(ucChart), new PropertyMetadata(1));

        //----------------------------------------------------------------------------------------------------------
        [Description("Толщина линий графика")]
        [Category("Setting")]
        public int lineChartWight
        {
            get { return (int)GetValue(lineChartWight_Property); }
            set { SetValue(lineChartWight_Property, value); }
        }

        public static readonly DependencyProperty lineChartWight_Property =
                   DependencyProperty.Register("lineChartWight", typeof(int),
                   typeof(ucChart), new PropertyMetadata(1));

        //----------------------------------------------------------------------------------------------------------
        string _NameXAxes;
        [Category("Setting")]
        public string NameXAxes { get { return _NameXAxes; } set { _NameXAxes = value;  } }

        int _stepLineY = 1;
        [Category("Setting")]
        public int stepLineY { get { return _stepLineY; } set { _stepLineY = value; drowAxes(); } }


        int _MaxY;
        [Category("Setting")]
        public int MaxY { get { return _MaxY; } set { _MaxY = value; drowAxes(); } }

        int _MinY;
        [Category("Setting")]
        public int MinY { get { return _MinY; } set { _MinY = value; drowAxes(); } }

        decimal _SizeX_point = 10;
        [Category("Setting")]
        public decimal SizeX_point { get { return _SizeX_point; } set { _SizeX_point = value; } }

        int _offsetY;
        [Category("Setting")]
        public int offsetY { get { return _offsetY; } set { _offsetY = value; drowAxes(); } }

        int _offsetX;
        [Category("Setting")]
        public int offsetX { get { return _offsetX; } set { _offsetX = value; drowAxes(); } }

        LineType _Line;
        [Category("Setting")]
        public LineType Line { get { return _Line; } set { _Line = value; drowAxes(); } }

        private int ySize,xSize;//текущие размеры
        private decimal yAxesStart;//координата оси - y
        private decimal yPoinPixel, xPointPixel;

        //********************************************************************************************************************
        public void ClearChart()
        {
            Charter.Children.Clear();
            points.Clear();
        }

        //********************************************************************************************************************
        public void ClearOnlyChart()
        {
            Charter.Children.Clear();
        }

        //********************************************************************************************************************
        public void SortChart()
        {
            points.Sort((p, z) => p.x > z.x ? 1 : -1);
        }

        //********************************************************************************************************************
        public ucChart()
        {
            InitializeComponent();
            this.SizeChanged += UcChart_SizeChanged;
        }

        //********************************************************************************************************************
        public void drowChart()
        {
           Charter.Children.Clear();
            Polyline vertArr = new Polyline();
            vertArr.Points = new PointCollection();
            bool breaked;
            do
            {
                breaked = false;
                foreach (PointChart item in points)
                {
                    if (item.y > MaxY)
                    {
                        MaxY = (int)item.y + ForChartYincrease;
                        vertArr.Points.Clear();
                        breaked = true;
                        break;
                    }
                    double X1 = (double)((item.x) * xPointPixel + offsetX);
                    double Y1 = (double)(ySize - (item.y * yPoinPixel));
                    Y1 -= (double)yAxesStart;
                    vertArr.Points.Add(new Point(X1, Y1));
                }
            } while (breaked);

            vertArr.Stroke = colorLineChart;
            vertArr.StrokeThickness = lineChartWight;
            Charter.Children.Add(vertArr);
            
        }

        //********************************************************************************************************************
        private void UcChart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Charter.Children.Clear();
            drowAxes();
            drowChart();
        }

        //********************************************************************************************************************
        public void AddChartItem(decimal x, decimal y)
        {
            points.Add(new PointChart(x,y));
            
        }

        //********************************************************************************************************************
        private void GridChart_Loaded(object sender, RoutedEventArgs e)
        {
            drowAxes();
        }

        //********************************************************************************************************************
        private void drowAxes()
        {
            getSize();
            if (xPointPixel == 0) return;
            LinesGrid.Children.Clear();

            for (int d = MinY; d < (MaxY) && stepLineY != 0; d += stepLineY)
            {
                double Y1 = (double)(d * yPoinPixel);
                UserControl_line line = new UserControl_line();
                line.Margin = new Thickness(0, 0, 0, Y1 + getYaxesStart());
                line.textLine = d.ToString();
                LinesGrid.Children.Add(line);
            }

            X.Margin = new Thickness(0, 0, 0, (double)getYaxesStart());
            Y.Margin = new Thickness(offsetX, 0, 0, 0);
        }

        //********************************************************************************************************************
        private void border_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point xy = e.GetPosition(GridChart);
            
            if (xy.X < 0 || xy.X > GridChart.ActualWidth)
                return;
            Marker.Margin = new Thickness(xy.X, Marker.Margin.Top, 0, 0);
            Xrezult = (double)Math.Round((SizeX_point / (decimal)GridChart.ActualWidth) * (decimal)xy.X);
            Yrezult = GetRezY(Xrezult);
            if (mouseRightDown) { XselectEnd = xy.X; CalcAvarageSelectedValue(); }
        }

        //********************************************************************************************************************
        public void CalcAvarageSelectedValue()
        {
            //Point xy = Mouse.GetPosition(GridChart);
            double XWidthSelect = XselectEnd - Xselect;
            IEnumerable<PointChart> point = null;
            if (XWidthSelect >= 0)
            {
                SelectGrid.RenderTransform = new TranslateTransform(Xselect, 0);
                SelectGrid.Width = XWidthSelect;
                point = points.Where(p => p.x <= (decimal)Xrezult && p.x >= (decimal)X1Select);

            }
            else
            {
                SelectGrid.RenderTransform = new TranslateTransform(XselectEnd, 0);
                SelectGrid.Width = XWidthSelect * -1;
                point = points.Where(p => p.x >= (decimal)Xrezult && p.x <= (decimal)X1Select);

            }
            decimal rezult = 0;
            int pointCout = 0;
            try
            {
                foreach (PointChart item in point)
                {
                    rezult += item.y;
                    pointCout++;
                }
            }
            catch { };
            if (pointCout != 0) AverageValueSelected = (double)(rezult / pointCout);
        }

        //********************************************************************************************************************
        private double GetRezY(double x)
        {
            decimal xFind = (decimal)x;
            PointChart p1=new PointChart(), p2=new PointChart();

            foreach (var item in points)
            {
                if (item.x <= xFind) p1 = item;
                else { p2 = item; break; }
            }
            return interpolate((double)p1.x, (double)p1.y, (double)p2.x, (double)p2.y,(double)xFind);
        }

        //********************************************************************************************************************
        double interpolate(double x0, double y0, double x1, double y1, double x)
        {
            if ((x1 - x0) == 0 || (x0 - x1) == 0) return 0;
            return y0 * (x - x1) / (x0 - x1) + y1 * (x - x0) / (x1 - x0);
        }

        //********************************************************************************************************************
        private void GridChart_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            VisibilitySelected = Visibility.Visible;
            Point xy = e.GetPosition(GridChart);
            Xselect = xy.X;
            mouseRightDown = true;
            X1Select = Xrezult;
            VisibilitySelected = Visibility.Visible;
        }

        private void Chart_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseRightDown = false;
        }

        //********************************************************************************************************************

        //********************************************************************************************************************
        private void GridChart_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseRightDown = false;
        }

        //********************************************************************************************************************
        private double getYaxesStart()
        {
            return (double)(yAxesStart = offsetY + (Math.Abs(MinY) * yPoinPixel));
        }

        //********************************************************************************************************************
        private void getSize()
        {
            ySize = (int)GridChart.ActualHeight;
            xSize = (int)GridChart.ActualWidth;
            if (MaxY == 0 && MinY == 0) return;
            yPoinPixel = (ySize - (offsetY * 2)) / (Math.Abs(MaxY) + Math.Abs(MinY));
            xPointPixel = (xSize - (offsetX * 2)) / (SizeX_point == 0 ? 1 : (SizeX_point));


        }

        //********************************************************************************************************************

    }

    //-------------------------------------------------------------------------------------------------------
    public class ConvertMinuteToHour : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            return Math.Round(val / 60);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }

    //-------------------------------------------------------------------------------------------------------
    public class ConvertDoubleToStringTime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            return Math.Truncate(val / 60).ToString() + "ч " + val % 60 + "м  ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "0ч 0м";
        }
    }

    //-------------------------------------------------------------------------------------------------------

}
