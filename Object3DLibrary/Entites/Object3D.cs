using System.Numerics;

namespace Object3DLibrary.Entites;

public class Object3D
{
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Vector3 Rotation { get; set; } = Vector3.Zero;

    public List<Vector3> Vertices { get; set; } = new();
    public List<FaceVertex[]> Faces { get; set; } = new();
}
