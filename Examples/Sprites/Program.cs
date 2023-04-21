using Meteorite;

static class Program
{
    static void Main()
    {
        Game.New("Sprites");

        var sprite = new SpriteRenderer()
        {
            Tint = Color.White,
            Scale = new(1, 2),
            GlobalRotation = 45,
        };
        var sprite1 = new SpriteRenderer()
        {
            Parent = sprite,
            Position = new(1.5f, 0),
            Tint = Color.Red,
            Layer = -1,
        };
        var sprite2 = new SpriteRenderer()
        {
            Parent = sprite,
            LossyScale = new(1, 1),
            Tint = Color.Green,
            Layer = 1,
            Position = new(-1, 1),
            Rotation = 20,
        };

        Game.AddNodes(sprite, sprite1, sprite2);
        Game.Run();
    }
}