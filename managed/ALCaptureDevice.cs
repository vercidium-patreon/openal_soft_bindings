namespace OpenAL.managed;

public unsafe class ALCaptureDevice
{
    IntPtr handle;
    readonly Action<string> Log;
    readonly int Format;
    readonly int BufferSize;
    readonly Action<nint, int> DataCallback;

    void* sampleBuffer;
    int bytesPerFrame;

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

    public void CaptureStart()
    {
        AL.CaptureStart(handle);
    }

    public void CaptureStop()
    {
        AL.CaptureStop(handle);
    }

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

    public void Close()
    {
        CaptureStop();
        AL.CaptureCloseDevice(handle);
        handle = 0;

        NativeMemory.Free(sampleBuffer);
        sampleBuffer = null;
    }
}

public class ALCaptureDeviceSettings
{
    public string DeviceName;
    public int SampleRate = 44100;
    public int Format = AL.AL_FORMAT_MONO16;
    public int BufferSize = 1024;
    public Action<string> LogCallback;
    public Action<nint, int> DataCallback;
}