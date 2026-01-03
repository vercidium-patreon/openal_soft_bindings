namespace OpenAL.managed;

/// <summary>
/// Represents an OpenAL EAX reverb effect
/// </summary>
public class ALReverbEffect
{
    /// <summary>
    /// Create a new reverb effect with an effect slot
    /// </summary>
    public ALReverbEffect()
    {
        // Create an effect handle for reverb
        effectID = AL.GenEffect();
        AL.Effecti(effectID, AL.AL_EFFECT_TYPE, AL.AL_EFFECT_EAXREVERB);

        // Create an effect slot for reverb
        effectSlotID = AL.GenAuxiliaryEffectSlot();

        // Apply the effect to the slot
        AL.AuxiliaryEffectSloti(effectSlotID, AL.AL_EFFECTSLOT_EFFECT, (int)effectID);
    }

    /// <summary>
    /// Copy all reverb parameters from a preset
    /// </summary>
    /// <param name="preset">The reverb preset to copy from</param>
    public void CopyFromPreset(ReverbPreset preset)
    {
        density = preset.density;
        diffusion = preset.diffusion;
        gain = preset.gain;
        gainHF = preset.gainHF;
        gainLF = preset.gainLF;
        decayTime = preset.decayTime;
        decayHFRatio = preset.decayHFRatio;
        decayLFRatio = preset.decayLFRatio;
        reflectionsGain = preset.reflectionsGain;
        reflectionsDelay = preset.reflectionsDelay;
        reflectionsPan = preset.reflectionsPan;
        lateReverbGain = preset.lateReverbGain;
        lateReverbDelay = preset.lateReverbDelay;
        lateReverbPan = preset.lateReverbPan;
        echoTime = preset.echoTime;
        echoDepth = preset.echoDepth;
        modulationTime = preset.modulationTime;
        modulationDepth = preset.modulationDepth;
        airAbsorptionGainHF = preset.airAbsorptionGainHF;
        hfReference = preset.hfReference;
        lfReference = preset.lfReference;
        roomRolloffFactor = preset.roomRolloffFactor;
        decayHFLimit = preset.decayHFLimit;
    }

    /// <summary>
    /// Apply pending reverb parameter changes to the effect
    /// </summary>
    public void Update()
    {
        // If we're changing audio devices, we'll be null for a split second
        if (effectID == 0)
            return;

        if (!dirty)
            return;

        AL.Effecti(effectID, AL.AL_EFFECT_TYPE, AL.AL_EFFECT_EAXREVERB);
        AL.Effectf(effectID, AL.AL_EAXREVERB_DENSITY, density);
        AL.Effectf(effectID, AL.AL_EAXREVERB_DIFFUSION, diffusion);
        AL.Effectf(effectID, AL.AL_EAXREVERB_GAIN, gain);
        AL.Effectf(effectID, AL.AL_EAXREVERB_GAINHF, gainHF);
        AL.Effectf(effectID, AL.AL_EAXREVERB_GAINLF, gainLF);
        AL.Effectf(effectID, AL.AL_EAXREVERB_DECAY_TIME, decayTime);
        AL.Effectf(effectID, AL.AL_EAXREVERB_DECAY_HFRATIO, decayHFRatio);
        AL.Effectf(effectID, AL.AL_EAXREVERB_DECAY_LFRATIO, decayLFRatio);
        AL.Effectf(effectID, AL.AL_EAXREVERB_REFLECTIONS_GAIN, reflectionsGain);
        AL.Effectf(effectID, AL.AL_EAXREVERB_REFLECTIONS_DELAY, reflectionsDelay);
        AL.Effectfv(effectID, AL.AL_EAXREVERB_REFLECTIONS_PAN, reflectionsPan);
        AL.Effectf(effectID, AL.AL_EAXREVERB_LATE_REVERB_GAIN, lateReverbGain);
        AL.Effectf(effectID, AL.AL_EAXREVERB_LATE_REVERB_DELAY, lateReverbDelay);
        AL.Effectfv(effectID, AL.AL_EAXREVERB_LATE_REVERB_PAN, lateReverbPan);
        AL.Effectf(effectID, AL.AL_EAXREVERB_ECHO_TIME, echoTime);
        AL.Effectf(effectID, AL.AL_EAXREVERB_ECHO_DEPTH, echoDepth);
        AL.Effectf(effectID, AL.AL_EAXREVERB_MODULATION_DEPTH, modulationDepth);
        AL.Effectf(effectID, AL.AL_EAXREVERB_AIR_ABSORPTION_GAINHF, airAbsorptionGainHF);
        AL.Effectf(effectID, AL.AL_EAXREVERB_HFREFERENCE, hfReference);
        AL.Effectf(effectID, AL.AL_EAXREVERB_LFREFERENCE, lfReference);
        AL.Effectf(effectID, AL.AL_EAXREVERB_ROOM_ROLLOFF_FACTOR, roomRolloffFactor);
        AL.Effecti(effectID, AL.AL_EAXREVERB_DECAY_HFLIMIT, decayHFLimit);

        AL.AuxiliaryEffectSloti(effectSlotID, AL.AL_EFFECTSLOT_EFFECT, (int)effectID);

        dirty = false;
    }

