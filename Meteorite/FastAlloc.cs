namespace Meteorite;

using System.Runtime.InteropServices;

internal static class FastAlloc
{
    internal static unsafe T* Alloc<T>(int size) where T : unmanaged
    {
        return (T*)NativeMemory.Alloc((nuint)(size * Marshal.SizeOf<T>()));
    }
    internal static unsafe T* AllocWith<T>(IEnumerable<T> array) where T : unmanaged
    {
        var ptr = Alloc<T>(array.Count());
        var i = 0;

        Log.Print("Allocated unmanaged array with count {0}, total size {1}", array.Count(), array.Count() * Marshal.SizeOf<T>());

        foreach (var v in array)
        {
            ptr[i] = v;
            i++;
        }

        return ptr;
    }
    internal static unsafe void Free(IntPtr ptr)
    {
        Log.Print("Freeing unmanaged pointer: {0}", ptr);
        NativeMemory.Free((void*)ptr);
    }
}