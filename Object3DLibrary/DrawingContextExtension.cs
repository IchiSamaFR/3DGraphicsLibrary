using Object3DLibrary.Entites;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace Object3DLibrary;

public static class DrawingContextExtension
{
    public static void DrawObject3D(this DrawingContext dc, Object3D obj, Camera camera, Brush brush)
    {
        var transformedVertices = new List<Vector3>();
        foreach (var face in obj.Faces)
        {
            var points = new List<Point>();
            foreach (var faceVertex in face)
            {
                var point = ToScreen(Get2DPoint(obj.Position + RotateY(obj.Vertices[faceVertex.VertexIndex], obj.Rotation.Y)), camera);
                var nextPoint = ToScreen(Get2DPoint(obj.Position + RotateY(obj.Vertices[face[(Array.IndexOf(face, faceVertex) + 1) % face.Length].VertexIndex], obj.Rotation.Y)), camera);
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

    private static Vector2 ToScreen(Vector2 point, Camera camera)
    {
        return new Vector2(
            (point.X + 1) / 2 * camera.SizePerUnit + (camera.Width - camera.SizePerUnit) / 2,
            (1 - (point.Y + 1) / 2) * camera.SizePerUnit + (camera.Height - camera.SizePerUnit) / 2);
    }

    private static Vector2 Get2DPoint(Vector3 point)
    {
        return new Vector2(point.X / point.Z, point.Y / point.Z);
    }
}
