using OpenTK.Graphics.OpenGL;

namespace Meteorite;

public struct RenderOptions
{
    public bool CullFace = true;
    public bool Blend = true;
    public bool DepthTest = true;
    public BlendingFactor BlendingSFactor = BlendingFactor.SrcAlpha;
    public BlendingFactor BlendingDFactor = BlendingFactor.OneMinusSrcAlpha;
    public CullFaceMode CullFaceMode = CullFaceMode.Front;

    public RenderOptions() {}

    public RenderOptions(bool cullFace, bool blend, bool depthTest)
    {
        CullFace = cullFace;
        Blend = blend;
        DepthTest = depthTest;
    }
    
    public void SetRenderOptions()
    {
        if (CullFace)
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode);
        }
        else GL.Disable(EnableCap.CullFace);

        if (Blend)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingSFactor, BlendingDFactor);
        }
        else GL.Disable(EnableCap.Blend);
        
        if (DepthTest) GL.Enable(EnableCap.DepthTest);
        else GL.Disable(EnableCap.DepthTest);
    }
}