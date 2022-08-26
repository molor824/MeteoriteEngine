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

        Input.CursorMode = CursorMode.Disabled;
    }

    public override void InputHandler(InputEvent ievent)
    {
        base.InputHandler(ievent);

        if (ievent is MouseMotionEvent motion)
        {
            var delta = motion.Delta;

            _xrot -= delta.y;
            _xrot = glm.Clamp(_xrot, -90, 90);
            _yrot += delta.x;

            Rotation = quat.FromAxisAngle(_xrot, vec3.UnitX) * quat.FromAxisAngle(_yrot, vec3.UnitY);
        }
        else if (ievent is KeyEvent key)
        {
            if (key.Key == KeyList.Escape && key.State == KeyState.Press)
            {
                _mouseLocked = !_mouseLocked;
                Input.CursorMode = _mouseLocked ? CursorMode.Disabled : CursorMode.Normal;
            }
        }
    }

    public override void Update(float delta)
    {
        base.Update(delta);

        var direction = new vec3();
        var crntSpeed = Input.IsKeyDown(KeyList.LeftShift) ? RunSpeed : Speed;

        if (Input.IsKeyDown(KeyList.W)) direction.z--;
        if (Input.IsKeyDown(KeyList.S)) direction.z++;
        if (Input.IsKeyDown(KeyList.D)) direction.x--;
        if (Input.IsKeyDown(KeyList.A)) direction.x++;
        if (Input.IsKeyDown(KeyList.Q)) direction.y--;
        if (Input.IsKeyDown(KeyList.E)) direction.y++;

        var acceleration = direction == vec3.Zero ? Deacceleration : Acceleration;

        Linear = Linear.MoveTowards(direction * crntSpeed, acceleration * delta);
    }
}