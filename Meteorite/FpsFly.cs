using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Meteorite;

public class FpsFly : LocalVelocity
{
    public float Acceleration = 100;
    public float Deacceleration = 50;
    public float Speed = 10;
    public float RunSpeed = 20;

    float _xrot, _yrot;
    bool _mouseLocked = true;

    public override void Start()
    {
        base.Start();

        Input.CursorMode = CursorModeValue.CursorDisabled;
    }

    public override void InputHandler(InputEvent ievent)
    {
        base.InputHandler(ievent);

        if (ievent is MouseMotionEvent motion)
        {
            var delta = motion.Delta;

            _xrot -= delta.Y;
            _xrot = MathHelper.Clamp(_xrot, -90, 90);
            _yrot += delta.X;

            Rotation = Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians(_xrot))
                       * Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(_yrot));
        }
        else if (ievent is KeyEvent key)
        {
            if (key.Key == Keys.Escape && key.State == InputAction.Press)
            {
                _mouseLocked = !_mouseLocked;
                Input.CursorMode = _mouseLocked ? CursorModeValue.CursorDisabled : CursorModeValue.CursorNormal;
            }
        }
    }

    public override void Update(float delta)
    {
        base.Update(delta);

        var direction = new Vector3();
        var crntSpeed = Input.IsKeyDown(Keys.LeftShift) ? RunSpeed : Speed;

        if (Input.IsKeyDown(Keys.W)) direction.Z--;
        if (Input.IsKeyDown(Keys.S)) direction.Z++;
        if (Input.IsKeyDown(Keys.D)) direction.X--;
        if (Input.IsKeyDown(Keys.A)) direction.X++;
        if (Input.IsKeyDown(Keys.Q)) direction.Y--;
        if (Input.IsKeyDown(Keys.E)) direction.Y++;

        var acceleration = direction == new Vector3() ? Deacceleration : Acceleration;

        Linear = Linear.MoveTowards(direction * crntSpeed, acceleration * delta);
    }
}