namespace OpenAL;

public static unsafe partial class AL
{
    public static void Enable(int capability) => alEnable(capability);
    
    public static void Disable(int capability) => alDisable(capability);
    
    public static bool IsEnabled(int capability) => alIsEnabled(capability);
    
    public static void DopplerFactor(float value) => alDopplerFactor(value);
    
    public static void DopplerVelocity(float value) => alDopplerVelocity(value);
    
    public static void DistanceModel(int distanceModel) => alDistanceModel(distanceModel);
    
    public static string GetString(int param) => alGetString(param);
    
    public static void GetBooleanv(int param, Span<byte> values) => alGetBooleanv(param, values);
    
    public static void GetIntegerv(int param, Span<int> values) => alGetIntegerv(param, values);
    
    public static void GetFloatv(int param, Span<float> values) => alGetFloatv(param, values);
    
    public static void GetDoublev(int param, Span<double> values) => alGetDoublev(param, values);
    
    public static bool GetBoolean(int param) => alGetBoolean(param);
    
    public static int GetInteger(int param) => alGetInteger(param);
    
    public static float GetFloat(int param) => alGetFloat(param);
    
    public static double GetDouble(int param) => alGetDouble(param);
    
    public static int GetError() => alGetError();
    
    public static bool IsExtensionPresent(string extname) => alIsExtensionPresent(extname);
    
    public static IntPtr GetProcAddress(string fname) => alGetProcAddress(fname);
    
    public static int GetEnumValue(string ename) => alGetEnumValue(ename);
    
    public static void Listenerf(int param, float value) => alListenerf(param, value);
    
    public static void Listener3f(int param, float value1, float value2, float value3) => alListener3f(param, value1, value2, value3);
    
    public static void Listenerfv(int param, ReadOnlySpan<float> values) => alListenerfv(param, values);
    
    public static void Listeneri(int param, int value) => alListeneri(param, value);
    
    public static void Listener3i(int param, int value1, int value2, int value3) => alListener3i(param, value1, value2, value3);
    
    public static void Listeneriv(int param, ReadOnlySpan<int> values) => alListeneriv(param, values);
    
    public static float GetListenerf(int param)
    {
        alGetListenerf(param, out float value);
        return value;
    }
    
    public static void GetListener3f(int param, out float value1, out float value2, out float value3) => alGetListener3f(param, out value1, out value2, out value3);
    
    public static void GetListenerfv(int param, Span<float> values) => alGetListenerfv(param, values);
    
    public static int GetListeneri(int param)
    {
        alGetListeneri(param, out int value);
        return value;
    }
    
    public static void GetListener3i(int param, out int value1, out int value2, out int value3) => alGetListener3i(param, out value1, out value2, out value3);
    
    public static void GetListeneriv(int param, Span<int> values) => alGetListeneriv(param, values);
    
    public static uint GenSource() => GenSources(1)[0];
    public static uint[] GenSources(int count)
    {
        var result = new uint[count];
        alGenSources(count, result);
        return result;
    }
    
    public static void DeleteSource(uint source) => DeleteSources([source]);
    public static void DeleteSources(ReadOnlySpan<uint> sources) => alDeleteSources(sources.Length, sources);
    
    public static bool IsSource(uint source) => alIsSource(source);
    
    public static void Sourcef(uint source, int param, float value) => alSourcef(source, param, value);
    
    public static void Source3f(uint source, int param, float value1, float value2, float value3) => alSource3f(source, param, value1, value2, value3);
    
    public static void Sourcefv(uint source, int param, ReadOnlySpan<float> values) => alSourcefv(source, param, values);
    
    public static void Sourcei(uint source, int param, int value) => alSourcei(source, param, value);
    
    public static void Source3i(uint source, int param, int value1, int value2, int value3) => alSource3i(source, param, value1, value2, value3);
    
    public static void Sourceiv(uint source, int param, ReadOnlySpan<int> values) => alSourceiv(source, param, values);
    
    public static float GetSourcef(uint source, int param)
    {
        alGetSourcef(source, param, out float value);
        return value;
    }
    
    public static void GetSource3f(uint source, int param, out float value1, out float value2, out float value3) => alGetSource3f(source, param, out value1, out value2, out value3);
    
