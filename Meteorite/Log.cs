public static class Log
{
	public static void Print(string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ResetColor();
		_Print("LOG: " + fmt, objs);
#endif
	}
	public static void Warn(string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ForegroundColor = ConsoleColor.Yellow;
		_Print("WARN: " + fmt, objs);
		Console.ResetColor();
#endif
	}
	public static void Error(string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ForegroundColor = ConsoleColor.Red;
		_Print("ERR: " + fmt, objs);
		Console.ResetColor();
#endif
	}

	public static void Success(string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ForegroundColor = ConsoleColor.Green;
		_Print("SUCCESS: " + fmt, objs);
		Console.ResetColor();
#endif
	}
	public static void PrintColored(ConsoleColor color, string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ForegroundColor = color;
		_Print("LOG: " + fmt, objs);
		Console.ResetColor();
#endif
	}
	public static Exception Panic(string fmt, params object?[] objs)
	{
		return new Exception("PANIC: " + string.Format(fmt, objs));
	}

	static void _Print(string fmt, params object?[] objs)
	{
		Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {fmt}", objs);
	}
}