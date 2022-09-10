using Meteorite;

static class Program
{
    static void Main()
    {
        Game.New("Sprites");

        var sprite = new Sprite()
        {
            Color = Color.White,
            Scale = new(1, 2),
            GlobalRotation = 45,
        };
        var sprite1 = new Sprite()
        {
            Parent = sprite,
            Position = new(1.5f, 0),
            Color = Color.Red,
            Layer = -1,
        };
        var sprite2 = new Sprite()
        {
            Parent = sprite,
            LossyScale = new(1, 1),
            Color = Color.Green,
            Layer = 1,
            Position = new(-1, 1),
            Rotation = 20,
        };

        Game.AddNodes(sprite, sprite1, sprite2);
        Game.Run();
    }
}