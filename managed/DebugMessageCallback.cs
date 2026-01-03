namespace OpenAL.managed;

internal class DebugMessageCallback
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void ALDebugMessageCallbackFunc(AL.ALDebugProc callback, IntPtr userParam);

    static ALDebugMessageCallbackFunc debugMessageCallback;
    static bool firstInvocation = true;

    static void Initialise()
    {
        firstInvocation = false;

        // Check if the extension exists
        if (!AL.alIsExtensionPresent("AL_EXT_debug"))
            return;

        // Get the function pointer
        var ptr = AL.GetProcAddress(IntPtr.Zero, "alDebugMessageCallbackEXT");

        if (ptr == IntPtr.Zero)
            return;

        // Convert to delegate and cache it
        debugMessageCallback = Marshal.GetDelegateForFunctionPointer<ALDebugMessageCallbackFunc>(ptr);
    }

    internal static void Invoke(AL.ALDebugProc callback, IntPtr userParam)
    {
        if (firstInvocation)
            Initialise();

        if (debugMessageCallback == null)
            return;

        // The extension is available - enable it and invoke it
        AL.Enable(AL.AL_DEBUG_OUTPUT_EXT);
        debugMessageCallback.Invoke(callback, userParam);
    }
}
