namespace OpenAL.managed;

internal class DebugMessageCallback
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void ALDebugMessageCallbackFunc(AL.ALDebugProc callback, IntPtr userParam);

    readonly ALDebugMessageCallbackFunc debugMessageCallback;

    public DebugMessageCallback(IntPtr handle)
    {
        // Check if the extension exists
        if (!AL.alIsExtensionPresent("AL_EXT_debug"))
            return;

        // Get the function pointer
        var ptr = AL.GetProcAddress(handle, "alDebugMessageCallbackEXT");

        if (ptr != IntPtr.Zero)
        {
            // Convert to delegate and cache it
            debugMessageCallback = Marshal.GetDelegateForFunctionPointer<ALDebugMessageCallbackFunc>(ptr);
        }
    }

    public void Invoke(AL.ALDebugProc callback, IntPtr userParam) => debugMessageCallback?.Invoke(callback, userParam);
}
