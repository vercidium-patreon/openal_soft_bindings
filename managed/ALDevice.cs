using System;

namespace OpenAL.managed;

/// <summary>
/// Represents an OpenAL audio output device
/// </summary>
public class ALDevice
{
    /// <summary>
    /// The native device handle
    /// </summary>
    public readonly IntPtr handle;

    DebugMessageCallback debugMessageCallback;
    ReopenDeviceSoft reopenDevice;

    /// <summary>
    /// Opens an audio device by name
    /// </summary>
    /// <param name="deviceName">The name of the device to open</param>
    /// <exception cref="Exception">Thrown if device initialization fails</exception>
    public ALDevice(string deviceName)
    {
        handle = AL.OpenDevice(deviceName);

        if (handle == IntPtr.Zero)
            throw new Exception("Failed to initialise OpenAL device");
    }

    /// <summary>
    /// Closes the device
    /// </summary>
    public void Close() => AL.CloseDevice(handle);

    /// <summary>
    /// Gets a raw string pointer for a device parameter
    /// </summary>
    /// <param name="param">The parameter to query</param>
    /// <returns>Pointer to the string value</returns>
    public nint GetStringRaw(int param) => AL.GetStringPtr(handle, param);

    /// <summary>
    /// Gets a string value for a device parameter
    /// </summary>
    /// <param name="param">The parameter to query</param>
    /// <returns>The string value</returns>
    public string GetString(int param) => Marshal.PtrToStringUTF8(AL.GetStringPtr(handle, param));

    /// <summary>
    /// Gets an integer value for a device parameter
    /// </summary>
    /// <param name="param">The parameter to query</param>
    /// <returns>The integer value</returns>
    public int GetIntegerALC(int param) => AL.GetIntegerALC(handle, param);

    /// <summary>
    /// Gets the current error state for this device
    /// </summary>
    /// <returns>The error code</returns>
    public int GetErrorALC() => AL.GetError(handle);

    // TODO - should this live on the device or the context?
    /// <summary>
    /// Sets up a debug message callback for this device
    /// </summary>
    /// <param name="callback">The callback function to invoke for debug messages</param>
    /// <param name="userParam">User-defined parameter passed to the callback</param>
    public void SetupDebugMessageCallback(AL.ALDebugProc callback, IntPtr userParam)
    {
        // These delegates are created here rather than in the constructor, because the AL context must be created first
        Debug.Assert(AL.GetCurrentContext() != IntPtr.Zero);
        debugMessageCallback ??= new(handle);
        debugMessageCallback.Invoke(callback, userParam);
    }

    /// <summary>
    /// Switches this device to a different output device. Requires the ALC_SOFT_reopen_device extension
    /// </summary>
    /// <param name="deviceName">The name of the new device to switch to</param>
    /// <param name="attribs">Attribute list for context configuration (same format as alcCreateContext)</param>
    /// <returns>True if the device was successfully reopened</returns>
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

