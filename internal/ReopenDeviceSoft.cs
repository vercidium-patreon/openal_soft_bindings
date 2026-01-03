namespace OpenAL;

internal static unsafe class ReopenDeviceSoft
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    unsafe delegate bool AlcReopenDeviceSOFTDelegate(IntPtr device, [MarshalAs(UnmanagedType.LPUTF8Str)] string deviceName, int* attribs);

    private static readonly LazyExtensionLoader<AlcReopenDeviceSOFTDelegate> loader = new("ALC_SOFT_reopen_device", "alcReopenDeviceSOFT", isAlcExtension: true);

    internal static bool Invoke(IntPtr device, string deviceName, ReadOnlySpan<int> attribs)
    {
        if (!loader.IsAvailable)
            return false;

        fixed (int* attribsPtr = attribs)
        {
            return loader.Function(device, deviceName, attribsPtr);
        }
    }
}
