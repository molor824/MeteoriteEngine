﻿using Meteorite;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

static class Program
{
    class RotatingCamera : Transform
    {
        private float _rotationSpeed = 30;
        
        public override void Start()
        {
            base.Start();
            
            // Camera setup
            var cam = Game.MainCamera;

            cam.Parent = this;
            cam.Projection = CameraProjection.Perspective;
            cam.Near = 0.01f;
            cam.Far = 100;
            cam.Position = new(0, 0, 5);
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            
            Rotation *= Quaternion.FromAxisAngle(Vector3.UnitY, _rotationSpeed * delta * MathConst.Deg2Rad);
        }
    }
    class RotatingSprite : Sprite
    {
        private float _rotationSpeed = 90;

        public override void Added()
        {
            base.Added();

            Scale = new(2, 2);
            Texture = new(new[]
            {
                Color4.Green, Color4.Red,
                Color4.Red, Color4.Green
            }, 2, 2, new()
            {
                MagFilter = TextureMagFilter.Nearest
            });
            Texture.PixelsPerUnit = 1;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            Rotation += _rotationSpeed * delta;
        }
    }
    static void Main()
    {
        Game.New("Perspective Camera");
        
        // Disabling it to see the back side of sprite aswell
        GL.Disable(EnableCap.CullFace);

        Game.AddNode(new RotatingSprite());
        Game.AddNode(new RotatingCamera());
        Game.Run();
    }
}