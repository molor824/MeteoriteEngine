using Meteorite;

static class Program
{
    static void Main()
    {
        Game.New("Hello Triangle!");

        var mesh = new Mesh(new Vertex[]
        {
            new(new(-0.5f, -0.5f, 0)),
            new(new(0, 0.5f, 0)),
            new(new(0.5f, -0.5f, 0))
        }, new ushort[] { 2, 1, 0 }, new Color[] { Color.Red, Color.Green, Color.Blue });

        Game.MainCamera.Size = 2;
        Game.AddNode(new MeshRenderer(mesh));
        Game.Run();
    }
}