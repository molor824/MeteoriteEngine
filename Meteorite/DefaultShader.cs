namespace Meteorite;

internal static class DefaultShader
{
    internal static int TintLocation, TransformLocation;

    static DefaultShader()
    {
        TintLocation = Shader.Default.GetUniformLocation("tint");
        TransformLocation = Shader.Default.GetUniformLocation("transform");
    }
}