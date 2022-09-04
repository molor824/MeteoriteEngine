using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Meteorite;

public abstract class InputEvent{}

public class MouseMotionEvent : InputEvent
{
    public vec2 Position;
    public vec2 Delta;
}

public class KeyEvent : InputEvent
{
    public Keys Key;
    public InputAction State;
}
