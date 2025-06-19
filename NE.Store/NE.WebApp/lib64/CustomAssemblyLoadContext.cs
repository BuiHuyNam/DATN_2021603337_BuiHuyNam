using System.Runtime.InteropServices;
using System.Runtime.Loader;

public class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public IntPtr LoadUnmanagedLibrary(string absolutePath)
    {
        return LoadUnmanagedDll(absolutePath);
    }

    protected override IntPtr LoadUnmanagedDll(String unmanagedDllPath)
    {
        return LoadUnmanagedDllFromPath(unmanagedDllPath);
    }
}
