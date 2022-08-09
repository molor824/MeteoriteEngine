using Meteorite;
using GlmSharp;
using Raylib_cs;

static class Program
{
    static void Main()
    {
        var game = new Game();

        var sprite = new Sprite()
        {
            Color = new(1, 1, 1),
            LocalScale = new(1, 2),
            Rotation = 45,
        };
        var sprite1 = new Sprite()
        {
            Parent = sprite,
            LocalPosition = new(1.5f, 0),
            Color = new(1, 0, 0),
            Layer = -1,
        };
        var sprite2 = new Sprite()
        {
            Parent = sprite,
            LossyScale = new(1, 1),
            Color = new(0, 1, 0),
            Layer = 1,
            Position = new(-1, 1),
            Rotation = 20,
        };

        game.AddObjects(sprite, sprite1, sprite2);
        game.Run("Sprites", 800, 600);
    }
}