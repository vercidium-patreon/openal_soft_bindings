namespace OpenAL.managed;

public class ALDevice
{
    public readonly IntPtr handle;

    public ALDevice(string deviceName)
    {
        handle = AL.OpenDevice(deviceName);

        if (handle == IntPtr.Zero)
            throw new Exception("Failed to initialise OpenAL device");
    }

    public void Close() => AL.CloseDevice(handle);

    public nint GetStringRaw(int param) => AL.GetStringPtr(handle, param);
    public string GetString(int param) => Marshal.PtrToStringUTF8(AL.GetStringPtr(handle, param));

    public int GetIntegerALC(int param) => AL.GetIntegerALC(handle, param);
    public int GetErrorALC() => AL.GetError(handle);

    DebugMessageCallback debugMessageCallback;
    ReopenDeviceSoft reopenDevice;

    public void DebugMessageCallback(AL.ALDebugProc callback, IntPtr userParam)
    {
        // These delegates are created here rather than in the constructor, because the AL context must also be created first
        Debug.Assert(AL.GetCurrentContext() != IntPtr.Zero);
        debugMessageCallback ??= new(handle);
        debugMessageCallback.Invoke(callback, userParam);
    }

    public bool Reopen(string deviceName)
    {
        Debug.Assert(AL.GetCurrentContext() != IntPtr.Zero);
        reopenDevice ??= new(handle);
        return reopenDevice.Invoke(handle, deviceName, IntPtr.Zero);
    }
}

