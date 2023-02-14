using RapiD.Geometry.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace RapiD.Geometry.Views
{
    /// <summary>
    /// Interaction logic for Canvas2DView.xaml
    /// </summary>
    public partial class Canvas2DView : UserControl
    {
        public Canvas2DView()
        {
            InitializeComponent();

            scaleTransform = new ScaleTransform();
            canvas.LayoutTransform = scaleTransform;
        }

        GeometryBase selectedGeometry;
        Double offSetX;
        Double offSetY;

        private ScaleTransform scaleTransform;

        private void Canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var canvas = sender as Canvas;

            var point = e.GetPosition(canvas);

            var fe = canvas.InputHitTest(point) as FrameworkElement;

            selectedGeometry = fe.DataContext as GeometryBase;

            if (selectedGeometry != null)
            {
                offSetX = point.X - selectedGeometry.Position.X;
                offSetY = point.Y - selectedGeometry.Position.Y;
                //originalColor = selectedShape.ObjectColor;
            }


        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {

            if (selectedGeometry != null&&candrag)
            {
                var canvas = sender as Canvas;
                var point = e.GetPosition(canvas);
                var newpos = new Point(point.X - offSetX, point.Y - offSetY);

                selectedGeometry.Position = newpos;
                if (selectedGeometry.IsSelected == false)
                    selectedGeometry = null;


            }

        }

        bool candrag=false;
        private void canvas_KeyDown(object sender, KeyEventArgs e)
        {
           if( e.Key==Key.LeftCtrl||e.Key==Key.RightCtrl)
                candrag = true;
        }

        private void canvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                candrag = false;
        }

   
    }
}
