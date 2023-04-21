using Meteorite;

static class Program
{
    static void Main()
    {
        Game.New("UITransform Test");

        var ui = new UISpriteRenderer()
        {
            NormalizedPosition = new(0.5f, 0.5f),
            NormalizedScale = new(0.25f, 0.25f),
            AbsoluteScale = new(10, 10),
            Name = "FirstUI"
        };
        var ui1 = new UISpriteRenderer()
        {
            NormalizedPosition = new(0.5f, 0.5f),
            AbsoluteScale = new(200, 200),
            Pivot = new(0, 0),
            Parent = ui,
            Tint = Color.Green with {A = 0.5f},
            Name = "SecondUI"
        };
        var ui2 = new UISpriteRenderer()
        {
            NormalizedPosition = new(1, 1),
            // AbsolutePosition = new(-100, -100),
            NormalizedScale = new(0.5f, 0.5f),
            Pivot = new(0, 0),
            Parent = ui1,
            Tint = Color.Red with {A = 0.5f},
            Name = "ThirdUI"
        };
        
        Game.AddNode(ui);
        Game.Run();
    }
}