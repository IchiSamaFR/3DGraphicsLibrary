using Object3DLibrary.Entites;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace Object3DLibrary;

public static class DrawingContextExtension
{
    public static void DrawObject3D(this DrawingContext dc, Object3D obj, Brush brush, float width, float height)
    {
        var transformedVertices = new List<Vector3>();
        foreach (var face in obj.Faces)
        {
            var points = new List<Point>();
            foreach (var faceVertex in face)
            {
                var point = ToScreen(Get2DPoint(obj.Position + RotateY(obj.Vertices[faceVertex.VertexIndex], obj.Rotation.Y)), width, height);
                var nextPoint = ToScreen(Get2DPoint(obj.Position + RotateY(obj.Vertices[face[(Array.IndexOf(face, faceVertex) + 1) % face.Length].VertexIndex], obj.Rotation.Y)), width, height);
                dc.DrawLine(new Pen(brush, 1),
                    new Point(point.X, point.Y),
                    new Point(nextPoint.X, nextPoint.Y));
            }
        }
    }

    private static Vector3 RotateY(Vector3 point, float angle)
    {
        float cos = (float)Math.Cos(angle);
        float sin = (float)Math.Sin(angle);
        return new Vector3(
            point.X * cos - point.Z * sin,
            point.Y,
            point.X * sin + point.Z * cos);
    }

    private static Vector2 ToScreen(Vector2 point, float width, float height)
    {
        return new Vector2(
            (point.X + 1) / 2 * width,
            (1 - (point.Y + 1) / 2) * height);
    }

    private static Vector2 Get2DPoint(Vector3 point)
    {
        return new Vector2(point.X / point.Z, point.Y / point.Z);
    }
}
