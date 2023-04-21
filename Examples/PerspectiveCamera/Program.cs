using Meteorite;
using OpenTK.Graphics.OpenGL;
using GlmSharp;

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
            
            Rotation *= quat.FromAxisAngle(_rotationSpeed * delta * MathConst.Deg2Rad, vec3.UnitY);
        }
    }
    class RotatingSprite : SpriteRenderer
    {
        private float _rotationSpeed = 90;

        public override void Added()
        {
            base.Added();

            Scale = new(2, 2);
            Texture = new(new[]
            {
                Color.Blue, Color.Red,
                Color.Red, Color.Blue
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

        Game.AddNode(new RotatingSprite());
        Game.AddNode(new RotatingCamera());
        Game.Run();
    }
}