    public static void GetSourcefv(uint source, int param, Span<float> values) => alGetSourcefv(source, param, values);
    
    public static int GetSourcei(uint source, int param)
    {
        alGetSourcei(source, param, out int value);
        return value;
    }
    
    public static void GetSource3i(uint source, int param, out int value1, out int value2, out int value3) => alGetSource3i(source, param, out value1, out value2, out value3);
    
    public static void GetSourceiv(uint source, int param, Span<int> values) => alGetSourceiv(source, param, values);
    
    public static void SourcePlay(uint source) => alSourcePlay(source);
    
    public static void SourceStop(uint source) => alSourceStop(source);
    
    public static void SourceRewind(uint source) => alSourceRewind(source);
    
    public static void SourcePause(uint source) => alSourcePause(source);
    
    public static void SourcePlayv(ReadOnlySpan<uint> sources) => alSourcePlayv(sources.Length, sources);
    
    public static void SourceStopv(ReadOnlySpan<uint> sources) => alSourceStopv(sources.Length, sources);
    
    public static void SourceRewindv(ReadOnlySpan<uint> sources) => alSourceRewindv(sources.Length, sources);
    
    public static void SourcePausev(ReadOnlySpan<uint> sources) => alSourcePausev(sources.Length, sources);
    
    public static void SourceQueueBuffers(uint source, int nb, ReadOnlySpan<uint> buffers) => alSourceQueueBuffers(source, nb, buffers);
    
    public static void SourceUnqueueBuffers(uint source, int nb, ReadOnlySpan<uint> buffers) => alSourceUnqueueBuffers(source, nb, buffers);
    
    public static uint GenBuffer() => GenBuffers(1)[0];
    public static uint[] GenBuffers(int count)
    {
        var result = new uint[count];
        alGenBuffers(count, result);
        return result;
    }
    
    public static void DeleteBuffer(uint buffer) => DeleteBuffers([buffer]);
    public static void DeleteBuffers(ReadOnlySpan<uint> buffers) => alDeleteBuffers(buffers.Length, buffers);
    
    public static bool IsBuffer(uint buffer) => alIsBuffer(buffer);
    
    public static void BufferData(uint buffer, int format, nint data, int size, int samplerate) => alBufferDataPtr(buffer, format, data, size, samplerate);
    
    public static void BufferData(uint buffer, int format, ReadOnlySpan<byte> data, int size, int samplerate) => alBufferData(buffer, format, data, size, samplerate);
    
    public static void Bufferf(uint buffer, int param, float value) => alBufferf(buffer, param, value);
    
    public static void Buffer3f(uint buffer, int param, float value1, float value2, float value3) => alBuffer3f(buffer, param, value1, value2, value3);
    
    public static void Bufferfv(uint buffer, int param, ReadOnlySpan<float> values) => alBufferfv(buffer, param, values);
    
    public static void Bufferi(uint buffer, int param, int value) => alBufferi(buffer, param, value);
    
    public static void Buffer3i(uint buffer, int param, int value1, int value2, int value3) => alBuffer3i(buffer, param, value1, value2, value3);
    
    public static void Bufferiv(uint buffer, int param, ReadOnlySpan<int> values) => alBufferiv(buffer, param, values);
    
    public static float GetBufferf(uint buffer, int param)
    {
        alGetBufferf(buffer, param, out float value);
        return value;
    }
    
    public static void GetBuffer3f(uint buffer, int param, out float value1, out float value2, out float value3) => alGetBuffer3f(buffer, param, out value1, out value2, out value3);
    
    public static void GetBufferfv(uint buffer, int param, Span<float> values) => alGetBufferfv(buffer, param, values);
    
    public static int GetBufferi(uint buffer, int param)
    {
        alGetBufferi(buffer, param, out int value);
        return value;
    }
    
    public static void GetBuffer3i(uint buffer, int param, out int value1, out int value2, out int value3) => alGetBuffer3i(buffer, param, out value1, out value2, out value3);
    
    public static void GetBufferiv(uint buffer, int param, Span<int> values) => alGetBufferiv(buffer, param, values);
    
    public static void SpeedOfSound(float value) => alSpeedOfSound(value);
    
}