    /// <summary>
    /// Dispose the reverb effect and release its resources
    /// </summary>
    public void Dispose()
    {
        Debug.Assert(effectID != 0);
        Debug.Assert(effectSlotID != 0);

        // Delete in reverse order
        AL.DeleteAuxiliaryEffectSlot(effectSlotID);
        effectSlotID = 0;

        AL.DeleteEffect(effectID);
        effectID = 0;
    }

    /// <summary>
    /// OpenAL effect ID
    /// </summary>
    public uint effectID;

    /// <summary>
    /// OpenAL auxiliary effect slot ID
    /// </summary>
    public uint effectSlotID;

    /// <summary>
    /// Whether parameters have been modified and need updating
    /// </summary>
    public bool dirty = true;

    /// <summary>Modal density of the reverb tail</summary>
    public float density;

    /// <summary>Diffusion of sound energy throughout the reverb field</summary>
    public float diffusion;

    /// <summary>Master output level for the reverb effect</summary>
    public float gain;

    /// <summary>High-frequency damping factor</summary>
    public float gainHF;

    /// <summary>Low-frequency gain adjustment</summary>
    public float gainLF;

    /// <summary>Reverb decay time in seconds</summary>
    public float decayTime;

    /// <summary>High-frequency decay ratio</summary>
    public float decayHFRatio;

    /// <summary>Low-frequency decay ratio</summary>
    public float decayLFRatio;

    /// <summary>Early reflections gain</summary>
    public float reflectionsGain;

    /// <summary>Early reflections delay</summary>
    public float reflectionsDelay;

    /// <summary>Early reflections panning vector</summary>
    public float[] reflectionsPan = [0, 0, 0];

    /// <summary>Late reverb gain</summary>
    public float lateReverbGain;

    /// <summary>Late reverb delay</summary>
    public float lateReverbDelay;

    /// <summary>Late reverb panning vector</summary>
    public float[] lateReverbPan = [0, 0, 0];

    /// <summary>Echo time</summary>
    public float echoTime;

    /// <summary>Echo depth</summary>
    public float echoDepth;

    /// <summary>Modulation time</summary>
    public float modulationTime;

    /// <summary>Modulation depth</summary>
    public float modulationDepth;

    /// <summary>Air absorption gain for high frequencies</summary>
    public float airAbsorptionGainHF;

    /// <summary>High-frequency reference</summary>
    public float hfReference;

    /// <summary>Low-frequency reference</summary>
    public float lfReference;

    /// <summary>Room rolloff factor</summary>
    public float roomRolloffFactor;

    /// <summary>High-frequency decay limit flag</summary>
    public int decayHFLimit;

#if DEBUG
    ~ALReverbEffect()
    {
        Debug.Assert(effectID == 0);
        Debug.Assert(effectSlotID == 0);
    }
#endif
}