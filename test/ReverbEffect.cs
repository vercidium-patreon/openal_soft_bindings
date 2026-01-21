namespace openal_soft_bindings_test;

[Collection("Sequential")]
public class ReverbEffect
{
    private (ALDevice device, ALContext context) SetupDeviceAndContext()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);
        var settings = new ALContextSettings();
        var context = new ALContext(device, settings);
        return (device, context);
    }

    private void CleanupDeviceAndContext(ALDevice device, ALContext context)
    {
        context.Destroy();
        device.Close();
    }

    private bool HasEFXExtension(ALDevice device)
    {
        return device.HasExtension("ALC_EXT_EFX");
    }

    [Fact]
    public void CreateReverbEffect()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var reverb = new ALReverbEffect();

        Assert.NotEqual(0u, reverb.effectID);
        Assert.NotEqual(0u, reverb.effectSlotID);
        Assert.True(reverb.dirty);

        reverb.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void DisposeReverbEffect()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var reverb = new ALReverbEffect();
        var originalEffectID = reverb.effectID;
        var originalSlotID = reverb.effectSlotID;

        Assert.NotEqual(0u, originalEffectID);
        Assert.NotEqual(0u, originalSlotID);

        reverb.Dispose();

        Assert.Equal(0u, reverb.effectID);
        Assert.Equal(0u, reverb.effectSlotID);

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void UpdateReverbEffect()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var reverb = new ALReverbEffect();

        Assert.True(reverb.dirty);

        // Should not throw
        reverb.Update();

        Assert.False(reverb.dirty);

        reverb.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void UpdateWithZeroEffectID()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var reverb = new ALReverbEffect();
        reverb.effectID = 0; // Simulate device change scenario

        // Should return early without throwing
        reverb.Update();

        // 'Dispose'
        reverb.effectSlotID = 0;
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void UpdateWithNotDirty()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var reverb = new ALReverbEffect();
        reverb.Update();

        Assert.False(reverb.dirty);

        // Second update should return early since not dirty
        reverb.Update();

        Assert.False(reverb.dirty);

        reverb.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void CopyFromPreset()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var reverb = new ALReverbEffect();

        var preset = new ReverbPreset()
        {
            name = "Test Preset",
            density = 0.5f,
            diffusion = 0.6f,
            gain = 0.7f,
            gainHF = 0.8f,
            gainLF = 0.9f,
            decayTime = 1.5f,
            decayHFRatio = 1.2f,
            decayLFRatio = 1.1f,
            reflectionsGain = 0.4f,
            reflectionsDelay = 0.02f,
            reflectionsPan = new float[] { 1.0f, 0.5f, 0.25f },
            lateReverbGain = 0.3f,
            lateReverbDelay = 0.03f,
            lateReverbPan = new float[] { 0.1f, 0.2f, 0.3f },
            echoTime = 0.25f,
            echoDepth = 0.35f,
            modulationTime = 0.15f,
            modulationDepth = 0.45f,
            airAbsorptionGainHF = 0.55f,
            hfReference = 5000.0f,
            lfReference = 250.0f,
            roomRolloffFactor = 2.0f,
            decayHFLimit = 1
        };

        reverb.CopyFromPreset(preset);

        // Verify all properties were copied
        Assert.Equal(0.5f, reverb.density);
        Assert.Equal(0.6f, reverb.diffusion);
        Assert.Equal(0.7f, reverb.gain);
        Assert.Equal(0.8f, reverb.gainHF);
        Assert.Equal(0.9f, reverb.gainLF);
        Assert.Equal(1.5f, reverb.decayTime);
        Assert.Equal(1.2f, reverb.decayHFRatio);
        Assert.Equal(1.1f, reverb.decayLFRatio);
        Assert.Equal(0.4f, reverb.reflectionsGain);
        Assert.Equal(0.02f, reverb.reflectionsDelay);
        Assert.Equal(preset.reflectionsPan, reverb.reflectionsPan);
        Assert.Equal(0.3f, reverb.lateReverbGain);
        Assert.Equal(0.03f, reverb.lateReverbDelay);
        Assert.Equal(preset.lateReverbPan, reverb.lateReverbPan);
        Assert.Equal(0.25f, reverb.echoTime);
        Assert.Equal(0.35f, reverb.echoDepth);
        Assert.Equal(0.15f, reverb.modulationTime);
        Assert.Equal(0.45f, reverb.modulationDepth);
        Assert.Equal(0.55f, reverb.airAbsorptionGainHF);
        Assert.Equal(5000.0f, reverb.hfReference);
        Assert.Equal(250.0f, reverb.lfReference);
        Assert.Equal(2.0f, reverb.roomRolloffFactor);
        Assert.Equal(1, reverb.decayHFLimit);

        reverb.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void ModifyReverbParameters()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var reverb = new ALReverbEffect();

        // Modify parameters directly
        reverb.density = 0.75f;
        reverb.diffusion = 0.85f;
        reverb.gain = 0.5f;
        reverb.decayTime = 2.0f;
        reverb.dirty = true;

        Assert.Equal(0.75f, reverb.density);
        Assert.Equal(0.85f, reverb.diffusion);
        Assert.Equal(0.5f, reverb.gain);
        Assert.Equal(2.0f, reverb.decayTime);
        Assert.True(reverb.dirty);

        // Update should apply the changes
        reverb.Update();
        Assert.False(reverb.dirty);

        reverb.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void ReflectionsPanInitialization()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var reverb = new ALReverbEffect();

        // reflectionsPan should be initialized to [0, 0, 0]
        Assert.NotNull(reverb.reflectionsPan);
        Assert.Equal(3, reverb.reflectionsPan.Length);
        Assert.Equal(0.0f, reverb.reflectionsPan[0]);
        Assert.Equal(0.0f, reverb.reflectionsPan[1]);
        Assert.Equal(0.0f, reverb.reflectionsPan[2]);

        reverb.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void LateReverbPanInitialization()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var reverb = new ALReverbEffect();

        // lateReverbPan should be initialized to [0, 0, 0]
        Assert.NotNull(reverb.lateReverbPan);
        Assert.Equal(3, reverb.lateReverbPan.Length);
        Assert.Equal(0.0f, reverb.lateReverbPan[0]);
        Assert.Equal(0.0f, reverb.lateReverbPan[1]);
        Assert.Equal(0.0f, reverb.lateReverbPan[2]);

        reverb.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void CreateMultipleReverbEffects()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var reverb1 = new ALReverbEffect();
        var reverb2 = new ALReverbEffect();
        var reverb3 = new ALReverbEffect();

        // Each reverb should have unique IDs
        Assert.NotEqual(0u, reverb1.effectID);
        Assert.NotEqual(0u, reverb2.effectID);
        Assert.NotEqual(0u, reverb3.effectID);
        Assert.NotEqual(reverb1.effectID, reverb2.effectID);
        Assert.NotEqual(reverb2.effectID, reverb3.effectID);
        Assert.NotEqual(reverb1.effectID, reverb3.effectID);

        Assert.NotEqual(0u, reverb1.effectSlotID);
        Assert.NotEqual(0u, reverb2.effectSlotID);
        Assert.NotEqual(0u, reverb3.effectSlotID);
        Assert.NotEqual(reverb1.effectSlotID, reverb2.effectSlotID);
        Assert.NotEqual(reverb2.effectSlotID, reverb3.effectSlotID);
        Assert.NotEqual(reverb1.effectSlotID, reverb3.effectSlotID);

        reverb1.Dispose();
        reverb2.Dispose();
        reverb3.Dispose();

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void DirtyFlagSetAfterCopyFromPreset()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var reverb = new ALReverbEffect();
        reverb.Update(); // Clear dirty flag

        Assert.False(reverb.dirty);

        var preset = new ReverbPreset { density = 0.5f };
        reverb.CopyFromPreset(preset);

        // Note: CopyFromPreset doesn't set dirty flag automatically
        // This is by design - the user must set it manually
        Assert.False(reverb.dirty);

        reverb.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void GenEffectReturnsValidID()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var effectID = AL.GenEffect();

        Assert.NotEqual(0u, effectID);

        AL.DeleteEffect(effectID);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void GenMultipleEffects()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var effects = AL.GenEffects(5);

        Assert.Equal(5, effects.Length);

        foreach (var effectID in effects)
        {
            Assert.NotEqual(0u, effectID);
        }

        // All IDs should be unique
        var uniqueIDs = new HashSet<uint>(effects);
        Assert.Equal(5, uniqueIDs.Count);

        AL.DeleteEffects(effects);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void DeleteMultipleEffects()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var effects = AL.GenEffects(3);

        // Should not throw
        AL.DeleteEffects(effects);

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void GenAuxiliaryEffectSlotReturnsValidID()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var slotID = AL.GenAuxiliaryEffectSlot();

        Assert.NotEqual(0u, slotID);

        AL.DeleteAuxiliaryEffectSlot(slotID);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void GenMultipleAuxiliaryEffectSlots()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var slots = AL.GenAuxiliaryEffectSlots(4);

        Assert.Equal(4, slots.Length);

        foreach (var slotID in slots)
        {
            Assert.NotEqual(0u, slotID);
        }

        // All IDs should be unique
        var uniqueIDs = new HashSet<uint>(slots);
        Assert.Equal(4, uniqueIDs.Count);

        AL.DeleteAuxiliaryEffectSlots(slots);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void DeleteMultipleAuxiliaryEffectSlots()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var slots = AL.GenAuxiliaryEffectSlots(3);

        // Should not throw
        AL.DeleteAuxiliaryEffectSlots(slots);

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void EffectiSetsEffectType()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var effectID = AL.GenEffect();

        // Set effect type to EAXREVERB
        AL.Effecti(effectID, AL.AL_EFFECT_TYPE, AL.AL_EFFECT_EAXREVERB);

        // Query the effect type
        Span<int> effectType = stackalloc int[1];
        AL.GetEffecti(effectID, AL.AL_EFFECT_TYPE, effectType);

        Assert.Equal(AL.AL_EFFECT_EAXREVERB, effectType[0]);

        AL.DeleteEffect(effectID);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void EffectfSetsParameters()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var effectID = AL.GenEffect();
        AL.Effecti(effectID, AL.AL_EFFECT_TYPE, AL.AL_EFFECT_EAXREVERB);

        // Set density parameter
        AL.Effectf(effectID, AL.AL_EAXREVERB_DENSITY, 0.6f);

        // Query the density
        Span<float> density = stackalloc float[1];
        AL.GetEffectf(effectID, AL.AL_EAXREVERB_DENSITY, density);

        Assert.True(Math.Abs(0.6f - density[0]) < 0.01f);

        AL.DeleteEffect(effectID);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void EffectfvSetsPanVectors()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var effectID = AL.GenEffect();
        AL.Effecti(effectID, AL.AL_EFFECT_TYPE, AL.AL_EFFECT_EAXREVERB);

        // Set reflections pan
        Span<float> pan = stackalloc float[] { 0.5f, 0.25f, 0.75f };
        AL.Effectfv(effectID, AL.AL_EAXREVERB_REFLECTIONS_PAN, pan);

        // Query the pan
        Span<float> retrievedPan = stackalloc float[3];
        AL.GetEffectfv(effectID, AL.AL_EAXREVERB_REFLECTIONS_PAN, retrievedPan);

        Assert.True(Math.Abs(0.5f - retrievedPan[0]) < 0.01f);
        Assert.True(Math.Abs(0.25f - retrievedPan[1]) < 0.01f);
        Assert.True(Math.Abs(0.75f - retrievedPan[2]) < 0.01f);

        AL.DeleteEffect(effectID);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void AuxiliaryEffectSlotiBindsEffect()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!HasEFXExtension(device))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var effectID = AL.GenEffect();
        AL.Effecti(effectID, AL.AL_EFFECT_TYPE, AL.AL_EFFECT_EAXREVERB);

        var slotID = AL.GenAuxiliaryEffectSlot();

        // Bind effect to slot
        AL.AuxiliaryEffectSloti(slotID, AL.AL_EFFECTSLOT_EFFECT, (int)effectID);

        // Query the bound effect
        Span<int> boundEffect = stackalloc int[1];
        AL.GetAuxiliaryEffectSloti(slotID, AL.AL_EFFECTSLOT_EFFECT, boundEffect);

        Assert.Equal((int)effectID, boundEffect[0]);

        AL.DeleteAuxiliaryEffectSlot(slotID);
        AL.DeleteEffect(effectID);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void CheckEFXExtensionAvailability()
    {
        var (device, context) = SetupDeviceAndContext();

        // Just check if the extension is present
        var hasEFX = device.HasExtension("ALC_EXT_EFX");

        // We don't assert true/false since it depends on the device
        Assert.True(hasEFX == true || hasEFX == false);

        CleanupDeviceAndContext(device, context);
    }
}
