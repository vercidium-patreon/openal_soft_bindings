namespace OpenAL.managed;

public unsafe class ALCaptureDevice
{
    IntPtr handle;
    readonly Action<string> Log;
    readonly int BufferSize;
    readonly int InputThreshold;
    readonly Action<nint, int> DataCallback;

    short* sampleBuffer;

    public ALCaptureDevice(ALCaptureDeviceSettings settings)
    {
        if (settings.DataCallback == null)
            throw new ArgumentException("settings.DataCallback must be provided");

        Log = settings.LogCallback ?? Console.WriteLine;

        this.BufferSize = settings.BufferSize;
        this.InputThreshold = settings.InputThreshold;
        this.DataCallback = settings.DataCallback;

        handle = AL.CaptureOpenDevice(settings.DeviceName, (uint)settings.SampleRate, settings.Format, BufferSize);

        if (handle == IntPtr.Zero)
            throw new Exception($"[OpenAL] Failed to open the input device: {settings.DeviceName}");

        sampleBuffer = (short*)NativeMemory.Alloc((nuint)(BufferSize * sizeof(short)));
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
        var sampleCount = AL.GetIntegerALC(handle, AL.ALC_CAPTURE_SAMPLES);

        Debug.Assert(sampleCount <= BufferSize);

        // Sometimes there's no samples. Not sure why. It seems to only be on the first time this function runs
        if (sampleCount == 0)
            return;

        AL.CaptureSamples(handle, (nint)sampleBuffer, sampleCount);

        if (InputThreshold > 0)
        {
            int total = 0;
            var read = sampleBuffer;
            var end = sampleBuffer + sampleCount;

            while (read < end)
            {
                total += Math.Abs((int)(*read++));
            }

            // If silent (e.g. microphone is off)
            if (total < sampleCount * InputThreshold)
                return;
        }

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
    public int InputThreshold = 600;
    public Action<string> LogCallback;
    public Action<nint, int> DataCallback;
}