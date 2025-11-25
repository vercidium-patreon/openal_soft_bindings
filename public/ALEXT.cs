namespace OpenAL;

public static unsafe partial class AL
{
    public static bool SetThreadContext(IntPtr context) => alcSetThreadContext(context);
    
    public static IntPtr GetThreadContext() => alcGetThreadContext();
    
    public static void BufferSubDataSOFT(uint buffer, int format, ReadOnlySpan<byte> data, int offset, int length) => alBufferSubDataSOFT(buffer, format, data, offset, length);
    
    public static void RequestFoldbackStart(int mode, int count, int length, out float mem, IntPtr callback) => alRequestFoldbackStart(mode, count, length, out mem, callback);
    
    public static void RequestFoldbackStop() => alRequestFoldbackStop();
    
    public static void BufferSamplesSOFT(uint buffer, uint samplerate, int internalformat, int samples, int channels, int type, ReadOnlySpan<byte> data) => alBufferSamplesSOFT(buffer, samplerate, internalformat, samples, channels, type, data);
    
    public static void BufferSubSamplesSOFT(uint buffer, int offset, int samples, int channels, int type, ReadOnlySpan<byte> data) => alBufferSubSamplesSOFT(buffer, offset, samples, channels, type, data);
    
    public static void GetBufferSamplesSOFT(uint buffer, int offset, int samples, int channels, int type, Span<byte> data) => alGetBufferSamplesSOFT(buffer, offset, samples, channels, type, data);
    
    public static bool IsBufferFormatSupportedSOFT(int format) => alIsBufferFormatSupportedSOFT(format);
    
    public static IntPtr LoopbackOpenDeviceSOFT(string deviceName) => alcLoopbackOpenDeviceSOFT(deviceName);
    
    public static bool IsRenderFormatSupportedSOFT(IntPtr device, int freq, int channels, int type) => alcIsRenderFormatSupportedSOFT(device, freq, channels, type);
    
    public static void RenderSamplesSOFT(IntPtr device, Span<byte> buffer, int samples) => alcRenderSamplesSOFT(device, buffer, samples);
    
    public static void SourcedSOFT(uint source, int param, double value) => alSourcedSOFT(source, param, value);
    
    public static void Source3dSOFT(uint source, int param, double value1, double value2, double value3) => alSource3dSOFT(source, param, value1, value2, value3);
    
    public static void SourcedvSOFT(uint source, int param, ReadOnlySpan<double> values) => alSourcedvSOFT(source, param, values);
    
    public static double GetSourcedSOFT(uint source, int param)
    {
        alGetSourcedSOFT(source, param, out double value);
        return value;
    }
    
    public static void GetSource3dSOFT(uint source, int param, out double value1, out double value2, out double value3) => alGetSource3dSOFT(source, param, out value1, out value2, out value3);
    
    public static double GetSourcedvSOFT(uint source, int param)
    {
        alGetSourcedvSOFT(source, param, out double value);
        return value;
    }
    
    public static void Sourcei64SOFT(uint source, int param, long value) => alSourcei64SOFT(source, param, value);
    
    public static void Source3i64SOFT(uint source, int param, long value1, long value2, long value3) => alSource3i64SOFT(source, param, value1, value2, value3);
    
    public static void Sourcei64vSOFT(uint source, int param, ReadOnlySpan<long> values) => alSourcei64vSOFT(source, param, values);
    
    public static long GetSourcei64SOFT(uint source, int param)
    {
        alGetSourcei64SOFT(source, param, out long value);
        return value;
    }
    
    public static void GetSource3i64SOFT(uint source, int param, out long value1, out long value2, out long value3) => alGetSource3i64SOFT(source, param, out value1, out value2, out value3);
    
    public static long GetSourcei64vSOFT(uint source, int param)
    {
        alGetSourcei64vSOFT(source, param, out long value);
        return value;
    }
    
    public static void DeferUpdatesSOFT() => alDeferUpdatesSOFT();
    
    public static void ProcessUpdatesSOFT() => alProcessUpdatesSOFT();
    
    public static void DevicePauseSOFT(IntPtr device) => alcDevicePauseSOFT(device);
    
    public static void DeviceResumeSOFT(IntPtr device) => alcDeviceResumeSOFT(device);
    
    public static string GetStringiSOFT(IntPtr device, int paramName, int index) => alcGetStringiSOFT(device, paramName, index);
    
    public static bool ResetDeviceSOFT(IntPtr device, ReadOnlySpan<int> attribs) => alcResetDeviceSOFT(device, attribs);
    
    public static string GetStringiSOFT(int pname, int index) => alGetStringiSOFT(pname, index);
    
    public static long GetInteger64vSOFT(IntPtr device, int pname, int size)
    {
        alcGetInteger64vSOFT(device, pname, size, out long value);
        return value;
    }
    
    public static void EventControlSOFT(int count, ReadOnlySpan<int> types, bool enable) => alEventControlSOFT(count, types, enable);
    
    public static void EventCallbackSOFT(IntPtr callback, IntPtr userParam) => alEventCallbackSOFT(callback, userParam);
    
    public static IntPtr GetPointerSOFT(int pname) => alGetPointerSOFT(pname);
    
    public static void GetPointervSOFT(int pname, IntPtr values) => alGetPointervSOFT(pname, values);
    
    public static void BufferCallbackSOFT(uint buffer, int format, int freq, IntPtr callback, IntPtr userptr) => alBufferCallbackSOFT(buffer, format, freq, callback, userptr);
    
    public static void GetBufferPtrSOFT(uint buffer, int param, Span<byte> ptr) => alGetBufferPtrSOFT(buffer, param, ptr);
    
    public static void GetBuffer3PtrSOFT(uint buffer, int param, IntPtr ptr0, IntPtr ptr1, IntPtr ptr2) => alGetBuffer3PtrSOFT(buffer, param, ptr0, ptr1, ptr2);
    
    public static void GetBufferPtrvSOFT(uint buffer, int param, Span<byte> ptr) => alGetBufferPtrvSOFT(buffer, param, ptr);
    
}
