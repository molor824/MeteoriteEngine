namespace Meteorite;

using System.IO;

public static class ResourceLoader
{
    public static byte[] LoadFile(string path)
    {
        var rPath = Path.Combine(Game.ResourcesDir, path);

        if (!File.Exists(rPath)) throw Log.Panic("Invalid path `{0}` when attempting to load Resource!", rPath);
        
        return File.ReadAllBytes(rPath);
    }

    public static string LoadTextFile(string path)
    {
        var rPath = Path.Combine(Game.ResourcesDir, path);

        if (!File.Exists(rPath)) throw Log.Panic("Invalid path `{0}` when attempting to load Resource!", rPath);
        
        return File.ReadAllText(rPath);
    }
}