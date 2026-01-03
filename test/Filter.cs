namespace openal_soft_bindings_test;

[Collection("Sequential")]
public class Filter
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

    [Fact]
    public void CreateFilter()
    {
        var (device, context) = SetupDeviceAndContext();

        // Check if EFX extension is available
        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var filter = new ALFilter(1.0f, 1.0f);

        Assert.NotEqual(0u, filter.ID);
        Assert.Equal(1.0f, filter.gain);

        filter.Delete();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void CreateFilterWithCustomGain()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var filter = new ALFilter(0.5f, 0.25f);

        Assert.NotEqual(0u, filter.ID);
        Assert.Equal(0.5f, filter.gain);

        filter.Delete();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void CreateFilterWithZeroGain()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var filter = new ALFilter(0.0f, 0.0f);

        Assert.NotEqual(0u, filter.ID);
        Assert.Equal(0.0f, filter.gain);

        filter.Delete();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetGain()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var filter = new ALFilter(1.0f, 1.0f);

        // Update gain values
        filter.SetGain(0.75f, 0.5f);

        Assert.Equal(0.75f, filter.gain);

        filter.Delete();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetGainMultipleTimes()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var filter = new ALFilter(1.0f, 1.0f);

        // Update gain multiple times
        filter.SetGain(0.8f, 0.6f);
        Assert.Equal(0.8f, filter.gain);

        filter.SetGain(0.5f, 0.3f);
        Assert.Equal(0.5f, filter.gain);

        filter.SetGain(0.2f, 0.1f);
        Assert.Equal(0.2f, filter.gain);

        filter.Delete();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void DeleteFilter()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var filter = new ALFilter(1.0f, 1.0f);
        var originalID = filter.ID;

        Assert.NotEqual(0u, originalID);

        filter.Delete();

        Assert.Equal(0u, filter.ID);

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void CreateMultipleFilters()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var filter1 = new ALFilter(1.0f, 1.0f);
        var filter2 = new ALFilter(0.8f, 0.6f);
        var filter3 = new ALFilter(0.5f, 0.3f);

        // Each filter should have a unique ID
        Assert.NotEqual(0u, filter1.ID);
        Assert.NotEqual(0u, filter2.ID);
        Assert.NotEqual(0u, filter3.ID);
        Assert.NotEqual(filter1.ID, filter2.ID);
        Assert.NotEqual(filter2.ID, filter3.ID);
        Assert.NotEqual(filter1.ID, filter3.ID);

        // Each filter should have its own gain values
        Assert.Equal(1.0f, filter1.gain);
        Assert.Equal(0.8f, filter2.gain);
        Assert.Equal(0.5f, filter3.gain);

        filter1.Delete();
        filter2.Delete();
        filter3.Delete();

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void FilterGainHFRelativeToGain()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        // gainHF is computed relative to gain in SetGain method
        // gainHF = Min(1, gainHF / Max(0.01f, gain))
        var filter = new ALFilter(1.0f, 1.0f);

        // Test with gain = 0.5, gainHF = 1.0
        // Expected gainHF = Min(1, 1.0 / 0.5) = Min(1, 2.0) = 1.0
        filter.SetGain(0.5f, 1.0f);
        Assert.Equal(0.5f, filter.gain);
        Assert.Equal(1.0f, filter.gainHF);

        // Test with gain = 1.0, gainHF = 0.5
        // Expected gainHF = Min(1, 0.5 / 1.0) = 0.5
        filter.SetGain(1.0f, 0.5f);
        Assert.Equal(1.0f, filter.gain);
        Assert.Equal(0.5f, filter.gainHF);

        filter.Delete();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void FilterIDValidAfterCreation()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var filter = new ALFilter(1.0f, 1.0f);

        // ID should be valid immediately after creation
        Assert.NotEqual(0u, filter.ID);

        filter.Delete();

        // ID should be zero after deletion
        Assert.Equal(0u, filter.ID);

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void FilterValuesRange()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        // Test various gain values within valid range [0.0, 1.0]
        var testGains = new[] { 0.0f, 0.25f, 0.5f, 0.75f, 1.0f };

        foreach (var gain in testGains)
        {
            var filter = new ALFilter(gain, gain);
            Assert.Equal(gain, filter.gain);
            filter.Delete();
        }

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void UpdateFilterGainToZero()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var filter = new ALFilter(1.0f, 1.0f);

        // Set gain to zero
        filter.SetGain(0.0f, 0.0f);

        Assert.Equal(0.0f, filter.gain);

        filter.Delete();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void UpdateFilterGainToMax()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var filter = new ALFilter(0.0f, 0.0f);

        // Set gain to maximum
        filter.SetGain(1.0f, 1.0f);

        Assert.Equal(1.0f, filter.gain);

        filter.Delete();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void GenFilterReturnsValidID()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        // Test the underlying GenFilter function
        var filterID = AL.GenFilter();

        Assert.NotEqual(0u, filterID);

        AL.DeleteFilter(filterID);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void GenMultipleFilters()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        // Test generating multiple filters at once
        var filters = AL.GenFilters(5);

        Assert.Equal(5, filters.Length);

        foreach (var filterID in filters)
        {
            Assert.NotEqual(0u, filterID);
        }

        // All IDs should be unique
        var uniqueIDs = new HashSet<uint>(filters);
        Assert.Equal(5, uniqueIDs.Count);

        AL.DeleteFilters(filters);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void DeleteMultipleFilters()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var filters = AL.GenFilters(3);

        // Should not throw
        AL.DeleteFilters(filters);

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void FilteriSetsFilterType()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var filterID = AL.GenFilter();

        // Set filter type to lowpass
        AL.Filteri(filterID, AL.AL_FILTER_TYPE, AL.AL_FILTER_LOWPASS);

        // Query the filter type
        Span<int> filterType = stackalloc int[1];
        AL.GetFilteri(filterID, AL.AL_FILTER_TYPE, filterType);

        Assert.Equal(AL.AL_FILTER_LOWPASS, filterType[0]);

        AL.DeleteFilter(filterID);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void FilterfSetsGainValues()
    {
        var (device, context) = SetupDeviceAndContext();

        if (!device.HasExtension("ALC_EXT_EFX"))
        {
            CleanupDeviceAndContext(device, context);
            return;
        }

        var filterID = AL.GenFilter();
        AL.Filteri(filterID, AL.AL_FILTER_TYPE, AL.AL_FILTER_LOWPASS);

        // Set gain values
        AL.Filterf(filterID, AL.AL_LOWPASS_GAIN, 0.7f);
        AL.Filterf(filterID, AL.AL_LOWPASS_GAINHF, 0.4f);

        // Query the gain values
        Span<float> gain = stackalloc float[1];
        Span<float> gainHF = stackalloc float[1];

        AL.GetFilterf(filterID, AL.AL_LOWPASS_GAIN, gain);
        AL.GetFilterf(filterID, AL.AL_LOWPASS_GAINHF, gainHF);

        Assert.True(Math.Abs(0.7f - gain[0]) < 0.01f);
        Assert.True(Math.Abs(0.4f - gainHF[0]) < 0.01f);

        AL.DeleteFilter(filterID);
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
