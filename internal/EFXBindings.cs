namespace OpenAL;

public static unsafe partial class AL
{
    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGenEffects(int n, Span<uint> effects);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alDeleteEffects(int n, ReadOnlySpan<uint> effects);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool alIsEffect(uint effect);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alEffecti(uint effect, int param, int iValue);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alEffectiv(uint effect, int param, ReadOnlySpan<int> piValues);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alEffectf(uint effect, int param, float flValue);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alEffectfv(uint effect, int param, ReadOnlySpan<float> pflValues);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetEffecti(uint effect, int param, Span<int> iValue);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetEffectiv(uint effect, int param, Span<int> piValues);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetEffectf(uint effect, int param, Span<float> flValue);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetEffectfv(uint effect, int param, Span<float> pflValues);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGenFilters(int n, Span<uint> filters);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alDeleteFilters(int n, ReadOnlySpan<uint> filters);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool alIsFilter(uint filter);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alFilteri(uint filter, int param, int iValue);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alFilteriv(uint filter, int param, ReadOnlySpan<int> piValues);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alFilterf(uint filter, int param, float flValue);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alFilterfv(uint filter, int param, ReadOnlySpan<float> pflValues);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetFilteri(uint filter, int param, Span<int> iValue);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetFilteriv(uint filter, int param, Span<int> piValues);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetFilterf(uint filter, int param, Span<float> flValue);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetFilterfv(uint filter, int param, Span<float> pflValues);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGenAuxiliaryEffectSlots(int n, Span<uint> effectslots);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alDeleteAuxiliaryEffectSlots(int n, ReadOnlySpan<uint> effectslots);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool alIsAuxiliaryEffectSlot(uint effectslot);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alAuxiliaryEffectSloti(uint effectslot, int param, int iValue);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alAuxiliaryEffectSlotiv(uint effectslot, int param, ReadOnlySpan<int> piValues);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alAuxiliaryEffectSlotf(uint effectslot, int param, float flValue);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alAuxiliaryEffectSlotfv(uint effectslot, int param, ReadOnlySpan<float> pflValues);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetAuxiliaryEffectSloti(uint effectslot, int param, Span<int> iValue);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetAuxiliaryEffectSlotiv(uint effectslot, int param, Span<int> piValues);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetAuxiliaryEffectSlotf(uint effectslot, int param, Span<float> flValue);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void alGetAuxiliaryEffectSlotfv(uint effectslot, int param, Span<float> pflValues);

}
