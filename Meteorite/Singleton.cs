namespace Meteorite;

public interface ISingleton
{
    public virtual void Start() {}
    public virtual void Update(float delta) {}
    public virtual void Render(float delta) {}
    public virtual void Close() {}
}