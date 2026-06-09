using Object3DLibrary.Entites;
using System.Collections.Concurrent;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace Object3DLibrary;

public static class DrawingContextExtension
{
    private static Matrix4x4 GetRotationMatrix(Vector3 rotation)
    {
        // Retourner une matrice 4x4 pré-calculée
        return Matrix4x4.CreateRotationX(rotation.X) *
               Matrix4x4.CreateRotationY(rotation.Y) *
               Matrix4x4.CreateRotationZ(rotation.Z);
    }

    public static void DrawObject3D(this DrawingContext dc, Object3D obj, Camera camera, Brush brush)
    {
        var lines = new ConcurrentBag<ObjectLine>();
        var rotationMatrix = GetRotationMatrix(obj.RadianRotation);

        // Calcul parallèle (thread-safe)
        Parallel.ForEach(obj.Faces, face =>
        {
            foreach (var faceVertex in face)
            {
                var rotated = Vector3.Transform(obj.Vertices[faceVertex.VertexIndex], rotationMatrix);
                var verticePosition = obj.Position + rotated;

                var nextRotated = Vector3.Transform(obj.Vertices[face[(Array.IndexOf(face, faceVertex) + 1) % face.Length].VertexIndex], rotationMatrix);
                var nextVerticePosition = obj.Position + nextRotated;

                var point = ToScreen(Get2DPoint(verticePosition), camera);
                var nextPoint = ToScreen(Get2DPoint(nextVerticePosition), camera);
                lines.Add(new ObjectLine(point, nextPoint));
            }
        });

        // Dessin synchrone du contexte
        foreach (var line in lines)
        {
            dc.DrawLine(new Pen(brush, 1),
                new Point((int)line.Start.X, (int)line.Start.Y),
                new Point((int)line.End.X, (int)line.End.Y));
        }
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
