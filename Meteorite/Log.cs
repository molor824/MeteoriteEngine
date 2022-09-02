public static class Log
{
	public static void Print(string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ResetColor();
		Console.WriteLine("LOG: " + fmt, objs);
#endif
	}
	public static void Warn(string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine("WARN: " + fmt, objs);
		Console.ResetColor();
#endif
	}
	public static void Error(string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("ERR: " + fmt, objs);
		Console.ResetColor();
#endif
	}
	public static void PrintColored(ConsoleColor color, string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ForegroundColor = color;
		Console.WriteLine("LOG: " + fmt, objs);
		Console.ResetColor();
#endif
	}
	public static Exception Panic(string fmt, params object?[] objs)
	{
		return new Exception("PANIC: " + string.Format(fmt, objs));
	}
}