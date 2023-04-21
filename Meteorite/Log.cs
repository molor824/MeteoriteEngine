using System.Text;

public static class Log
{
	public static void Print(string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ResetColor();
		Console.WriteLine(_Print("LOG: ", fmt, objs));
#endif
	}
	public static void Warn(string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine(_Print("WARN: ", fmt, objs));
		Console.ResetColor();
#endif
	}
	public static void Error(string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(_Print("ERROR: ", fmt, objs));
		Console.ResetColor();
#endif
	}

	public static void Success(string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine(_Print("ERROR: ", fmt, objs));
		Console.ResetColor();
#endif
	}
	public static void PrintColored(ConsoleColor color, string fmt, params object?[] objs)
	{
#if DEBUG
		Console.ForegroundColor = color;
		Console.WriteLine(_Print("LOG: ", fmt, objs));
		Console.ResetColor();
#endif
	}
	public static Exception Panic(string fmt, params object?[] objs)
	{
		return new Exception(_Print("PANIC: ", fmt, objs));
	}

	static string _Print(string initial, string fmt, params object?[] objs)
	{
		initial = $"[{DateTime.Now:HH:mm:ss}] {initial}";
		var builder = new StringBuilder();
		var result = string.Format(fmt, objs);

		builder.Append(initial);

		for (var i = 0; i < result.Length; i++)
		{
			builder.Append(result[i]);

			if (result[i] != '\n') continue;

			builder.Append(initial);
		}

		return builder.ToString();
	}
}