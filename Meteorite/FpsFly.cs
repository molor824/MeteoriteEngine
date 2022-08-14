namespace Meteorite;

public class FpsFly : Velocity
{
    public float Acceleration = 100;
    public float Deacceleration = 50;
    public float Speed = 10;
    public float RunSpeed = 20;
    public float XAngleLimit = 85;

    float _xrot;
    bool _mouseLocked = true;

    public override void Start()
    {
        base.Start();

        Game.MainCamera.Projection = CameraProjection.Perspective;
        Game.MainCamera.FovY = 70;
        Input.LockCursor();
    }
    public override void Update(float delta)
    {
        base.Update(delta);

        var mDelta = Raylib.GetMouseDelta();

        _xrot -= mDelta.Y;
        _xrot = Math.Clamp(_xrot, -XAngleLimit, XAngleLimit);

        Rotation = quat.Identity;
        Rotation *= quat.FromAxisAngle(_xrot, vec3.UnitX);
        Rotation *= quat.FromAxisAngle(mDelta.X, vec3.UnitY);

        var direction = new vec3();
        var crntSpeed = Input.IsKeyDown(KeyEnum.LeftShift) ? RunSpeed : Speed;

        if (Input.IsKeyDown(KeyEnum.W)) direction.z--;
        if (Input.IsKeyDown(KeyEnum.S)) direction.z++;
        if (Input.IsKeyDown(KeyEnum.D)) direction.x--;
        if (Input.IsKeyDown(KeyEnum.A)) direction.x++;
        if (Input.IsKeyDown(KeyEnum.Q)) direction.y--;
        if (Input.IsKeyDown(KeyEnum.E)) direction.y++;

        var acceleration = direction == vec3.Zero ? Deacceleration : Acceleration;

        Linear = Linear.MoveTowards(direction * crntSpeed, acceleration * delta);

        if (Input.IsKeyPressed(KeyEnum.Escape))
        {
            _mouseLocked = !_mouseLocked;

            if (_mouseLocked) Input.LockCursor();
            else Input.UnlockCursor();
        }
    }
}