namespace Meteorite;

/// <summary>
/// 2D sprite with texture and color.
/// Position Z index acts as depth.
/// Sprite acts as quad mesh and renders just as same as 3D.
/// </summary>
public class Sprite : Transform2D
{
    static Mesh _quad = null!;
    static Texture _default = null!;

    public Texture? Texture = null;
    public Color Color;

    public override void Added()
    {
        if (_quad == null)
        {
            _quad = new(
                new Vertex[]
                {
                    new(new(-0.5f, 0.5f, 0), new(0, 0)),
                    new(new(0.5f, 0.5f, 0), new(1, 0)),
                    new(new(0.5f, -0.5f, 0), new(1, 1)),
                    new(new(-0.5f, -0.5f, 0), new(0, 1)),
                },
                new ushort[] { 0, 3, 2, 2, 1, 0 }
                // new vec3[]
                // {
                //     new(-0.5f, 0.5f, 0),
                //     new(0.5f, 0.5f, 0),
                //     new(0.5f, -0.5f, 0),
                //     new(-0.5f, -0.5f, 0)
                // },
                // new ushort[]
                // {
                //     0, 3, 2, 2, 1, 0
                // },
                // new vec2[]
                // {
                //     new(0, 0),
                //     new(1, 0),
                //     new(1, 1),
                //     new(0, 1)
                // }
            );
            _quad.Upload();
        }
        if (_default == null)
        {
            _default = new(new Color[] { Color.White }, 1, 1);
            _default.Upload();
        }
        if (Texture == null)
        {
            Log.Print("Texture is null, using default texture");
        }

        base.Added();
    }
    public override void Render(float delta)
    {
        var texture = Texture ?? _default;
        var oldScale = Scale;

        Scale *= texture.Size / texture.PixelsPerUnit;

        texture.Bind();
        _quad.Render();

        Scale = oldScale;

        base.Render(delta);
    }
}