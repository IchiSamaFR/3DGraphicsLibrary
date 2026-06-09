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
                var point = ToScreen(Get2DPoint(obj.Position + Rotate(obj.Vertices[faceVertex.VertexIndex], obj.RadianRotation)), camera);
                var nextPoint = ToScreen(Get2DPoint(obj.Position + Rotate(obj.Vertices[face[(Array.IndexOf(face, faceVertex) + 1) % face.Length].VertexIndex], obj.RadianRotation)), camera);
                dc.DrawLine(new Pen(brush, 1),
                    new Point(point.X, point.Y),
                    new Point(nextPoint.X, nextPoint.Y));
            }
        }
    }

    private static Vector3 Rotate(Vector3 point, Vector3 rotation)
    {
        float cosRotationX = (float)Math.Cos(rotation.X);
        float sinRotationX = (float)Math.Sin(rotation.X);

        float cosRotationY = (float)Math.Cos(rotation.Y);
        float sinRotationY = (float)Math.Sin(rotation.Y);

        float cosRotationZ = (float)Math.Cos(rotation.Z);
        float sinRotationZ = (float)Math.Sin(rotation.Z);

        // Rotation autour de l'axe X
        float x1 = point.X;
        float y1 = point.Y * cosRotationX - point.Z * sinRotationX;
        float z1 = point.Z * cosRotationX + point.Y * sinRotationX;

        // Rotation autour de l'axe Y
        float x2 = x1 * cosRotationY + z1 * sinRotationY;
        float y2 = y1;
        float z2 = z1 * cosRotationY - x1 * sinRotationY;

        // Rotation autour de l'axe Z
        float x = x2 * cosRotationZ - y2 * sinRotationZ;
        float y = y2 * cosRotationZ + x2 * sinRotationZ;
        float z = z2;

        return new Vector3(x, y, z);
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
