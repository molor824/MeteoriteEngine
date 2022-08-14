using Meteorite;

static class Program
{
    static void Main()
    {
        var game = new Game("Model Loading", 800, 600);
        var fpsFly = new FpsFly();
        var model = new Model("ModelLoading/Contents/m9.obj");

        game.MainCamera.Parent = fpsFly;
        game.MainCamera = new Camera(CameraProjection.Perspective, 60);
        game.AddObject(fpsFly);
        game.Run();
    }
}