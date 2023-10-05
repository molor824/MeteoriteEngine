using Meteorite;
using Meteorite.Mathematics;
using OpenTK.Graphics.OpenGL;

static class Program
{
    static void Main()
    {
        Game.New("First Person Flying");

        var cam = Game.MainCamera3D;
        cam.Projection = CameraProjection.Perspective;
        cam.Far = 1000;
        cam.Near = 0.01f;

        Game.AddNode(new RotatingSprite()
        {
            Texture = new(new[]
            {
                Color.Cyan, Color.Red,
                Color.Red, Color.Cyan
            }, 2, 2, new()
            {
                MagFilter = TextureMagFilter.Nearest
            })
            {
                PixelsPerUnit = 1
            },
            RotateSpeed = 90
        });
        
        var fpf = new FirstPersonFly();
        fpf.AddChild(cam);
        
        Game.AddNode(fpf);
        Game.Run();
    }
}

public class RotatingSprite : SpriteRenderer
{
    public float RotateSpeed;

    public override void Update(float delta)
    {
        base.Update(delta);
        
        ApplyGlobalRotation(RotateSpeed * delta * Mathf.DegToRad);
    }
}