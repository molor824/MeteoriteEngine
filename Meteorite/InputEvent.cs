namespace Meteorite;

public abstract class InputEvent{}

public class MouseMotionEvent : InputEvent
{
    public vec2 Position;
    public vec2 Delta;
}

public class KeyEvent : InputEvent
{
    public KeyList Key;
    public KeyState State;
}
public enum KeyState
{
    /// <summary>The key or mouse button was released.</summary>
    Release,
    /// <summary>The key or mouse button was pressed.</summary>
    Press,
    /// <summary>The key was held down until it repeated.</summary>
    Repeat,
}
