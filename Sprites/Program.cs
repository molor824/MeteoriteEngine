﻿using Meteorite;
using GlmSharp;

static class Program
{
    static void Main()
    {
        var game = new Game("Sprites", 800, 600);

        var sprite = new Sprite()
        {
            Color = new(1, 1, 1),
            Scale = new(1, 2),
            GlobalRotation = 45,
        };
        var sprite1 = new Sprite()
        {
            Parent = sprite,
            Position = new(1.5f, 0),
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

        game.AddNode(sprite).AddNode(sprite1).AddNode(sprite2);
        game.Run();
    }
}