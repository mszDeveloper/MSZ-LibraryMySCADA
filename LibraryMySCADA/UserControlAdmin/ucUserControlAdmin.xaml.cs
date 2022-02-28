using LibraryMySCADA.Virt;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace LibraryMySCADA.UserControlAdmin
{
    /// <summary>
    /// Логика взаимодействия для ucUserControl.xaml
    /// </summary>
    /// 
    

    public enum UserType
    {
        Гость,
        Администратор,
        Пользователь
    }

    public partial class ucUserControlAdmin : ClassVirtualAdd
    {
        DispatcherTimer TimerSetGuest = new DispatcherTimer();

        [Category("Setting")]
        [Description("Время авто. перехода на гостя. 0 - нет авто. перехода")]
        public int TimerSetToGuest { get; set; } = 1;
        //------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public UserType userType
        {
            get { return (UserType)GetValue(userTypeProperty); }
            set { SetValue(userTypeProperty, value); }
        }
        public static readonly DependencyProperty userTypeProperty =
            DependencyProperty.Register("userType", typeof(UserType), typeof(ucUserControlAdmin), new PropertyMetadata(UserType.Гость, ChamngeuserType));

        private static void ChamngeuserType(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ucUserControlAdmin ob = d as ucUserControlAdmin;
            ob.TimerSetGuest.Stop();
            if (ob.TimerSetToGuest != 0) ob.TimerSetGuest.Interval = new TimeSpan(0, ob.TimerSetToGuest, 0);

            switch ((UserType)e.NewValue)
            {
                case UserType.Гость:
                    ob.Admin = false;
                    break;
                case UserType.Администратор:
                    //if (ob.TimerSetToGuest != 0) ob.TimerSetGuest.Start();
                    ob.Admin = true;
                    break;
                case UserType.Пользователь:
                    if (ob.TimerSetToGuest != 0) ob.TimerSetGuest.Start();
                    ob.Admin = false;
                    break;
                default:
                    break;
            }
        }

        //------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public bool ShowControlUser { get { return _ShowControlUser; } set { _ShowControlUser = value; ucChangedUser.Visibility = value ? Visibility.Visible : Visibility.Collapsed; } }
        bool _ShowControlUser;

        //------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public Grid ParentGrid
        {
            get { return (Grid)GetValue(ParentGridProperty); }
            set { SetValue(ParentGridProperty, value); }
        }
        public static readonly DependencyProperty ParentGridProperty =
            DependencyProperty.Register("ParentGrid", typeof(Grid), typeof(ucUserControlAdmin), new PropertyMetadata(null));

        //------------------------------------------------------------------------------------------------------
        [Description("Текущий пользователь")]
        [Category("Setting")]
        public string NameUser
        {
            get { return (string)GetValue(NameUser_Property); }
            set { SetValue(NameUser_Property, value); }
        }
        public static readonly DependencyProperty NameUser_Property =
                   DependencyProperty.Register("NameUser", typeof(string),
                   typeof(ucUserControlAdmin), new PropertyMetadata("Default User", ChangeNameUser));

        private static void ChangeNameUser(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RoutedEventArgs b = new RoutedEventArgs(ChangeUser_Event, (string)e.NewValue);
            (d as ucUserControlAdmin).RaiseEvent(b);
        }

        //------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public bool Admin
        {
            get { return (bool)GetValue(Admin_Property); }
            set { SetValue(Admin_Property, value); }
        }
        public static readonly DependencyProperty Admin_Property =
                   DependencyProperty.Register("Admin", typeof(bool),
                   typeof(ucUserControlAdmin), new PropertyMetadata(false));
        //------------------------------------------------------------------------------------------------------
        [Description("Смещение панели выбора пользователя.")]
        [Category("Setting")]
        public Thickness offsetPanelChangUser
        {
            get { return (Thickness)GetValue(offsetPanelChangUserProperty); }
            set { SetValue(offsetPanelChangUserProperty, value); }
        }
        public static readonly DependencyProperty offsetPanelChangUserProperty =
            DependencyProperty.Register("offsetPanelChangUser", typeof(Thickness), typeof(ucUserControlAdmin), new PropertyMetadata(new Thickness(), changeOffset));

        private static void changeOffset(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucUserControlAdmin).ucChangedUser.Margin = (Thickness)e.NewValue;
        }

        //-----------------------------------------------------------------------------------------------------------------

        [Description("Масштаб панели выбора пользователя.")]
        [Category("Setting")]
        public double scalePanelChangUser
        {
            get { return (double)GetValue(scalePanelChangUserProperty); }
            set { SetValue(scalePanelChangUserProperty, value); }
        }
        public static readonly DependencyProperty scalePanelChangUserProperty =
            DependencyProperty.Register("scalePanelChangUser", typeof(double), typeof(ucUserControlAdmin), new PropertyMetadata(1d, changedScale));

        private static void changedScale(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ucUserControlAdmin).ucChangedUser.RenderTransform = new ScaleTransform((double)e.NewValue, (double)e.NewValue);
        }

        public void setGuest()
        {
            ucChangedUser.setGuest();
        }

       //-----------------------------------------------------------------------------------------------------------------
        public ucUserControlAdmin()
        {
            InitializeComponent();
            ucChangedUser.OwnerClass = this;
            
            TimerSetGuest.Tick += TimerSetGuest_Tick;
        }

        //-----------------------------------------------------------------------------------------------------------------
        private void TimerSetGuest_Tick(object sender, EventArgs e)
        {
            TimerSetGuest.Stop();
            setGuest();
        }
         
        //-----------------------------------------------------------------------------------------------------------------
        [Serializable]
        public struct SaveData
        {
            public ObservableCollection<UserData> Users;
            //public string currentUser;
        }
        //-----------------------------------------------------------------------------------------------------------------
        public override DataSave GetSaveData()
        {
            SaveData sd = new SaveData();
            sd.Users = ucChangedUser.AllUserSetting.Users;
            //sd.currentUser = NameUser;


            DataSave ds = new DataSave();
            ds.NameDataId = Name;
            ds.typ = GetType();
            ds.data = DataSaveSCADA.SerializableToString<SaveData>(sd);
            return ds;
        }

        //-----------------------------------------------------------------------------------------------------------------
        public override bool SetSaveData(DataSave data)
        {

            if (data == null) return false;
            if (data.typ != GetType() || data.NameDataId != Name) return false;
            SaveData sd;
            sd = DataSaveSCADA.SerializableStringToObject<SaveData>(data.data);
            if(sd.Users != null) ucChangedUser.AllUserSetting.Users = sd.Users;
            CheckPresentAdminAndUser();
            ucChangedUser.SetUser();


            return true;

        }

        //-----------------------------------------------------------------------------------------------------------------
        private void CheckPresentAdminAndUser()
        {
            if (ucChangedUser.AllUserSetting.GetUser("Admin") == null)
            {
                UserData ad = new UserData();
                ad.NameUser = "Admin";
                ad.Password = "1";
                ad.userType = UserType.Администратор;
                ucChangedUser.AllUserSetting.Users.Add(ad);
            }

            ucChangedUser.AllUserSetting.GetUser("Admin").userType = UserType.Администратор;
            // admin.userType = UserType.Администратор;

            if (ucChangedUser.AllUserSetting.GetAdmin("Гость") == null)
            {
                UserData ad = new UserData();
                ad.NameUser = "Гость";
                ad.Password = "";
                ad.userType = UserType.Гость;
                ucChangedUser.AllUserSetting.Users.Add(ad);
            }

        }
        //-----------------------------------------------------------------------------------------------------------------
        private void OperatorControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ParentGrid != null)
            {
                var asd = TranslatePoint(new Point(0, 0), ParentGrid);
                Grid par = Parent as Grid;
                if (par == null) return;
                par.Children.Remove(this);
                ParentGrid.Children.Add(this);
                //var asd = UcStathion.TranslatePoint(new Point(0, 0), MainWindow.win);
                Margin = new Thickness(asd.X, asd.Y, 0, 0);

            }
            CheckPresentAdminAndUser();

        }

        //-----------------------------------------------------------------------------------------------------------------
        private void button_ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            ucChangedUser.Visibility = Visibility.Visible;
        }

        //-----------------------------------------------------------------------------------------------------------------
        public static readonly RoutedEvent ChangeUser_Event = EventManager.RegisterRoutedEvent("ChangeUser", RoutingStrategy.Bubble, typeof(DependencyPropertyChangedEventHandler), typeof(ucUserControlAdmin));

        public event RoutedEventHandler User_Changed
        {
            add { AddHandler(ChangeUser_Event, value); }
            remove { RemoveHandler(ChangeUser_Event, value); }
        }

    }

    //**************************************************************************************************************************
    public class ConvertUserTypeToNullBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object isNulable, CultureInfo culture)
        {
            UserType userType = (UserType)value;
            string param = (string)isNulable;
            switch (userType)
            {
                case UserType.Администратор: return true;
                case UserType.Гость: if (param == "true") return null; else return false;
                case UserType.Пользователь: return false;
                default: return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    //**************************************************************************************************************************
    public class ConvertUserTypeAdminToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object isNulable, CultureInfo culture)
        {
            UserType userType = (UserType)value;
            switch (userType)
            {
                case UserType.Администратор: return Visibility.Visible;
                case UserType.Гость: return Visibility.Collapsed;
                case UserType.Пользователь: return Visibility.Collapsed;
                default: return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    //**************************************************************************************************************************
    public class ConvertUserTypeUserToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object isNulable, CultureInfo culture)
        {
            UserType userType = (UserType)value;
            switch (userType)
            {
                case UserType.Администратор: return Visibility.Visible;
                case UserType.Гость: return Visibility.Collapsed;
                case UserType.Пользователь: return Visibility.Visible;
                default: return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    //**************************************************************************************************************************
    public class ConvertUserTypeAdminToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object isNulable, CultureInfo culture)
        {
            UserType userType = (UserType)value;
            switch (userType)
            {
                case UserType.Администратор: return true;
                case UserType.Гость: return false;
                case UserType.Пользователь: return false;
                default: return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    //**************************************************************************************************************************
    public class ConvertUserTypeUserToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object isNulable, CultureInfo culture)
        {
            UserType userType = (UserType)value;
            switch (userType)
            {
                case UserType.Администратор: return true;
                case UserType.Гость: return false;
                case UserType.Пользователь: return true;
                default: return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    //**************************************************************************************************************************
}
