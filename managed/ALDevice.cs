using System;

namespace OpenAL.managed;

public class ALDevice
{
    public readonly IntPtr handle;

    DebugMessageCallback debugMessageCallback;
    ReopenDeviceSoft reopenDevice;

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

    // TODO - should this live on the device or the context?
    public void SetupDebugMessageCallback(AL.ALDebugProc callback, IntPtr userParam)
    {
        // These delegates are created here rather than in the constructor, because the AL context must be created first
        Debug.Assert(AL.GetCurrentContext() != IntPtr.Zero);
        debugMessageCallback ??= new(handle);
        debugMessageCallback.Invoke(callback, userParam);
    }

    /// <summary>
    /// Reopen this device as a new device name. Requires the ALC_SOFT_reopen_device extension
    /// </summary>
    /// <param name="deviceName">The name of the device</param>
    /// <param name="attribs">Attribute list to configure the device with, with the same attribute list that would be passed to alcCreateContext</param>
    /// <returns>True if the device was re-opened</returns>
    public bool Reopen(string deviceName, int[] attribs)
    {
        // Must have an existing context too
        Debug.Assert(AL.GetCurrentContext() != IntPtr.Zero);

        reopenDevice ??= new(handle);
        return reopenDevice.Invoke(handle, deviceName, attribs);
    }

    /// <summary>
    /// Check if an extension is present
    /// </summary>
    /// <param name="extension">The AL extension specifier</param>
    /// <returns>True if the extension is present</returns>
    public bool HasExtension(string extension) => AL.IsExtensionPresent(handle, extension);
}

