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

namespace wpfLibMszControl
{
    /// <summary>
    /// Логика взаимодействия для UserControl_line.xaml
    /// </summary>
    public partial class UserControl_line : UserControl
    {

        //----------------------------------------------------------------------------------------------------------
        [Description("Подпись линии")]
        [Category("Setting")]
        public string textLine
        {
            get { return (string)GetValue(textLine_Property); }
            set { SetValue(textLine_Property, value); }
        }

        public static readonly DependencyProperty textLine_Property =
                   DependencyProperty.Register("textLine", typeof(string),
                   typeof(UserControl_line), new PropertyMetadata("00"));

        public UserControl_line()
        {
            InitializeComponent();
        }
    }
}
