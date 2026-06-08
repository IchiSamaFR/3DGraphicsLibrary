namespace Object3DLibrary.Entites;

public class Camera
{
    private float _width;
    private float _height;

    public float Width
    {
        get => _width;
        set
        {
            _width = value;
            UpdateSizePerUnit();
        }
    }
    public float Height
    {
        get => _height;
        set
        {
            _height = value;
            UpdateSizePerUnit();
        }
    }
    public float SizePerUnit { get; set; }

    private void UpdateSizePerUnit()
    {
        SizePerUnit = Math.Min(Width, Height) / 2;
    }
}
