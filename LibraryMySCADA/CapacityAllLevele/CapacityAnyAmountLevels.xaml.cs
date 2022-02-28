using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraryMySCADA.CapacityAllLevele
{

    public partial class CapacityAnyAmountLevels : UserControl
    {
        [Category("Настройки")]
        [Description("Коллекция уровней")]
        public List<ucLevel> levels { get; set; } = new List<ucLevel>();
        //------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки")]
        [Description("Режим при авто уровнях")]
        public int Mode
        {
            get { return (int)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(int), typeof(CapacityAnyAmountLevels), new PropertyMetadata(0, changedval));

        private static void changedval(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        //------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки")]
        [Description("Автомотический контроль установки уровней")]
        public bool autoControlRange { get; set; } = true;

        [Category("Настройки")]
        [Description("Максимальное значение")]
        public double fullValue { get; set; } = 10;

        [Category("Настройки")]
        [Description("Еденица измерения")]
        public string unit { get; set; } = "M";

        [Category("Настройки")]
        [Description("Количество разрядов после точки")]
        public int unitDigit { get; set; } = 1;

        [Category("Настройки вида")]
        [Description("Использовать цвет указателя уровня для цвета фона подписи")]
        public bool colorFont { get; set; } = true;

        [Category("Настройки вида")]
        [Description("Фон для емкости")]
        public ImageSource imgFill
        {
            get { return (ImageSource)GetValue(imgFillProperty); }
            set { SetValue(imgFillProperty, value); }
        }
        public static readonly DependencyProperty imgFillProperty =
            DependencyProperty.Register("imgFill", typeof(ImageSource), typeof(CapacityAnyAmountLevels), new PropertyMetadata(null));

        //------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки")]
        [Description("уровень наполнения, 0-100 процентов")]
        public double value
        {
            get { return (double)GetValue(valueProperty); }
            set { SetValue(valueProperty, value); }
        }
        public static readonly DependencyProperty valueProperty =
            DependencyProperty.Register("value", typeof(double), typeof(CapacityAnyAmountLevels), new PropertyMetadata(0d, changeValue));

        private static void changeValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CapacityAnyAmountLevels obj = d as CapacityAnyAmountLevels;
            obj.FillRect();
            foreach (var item in obj.levels)
            {
                item.CheckValueLevel((double)e.NewValue);
            }
            obj.LabelCurrentLevel.Content = obj.CalcValue((double)e.NewValue).ToString() +  obj.unit;
            obj.currentLevelValue = obj.CalcValue((double)e.NewValue);
        }

        //------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        [Description("Размер шрифта")]
        public double fontSize
        {
            get { return (double)GetValue(fontSizeProperty); }
            set { SetValue(fontSizeProperty, value); }
        }
        public static readonly DependencyProperty fontSizeProperty =
            DependencyProperty.Register("fontSize", typeof(double), typeof(CapacityAnyAmountLevels));

        //------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        [Description("Видимость заголовка")]
        public Visibility showHead
        {
            get { return (Visibility)GetValue(showHeadProperty); }
            set { SetValue(showHeadProperty, value); }
        }
        public static readonly DependencyProperty showHeadProperty =
            DependencyProperty.Register("showHead", typeof(Visibility), typeof(CapacityAnyAmountLevels), new PropertyMetadata( Visibility.Visible ));

        //------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        [Description("Размер шрифта")]
        public string textHeader
        {
            get { try { return LabelHeader.Content.ToString(); } catch { return null; } }
            set { LabelHeader.Content = value; }
        }

        [Category("Настройки вида")]
        [Description("Толщина линий")]
        public double StrokeThikenes
        {
            get { return border.BorderThickness.Left; }
            set { border.BorderThickness = new Thickness(value, value, value, value); }
        }

        [Category("Настройки вида")]
        [Description("Радиус бордюра")]
        public CornerRadius Radius
        {
            get { return border.CornerRadius; }
            set { border.CornerRadius = value; }
        }

        //------------------------------------------------------------------------------------------------------------------------
        public double currentLevelValue
        {
            get { return (double)GetValue(currentLevelVlueProperty); }
            set { SetValue(currentLevelVlueProperty, value); }
        }
        public static readonly DependencyProperty currentLevelVlueProperty =
            DependencyProperty.Register("currentLevelValue", typeof(double), typeof(CapacityAnyAmountLevels), new PropertyMetadata(0d,changedVal));

        private static void changedVal(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CapacityAnyAmountLevels obj = d as CapacityAnyAmountLevels;
            obj.labelValue.Content = e.NewValue.ToString()+ obj.unit;
        }

        //----------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Цвет заполнения")]
        public System.Windows.Media.Color CapacityFillColor
        {
            get { return (System.Windows.Media.Color)GetValue(CapacityFillColor_Property); }
            set { SetValue(CapacityFillColor_Property, value); }
        }
        public static readonly DependencyProperty CapacityFillColor_Property =
                   DependencyProperty.Register("CapacityFillColor", typeof(System.Windows.Media.Color),
                   typeof(CapacityAnyAmountLevels), new PropertyMetadata(System.Windows.Media.Colors.Blue));
        //----------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        [Description("Цвет пустоты")]
        public System.Windows.Media.Color CapacityNoFillColor
        {
            get { return (System.Windows.Media.Color)GetValue(CapacityNoFillColor_Property); }
            set { SetValue(CapacityNoFillColor_Property, value); }
        }
        public static readonly DependencyProperty CapacityNoFillColor_Property =
                   DependencyProperty.Register("CapacityNoFillColor", typeof(System.Windows.Media.Color),
                   typeof(CapacityAnyAmountLevels), new PropertyMetadata(System.Windows.Media.Colors.Transparent));

        //------------------------------------------------------------------------------------------------------------------------
        public CapacityAnyAmountLevels()
        {
            InitializeComponent();
        }
        //----------------------------------------------------------------------------------------------------------
        private void FillRect()
        {
            double y = (value / 50 - 1) * -1;
            Resources["Point1"] = new System.Windows.Point(0, y);
        }

        //------------------------------------------------------------------------------------------------------------------------
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (autoControlRange)
            {
                levels.Sort((lhs, rhs) => lhs.levelValue > rhs.levelValue ? 1 : 0);
            }
            int index = 0;
            foreach (ucLevel item in levels)
            {
                rootGrid.Children.Add(item);
                item.index = index;
                item.LevelValueChanged += Item_LevelValueChanged;
                item.sliderA.MouseEnter += SliderA_PreviewMouseMove;
                item.sliderA.MouseLeave += SliderA_MouseLeave;
                item.LevelIsStateChange += Item_LevelIsStateChange;
                index++;
            }
            Canvas.SetZIndex(LabelHeader, 1);
            RoutedEventArgs b = new RoutedEventArgs(ChangeLevelState_Event, e.OriginalSource);
            RaiseEvent(b);

        }

        //------------------------------------------------------------------------------------------------------------------------
        private void Item_LevelIsStateChange(object sender, RoutedEventArgs e)
        {
            
            int ind = (int)e.OriginalSource;
            if (value >= levels[ind].levelValue && !levels[ind].modeFull) { Mode = ind; levels[ind].modeFull = true; }
            if (value < levels[ind].levelValue && levels[ind].modeFull) { Mode = ind; levels[ind].modeFull = false; }

            RoutedEventArgs b = new RoutedEventArgs(ChangeLevelState_Event, e.OriginalSource);
            RaiseEvent(b);
        }
        public static readonly RoutedEvent ChangeLevelState_Event = EventManager.RegisterRoutedEvent("ChangeLevelState", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(ucLevel));
        

        public event RoutedEventHandler ChangeLevelState
        {
            add { AddHandler(ChangeLevelState_Event, value); }
            remove { RemoveHandler(ChangeLevelState_Event, value); }
        }

        //------------------------------------------------------------------------------------------------------------------------
        public void RecalcModeState()
        {
            //RoutedEventArgs b = new RoutedEventArgs(ChangeLevelState_Event, -1);
            //RaiseEvent(b);
            Mode = -1 ;
            int ind = 0;
            foreach (var item in levels) { levels[ind].modeFull = false; }
            foreach (var item in levels)
            {
                if (value >= levels[ind].levelValue) { Mode = ind; levels[ind].modeFull = true; }
                ind++;
            }
            if (Mode == -1) Mode = 0;
            RoutedEventArgs b = new RoutedEventArgs(ChangeLevelState_Event, Mode);
            RaiseEvent(b);
        }

        //------------------------------------------------------------------------------------------------------------------------
        private int StrCmpLogicalW(ucLevel p1, ucLevel p2)
        {
            if (p1.levelValue >= p2.levelValue) return 1;
            else return 0;
        }

        //------------------------------------------------------------------------------------------------------------------------
        private void SliderA_MouseLeave(object sender, MouseEventArgs e)
        {
            ucLevel obj = ((sender as Slider).Parent as Grid).Parent as ucLevel;
            labelValue.Visibility = Visibility.Collapsed;
        }

        //------------------------------------------------------------------------------------------------------------------------
        private void SliderA_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            ucLevel obj = ((sender as Slider).Parent as Grid).Parent as ucLevel;
            labelValue.Visibility = Visibility.Visible;

            double w = RenderSize.Height;
            var g = (w / 100) * obj.sliderA.Value;
            CanvasValue.Margin = new Thickness(0, 0, 0, g);
            currentLevelValue = CalcValue(obj.levelValue);
            if (colorFont) labelValue.Background = obj.sliderA.Background;
        }

        //------------------------------------------------------------------------------------------------------------------------
        private void Item_LevelValueChanged(object sender, RoutedEventArgs e)
        {
            if (!autoControlRange) return;
            double val = (double)e.OriginalSource;
            int ind = levels.IndexOf((ucLevel)sender);
            double max =  (ind != levels.Count-1) ? levels[ind + 1].levelValue: 100;
            double min = (ind != 0) ? levels[ind - 1].levelValue : 0;
            if (val >= max && ind != levels.Count - 1) { levels[ind].levelValue = max - 1; }
            else if (val <= min && ind != 0) { levels[ind].levelValue = min + 1; }
            double w = RenderSize.Height;
            var g = (w / 100) * val;
            CanvasValue.Margin = new Thickness(0, 0, 0, g);
            currentLevelValue = CalcValue(levels[ind].levelValue);
        }

        //------------------------------------------------------------------------------------------------------------------------
        double CalcValue(double val)
        {
            return Math.Round(fullValue / 100 * val, unitDigit);
        }
        //------------------------------------------------------------------------------------------------------------------------
    }
}
