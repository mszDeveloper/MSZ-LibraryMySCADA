using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LibraryMySCADA.PathAnimation
{
    public class dataGeometryes : INotifyPropertyChanged// DependencyObject
    {
        private void OnChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //---------------------------------------------------------------
        [Description("геометрия пути перемещения")]
        public string pathGeom { get { return _pathGeom; } set { _pathGeom = value;OnChanged(); } }
        string _pathGeom;
        ////---------------------------------------------------------------
        [Description("Цвет")]
        public Brush brushMovedItem { get { return _brushMovedItem; } set { _brushMovedItem = value; OnChanged(); } }
        Brush _brushMovedItem = Brushes.Blue;
        //--------------------------------------------------------------------------------------------------------
        [Description("Пользовательская фигура перемещения")]
        public StreamGeometry Figure { get { return _Figure; } set { _Figure = value; OnChanged(); } }
        StreamGeometry _Figure;
        //---------------------------------------------------------------
        [Description("Имя анимации пути")]
        public string NamePath { get { return _NamePath; } set { _NamePath = value; OnChanged(); } }
        string _NamePath = "Default";
        //---------------------------------------------------------------
        [Description("Количество обьектов")]
        public int coutObj { get { return _coutObj; } set { _coutObj = value; OnChanged(); } }
        int _coutObj = 4;
        //---------------------------------------------------------------
        [Description("Время перемещения")]
        public TimeSpan timeMove { get { return _timeMove; } set { _timeMove = value; OnChanged(); } }
        TimeSpan _timeMove;
        //---------------------------------------------------------------
        [Description("Масштаб перемещаемой фигуры")]
        public double scale { get { return _scale; } set { _scale = value; OnChanged(); } }
        double _scale;
        //---------------------------------------------------------------
        [Description("Revers")]
        public bool isRevers { get { return _isRevers; } set { _isRevers = value; OnChanged(); } }
        bool _isRevers;
        //---------------------------------------------------------------
        [Description("Вид перемещаемой фигуры")]
        public ViewObject ViewObg { get { return _ViewObg; } set { _ViewObg = value; OnChanged(); } }
        ViewObject _ViewObg;
       //---------------------------------------------------------------
       [Description("Цэнтрирование фигуры")]
        public double offset { get; set; } = -5;
        //---------------------------------------------------------------
        //internal Storyboard pathAnimationStoryboard { get { return _pathAnimationStoryboard; } set { _pathAnimationStoryboard = value;OnChanged(); } }// = new Storyboard();
        //Storyboard _pathAnimationStoryboard = new Storyboard();

        public event PropertyChangedEventHandler PropertyChanged;

        //--------------------------------------------------------
        //internal void StartAnim(Canvas canvas)
        //{
        //    //pathAnimationStoryboard.Resume(canvas);
        //}

        //internal void StopAnim(Canvas rootCanvas)
        //{
        //   // pathAnimationStoryboard.Pause(rootCanvas);
        //}
        //--------------------------------------------------------
    }
}
