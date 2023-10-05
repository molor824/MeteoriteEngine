using Meteorite.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Meteorite;

public abstract class InputEvent{}

public class MouseMotionEvent : InputEvent
{
    public Vec2 Position;
    public Vec2 Delta;
}

public class KeyEvent : InputEvent
{
    public Keys Key;
    public InputAction State;
}
