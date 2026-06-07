using Object3DLibrary;
using Object3DLibrary.Entites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Timers;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace _3DGraphicsLibrary
{
    public partial class DrawElement : FrameworkElement
    {
        public float X
        {
            get => (float)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public float Y
        {
            get => (float)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        public float Z
        {
            get => (float)GetValue(ZProperty);
            set => SetValue(ZProperty, value);
        }

        private Vector3 Position => new Vector3(X, Y, Z);

        private Object3D _object3D;
        private float _angle = 0;
        private readonly TimeSpan _dt = TimeSpan.FromSeconds(1.0 / 60.0);
        private readonly DispatcherTimer _timer;

        public DrawElement()
        {
            _object3D = ObjectParser.Parse(File.ReadAllText("Resources/cube.obj"));

            _timer = new DispatcherTimer { Interval = _dt };
            _timer.Tick += Timer_Tick;

            Loaded += DrawElement_Loaded;
            Unloaded += DrawElement_Unloaded;
        }

        private void DrawElement_Loaded(object sender, RoutedEventArgs e) => _timer.Start();
        private void DrawElement_Unloaded(object sender, RoutedEventArgs e) => _timer.Stop();

        private void Timer_Tick(object sender, EventArgs e)
        {
            _angle += (float)_dt.TotalSeconds;
            //Z += (float)_dt.TotalSeconds;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext context)
        {
            var brush = new SolidColorBrush(Colors.LightBlue);

            foreach (var point in _object3D.Vertices)
            {
                //var rotatedPoint = RotateY(point, _angle);
                //context.DrawRectangle(brush, null, GetRect(Position + rotatedPoint));
            }

            foreach (var faces in _object3D.Faces)
            {
                for (int i = 0; i < faces.Length; i++)
                {
                    var point = ToScreen(Get2DPoint(Position + RotateY(_object3D.Vertices[faces[i].VertexIndex], _angle)));
                    var nextPoint = ToScreen(Get2DPoint(Position + RotateY(_object3D.Vertices[faces[(i + 1) % faces.Length].VertexIndex], _angle)));
                    context.DrawLine(new Pen(brush, 1),
                        new Point(X + point.X, Y + point.Y),
                        new Point(X + nextPoint.X, Y + nextPoint.Y));
                }
            }
        }

        private Vector3 RotateY(Vector3 point, float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new Vector3(
                point.X * cos - point.Z * sin,
                point.Y,
                point.X * sin + point.Z * cos);
        }

        private Rect GetRect(Vector3 point)
        {
            if (point.Z <= 0)
            {
                return new Rect();
            }

            int size = 10;
            var screenPoint = ToScreen(Get2DPoint(point));
            return new Rect(new Point(screenPoint.X - size / 2, screenPoint.Y - size / 2), new Point(screenPoint.X + size / 2, screenPoint.Y + size / 2));
        }
        private Vector2 ToScreen(Vector2 point)
        {
            return new Vector2(
                (point.X + 1) / 2 * (float)ActualWidth,
                (1 - (point.Y + 1) / 2) * (float)ActualHeight);
        }

        private Vector2 Get2DPoint(Vector3 point)
        {
            return new Vector2(point.X / point.Z, point.Y / point.Z);
        }
    }
}