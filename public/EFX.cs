namespace OpenAL;

public static unsafe partial class AL
{
    public static uint GenEffect() => GenEffects(1)[0];
    public static uint[] GenEffects(int count)
    {
        var result = new uint[count];
        alGenEffects(count, result);
        return result;
    }
    
    public static void DeleteEffect(uint effect) => DeleteEffects([effect]);
    public static void DeleteEffects(ReadOnlySpan<uint> effects) => alDeleteEffects(effects.Length, effects);
    
    public static bool IsEffect(uint effect) => alIsEffect(effect);
    
    public static void Effecti(uint effect, int param, int iValue) => alEffecti(effect, param, iValue);
    
    public static void Effectiv(uint effect, int param, ReadOnlySpan<int> piValues) => alEffectiv(effect, param, piValues);
    
    public static void Effectf(uint effect, int param, float flValue) => alEffectf(effect, param, flValue);
    
    public static void Effectfv(uint effect, int param, ReadOnlySpan<float> pflValues) => alEffectfv(effect, param, pflValues);
    
    public static void GetEffecti(uint effect, int param, Span<int> iValue) => alGetEffecti(effect, param, iValue);
    
    public static void GetEffectiv(uint effect, int param, Span<int> piValues) => alGetEffectiv(effect, param, piValues);
    
    public static void GetEffectf(uint effect, int param, Span<float> flValue) => alGetEffectf(effect, param, flValue);
    
    public static void GetEffectfv(uint effect, int param, Span<float> pflValues) => alGetEffectfv(effect, param, pflValues);
    
    public static uint GenFilter() => GenFilters(1)[0];
    public static uint[] GenFilters(int count)
    {
        var result = new uint[count];
        alGenFilters(count, result);
        return result;
    }
    
    public static void DeleteFilter(uint filter) => DeleteFilters([filter]);
    public static void DeleteFilters(ReadOnlySpan<uint> filters) => alDeleteFilters(filters.Length, filters);
    
    public static bool IsFilter(uint filter) => alIsFilter(filter);
    
    public static void Filteri(uint filter, int param, int iValue) => alFilteri(filter, param, iValue);
    
    public static void Filteriv(uint filter, int param, ReadOnlySpan<int> piValues) => alFilteriv(filter, param, piValues);
    
    public static void Filterf(uint filter, int param, float flValue) => alFilterf(filter, param, flValue);
    
    public static void Filterfv(uint filter, int param, ReadOnlySpan<float> pflValues) => alFilterfv(filter, param, pflValues);
    
    public static void GetFilteri(uint filter, int param, Span<int> iValue) => alGetFilteri(filter, param, iValue);
    
    public static void GetFilteriv(uint filter, int param, Span<int> piValues) => alGetFilteriv(filter, param, piValues);
    
    public static void GetFilterf(uint filter, int param, Span<float> flValue) => alGetFilterf(filter, param, flValue);
    
    public static void GetFilterfv(uint filter, int param, Span<float> pflValues) => alGetFilterfv(filter, param, pflValues);
    
    public static uint GenAuxiliaryEffectSlot() => GenAuxiliaryEffectSlots(1)[0];
    public static uint[] GenAuxiliaryEffectSlots(int count)
    {
        var result = new uint[count];
        alGenAuxiliaryEffectSlots(count, result);
        return result;
    }
    
    public static void DeleteAuxiliaryEffectSlot(uint auxiliaryeffectslot) => DeleteAuxiliaryEffectSlots([auxiliaryeffectslot]);
    public static void DeleteAuxiliaryEffectSlots(ReadOnlySpan<uint> auxiliaryeffectslots) => alDeleteAuxiliaryEffectSlots(auxiliaryeffectslots.Length, auxiliaryeffectslots);
    
    public static bool IsAuxiliaryEffectSlot(uint effectslot) => alIsAuxiliaryEffectSlot(effectslot);
    
    public static void AuxiliaryEffectSloti(uint effectslot, int param, int iValue) => alAuxiliaryEffectSloti(effectslot, param, iValue);
    
    public static void AuxiliaryEffectSlotiv(uint effectslot, int param, ReadOnlySpan<int> piValues) => alAuxiliaryEffectSlotiv(effectslot, param, piValues);
    
    public static void AuxiliaryEffectSlotf(uint effectslot, int param, float flValue) => alAuxiliaryEffectSlotf(effectslot, param, flValue);
    
    public static void AuxiliaryEffectSlotfv(uint effectslot, int param, ReadOnlySpan<float> pflValues) => alAuxiliaryEffectSlotfv(effectslot, param, pflValues);
    
    public static void GetAuxiliaryEffectSloti(uint effectslot, int param, Span<int> iValue) => alGetAuxiliaryEffectSloti(effectslot, param, iValue);
    
    public static void GetAuxiliaryEffectSlotiv(uint effectslot, int param, Span<int> piValues) => alGetAuxiliaryEffectSlotiv(effectslot, param, piValues);
    
    public static void GetAuxiliaryEffectSlotf(uint effectslot, int param, Span<float> flValue) => alGetAuxiliaryEffectSlotf(effectslot, param, flValue);
    
    public static void GetAuxiliaryEffectSlotfv(uint effectslot, int param, Span<float> pflValues) => alGetAuxiliaryEffectSlotfv(effectslot, param, pflValues);
    
}
