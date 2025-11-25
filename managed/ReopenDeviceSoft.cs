namespace OpenAL.managed;

internal class ReopenDeviceSoft
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate bool AlcReopenDeviceSOFTDelegate(IntPtr device, [MarshalAs(UnmanagedType.LPUTF8Str)] string deviceName, IntPtr attribs);

    readonly AlcReopenDeviceSOFTDelegate reopenDevice;

    public ReopenDeviceSoft(IntPtr handle)
    {
        // Check if the extension exists
        if (!AL.alIsExtensionPresent("ALC_SOFT_reopen_device"))
            return;

        // Get the function pointer
        var ptr = AL.alcGetProcAddress(handle, "alcReopenDeviceSOFT");

        if (ptr != IntPtr.Zero)
        {
            // Convert to delegate and cache it
            reopenDevice = Marshal.GetDelegateForFunctionPointer<AlcReopenDeviceSOFTDelegate>(ptr);
        }
    }

    public bool Invoke(IntPtr device, string deviceName, IntPtr attribs) => reopenDevice?.Invoke(device, deviceName, attribs) ?? false;
}
