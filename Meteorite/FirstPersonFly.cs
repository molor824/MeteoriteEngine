using Meteorite.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Meteorite;

public class FirstPersonFly : LocalVelocity
{
    public float Acceleration = 100;
    public float Deacceleration = 50;
    public float Speed = 10;
    public float RunSpeed = 20;
    public float Sensitivity = 0.5f;

    float _xrot, _yrot;
    bool _mouseLocked = true;

    public override void Start()
    {
        Input.CursorMode = CursorModeValue.CursorDisabled;
    }

    public override void InputHandler(InputEvent ievent)
    {
        if (ievent is MouseMotionEvent motion)
        {
            var delta = motion.Delta;

            _xrot += delta.Y * Sensitivity;
            _xrot = Mathf.Clamp(_xrot, -90, 90);
            _yrot -= delta.X * Sensitivity;
        }
        else if (ievent is KeyEvent key)
        {
            if (key.Key == Keys.Escape && key.State == InputAction.Press)
            {
                _yrot = 0;
                _xrot = 0;
                _mouseLocked = !_mouseLocked;
                Input.CursorMode = _mouseLocked ? CursorModeValue.CursorDisabled : CursorModeValue.CursorNormal;
            }
        }
    }

    public override void Update(float delta)
    {
        Rotation = Quat.FromAxisAngle(Vec3.UnitY, _yrot * Mathf.DegToRad) *
                   Quat.FromAxisAngle(Vec3.UnitX, _xrot * Mathf.DegToRad);

        var direction = new Vec3();
        var crntSpeed = Input.IsKeyDown(Keys.LeftShift) ? RunSpeed : Speed;

        if (Input.IsKeyDown(Keys.W)) direction.Z++;
        if (Input.IsKeyDown(Keys.S)) direction.Z--;
        if (Input.IsKeyDown(Keys.D)) direction.X++;
        if (Input.IsKeyDown(Keys.A)) direction.X--;
        if (Input.IsKeyDown(Keys.Q)) direction.Y++;
        if (Input.IsKeyDown(Keys.E)) direction.Y--;
        
        direction = Rotation * direction;
        
        var acceleration = direction.IsZeroApprox() ? Deacceleration : Acceleration;

        Linear = Linear.MoveTowards(direction * crntSpeed, acceleration * delta);
        
        base.Update(delta);
    }
}