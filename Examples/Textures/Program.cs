using Meteorite;
using OpenTK.Graphics.OpenGL;

public class TexturedSprites : Transform2D
{
    SpriteRenderer _sprite = null!;
    SpriteRenderer _sprite1 = null!;

    float _rotationSpeed = 45;
    float _rotationSpeed1 = -30;

    public override void Added()
    {
        base.Added();

        Scale = new(2, 1);

        // Make sure to create sprite after `Game.New()` otherwise it throws error
        _sprite = new();
        _sprite1 = new();
        
        AddChildren(_sprite, _sprite1);

        // By default it is set to "Resources" but i'll set it manually to show you how to do so
        // Check out .csproj to learn how to load files onto output directory
        Game.ResourcesDir = "Resources";
        _sprite.Texture = new("raylib.png");
        // Custom texture parameters
        // OpenTK is required for advanced render options
        _sprite1.Texture = new("opengl.png", new()
        {
            MagFilter = TextureMagFilter.Nearest
        });

        // by default, camera's height is 10 units
        _sprite.Texture.PixelsPerUnit = 100;
        _sprite1.Texture.PixelsPerUnit = 50;

        // layer is z index in 3D rendering
        _sprite.Layer = -1;
        _sprite1.Layer = 0;
    }
    public override void Update(float delta)
    {
        base.Update(delta);
        
        _sprite.Rotation += _rotationSpeed * delta;
        _sprite1.Rotation += _rotationSpeed1 * delta;
    }
}
static class Program
{
    static void Main()
    {
        Game.New("Test");

        Game.AddNode(new TexturedSprites());
        Game.Run();
    }
}