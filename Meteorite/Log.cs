namespace Meteorite;

public static class Log
{
    public static void Print(string fmt, params object?[] objs)
    {
#if DEBUG
        Console.ResetColor();
        Console.WriteLine(fmt, objs);
#endif
    }
    public static void Warn(string fmt, params object?[] objs)
    {
#if DEBUG
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(fmt, objs);
        Console.ResetColor();
#endif
    }
    public static void Error(string fmt, params object?[] objs)
    {
#if DEBUG
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(fmt, objs);
        Console.ResetColor();
#endif
    }
    public static void PrintColored(string fmt, ConsoleColor color, params object?[] objs)
    {
#if DEBUG
        Console.ForegroundColor = color;
        Console.WriteLine(fmt, objs);
        Console.ResetColor();
#endif
    }
    public static void Panic(string fmt, params object?[] objs)
    {
        throw new Exception("\x1b[31m\n" + String.Format(fmt, objs) + "\x1b[0m");
    }
}