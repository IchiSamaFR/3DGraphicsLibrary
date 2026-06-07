namespace Object3DLibrary.Entites;

public class FaceVertex
{
    public int VertexIndex { get; set; }
    public int? TextureIndex { get; set; }
    public int? NormalIndex { get; set; }

    public FaceVertex(int vertexIndex, int? textureIndex, int? normalIndex)
    {
        VertexIndex = vertexIndex;
        TextureIndex = textureIndex;
        NormalIndex = normalIndex;
    }
}
