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

    public Texture Texture
    {
        get => _texture;
        set
        {
            if (_material != null) { _material.SetTexture(_texture); }
            _texture = value;
        }
    }
    public Color Color
    {
        get => _color;
        set
        {
            if (_material != null) { _material.SetColor(_color); }
            _color = value;
        }
    }

    Material _material = null!;
    Texture _texture = null!;
    Color _color = new(1, 1, 1);

    public override void Added()
    {
        if (_quad == null)
        {
            _quad = new(
                new vec3[]
                {
                    new(-0.5f, 0.5f, 0),
                    new(0.5f, 0.5f, 0),
                    new(0.5f, -0.5f, 0),
                    new(-0.5f, -0.5f, 0)
                },
                new ushort[]
                {
                    0, 3, 2, 2, 1, 0
                },
                new vec2[]
                {
                    new(0, 0),
                    new(1, 0),
                    new(1, 1),
                    new(0, 1)
                }
            );
            Log.Print("Loaded sprite mesh");
        }
        if (_default == null)
        {
            _default = new(1, 1, new Color[] { Color.White })
            {
                PixelsPerUnit = 1
            };
            Log.Print("Loaded sprite default texture");
        }
        if (_texture == null)
        {
            Log.Print("Texture is null, using default texture");
            _texture = _default;
        }

        _material = new(_texture, _color);

        base.Added();
    }
    public override void Render(float delta)
    {
        var oldScale = Scale;

        Scale *= Texture.Size / Texture.PixelsPerUnit;
        Raylib.DrawMesh(_quad.Raw, _material.Raw, GlobalTransformMatrix.Transposed.ToSystem());
        Scale = oldScale;

        base.Render(delta);
    }
}