using System.IO;
using System.Reflection;

namespace OpenAL;

public static unsafe partial class AL
{
    private const string nativeLibName = "soft_oal.dll";
    private static IntPtr libraryHandle;

    static AL()
    {
        // Diagnostic logging
        Logger.Log($"[openal_soft_bindings] OS Description: {RuntimeInformation.OSDescription}");
        Logger.Log($"[openal_soft_bindings] OS Architecture: {RuntimeInformation.OSArchitecture}");
        Logger.Log($"[openal_soft_bindings] Process Architecture: {RuntimeInformation.ProcessArchitecture}");
        Logger.Log($"[openal_soft_bindings] Framework Description: {RuntimeInformation.FrameworkDescription}");
        Logger.Log($"[openal_soft_bindings] Is Windows: {RuntimeInformation.IsOSPlatform(OSPlatform.Windows)}");
        Logger.Log($"[openal_soft_bindings] Is Linux: {RuntimeInformation.IsOSPlatform(OSPlatform.Linux)}");
        Logger.Log($"[openal_soft_bindings] Is OSX: {RuntimeInformation.IsOSPlatform(OSPlatform.OSX)}");

        string platformLibrary;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            platformLibrary = "soft_oal.dll";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            platformLibrary = "libopenal.so.1";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            platformLibrary = "libopenal.1.dylib";
        }
        else
        {
            string osDescription = RuntimeInformation.OSDescription;
            Logger.Error($"[openal_soft_bindings] Unknown platform: {osDescription}");
            throw new PlatformNotSupportedException($"Unsupported platform: {osDescription}");
        }

        Logger.Log($"[openal_soft_bindings] Selected library: {platformLibrary}");
        Logger.Log($"[openal_soft_bindings] Environment.CurrentDirectory: {Environment.CurrentDirectory}");
        Logger.Log($"[openal_soft_bindings] AppContext.BaseDirectory: {AppContext.BaseDirectory}");
        Logger.Log($"[openal_soft_bindings] Assembly.GetExecutingAssembly().Location: {Assembly.GetExecutingAssembly().Location}");

        if (NativeLibrary.TryLoad(platformLibrary, out libraryHandle))
        {
            Logger.Log($"[openal_soft_bindings] Successfully loaded: {platformLibrary}");
        }
        else
        {
            Logger.Error($"[openal_soft_bindings] Failed to load: {platformLibrary}");

            string ldLibraryPath = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
            if (!string.IsNullOrEmpty(ldLibraryPath))
                Logger.Error($"[openal_soft_bindings] LD_LIBRARY_PATH: {ldLibraryPath}");

            throw new DllNotFoundException($"[openal_soft_bindings] Unable to load {platformLibrary}. Make sure the library is in one of the search paths above.");
        }

        NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);
    }

    static bool logged = false;

    private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        // Avoid log spam
        var shouldLog = !logged;
        logged = true;

        if (shouldLog)
            Logger.Log($"[openal_soft_bindings] DllImportResolver called for: {libraryName}");

        if (libraryName == nativeLibName)
        {
            if (shouldLog)
                Logger.Log($"[openal_soft_bindings] Returning preloaded handle: {libraryHandle}");

            return libraryHandle;
        }

        if (shouldLog)
            Logger.Error($"[openal_soft_bindings] Returning null handle");

        return IntPtr.Zero;
    }

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alEnable(int capability);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alDisable(int capability);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool alIsEnabled(int capability);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alDopplerFactor(float value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alDopplerVelocity(float value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alDistanceModel(int distanceModel);

    [LibraryImport(nativeLibName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial string alGetString(int param);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetBooleanv(int param, Span<byte> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetIntegerv(int param, Span<int> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetFloatv(int param, Span<float> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetDoublev(int param, Span<double> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool alGetBoolean(int param);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial int alGetInteger(int param);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial float alGetFloat(int param);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial double alGetDouble(int param);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial int alGetError();

    [LibraryImport(nativeLibName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool alIsExtensionPresent(string extname);

    [LibraryImport(nativeLibName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr alGetProcAddress(string fname);

    [LibraryImport(nativeLibName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial int alGetEnumValue(string ename);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alListenerf(int param, float value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alListener3f(int param, float value1, float value2, float value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alListenerfv(int param, ReadOnlySpan<float> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alListeneri(int param, int value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alListener3i(int param, int value1, int value2, int value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alListeneriv(int param, ReadOnlySpan<int> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetListenerf(int param, out float value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetListener3f(int param, out float value1, out float value2, out float value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetListenerfv(int param, Span<float> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetListeneri(int param, out int value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetListener3i(int param, out int value1, out int value2, out int value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetListeneriv(int param, Span<int> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGenSources(int n, Span<uint> sources);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alDeleteSources(int n, ReadOnlySpan<uint> sources);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool alIsSource(uint source);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourcef(uint source, int param, float value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSource3f(uint source, int param, float value1, float value2, float value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourcefv(uint source, int param, ReadOnlySpan<float> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourcei(uint source, int param, int value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSource3i(uint source, int param, int value1, int value2, int value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourceiv(uint source, int param, ReadOnlySpan<int> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetSourcef(uint source, int param, out float value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetSource3f(uint source, int param, out float value1, out float value2, out float value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetSourcefv(uint source, int param, Span<float> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetSourcei(uint source, int param, out int value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetSource3i(uint source, int param, out int value1, out int value2, out int value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetSourceiv(uint source, int param, Span<int> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourcePlay(uint source);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourceStop(uint source);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourceRewind(uint source);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourcePause(uint source);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourcePlayv(int n, ReadOnlySpan<uint> sources);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourceStopv(int n, ReadOnlySpan<uint> sources);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourceRewindv(int n, ReadOnlySpan<uint> sources);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourcePausev(int n, ReadOnlySpan<uint> sources);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourceQueueBuffers(uint source, int nb, ReadOnlySpan<uint> buffers);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourceUnqueueBuffers(uint source, int nb, ReadOnlySpan<uint> buffers);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGenBuffers(int n, Span<uint> buffers);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alDeleteBuffers(int n, ReadOnlySpan<uint> buffers);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool alIsBuffer(uint buffer);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alBufferData(uint buffer, int format, ReadOnlySpan<byte> data, int size, int samplerate);

    [LibraryImport(nativeLibName, EntryPoint = "alBufferData")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alBufferDataPtr(uint buffer, int format, nint data, int size, int samplerate);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alBufferf(uint buffer, int param, float value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alBuffer3f(uint buffer, int param, float value1, float value2, float value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alBufferfv(uint buffer, int param, ReadOnlySpan<float> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alBufferi(uint buffer, int param, int value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alBuffer3i(uint buffer, int param, int value1, int value2, int value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alBufferiv(uint buffer, int param, ReadOnlySpan<int> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetBufferf(uint buffer, int param, out float value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetBuffer3f(uint buffer, int param, out float value1, out float value2, out float value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetBufferfv(uint buffer, int param, Span<float> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetBufferi(uint buffer, int param, out int value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetBuffer3i(uint buffer, int param, out int value1, out int value2, out int value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetBufferiv(uint buffer, int param, Span<int> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSpeedOfSound(float value);

}
