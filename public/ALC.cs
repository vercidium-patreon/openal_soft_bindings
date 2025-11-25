namespace OpenAL;

public static unsafe partial class AL
{
    public static IntPtr CreateContext(IntPtr device, ReadOnlySpan<int> attrlist) => alcCreateContext(device, attrlist);
    
    public static bool MakeContextCurrent(IntPtr context) => alcMakeContextCurrent(context);
    
    public static void ProcessContext(IntPtr context) => alcProcessContext(context);
    
    public static void SuspendContext(IntPtr context) => alcSuspendContext(context);
    
    public static void DestroyContext(IntPtr context) => alcDestroyContext(context);
    
    public static IntPtr GetCurrentContext() => alcGetCurrentContext();
    
    public static IntPtr GetContextsDevice(IntPtr context) => alcGetContextsDevice(context);
    
    public static IntPtr OpenDevice(string devicename) => alcOpenDevice(devicename);
    
    public static bool CloseDevice(IntPtr device) => alcCloseDevice(device);
    
    public static int GetError(IntPtr device) => alcGetError(device);
    
    public static bool IsExtensionPresent(IntPtr device, string extname) => alcIsExtensionPresent(device, extname);
    
    public static IntPtr GetProcAddress(IntPtr device, string funcname) => alcGetProcAddress(device, funcname);
    
    public static int GetEnumValue(IntPtr device, string enumname) => alcGetEnumValue(device, enumname);
    
    public static IntPtr GetStringPtr(IntPtr device, int param) => alcGetStringPtr(device, param);
    
    public static string GetString(IntPtr device, int param) => alcGetString(device, param);
    
    public static int GetIntegerALC(IntPtr device, int param)
    {
        int value = 0;
        alcGetIntegerv(device, param, 1, new Span<int>(ref value));
        return value;
    }
    
    public static void GetIntegerv(IntPtr device, int param, int size, Span<int> values) => alcGetIntegerv(device, param, size, values);
    
    public static IntPtr CaptureOpenDevice(string devicename, uint frequency, int format, int buffersize) => alcCaptureOpenDevice(devicename, frequency, format, buffersize);
    
    public static bool CaptureCloseDevice(IntPtr device) => alcCaptureCloseDevice(device);
    
    public static void CaptureStart(IntPtr device) => alcCaptureStart(device);
    
    public static void CaptureStop(IntPtr device) => alcCaptureStop(device);
    
    public static void CaptureSamples(IntPtr device, nint buffer, int samples) => alcCaptureSamplesPtr(device, buffer, samples);
    
    public static void CaptureSamples(IntPtr device, Span<byte> buffer, int samples) => alcCaptureSamples(device, buffer, samples);
    
}
