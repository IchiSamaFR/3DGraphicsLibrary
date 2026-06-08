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

namespace Object3DView
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
            _object3D.Position = new Vector3(0, 0, 5);

            _timer = new DispatcherTimer { Interval = _dt };
            _timer.Tick += Timer_Tick;

            Loaded += DrawElement_Loaded;
            Unloaded += DrawElement_Unloaded;
        }

        private void DrawElement_Loaded(object sender, RoutedEventArgs e) => _timer.Start();
        private void DrawElement_Unloaded(object sender, RoutedEventArgs e) => _timer.Stop();

        private void Timer_Tick(object sender, EventArgs e)
        {
            _object3D.Rotation = _object3D.Rotation + new Vector3(0, (float)_dt.TotalSeconds, 0);
            //Z += (float)_dt.TotalSeconds;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext context)
        {
            var brush = new SolidColorBrush(Colors.LightBlue);
            context.DrawObject3D(_object3D, brush, (float)ActualWidth, (float)ActualHeight);
        }
    }
}