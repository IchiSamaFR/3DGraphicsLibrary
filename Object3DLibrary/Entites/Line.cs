using System.Numerics;
using System.Windows;

namespace Object3DLibrary.Entites
{
    internal class ObjectLine
    {
        public Vector2 Start { get; }
        public Vector2 End { get; }

        public ObjectLine(Vector2 pointA, Vector2 pointB)
        {
            Start = pointA;
            End = pointB;
        }
    }
}
