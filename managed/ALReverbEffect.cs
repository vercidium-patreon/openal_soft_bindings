namespace OpenAL.managed;

public class ALReverbEffect
{
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
        AL.Effectf(effectID, AL.AL_EAXREVERB_GAIN, Math.Min(1, gain)); // Sometimes it's above 1
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
        AL.Effectf(effectID, AL.AL_EAXREVERB_HFREFERENCE, hFReference);
        AL.Effectf(effectID, AL.AL_EAXREVERB_LFREFERENCE, lFReference);
        AL.Effectf(effectID, AL.AL_EAXREVERB_ROOM_ROLLOFF_FACTOR, roomRolloffFactor);
        AL.Effecti(effectID, AL.AL_EAXREVERB_DECAY_HFLIMIT, decayHFLimit);

        AL.AuxiliaryEffectSloti(effectSlotID, AL.AL_EFFECTSLOT_EFFECT, (int)effectID);

        dirty = false;
    }

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

    public uint effectID;
    public uint effectSlotID;

    public bool dirty = true;

    public float density;
    public float diffusion;
    public float gain;
    public float gainHF;
    public float gainLF;
    public float decayTime;
    public float decayHFRatio;
    public float decayLFRatio;
    public float reflectionsGain;
    public float reflectionsDelay;
    public float[] reflectionsPan = [0, 0, 0];
    public float lateReverbGain;
    public float lateReverbDelay;
    public float[] lateReverbPan = [0, 0, 0];
    public float echoTime;
    public float echoDepth;
    public float modulationTime;
    public float modulationDepth;
    public float airAbsorptionGainHF;
    public float hFReference;
    public float lFReference;
    public float roomRolloffFactor;
    public int decayHFLimit;
}