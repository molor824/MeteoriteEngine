using Meteorite;

public class Print : Node
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
        Game.New("Hello Meteorite Engine!");
        
        // Equivalent to Node.MainRoot.AddChild(new Print());
        Game.AddNode(new Print());
        Game.Run();
    }
}