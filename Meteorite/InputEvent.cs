using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Meteorite;

public abstract class InputEvent{}

public class MouseMotionEvent : InputEvent
{
    public Vector2 Position;
    public Vector2 Delta;
}

public class KeyEvent : InputEvent
{
    public Keys Key;
    public InputAction State;
}
