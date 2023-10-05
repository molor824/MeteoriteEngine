using Meteorite;
using Meteorite.Mathematics;
using OpenTK.Graphics.OpenGL;

static class Program
{
    class RotatingCamera : Transform3D
    {
        private float _rotationSpeed = 60;
        
        public override void Start()
        {
            base.Start();
            
            // Camera setup
            var cam = Game.MainCamera3D;

            cam.Parent = this;
            cam.Projection = CameraProjection.Perspective;
            cam.Near = 0.01f;
            cam.Far = 100;
            cam.Position = new(0, 0, 4);
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            
            Rotation *= Quat.FromAxisAngle(Vec3.UnitY, _rotationSpeed * delta * Mathf.DegToRad);
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

            ApplyGlobalRotation(Quat.FromRotationZ(_rotationSpeed * delta * Mathf.DegToRad));
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