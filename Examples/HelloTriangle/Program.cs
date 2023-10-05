using Meteorite;
using Meteorite.Mathematics;

static class Program
{
    static void Main()
    {
        Game.New("Hello Triangle!");

        var mesh = new Mesh(new Vec3[]
        {
            new(-0.5f, -0.5f, 0),
            new(0, 0.5f, 0),
            new(0.5f, -0.5f, 0)
        }, new ushort[] { 2, 1, 0 }, new Color[] { Color.Red, Color.Green, Color.Blue });

        Game.MainCamera2D.Size = 2;
        Game.AddNode(new MeshRenderer(mesh));
        Game.Run();
    }
}