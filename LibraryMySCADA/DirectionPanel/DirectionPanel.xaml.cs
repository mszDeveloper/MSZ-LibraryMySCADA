using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraryMySCADA.DirectionPanel
{
    #region Настройки вида
    public partial class DirectionPanel : Virt.ClassVirtualAdd
    {
        [Category("Variable")]
        public bool? PrevievOnOffinManual = null;


        Grid OldGrid = null;
        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]//buttonPID
        public string ButtonText { get { return _ButtonText; } set { _ButtonText = value; buttonPID.Content = value.ToString(); } }
        string _ButtonText = "Pid";
        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Кисть")]
        public Brush BackArrowColor
        {
            get { return (Brush)GetValue(BackArrowColorProperty); }
            set { SetValue(BackArrowColorProperty, value); }
        }
        public static readonly DependencyProperty BackArrowColorProperty =
            DependencyProperty.Register("BackArrowColor", typeof(Brush), typeof(DirectionPanel), new PropertyMetadata(Brushes.Silver));

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        [Description("Если установленно - то при инициализации панель будет перемещена в дочернии данного элемента")]
        public Grid obj
        {
            get { return (Grid)GetValue(objProperty); }
            set { SetValue(objProperty, value); }
        }
        public static readonly DependencyProperty objProperty =
            DependencyProperty.Register("obj", typeof(Grid), typeof(DirectionPanel), new PropertyMetadata(null, SetsObj));

        private static void SetsObj(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DirectionPanel dp = d as DirectionPanel;
            if (dp.obj != null)
            {
                Grid ne = (Grid)e.NewValue;
                Grid par = dp.Parent as Grid;
                dp.OldGrid = par;
                if (par != null) par.Children.Remove(dp); ;
                ne.Children.Add(dp);
                dp.HorizontalAlignment = HorizontalAlignment.Left;
                dp.VerticalAlignment = VerticalAlignment.Top;
                dp.Margin = new Thickness(0);
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public Visibility AMperageArrow
        {
            get { return (Visibility)GetValue(AMperageArrowProperty); }
            set { SetValue(AMperageArrowProperty, value); }
        }
        public static readonly DependencyProperty AMperageArrowProperty =
            DependencyProperty.Register("AMperageArrow", typeof(Visibility), typeof(DirectionPanel), new PropertyMetadata(Visibility.Visible));
        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public Visibility FRQArrow
        {
            get { return (Visibility)GetValue(FRQArrowProperty); }
            set { SetValue(FRQArrowProperty, value); }
        }
        public static readonly DependencyProperty FRQArrowProperty =
            DependencyProperty.Register("FRQArrow", typeof(Visibility), typeof(DirectionPanel), new PropertyMetadata(Visibility.Visible));

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public Visibility ShowTitle
        {
            get { return (Visibility)GetValue(ShowTitleProperty); }
            set { SetValue(ShowTitleProperty, value); }
        }
        public static readonly DependencyProperty ShowTitleProperty =
            DependencyProperty.Register("ShowTitle", typeof(Visibility), typeof(DirectionPanel), new PropertyMetadata(Visibility.Visible));
        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public Visibility MaxAmperagePanel
        {
            get { return (Visibility)GetValue(MaxAmperagePanelProperty); }
            set { SetValue(MaxAmperagePanelProperty, value); }
        }
        public static readonly DependencyProperty MaxAmperagePanelProperty =
            DependencyProperty.Register("MaxAmperagePanel", typeof(Visibility), typeof(DirectionPanel), new PropertyMetadata(Visibility.Visible));
        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public Visibility buttonPIDVisible
        {
            get { return (Visibility)GetValue(buttonPIDVisibleProperty); }
            set { SetValue(buttonPIDVisibleProperty, value); }
        }
        public static readonly DependencyProperty buttonPIDVisibleProperty =
            DependencyProperty.Register("buttonPIDVisible", typeof(Visibility), typeof(DirectionPanel), new PropertyMetadata(Visibility.Visible));
        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public Visibility manualAutoVisible
        {
            get { return (Visibility)GetValue(manualAutoVisibleProperty); }
            set { SetValue(manualAutoVisibleProperty, value); }
        }
        public static readonly DependencyProperty manualAutoVisibleProperty =
            DependencyProperty.Register("manualAutoVisible", typeof(Visibility), typeof(DirectionPanel), new PropertyMetadata(Visibility.Visible));
        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public Visibility StartStopVisible
        {
            get { return (Visibility)GetValue(StartStopVisibleProperty); }
            set { SetValue(StartStopVisibleProperty, value); }
        }
        public static readonly DependencyProperty StartStopVisibleProperty =
            DependencyProperty.Register("StartStopVisible", typeof(Visibility), typeof(DirectionPanel), new PropertyMetadata(Visibility.Visible));
        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Настройки вида")]
        public Visibility ResetVisible
        {
            get { return (Visibility)GetValue(ResetVisibleProperty); }
            set { SetValue(ResetVisibleProperty, value); }
        }
        public static readonly DependencyProperty ResetVisibleProperty =
            DependencyProperty.Register("ResetVisible", typeof(Visibility), typeof(DirectionPanel), new PropertyMetadata(Visibility.Collapsed));
        //--------------------------------------------------------------------------------------------------------------------------
        #endregion

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        public bool isEneblePanel
        {
            get { return (bool)GetValue(isEneblePanelProperty); }
            set { SetValue(isEneblePanelProperty, value); }
        }
        public static readonly DependencyProperty isEneblePanelProperty =
            DependencyProperty.Register("isEneblePanel", typeof(bool), typeof(DirectionPanel), new PropertyMetadata(true));

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        [Description("Присутствие в шкале частоты уставки ограничения мин. и макс. значения.")]
        public bool isUseMinMaxSetting
        {
            get { return (bool)GetValue(isUseMinMaxSettingProperty); }
            set { SetValue(isUseMinMaxSettingProperty, value); }
        }
        public static readonly DependencyProperty isUseMinMaxSettingProperty =
            DependencyProperty.Register("isUseMinMaxSetting", typeof(bool), typeof(DirectionPanel), new PropertyMetadata(false));

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        public bool isUseSaved { get; set; } = false;

        //--------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        public bool HideVisiblePanel
        {
            get { return (bool)GetValue(HideVisiblePanelProperty); }
            set { SetValue(HideVisiblePanelProperty, value); }
        }
        public static readonly DependencyProperty HideVisiblePanelProperty =
            DependencyProperty.Register("HideVisiblePanel", typeof(bool), typeof(DirectionPanel), new PropertyMetadata(true));

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        public bool OnOff
        {
            get { return (bool)GetValue(OnOffProperty); }
            set { SetValue(OnOffProperty, value); }
        }
        public static readonly DependencyProperty OnOffProperty =
            DependencyProperty.Register("OnOff", typeof(bool), typeof(DirectionPanel), new PropertyMetadata(false, changedOnOff));

        private static void changedOnOff(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DirectionPanel obj = d as DirectionPanel;
            if (obj.FRQArrow != Visibility.Collapsed) obj.arrowFRQ.InvalidateValue();
            else obj.SetOnOffWorkDriver();
            RoutedEventArgs b = new RoutedEventArgs(OnOff_Event, e.NewValue);
            (d as DirectionPanel).RaiseEvent(b);
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public double ArrowValue
        {
            get { return (double)GetValue(ArroeValueProperty); }
            set { SetValue(ArroeValueProperty, value); }
        }
        public static readonly DependencyProperty ArroeValueProperty =
            DependencyProperty.Register("ArroeValue", typeof(double), typeof(DirectionPanel), new PropertyMetadata(0d, ffff));

        private static void ffff(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DirectionPanel dp = d as DirectionPanel;
            dp.arrowFRQ.Value = (double)e.NewValue;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        [Description("Уставка для авто. режима")]
        public double setValue
        {
            get { return (double)GetValue(setValueProperty); }
            set { SetValue(setValueProperty, value); }
        }
        public static readonly DependencyProperty setValueProperty =
            DependencyProperty.Register("setValue", typeof(double), typeof(DirectionPanel), new PropertyMetadata(0d));

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        public double setAmperage
        {
            get { return (double)GetValue(setAmperageProperty); }
            set { SetValue(setAmperageProperty, value); }
        }
        public static readonly DependencyProperty setAmperageProperty =
            DependencyProperty.Register("setAmperage", typeof(double), typeof(DirectionPanel), new PropertyMetadata(0d, ChangedAmperageValue));

        private static void ChangedAmperageValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DirectionPanel obj = d as DirectionPanel;

            obj.arrowAmperage.Value = (double)e.NewValue;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        [Description("Выходное значение")]
        public double outValue
        {
            get { return (double)GetValue(outValueProperty); }
            set { SetValue(outValueProperty, value); }
        }
        public static readonly DependencyProperty outValueProperty =
            DependencyProperty.Register("outValue", typeof(double), typeof(DirectionPanel), new PropertyMetadata(0d, ChangOutValue));

        private static void ChangOutValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DirectionPanel).SetOnOffWorkDriver((double)e.NewValue);
        }

        //--------------------------------------------------------------------------------------------------------------------------
        private void SetOnOffWorkDriver(double? speed = null)
        {
            bool tempWorkDriver = workDriver;
            if (speed == null) speed = outValue;

            if (FRQArrow == Visibility.Collapsed)
            {
                if (!ManualAuto.Value && OnOff) workDriver = true;
                else if (ManualAuto.Value && autoOnOff && OnOff) workDriver = true;
                else workDriver = false;
                //if (!ManualAuto.Value) PrevievOnOffinManual = tempWorkDriver;
                return;
            }

            if (speed > arrowFRQ.MaxValue) outValue = arrowFRQ.MaxValue;

            if (OnOff && (speed > 0 || FRQArrow == Visibility.Collapsed))
            {

                if (speed <= arrowFRQ.MinValue && !isMinSetWork) workDriver = false;
                else if (speed >= arrowFRQ.MinValue) workDriver = true;

                //if (!ManualAuto.Value) PrevievOnOffinManual = tempWorkDriver;
                return;
            }
            workDriver = false;

        }

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        public bool workDriver
        {
            get { return (bool)GetValue(workDriverProperty); }
            set { SetValue(workDriverProperty, value); }
        }
        public static readonly DependencyProperty workDriverProperty =
            DependencyProperty.Register("workDriver", typeof(bool), typeof(DirectionPanel), new PropertyMetadata(false, ChangedWorkDriver));

        private static void ChangedWorkDriver(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RoutedEventArgs b = new RoutedEventArgs(workDriver_Event, e.NewValue);
            (d as DirectionPanel).RaiseEvent(b);
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public void Test() { }

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        public bool Error
        {
            get { return (bool)GetValue(ErrorProperty); }
            set { SetValue(ErrorProperty, value); }
        }
        public static readonly DependencyProperty ErrorProperty =
            DependencyProperty.Register("Error", typeof(bool), typeof(DirectionPanel), new PropertyMetadata(false, changedError));//, changeOnOff));

        private static void changedError(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if ((d as DirectionPanel).ResetVisible != Visibility.Collapsed)
            //{
            (d as DirectionPanel).buttonReset.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
            if ((bool)e.NewValue) (d as DirectionPanel).OnOff = false;
            else if ((d as DirectionPanel).ManualAuto.Value) (d as DirectionPanel).OnOff = (d as DirectionPanel).autoOnOff;
            //}
        }

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        public bool autoOnOff
        {
            get { return (bool)GetValue(autoOnOffProperty); }
            set { SetValue(autoOnOffProperty, value); }
        }
        public static readonly DependencyProperty autoOnOffProperty =
            DependencyProperty.Register("autoOnOff", typeof(bool), typeof(DirectionPanel), new PropertyMetadata(false, changeautoOnOff));

        private static void changeautoOnOff(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DirectionPanel obj = d as DirectionPanel;
            if (!obj.ManualAuto.Value) return;
            if (obj.FRQArrow == Visibility.Visible) obj.SetBind_SetValueToArrowValue((bool)e.NewValue);
            obj.SetOnOffWorkDriver();
        }


        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        [Description("будет ли привязыватся уставка частоты в авто режиме")]
        public bool isArrowBinding
        {
            get { return (bool)GetValue(isArrowBindingProperty); }
            set { SetValue(isArrowBindingProperty, value); }
        }
        public static readonly DependencyProperty isArrowBindingProperty =
            DependencyProperty.Register("isArrowBinding", typeof(bool), typeof(DirectionPanel), new PropertyMetadata(ChangeisArrowBinding));

        private static void ChangeisArrowBinding(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DirectionPanel obj = d as DirectionPanel;
            obj.ManualAutoBinding(obj.ManualAuto.Value, (bool)e.NewValue);
        }

        public void SetisArrowBinding(bool setOrUnset)
        {
            isArrowBinding = setOrUnset;
            ManualAutoBinding(ManualAuto.Value, (bool)setOrUnset);
        }
        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        public bool? ManualAuto
        {
            get { return (bool?)GetValue(ManualAutoProperty); }
            set { SetValue(ManualAutoProperty, value); }
        }
        public static readonly DependencyProperty ManualAutoProperty =
            DependencyProperty.Register("ManualAuto", typeof(bool?), typeof(DirectionPanel), new PropertyMetadata(false, changeManualAuto));

        private static void changeManualAuto(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DirectionPanel dp = d as DirectionPanel;
            if ((bool?)e.NewValue == false) dp.PrevievOnOffinManual = dp.workDriver;
            dp.ManualAutoBinding(e.NewValue == null || (bool)e.NewValue == false ? false : true, dp.isArrowBinding);
            if ((bool?)e.NewValue == false) dp.arrowFRQ.useSettingValue = true;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        //привязка выхода к входу или ручной установки частоты
        //
        //в зависимости от режима работы
        private void ManualAutoBinding(bool manAuto, bool ArrowBinding)
        {
            double tempFRQ = outValue;
            SetBind_autoOnOffToOnOff(manAuto);
            if (manAuto)//авто режим
            {
                SetBind_SetValueToArrowValue(ArrowBinding);
                arrowFRQ.useSettingValue = !(bool)isArrowBinding;
                //if(!(bool)isArrowBinding && autoOnOff) SetFRQValue(tempFRQ);
            }
            else//ручной режим
            {
                SetBind_SetValueToArrowValue(false);
                arrowFRQ.useSettingValue = true;
                //SetFRQValue(tempFRQ);
            }
            SetFRQValue(tempFRQ);

            if (FRQArrow != Visibility.Collapsed) arrowFRQ.InvalidateValue();
            else SetOnOffWorkDriver();
            comboBox.SelectedIndex = manAuto ? 1 : 0;

            //сообщение о изменении режима работы
            RoutedEventArgs b = new RoutedEventArgs(ChangeManualAuto_Event, manAuto);
            RaiseEvent(b);
        }


        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        public string Title { get { return (string)LabelTitle.Content; } set { LabelTitle.Content = value; } }

        //--------------------------------------------------------------------------------------------------------------------------
        [Category("Переменные")]
        [Description("Если установленно то при минимальном значении выход будет в состоянии включения" +
            "в противном случае выключен.")]
        public bool isMinSetWork
        {
            get { return _isMinSetWork; }
            set { _isMinSetWork = value; arrowFRQ.isMinOut = value; }
        }
        bool _isMinSetWork = false;

        //-------------------------------------------------------------------------------------------------------------------------- 
        public DirectionPanel()
        {
            InitializeComponent();
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public void SetFRQValue(double v) { arrowFRQ.Value = v; }

        //--------------------------------------------------------------------------------------------------------------------------
        private void buttonHide_Click(object sender, RoutedEventArgs e)
        {
            HideVisiblePanel = false;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public bool isBindingOnOffAndAutoOnOff
        {
            get { return _isBindingOnOffAndAutoOnOff; }
            set { _isBindingOnOffAndAutoOnOff = value; SetBindingOnOffAndAutoOnOff(value); }
        }


        bool _isBindingOnOffAndAutoOnOff = false;
        //--------------------------------------------------------------------------------------------------------------------------
        private void SetBindingOnOffAndAutoOnOff(bool isBind)
        {
            bool temp = autoOnOff;
            if (!isBind)
            {
                BindingOperations.ClearBinding(this, autoOnOffProperty);
                autoOnOff = temp;
                return;
            }

            Binding bind = new Binding();
            bind.Source = this;
            bind.Path = new PropertyPath("OnOff");
            bind.Mode = BindingMode.OneWayToSource;
            SetBinding(autoOnOffProperty, bind);
            autoOnOff = temp;
        }
        //--------------------------------------------------------------------------------------------------------------------------
        private void classVirtualAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            ClearValue(ErrorProperty);
            Error = false;
            SetBind_SetValueToArrowValue(ManualAuto.Value);
            if (obj != null)
            {
                var asd = OldGrid.TranslatePoint(new Point(0, 0), obj);
                Margin = new Thickness(asd.X, asd.Y, 0, 0);
            }

        }

        //--------------------------------------------------------------------------------------------------------------------------
        //привязка авто. включеия к включению
        private void SetBind_autoOnOffToOnOff(bool bing)
        {
            bool temp = autoOnOff;
            //MultiBinding mBind = new MultiBinding();
            //mBind.Converter = multiBindingConverter;
            Binding bind = new Binding();
            if (bing)
            {
                //Binding binding = BindingOperations.GetBinding(this, autoOnOffProperty);

                BindingOperations.ClearBinding(this, autoOnOffProperty);

                bind.Source = this;
                bind.Path = new PropertyPath("OnOff");
                bind.Mode = BindingMode.OneWayToSource;

                autoOnOff = temp;

                // mBind.Bindings.Add(bind);
                //if(binding != null) mBind.Bindings.Add(binding);
                SetBinding(autoOnOffProperty, bind);
                autoOnOff = temp;
            }
            else
            {
                BindingOperations.ClearBinding(this, autoOnOffProperty);
                OnOff = autoOnOff = temp;
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------
        private void SetBind_SetValueToArrowValue(bool ManualAuto)
        {
            Binding bind = new Binding();

            //double temp = setValue;

            if (ManualAuto && OnOff && autoOnOff && isArrowBinding)
            {
                BindingOperations.ClearBinding(this, ArroeValueProperty);
                bind.Source = this;
                bind.Path = new PropertyPath("setValue");
                bind.Mode = BindingMode.OneWay;
                SetBinding(ArroeValueProperty, bind);
            }
            else
            {
                BindingOperations.ClearBinding(this, ArroeValueProperty);
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class SaveData
        {
            public double maxAmperage;
            public double maxFRQ;
            public double minFRQ;
            public int manualAuto;
            public bool OnOff;
            public double SetValue;
        }

        //--------------------------------------------------------------
        public override DataSave GetSaveData()
        {
            if (!isUseSaved) return null;

            DataSave ds = new DataSave();
            ds.NameDataId = Name;
            ds.typ = GetType();
            ds.data = DataSaveSCADA.SerializableToString<SaveData>(FillSaveData());
            return ds;
        }

        //--------------------------------------------------------------
        SaveData FillSaveData()
        {
            SaveData sd = new SaveData();
            sd.maxAmperage = maxAmperage.Value;
            sd.maxFRQ = (double)arrowFRQ.MaxValue;
            sd.minFRQ = (double)arrowFRQ.MinValue;
            sd.manualAuto = comboBox.SelectedIndex;
            sd.OnOff = OnOff;
            sd.SetValue = outValue;
            return sd;
        }

        SaveData sd;

        //--------------------------------------------------------------
        public override bool SetSaveData(DataSave data)
        {
            if (!isUseSaved) return false;
            if (data == null || data.typ != GetType() || data.NameDataId != Name) return false;

            sd = DataSaveSCADA.SerializableStringToObject<SaveData>(data.data);
            maxAmperage.Value = sd.maxAmperage;
            arrowFRQ.MaxValue = sd.maxFRQ;
            arrowFRQ.MinValue = sd.minFRQ;
            OnOff = sd.OnOff;
            comboBox.SelectedIndex = sd.manualAuto;

            arrowFRQ.Value = sd.SetValue;// 
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public void EnebleDisablePanel(bool value)
        {
            isEneblePanel = value;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ManualAuto = comboBox.SelectedIndex == 1 ? true : false;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        //сообщение о изменении - workDriver
        public static readonly RoutedEvent workDriver_Event = EventManager.RegisterRoutedEvent("ChangeWorkDriver", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(DirectionPanel));
        public event RoutedEventHandler ChangeWorkDriver
        {
            add { AddHandler(workDriver_Event, value); }
            remove { RemoveHandler(workDriver_Event, value); }
        }
        //--------------------------------------------------------------------------------------------------------------------------
        //сообщение о изменении - OnOff
        public static readonly RoutedEvent OnOff_Event = EventManager.RegisterRoutedEvent("ChangeOnOff", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(DirectionPanel));
        public event RoutedEventHandler ChangeOnOff
        {
            add { AddHandler(workDriver_Event, value); }
            remove { RemoveHandler(workDriver_Event, value); }
        }
        //--------------------------------------------------------------------------------------------------------------------------
        //сообщение о изменении - ManualAuto
        public static readonly RoutedEvent ChangeManualAuto_Event = EventManager.RegisterRoutedEvent("ChangeManualAuto", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(DirectionPanel));
        public event RoutedEventHandler ChangeManualAuto
        {
            add { AddHandler(workDriver_Event, value); }
            remove { RemoveHandler(workDriver_Event, value); }
        }
        //--------------------------------------------------------------------------------------------------------------------------
        //private void classVirtualAdd_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (!(bool)e.NewValue || obj == null) return;
        //    Point mouse = Mouse.GetPosition(obj);
        //    Margin = new Thickness(0);
        //    //TranslateTransform t = new TranslateTransform(mouse.X, mouse.Y);
        //    RenderTransform = new TranslateTransform(mouse.X, mouse.Y);
        //    //RenderTransform = new TranslateTransform(mouse.X, mouse.Y);

        //}

        //--------------------------------------------------------------------------------------------------------------------------
        public void ResetPosithion() { RenderTransform = new TranslateTransform(0, 0); }
        //--------------------------------------------------------------------------------------------------------------------------
    }



    //--------------------------------------------------------------------------------------------------------------------------
    public class ConfertorBoolTo_NOT_Bool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------
}
