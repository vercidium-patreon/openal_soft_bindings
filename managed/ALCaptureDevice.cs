namespace OpenAL.managed;

/// <summary>
/// Configuration settings for creating an ALCaptureDevice
/// </summary>
public class ALCaptureDeviceSettings
{
    /// <summary>Name of the capture device to open</summary>
    public string DeviceName;

    /// <summary>Audio sample rate in Hz</summary>
    public int SampleRate = 44100;

    /// <summary>OpenAL audio format (e.g., AL.AL_FORMAT_MONO16)</summary>
    public int Format = AL.AL_FORMAT_MONO16;

    /// <summary>Internal buffer size (in frames)</summary>
    public int BufferSize = 1024;

    /// <summary>Optional logging callback</summary>
    public Action<string> LogCallback;

    /// <summary>Callback invoked when captured audio data is available</summary>
    public Action<nint, int> DataCallback;
}

/// <summary>
/// Represents an OpenAL input capture device
/// </summary>
public unsafe class ALCaptureDevice
{
    IntPtr handle;
    readonly Action<string> Log;
    readonly int Format;
    readonly int BufferSize;
    readonly Action<nint, int> DataCallback;

    void* sampleBuffer;
    int bytesPerFrame;

    /// <summary>
    /// Opens a capture device with the specified settings
    /// </summary>
    /// <param name="settings">Capture device settings</param>
    /// <exception cref="ArgumentException">Thrown if DataCallback is not provided</exception>
    /// <exception cref="Exception">Thrown if the capture device fails to open</exception>
    public ALCaptureDevice(ALCaptureDeviceSettings settings)
    {
        if (settings.DataCallback == null)
            throw new ArgumentException("settings.DataCallback must be provided");

        Log = settings.LogCallback ?? Console.WriteLine;

        this.Format = settings.Format;
        this.BufferSize = settings.BufferSize;
        this.DataCallback = settings.DataCallback;

        handle = AL.CaptureOpenDevice(settings.DeviceName, (uint)settings.SampleRate, Format, settings.BufferSize);

        if (handle == IntPtr.Zero)
            throw new Exception($"[OpenAL] Failed to open the input device: {settings.DeviceName}");

        bytesPerFrame = AL.GetBytesPerFrame(Format);
        sampleBuffer = NativeMemory.Alloc((nuint)(BufferSize * bytesPerFrame));
    }

    /// <summary>
    /// Start capturing audio
    /// </summary>
    public void CaptureStart()
    {
        AL.CaptureStart(handle);
    }

    /// <summary>
    /// Stop capturing audio
    /// </summary>
    public void CaptureStop()
    {
        AL.CaptureStop(handle);
    }

    /// <summary>
    /// Poll for captured audio samples. Invokes DataCallback with audio samples
    /// </summary>
    public void Update()
    {
        // OpenAL calls this 'samples' but it's actually frames
        var sampleCount = AL.GetIntegerALC(handle, AL.ALC_CAPTURE_SAMPLES);

        // Ignore empty captures
        if (sampleCount == 0)
            return;

        // If there are 5000 samples available but bufferSize is only 1000, only read 1000 samples
        sampleCount = Math.Min(sampleCount, BufferSize);

        AL.CaptureSamples(handle, (nint)sampleBuffer, sampleCount);

        DataCallback((nint)sampleBuffer, sampleCount);
    }

    /// <summary>
    /// Close the capture device and free resources
    /// </summary>
    public void Close()
    {
        CaptureStop();

        AL.CaptureCloseDevice(handle);
        handle = IntPtr.Zero;

        NativeMemory.Free(sampleBuffer);
        sampleBuffer = null;
    }

#if DEBUG
    ~ALCaptureDevice()
    {
        Debug.Assert(handle == IntPtr.Zero);
    }
#endif
}