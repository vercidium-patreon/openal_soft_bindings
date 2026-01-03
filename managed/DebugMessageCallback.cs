namespace OpenAL.managed;

/// <summary>
/// Helper for setting up OpenAL debug message callbacks via extension
/// </summary>
internal class DebugMessageCallback
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void ALDebugMessageCallbackFunc(AL.ALDebugProc callback, IntPtr userParam);

    readonly ALDebugMessageCallbackFunc debugMessageCallback;

    /// <summary>
    /// Initializes the debug callback extension function
    /// </summary>
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

    /// <summary>
    /// Sets the debug message callback function
    /// </summary>
    public void Invoke(AL.ALDebugProc callback, IntPtr userParam) => debugMessageCallback?.Invoke(callback, userParam);
}
