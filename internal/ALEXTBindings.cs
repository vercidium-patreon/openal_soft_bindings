namespace OpenAL;

public static unsafe partial class AL
{
    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool alcSetThreadContext(IntPtr context);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr alcGetThreadContext();

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alBufferSubDataSOFT(uint buffer, int format, ReadOnlySpan<byte> data, int offset, int length);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alRequestFoldbackStart(int mode, int count, int length, out float mem, IntPtr callback);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alRequestFoldbackStop();

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alBufferSamplesSOFT(uint buffer, uint samplerate, int internalformat, int samples, int channels, int type, ReadOnlySpan<byte> data);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alBufferSubSamplesSOFT(uint buffer, int offset, int samples, int channels, int type, ReadOnlySpan<byte> data);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetBufferSamplesSOFT(uint buffer, int offset, int samples, int channels, int type, Span<byte> data);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool alIsBufferFormatSupportedSOFT(int format);

    [LibraryImport(nativeLibName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr alcLoopbackOpenDeviceSOFT(string deviceName);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool alcIsRenderFormatSupportedSOFT(IntPtr device, int freq, int channels, int type);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alcRenderSamplesSOFT(IntPtr device, Span<byte> buffer, int samples);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourcedSOFT(uint source, int param, double value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSource3dSOFT(uint source, int param, double value1, double value2, double value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourcedvSOFT(uint source, int param, ReadOnlySpan<double> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetSourcedSOFT(uint source, int param, out double value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetSource3dSOFT(uint source, int param, out double value1, out double value2, out double value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetSourcedvSOFT(uint source, int param, out double values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourcei64SOFT(uint source, int param, long value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSource3i64SOFT(uint source, int param, long value1, long value2, long value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alSourcei64vSOFT(uint source, int param, ReadOnlySpan<long> values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetSourcei64SOFT(uint source, int param, out long value);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetSource3i64SOFT(uint source, int param, out long value1, out long value2, out long value3);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetSourcei64vSOFT(uint source, int param, out long values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alDeferUpdatesSOFT();

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alProcessUpdatesSOFT();

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alcDevicePauseSOFT(IntPtr device);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alcDeviceResumeSOFT(IntPtr device);

    [LibraryImport(nativeLibName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial string alcGetStringiSOFT(IntPtr device, int paramName, int index);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool alcResetDeviceSOFT(IntPtr device, ReadOnlySpan<int> attribs);

    [LibraryImport(nativeLibName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial string alGetStringiSOFT(int pname, int index);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alcGetInteger64vSOFT(IntPtr device, int pname, int size, out long values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alEventControlSOFT(int count, ReadOnlySpan<int> types, [MarshalAs(UnmanagedType.I1)] bool enable);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alEventCallbackSOFT(IntPtr callback, IntPtr userParam);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr alGetPointerSOFT(int pname);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetPointervSOFT(int pname, IntPtr values);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alBufferCallbackSOFT(uint buffer, int format, int freq, IntPtr callback, IntPtr userptr);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetBufferPtrSOFT(uint buffer, int param, Span<byte> ptr);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetBuffer3PtrSOFT(uint buffer, int param, IntPtr ptr0, IntPtr ptr1, IntPtr ptr2);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetBufferPtrvSOFT(uint buffer, int param, Span<byte> ptr);

}
