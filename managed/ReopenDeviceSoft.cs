namespace OpenAL.managed;

/// <summary>
/// Helper for reopening devices using the ALC_SOFT_reopen_device extension
/// </summary>
internal class ReopenDeviceSoft
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    unsafe delegate bool AlcReopenDeviceSOFTDelegate(IntPtr device, [MarshalAs(UnmanagedType.LPUTF8Str)] string deviceName, int* attribs);

    readonly AlcReopenDeviceSOFTDelegate reopenDevice;

    /// <summary>
    /// Initializes the reopen device extension function
    /// </summary>
    public ReopenDeviceSoft(IntPtr device)
    {
        // Check if the extension exists (ALC extension, not AL extension)
        if (!AL.alcIsExtensionPresent(device, "ALC_SOFT_reopen_device"))
            return;

        // Get the function pointer
        var ptr = AL.alcGetProcAddress(device, "alcReopenDeviceSOFT");

        if (ptr != IntPtr.Zero)
        {
            // Convert to delegate and cache it
            reopenDevice = Marshal.GetDelegateForFunctionPointer<AlcReopenDeviceSOFTDelegate>(ptr);
        }
    }

    /// <summary>
    /// Reopens the device with new settings
    /// </summary>
    public unsafe bool Invoke(IntPtr device, string deviceName, ReadOnlySpan<int> attribs)
    {
        if (reopenDevice == null)
            return false;

        fixed (int* attribsPtr = attribs)
        {
            return reopenDevice(device, deviceName, attribsPtr);
        }
    }
}
