using Meteorite;

public class Print : GameObject
{
    public override void Added()
    {
        Log.Print("Added called!");
    }
    public override void Start()
    {
        Log.Print("Start called!");
    }
    public override void Update(float delta)
    {
        Log.Print("Update called with delta: {0}!", delta);
    }
    public override void Render(float delta)
    {
        Log.Print("Render called with delta: {0}!", delta);
    }
    public override void Close()
    {
        Log.Print("Close called!");
    }
    public override void Removed()
    {
        Log.Print("Removed called!");
    }
}
static class Program
{
    static void Main()
    {
        var game = new Game();

        game.AddObject(new Print());

        game.Run("Hello World!", 800, 600);
    }
}