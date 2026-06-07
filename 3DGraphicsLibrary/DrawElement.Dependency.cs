using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _3DGraphicsLibrary
{
    public partial class DrawElement
    {
        public static readonly DependencyProperty ZProperty = DependencyProperty.Register(
            nameof(Z),
            typeof(float),
            typeof(DrawElement),
            new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            nameof(X),
            typeof(float),
            typeof(DrawElement),
            new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            nameof(Y),
            typeof(float),
            typeof(DrawElement),
            new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsRender));

    }
}
