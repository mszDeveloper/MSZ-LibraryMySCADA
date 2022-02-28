using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LibraryMySCADA.PathAnimation
{
    public enum ViewObject
    {
        Circle,
        Arrow,
        ExternalPath
    }

    public class dataGeometryes
    {
        internal Canvas canvas;
        internal bool isStarted = false;

        [Description("геометрия пути перемещения")]
        public string pathGeom { get ;  set ; }

        [Description("Цвет")]
        public Brush brushMovedItem { get ;  set ;} 

        [Description("Пользовательская фигура перемещения")]
        public StreamGeometry Figure { get; set; }

        [Description("Имя анимации пути")]
        public string NamePath { get ;  set; }

        [Description("Количество обьектов")]
        public int coutObj { get; set; }

        [Description("Время перемещения sec")]
        public int timeMove { get; set; }

        [Description("Масштаб перемещаемой фигуры")]
        public double scale { get; set; }

        [Description("Revers")]
        public bool isRevers { get;  set; }

        [Description("Вид перемещаемой фигуры")]
        public ViewObject ViewObg { get ; set ; }

        [Description("Цэнтрирование фигуры")]
        public double offset { get; set; }

        public Storyboard pathAnimationStoryboard;

        [Description("Автозапуск")]
        public bool preStartAnim { get; set; }
    }
    //--------------------------------------------------------------------------------------------------------
    public partial class ucPathAnimation : UserControl
    {
        bool isInitialize = false;


        [Category("Setting")]
        public bool preStartAnim { get; set; } = true;

       // [Category("Setting")]
        List<dataGeometryes> _datas = new List<dataGeometryes>();

        [Category("Setting")]
        public List<dataGeometryes> datas { get { return _datas; } set { _datas = value; InitializedAnimation(); } }
   
        //--------------------------------------------------------------------------------------------------------
        public ucPathAnimation()
        {
            InitializeComponent();
           // var d = datas[0];
            //InitializedAnimation();
        }

        private void chKol(object sender, NotifyCollectionChangedEventArgs e)
        {
            
        }

        //--------------------------------------------------------------------------------------------------------
        private void OnOffAnim(string anim, bool onOff)
        {
        }

        //--------------------------------------------------------------------------------------------------------
        private Geometry GetGeometryMovedFogure(ViewObject vo)
        {
            switch (vo)
            {
                case ViewObject.Circle:
                    return (FindResource("GeometryCircle") as StreamGeometry).Clone();

                case ViewObject.Arrow:
                    return (FindResource("GeometryArrow") as StreamGeometry).Clone();
            }
            return null;
        }

        //--------------------------------------------------------------------------------------------------------
        public bool VisiblePanel
        {
            get { return (bool)GetValue(VisiblePanelProperty); }
            set { SetValue(VisiblePanelProperty, value); }
        }

        public static readonly DependencyProperty VisiblePanelProperty =
            DependencyProperty.Register("VisiblePanel", typeof(bool), typeof(ucPathAnimation), new PropertyMetadata(false));

        //--------------------------------------------------------------------------------------------------------
        public void InitializedAnimation()
        {
            if (isInitialize) return;
            isInitialize = true;
            VerticalAlignment = VerticalAlignment.Top;
            HorizontalAlignment = HorizontalAlignment.Left;
            Margin = new Thickness(0, 0, 0, 0);
            rootCanvas.HorizontalAlignment = HorizontalAlignment.Left;
            rootCanvas.VerticalAlignment = VerticalAlignment.Top;

            Width = borderInfo.Width;
            Height = borderInfo.Height;

            foreach (dataGeometryes itemDG in datas)
            {
                try
                {
                    itemDG.pathAnimationStoryboard = new Storyboard();
                    itemDG.canvas = new Canvas();
                    rootCanvas.Children.Add(itemDG.canvas);
                    for (int i = 0; i < itemDG.coutObj; i++)
                    {
                        MatrixTransform canvasMatrixTransform = new MatrixTransform();

                        Path geometryMovedFigure = new Path();
                        
                        itemDG.canvas.Children.Add(geometryMovedFigure);
                        geometryMovedFigure.Fill = itemDG.brushMovedItem;
                        geometryMovedFigure.Data = GetGeometryMovedFogure(itemDG.ViewObg);

                        geometryMovedFigure.Stroke = Brushes.Black;

                        TransformGroup tgData = new TransformGroup();
                        tgData.Children.Add(new ScaleTransform(itemDG.scale, itemDG.scale));
                        tgData.Children.Add(new TranslateTransform(itemDG.offset, itemDG.offset));
                        geometryMovedFigure.Data.Transform = tgData;

                        TransformGroup tg = new TransformGroup();
                        tg.Children.Add(canvasMatrixTransform);
                        geometryMovedFigure.RenderTransform = tg;

                        Geometry pg = Geometry.Parse(itemDG.pathGeom);
                        if (FindName(itemDG.NamePath + i.ToString()) != null) UnregisterName(itemDG.NamePath + i.ToString());
                        RegisterName(itemDG.NamePath + i.ToString(), canvasMatrixTransform);
                        MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath();
                        matrixAnimation.PathGeometry = new PathGeometry();
                        matrixAnimation.PathGeometry.AddGeometry(pg);

                        matrixAnimation.Duration = new TimeSpan(0,0, itemDG.timeMove );
                        matrixAnimation.RepeatBehavior = RepeatBehavior.Forever;
                        matrixAnimation.AutoReverse = itemDG.isRevers;
                        matrixAnimation.DoesRotateWithTangent = true;

                        matrixAnimation.BeginTime = new TimeSpan((new TimeSpan(0, 0, itemDG.timeMove).Ticks / itemDG.coutObj) * i * -1 );
                        Storyboard.SetTargetName(matrixAnimation, itemDG.NamePath + i.ToString());

                        Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));
                        itemDG.pathAnimationStoryboard.Children.Add(matrixAnimation);
                        matrixAnimation.FillBehavior = FillBehavior.Stop;
                        matrixAnimation.Freeze();
                    }
                    itemDG.canvas.Visibility = Visibility.Collapsed;
                }
                catch(Exception e) { MessageBox.Show(e.ToString()); }
            }
        }

        //--------------------------------------------------------------------------------------------------------
        private void PathAnimationClass_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            //InitializedAnimation();
            //foreach (var item in datas)
            //{
            //    comboboxAnimations.Items.Add(item.NamePath);
            //}
            //comboboxAnimations.SelectedIndex = 0;
        }

        //--------------------------------------------------------------------------------------------------------
        private void buttonStopAnim_Click(object sender, RoutedEventArgs e)
        {
            //datas[comboboxAnimations.SelectedIndex].StopAnim(rootCanvas);
        }

        //--------------------------------------------------------------------------------------------------------
        private void buttonStartAnim_Click(object sender, RoutedEventArgs e)
        {
            //datas[comboboxAnimations.SelectedIndex].StartAnim(rootCanvas);
        }

        //--------------------------------------------------------------------------------------------------------
        private void numericOffset_Value_Changed(object sender, RoutedEventArgs e)
        {
            //datas[comboboxAnimations.SelectedIndex].offset = numericOffset.Value;
            //restart();
        }

        //--------------------------------------------------------------------------------------------------------

        private void numberSpeed_Value_Changed(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    //datas[comboboxAnimations.SelectedIndex].timeMove = new TimeSpan(0, 0, (int)numberSpeed.Value);
            //    //restart();
            //}
            //catch { }
        }

        //--------------------------------------------------------------------------------------------------------
        private void restart()
        {
            //foreach (var item in datas)
            //{
            //    //item.StopAnim(rootCanvas);
            //    //item.pathAnimationStoryboard.Remove(rootCanvas);
            //}
            //rootCanvas.Children.Clear();
            //InitializedAnimation();
        }

        //--------------------------------------------------------------------------------------------------------
        private void comboboxAnimations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //numericOffset.Value = datas[comboboxAnimations.SelectedIndex].offset;
           // numberSpeed.Value = datas[comboboxAnimations.SelectedIndex].timeMove.Seconds;
        }
        //--------------------------------------------------------------------------------------------------------
        public void StartAnim(string anim)
        {
            if (!isInitialize) InitializedAnimation();
            foreach (var item in datas)
            {
                if (item.NamePath == anim)
                {
                    if (item.isStarted)  return;
                    item.isStarted = true;
                    item.canvas.Visibility = Visibility.Visible;
                    item.pathAnimationStoryboard.Begin(item.canvas, true);
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public void StopAnim(string anim)
        {
            if (!isInitialize) InitializedAnimation();
            foreach (var item in datas)
            {
                if (item.NamePath == anim)
                {
                    if (!item.isStarted)  return;
                    item.isStarted = false;
                    item.canvas.Visibility = Visibility.Collapsed;
                    item.pathAnimationStoryboard.Stop(item.canvas);
                    //item.pathAnimationStoryboard.SkipToFill (item.canvas);
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public void PauseAnim(string anim)
        {
            foreach (var item in datas)
            {
                if (item.NamePath == anim) item.pathAnimationStoryboard.Pause(rootCanvas);
            }
        }

        public void ResumeAnim(string anim)
        {
            foreach (var item in datas)
            {
                if (item.NamePath == anim) item.pathAnimationStoryboard.Resume(rootCanvas);
            }
        }


    }
}
