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
        base.Start();

        Input.CursorMode = CursorModeValue.CursorDisabled;
    }

    public override void InputHandler(InputEvent ievent)
    {
        base.InputHandler(ievent);

        if (ievent is MouseMotionEvent motion)
        {
            var delta = motion.Delta;

            _xrot -= delta.y * Sensitivity;
            _xrot = glm.Clamp(_xrot, -90, 90);
            _yrot -= delta.x * Sensitivity;
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

        Rotation = quat.FromAxisAngle(_yrot * MathConst.Deg2Rad, vec3.UnitY) *
                   quat.FromAxisAngle(_xrot * MathConst.Deg2Rad, vec3.UnitX);

        var direction = new vec3();
        var crntSpeed = Input.IsKeyDown(Keys.LeftShift) ? RunSpeed : Speed;

        if (Input.IsKeyDown(Keys.W)) direction.z--;
        if (Input.IsKeyDown(Keys.S)) direction.z++;
        if (Input.IsKeyDown(Keys.D)) direction.x++;
        if (Input.IsKeyDown(Keys.A)) direction.x--;
        if (Input.IsKeyDown(Keys.Q)) direction.y--;
        if (Input.IsKeyDown(Keys.E)) direction.y++;
        
        direction = Rotation * direction;
        
        var acceleration = direction == new vec3() ? Deacceleration : Acceleration;

        Linear = Linear.MoveTowards(direction * crntSpeed, acceleration * delta);
    }
}