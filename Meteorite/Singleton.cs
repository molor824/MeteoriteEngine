namespace Meteorite;

public class Singleton : IDisposable
{
    private bool _disposed;

    public void Test()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().Name);
    }
    public T Test<T>(T value)
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().Name);
        return value;
    }

    protected virtual void OnDispose() { }
    public void Dispose()
    {
        if (_disposed) return;
        
        OnDispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
    ~Singleton()
    {
        Dispose();
    }
}