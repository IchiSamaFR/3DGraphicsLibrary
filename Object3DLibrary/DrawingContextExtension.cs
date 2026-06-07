using Object3DLibrary.Entites;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace Object3DLibrary;

public static class DrawingContextExtension
{
    public static void DrawObject3D(this DrawingContext dc, Object3D obj, Brush brush)
    {
        var transformedVertices = new List<Vector3>();
        foreach (var vertex in obj.Vertices)
        {
            //var rotatedVertex = RotateVertex(vertex + obj.Position - cameraPosition, cameraRotation);
            //transformedVertices.Add(rotatedVertex);
        }
        foreach (var face in obj.Faces)
        {
            var points = new List<Point>();
            foreach (var faceVertex in face)
            {
                var point = ToScreen(Get2DPoint(RotateY(obj.Vertices[faceVertex.VertexIndex], obj.Rotation.Y)));
                var nextPoint = ToScreen(Get2DPoint(RotateY(obj.Vertices[face[(Array.IndexOf(face, faceVertex) + 1) % face.Length].VertexIndex], obj.Rotation.Y)));
                dc.DrawLine(new Pen(brush, 1),
                    new Point(point.X, point.Y),
                    new Point(nextPoint.X, nextPoint.Y));
            }
            if (points.Count >= 2)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    dc.DrawLine(new Pen(Brushes.White, 1), points[i], points[(i + 1) % points.Count]);
                }
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

    private static Rect GetRect(Vector3 point)
    {
        if (point.Z <= 0)
        {
            return new Rect();
        }

        int size = 10;
        var screenPoint = ToScreen(Get2DPoint(point));
        return new Rect(new Point(screenPoint.X - size / 2, screenPoint.Y - size / 2), new Point(screenPoint.X + size / 2, screenPoint.Y + size / 2));
    }
    private static Vector2 ToScreen(Vector2 point)
    {
        return new Vector2(
            (point.X + 1) / 2,
            (1 - (point.Y + 1) / 2));
    }

    private static Vector2 Get2DPoint(Vector3 point)
    {
        return new Vector2(point.X / point.Z, point.Y / point.Z);
    }
}
