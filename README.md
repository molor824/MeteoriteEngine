# Meteorite
An open source game engine with simple object oriented implementation and powerful OpenGL rendering

# Ongoing Plan
* 2D/3D physics engine implementation
* Easy to use resource management system

# Libraries used
* OpenTK
* GlmSharp
* SixLabors.ImageSharp

# Example
Here is how to create basic triangle with colored vertices
```cs
using Meteorite;
using GlmSharp;

static class Program
{
    static void Main()
    {
        Game.New("Hello Triangle!");

        var mesh = new Mesh(new vec3[]
        {
            new(-0.5f, -0.5f, 0),
            new(0, 0.5f, 0),
            new(0.5f, -0.5f, 0)
        }, new ushort[] { 2, 1, 0 }, new Color[] { Color.Red, Color.Green, Color.Blue });

        Game.MainCamera.Size = 2;
        Game.AddNode(new MeshRenderer(mesh));
        Game.Run();
    }
}
```