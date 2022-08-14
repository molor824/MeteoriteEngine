using Meteorite;

public class TexturedSprites : Transform2D
{
    Sprite _sprite = null!;
    Sprite _sprite1 = null!;

    float _rotationSpeed = 45;
    float _rotationSpeed1 = -30;

    public override void Added()
    {
        Scale = new(2, 1);

        _sprite = new();
        _sprite1 = new();

        _sprite.Parent = this;
        _sprite1.Parent = this;

        _sprite.Texture = new("Textures/Contents/raylib.png");
        _sprite1.Texture = new("Textures/Contents/opengl.png");

        // by default, camera's height is 10 units
        _sprite.Texture.PixelsPerUnit = 100;
        _sprite1.Texture.PixelsPerUnit = 50;

        // layer is z index in 3D rendering
        _sprite.Layer = -1;
        _sprite1.Layer = 0;

        Game.AddObjects(_sprite, _sprite1);

        base.Added();
    }
    public override void Update(float delta)
    {
        _sprite.Rotation += _rotationSpeed * delta;
        _sprite1.Rotation += _rotationSpeed1 * delta;

        base.Update(delta);
    }
}
static class Program
{
    static void Main()
    {
        var game = new Game("Textures", 800, 600);

        game.AddObject(new TexturedSprites());
        game.Run();
    }
}