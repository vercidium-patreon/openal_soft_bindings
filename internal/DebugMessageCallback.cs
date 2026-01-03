namespace OpenAL;

internal static class DebugMessageCallback
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void ALDebugMessageCallbackFunc(AL.ALDebugProc callback, IntPtr userParam);

    private static readonly LazyExtensionLoader<ALDebugMessageCallbackFunc> loader = new("AL_EXT_debug", "alDebugMessageCallbackEXT", isAlcExtension: false);

    internal static void Invoke(AL.ALDebugProc callback, IntPtr userParam)
    {
        if (!loader.IsAvailable)
            return;

        loader.Function(callback, userParam);
    }
}
