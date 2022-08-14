namespace Meteorite;

public class Model : Transform
{
    internal Raylib_cs.Model Raw;

    public int MeshCount => Raw.meshCount;
    public Mesh Mesh(int index)
    {
        if (index >= MeshCount || index < 0) Log.Panic("Mesh count is {0}, accessor index is {1}", MeshCount, index);

        unsafe
        {
            return new(Raw.meshes[index]);
        }
    }
    public void SetMesh(int index, Mesh mesh)
    {
        if (index >= MeshCount || index < 0) Log.Panic("Mesh count is {0}, accessor index is {1}", MeshCount, index);

        unsafe
        {
            Raw.meshes[index] = mesh.Raw;
        }
    }
    public Model(string path)
    {
        unsafe
        {
            fixed (char* ptr = path)
            {
                Raw = Raylib.LoadModel((sbyte*)ptr);
            }
        }
    }
    public override void Render(float delta)
    {
        base.Render(delta);

        Raw.transform = TransformMatrix.Transposed.ToSystem();

        Raylib.DrawModel(Raw, new(), 0, Raylib_cs.Color.WHITE);
    }
    public override void Removed()
    {
        base.Removed();

        Raylib.UnloadModel(Raw);
    }
}