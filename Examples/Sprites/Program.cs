using Meteorite;
using Meteorite.Mathematics;

static class Program
{
    static void Main()
    {
        Game.New("Sprites");
        
        var sprite = new SpriteRenderer()
        {
            Tint = Color.White,
            Scale = new(1, 2),
            GlobalRotation = 45 * Mathf.DegToRad,
        };
        var sprite1 = new SpriteRenderer()
        {
            Parent = sprite,
            Position = new(1.5f, 0),
            GlobalLayer = -1,
            Tint = Color.Red,
        };
        var sprite2 = new SpriteRenderer()
        {
            Parent = sprite,
            GlobalScale = new(1, 1),
            Tint = Color.Green,
            Position = new(-1, 1),
            GlobalLayer = 1,
            Rotation = 20 * Mathf.DegToRad,
        };

        Game.AddNodes(sprite);
        Game.Run();
    }
}