namespace OpenAL.managed;

internal unsafe class ReopenDeviceSoft
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    unsafe delegate bool AlcReopenDeviceSOFTDelegate(IntPtr device, [MarshalAs(UnmanagedType.LPUTF8Str)] string deviceName, int* attribs);

    static AlcReopenDeviceSOFTDelegate reopenDevice;
    static bool firstInvocation = true;

    static void Initialise()
    {
        firstInvocation = false;

        // Check if the extension exists (ALC extension, not AL extension)
        if (!AL.alcIsExtensionPresent(IntPtr.Zero, "ALC_SOFT_reopen_device"))
            return;

        // Get the function pointer
        var ptr = AL.alcGetProcAddress(IntPtr.Zero, "alcReopenDeviceSOFT");

        if (ptr == IntPtr.Zero)
            return;

        // Convert to delegate and cache it
        reopenDevice = Marshal.GetDelegateForFunctionPointer<AlcReopenDeviceSOFTDelegate>(ptr);
    }

    internal static bool Invoke(IntPtr device, string deviceName, ReadOnlySpan<int> attribs)
    {
        if (firstInvocation)
            Initialise();

        if (reopenDevice == null)
            return false;

        fixed (int* attribsPtr = attribs)
        {
            return reopenDevice(device, deviceName, attribsPtr);
        }
    }
}
