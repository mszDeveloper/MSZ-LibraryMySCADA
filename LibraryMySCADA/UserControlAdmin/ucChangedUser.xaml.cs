using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LibraryMySCADA.UserControlAdmin
{
    /// <summary>
    /// Логика взаимодействия для ucChangedUser.xaml
    /// </summary>
    public partial class ucChangedUser : UserControl
    {
        public ucUserControlAdmin OwnerClass;

        //--------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public ImageSource AvatarSource
        {
            get { return (ImageSource)GetValue(AvatarSourceProperty); }
            set { SetValue(AvatarSourceProperty, value); }
        }
        public static readonly DependencyProperty AvatarSourceProperty =
            DependencyProperty.Register("AvatarSource", typeof(ImageSource), typeof(ucChangedUser), new PropertyMetadata(new BitmapImage(new Uri(@"Avatars/avatar2.png", UriKind.RelativeOrAbsolute))));

        //--------------------------------------------------------------------------------------------------------------
        public ImageSource DefaultAvatar = new BitmapImage(new Uri(@"Avatars/avatar2.png", UriKind.RelativeOrAbsolute));
        public ImageSource AdminAvatar = new BitmapImage(new Uri(@"Avatars/avatar Admin.png", UriKind.RelativeOrAbsolute));
        public ImageSource GuestAvatar = new BitmapImage(new Uri(@"Avatars/GuestAvatar.png", UriKind.RelativeOrAbsolute));

        //--------------------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------
        [Category("Setting")]
        public string NameUser
        {
            get { return (string)GetValue(NameUser_Property); }
            set { SetValue(NameUser_Property, value); }
        }
        public static readonly DependencyProperty NameUser_Property =
                   DependencyProperty.Register("NameUser", typeof(string),
                   typeof(ucChangedUser), new PropertyMetadata("Default User",changeUser));

        private static void changeUser(DependencyObject d, DependencyPropertyChangedEventArgs e)//
        {
            ucChangedUser obj = d as ucChangedUser;
            try
            {
                if (obj.AllUserSetting.GetUser((string)e.NewValue) == null) return;
                obj.Admin = (bool)obj.AllUserSetting.GetAdmin((string)e.NewValue);
                if ((string)e.NewValue == "Admin") { obj.AvatarSource = obj.AdminAvatar; return; }
                if ((string)e.NewValue == "Гость") { obj.AvatarSource = obj.GuestAvatar; return; }
                string PathToAvatar = obj.AllUserSetting.GetAvatarPath((string)e.NewValue);
                if (PathToAvatar != null && PathToAvatar != "")
                {
                    obj.AvatarSource = new BitmapImage(new Uri(@obj.AllUserSetting.GetAvatarPath((string)e.NewValue), UriKind.RelativeOrAbsolute));
                    
                }
                else
                {
                    obj.AvatarSource = obj.DefaultAvatar;
                }

            }
            catch { }

            
        }

        //------------------------------------------------------------------------------------------------------
        [Description("Admin ?")]
        [Category("Setting")]
        public bool Admin
        {
            get { return (bool)GetValue(Admin_Property); }
            set { SetValue(Admin_Property, value); }
        }
        public static readonly DependencyProperty Admin_Property =
                   DependencyProperty.Register("Admin", typeof(bool),
                   typeof(ucChangedUser), new PropertyMetadata(false));
        //------------------------------------------------------------------------------------------------------

        public ucChangedUser()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------------------------------
        public void SetUser(string User = "Гость")
        {
            NameUser = "Гость";
        }

        //------------------------------------------------------------------------------------------------------
        public void setGuest()
        {
            NameUser = "Гость";
            Admin = false;
            OwnerClass.userType = UserType.Гость;

        }

        //------------------------------------------------------------------------------------------------------
        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            if(comboBox_users.Text == "Гость")
            {
                NameUser = "Гость";
                //Admin = false;
                OwnerClass.userType = UserType.Гость;
                Visibility = Visibility.Collapsed;
                return;
            }

            foreach (var item in AllUserSetting.Users)
            {
                
                if (item.NameUser == comboBox_users.Text)
                    if (passwordBox.Password == item.Password)
                    {
                        NameUser = item.NameUser;
                        OwnerClass.userType = item.userType;
                        Visibility = Visibility.Collapsed;
                    }
                    else passwordBox.Background = Brushes.Red;
            }
            passwordBox.Password = null;
        }

        //------------------------------------------------------------------------------------------------------
        public bool ChangePassword(string oldPassword, string newPassword)
        {
            foreach (var item in AllUserSetting.Users)
            {

                if (item.NameUser == comboBox_users.Text)
                    if (oldPassword == item.Password)
                    {
                        item.Password = newPassword;
                        MessageBox.Show("Пароль изменен", "Смена пароля", MessageBoxButton.OK);
                        return true;
                    }

                    else
                    {
                        MessageBox.Show("Неверный старый пароль!", "Смена пароля", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
            }
            return false;
        }
        
        //------------------------------------------------------------------------------------------------------
        private void passwordBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            passwordBox.Background = Brushes.Silver;
        }

        //------------------------------------------------------------------------------------------------------
        private void comboBox_users_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            BorderChangePassword.Visibility = Visibility.Collapsed;
            comboBox_users.Items.Clear();
            foreach (var item in AllUserSetting.Users)
            {
                comboBox_users.Items.Add(item.NameUser);
            }
            comboBox_users.SelectedIndex = 0;
            
        }

        //------------------------------------------------------------------------------------------------------
        private void comboBox_users_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            passwordBox.Password = null;
        }

        //------------------------------------------------------------------------------------------------------
        private void buttonAvatar_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openDialog = new System.Windows.Forms.OpenFileDialog();
            openDialog.DefaultExt = "png";
            openDialog.Filter = " Image Files | *.PNG;*.JPG ";
            openDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory +"\\Avatars"; ;
            openDialog.ShowDialog();
            AllUserSetting.SetUserAvatar(NameUser, openDialog.FileName);
            string _NameUser = NameUser;
            NameUser = "Гость";
            NameUser = _NameUser;
            //OwnerClass.ImageAvatar.Source = new BitmapImage(new Uri(openDialog.FileName, UriKind.RelativeOrAbsolute));
        }

        //------------------------------------------------------------------------------------------------------
        private void buttonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            BorderChangePassword.Visibility = Visibility.Visible;
        }

        //------------------------------------------------------------------------------------------------------
        private void button_closeChangePassword_Click(object sender, RoutedEventArgs e)
        {
            BorderChangePassword.Visibility = Visibility.Collapsed;
        }

        //------------------------------------------------------------------------------------------------------
        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
            BorderChangePassword.Visibility = Visibility.Collapsed;
        }

        //------------------------------------------------------------------------------------------------------
        private void BorderChangeUser_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BorderChangePassword.Visibility = Visibility.Collapsed;
        }

        //------------------------------------------------------------------------------------------------------
        private void button_OKChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if(NewPassword.Password == NewPassword2.Password)
            {
                ChangePassword(OldPassword.Password, NewPassword.Password);
                //    MessageBox.Show("Пароль изменен", "Смена пароля", MessageBoxButton.OK);
                //else
                //    MessageBox.Show("Ошибка!", "Смена пароля", MessageBoxButton.OK, MessageBoxImage.Warning);
                //return;
            }
            else MessageBox.Show("Несовпадение паролей", "Смена пароля", MessageBoxButton.OK, MessageBoxImage.Warning);
            BorderChangePassword.Visibility = Visibility.Collapsed;
        }

        //------------------------------------------------------------------------------------------------------
    }

    //------------------------------------------------------------------------------------------------------
    public class ConverterUserToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UserType typeUser = (UserType)value;
            if (typeUser == UserType.Гость) return Visibility.Hidden;
            else return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    //------------------------------------------------------------------------------------------------------
}
