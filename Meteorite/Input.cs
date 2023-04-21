namespace Meteorite;

using OpenTK.Windowing.GraphicsLibraryFramework;

public class Input : ISingleton
{
    static unsafe GLFWCallbacks.CursorPosCallback _cursorCallback = (_, x, y) =>
    {
        var position = new vec2((float)x, (float)y);

        CallInputHandler(Node.MainRoot, new MouseMotionEvent()
        {
            Position = position,
            Delta = position - _lastMousePos
        });

        _lastMousePos = position;
    };
    static unsafe GLFWCallbacks.KeyCallback _keyCallback = (_, key, code, action, mods) =>
    {
        CallInputHandler(Node.MainRoot, new KeyEvent()
        {
            Key = key,
            State = action
        });
    };
    static vec2 _lastMousePos;
    static Input()
    {
        Game.AddSingleton<Input>();
    }
    void ISingleton.Start()
    {
        unsafe
        {
            GLFW.SetCursorPosCallback(Game.RawWindow, _cursorCallback);
            GLFW.SetKeyCallback(Game.RawWindow, _keyCallback);
        }
    }

    static void CallInputHandler(Node root, InputEvent ievent)
    {
        root.InputHandler(ievent);
        for (var i = 0; i < root.ChildrenCount; i++) CallInputHandler(root.GetChild(i), ievent);
    }
    public static vec2 MousePosition
    {
        get
        {
            unsafe
            {
                GLFW.GetCursorPos(Game.RawWindow, out var x, out var y);
                return new((float)x, (float)y);
            }
        }
        set { unsafe { GLFW.SetCursorPos(Game.RawWindow, value.x, value.y); } }
    }
    public static CursorModeValue CursorMode
    {
        get
        {
            unsafe
            {
                return GLFW.GetInputMode(Game.RawWindow, CursorStateAttribute.Cursor);
            }
        }
        set
        {
            unsafe
            {
                GLFW.SetInputMode(Game.RawWindow, CursorStateAttribute.Cursor, value);
            }
        }
    }

    public static bool IsKeyDown(Keys key)
    {
        unsafe
        {
            return (int)GLFW.GetKey(Game.RawWindow, key) != 0;
        }
    }
    public static bool IsKeyUp(Keys key)
    {
        unsafe
        {
            return (int)GLFW.GetKey(Game.RawWindow, key) == 0;
        }
    }
}
