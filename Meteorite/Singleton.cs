namespace Meteorite;

public class Singleton : IDisposable
{
    private bool _updated, _fixedUpdated;
    private bool _disposed;
    private App? _app;

    public App? App => _app;

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

    public void Start(App app)
    {
        if (_app != null) return;
        _app = app;
        OnStart();
    }

    public void ClearFlags()
    {
        _updated = _fixedUpdated = false;
    }
    
    protected virtual void OnStart() { }
    protected virtual void OnUpdate(double dt) { }
    protected virtual void OnFixedUpdate(double dt) { }

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