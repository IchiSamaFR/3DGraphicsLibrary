using System.Numerics;

namespace Object3DLibrary.Entites;

public class Object3D
{
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Vector3 Rotation
    {
        get
        {
            return RadianRotation * (180 / (float)Math.PI);
        }
        set
        {
            RadianRotation = value * ((float)Math.PI / 180);
        }
    }
    public Vector3 RadianRotation { get; set; } = Vector3.Zero;

    public List<Vector3> Vertices { get; set; } = new();
    public List<FaceVertex[]> Faces { get; set; } = new();
}
