using System.Numerics;

namespace Object3DLibrary.Entites;

public class Camera
{
    public Vector2 Size { get; private set; }
    public float Width
    {
        get => Size.X;
        set
        {
            Size = new Vector2(value, Height);
            UpdateSizePerUnit();
        }
    }
    public float Height
    {
        get => Size.Y;
        set
        {
            Size = new Vector2(Width, value);
            UpdateSizePerUnit();
        }
    }
    public float SizePerUnit { get; set; }

    private void UpdateSizePerUnit()
    {
        SizePerUnit = Math.Min(Width, Height) / 2;
    }
}
