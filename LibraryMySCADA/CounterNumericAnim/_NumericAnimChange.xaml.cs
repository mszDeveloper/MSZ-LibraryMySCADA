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

namespace LibraryMySCADA.CounterNumericAnim
{
     public partial class _NumericAnimChange : UserControl
    {
        [Category("Кисть")]
        public Brush backColorNumeric
        {
            get { return (Brush)GetValue(backColorNumericProperty); }
            set { SetValue(backColorNumericProperty, value); }
        }
        public static readonly DependencyProperty backColorNumericProperty =
            DependencyProperty.Register("backColorNumeric", typeof(Brush), typeof(_NumericAnimChange), new PropertyMetadata(null));

        //-------------------------------------------------------------------------------------------------------------------
        [Category("Макет")]
        public int BorderRadius
        {
            get { return (int)GetValue(BorderRadiusProperty); }
            set { SetValue(BorderRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderRadiusProperty =
            DependencyProperty.Register("BorderRadius", typeof(int), typeof(_NumericAnimChange), new PropertyMetadata(4));

        //-------------------------------------------------------------------------------------------------------------------
        [Category("Макет")]
        public Thickness NumericBorderThickness
        {
            get { return (Thickness)GetValue(NumericBorderThicknessProperty); }
            set { SetValue(NumericBorderThicknessProperty, value); }
        }
        public static readonly DependencyProperty NumericBorderThicknessProperty =
            DependencyProperty.Register("NumericBorderThickness", typeof(Thickness), typeof(_NumericAnimChange), new PropertyMetadata(new Thickness(1,1,1,1)));

        //-------------------------------------------------------------------------------------------------------------------
        [Category("Макет")]
        public Thickness textThickness
        {
            get { return (Thickness)GetValue(textThicknessProperty); }
            set { SetValue(textThicknessProperty, value); }
        }
        public static readonly DependencyProperty textThicknessProperty =
            DependencyProperty.Register("textThickness", typeof(Thickness), typeof(_NumericAnimChange), new PropertyMetadata(new Thickness(0,0,0,0)));


        //-------------------------------------------------------------------------------------------------------------------
        public Char newChar
        {
            get { return (Char)GetValue(newCharProperty); }
            set { SetValue(newCharProperty, value); }
        }
        public static readonly DependencyProperty newCharProperty =
            DependencyProperty.Register("newChar", typeof(Char), typeof(_NumericAnimChange), new PropertyMetadata('0', changeeNewChar));

        private static void changeeNewChar(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            _NumericAnimChange obj = d as _NumericAnimChange;
            obj.ChangeChar.Storyboard.Begin(obj);
        }

        //-------------------------------------------------------------------------------------------------------------------
        public Char oldChar
        {
            get { return (Char)GetValue(oldCharProperty); }
            set { SetValue(oldCharProperty, value); }
        }
        public static readonly DependencyProperty oldCharProperty =
            DependencyProperty.Register("oldChar", typeof(Char), typeof(_NumericAnimChange), new PropertyMetadata('0'));

        //-------------------------------------------------------------------------------------------------------------------

        public _NumericAnimChange()
        {
            InitializeComponent();
            ChangeChar.Storyboard.Completed += Storyboard_Completed;
        }

        //-------------------------------------------------------------------------------------------------------------------
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            oldChar = newChar;
            BaseAnim.Storyboard.Begin(this);
        }
        //-------------------------------------------------------------------------------------------------------------------
    }
}
