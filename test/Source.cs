namespace openal_soft_bindings_test;

[Collection("Sequential")]
public class Source
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

    private uint CreateTestBuffer()
    {
        // Create a simple test buffer with 1 second of silence (mono 16-bit at 44100Hz)
        var sampleRate = 44100;
        var samples = new short[sampleRate]; // 1 second of silence
        Array.Fill(samples, (short)0);

        var bufferID = AL.GenBuffer();
        AL.BufferData(bufferID, AL.AL_FORMAT_MONO16, samples, samples.Length * sizeof(short), sampleRate);

        return bufferID;
    }

    [Fact]
    public void CreateSource()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        Assert.NotEqual(0u, source.ID);
        Assert.Equal(sourceID, source.ID);
        Assert.False(source.IsDisposed());

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void DisposeSource()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);
        var originalID = source.ID;

        Assert.NotEqual(0u, originalID);

        source.Dispose();

        Assert.Equal(0u, source.ID);
        Assert.True(source.IsDisposed());

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void PlayAndStopSource()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);
        var bufferID = CreateTestBuffer();

        source.SetBuffer(bufferID);

        // Should not throw
        source.Play();
        var state = AL.GetSourcei(source.ID, AL.AL_SOURCE_STATE);
        Assert.Equal(AL.AL_PLAYING, state);

        source.Stop();
        state = AL.GetSourcei(source.ID, AL.AL_SOURCE_STATE);
        Assert.Equal(AL.AL_STOPPED, state);

        source.Dispose();
        AL.DeleteBuffer(bufferID);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetBuffer()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);
        var bufferID = CreateTestBuffer();

        source.SetBuffer(bufferID);

        var attachedBuffer = AL.GetSourcei(source.ID, AL.AL_BUFFER);
        Assert.Equal((int)bufferID, attachedBuffer);

        source.Dispose();
        AL.DeleteBuffer(bufferID);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetGain()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        source.SetGain(0.5f);

        var gain = AL.GetSourcef(source.ID, AL.AL_GAIN);
        Assert.True(Math.Abs(0.5f - gain) < 0.01f);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetPitch()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        source.SetPitch(1.5f);

        var pitch = AL.GetSourcef(source.ID, AL.AL_PITCH);
        Assert.True(Math.Abs(1.5f - pitch) < 0.01f);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetRelative()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        source.SetRelative(true);
        var relative = AL.GetSourcei(source.ID, AL.AL_SOURCE_RELATIVE);
        Assert.Equal(1, relative);

        source.SetRelative(false);
        relative = AL.GetSourcei(source.ID, AL.AL_SOURCE_RELATIVE);
        Assert.Equal(0, relative);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetSpatialise()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        source.SetSpatialise(true);
        var spatialise = AL.GetSourcei(source.ID, AL.AL_SOURCE_SPATIALIZE_SOFT);
        Assert.Equal(1, spatialise);

        source.SetSpatialise(false);
        spatialise = AL.GetSourcei(source.ID, AL.AL_SOURCE_SPATIALIZE_SOFT);
        Assert.Equal(0, spatialise);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetReferenceDistance()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        source.SetReferenceDistance(2.5f);

        var refDist = AL.GetSourcef(source.ID, AL.AL_REFERENCE_DISTANCE);
        Assert.True(Math.Abs(2.5f - refDist) < 0.01f);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetMaxDistance()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        source.SetMaxDistance(100.0f);

        var maxDist = AL.GetSourcef(source.ID, AL.AL_MAX_DISTANCE);
        Assert.True(Math.Abs(100.0f - maxDist) < 0.01f);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetAirAbsorptionFactor()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        source.SetAirAbsorptionFactor(0.5f);

        var absorption = AL.GetSourcef(source.ID, AL.AL_AIR_ABSORPTION_FACTOR);
        Assert.True(Math.Abs(0.5f - absorption) < 0.01f);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetRolloff()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        source.SetRolloff(2.0f);

        var rolloff = AL.GetSourcef(source.ID, AL.AL_ROLLOFF_FACTOR);
        Assert.True(Math.Abs(2.0f - rolloff) < 0.01f);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetPosition()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        Span<float> position = stackalloc float[] { 1.0f, 2.0f, 3.0f };
        source.SetPosition(position);

        Span<float> retrievedPos = stackalloc float[3];
        AL.GetSourcefv(source.ID, AL.AL_POSITION, retrievedPos);

        Assert.True(Math.Abs(1.0f - retrievedPos[0]) < 0.01f);
        Assert.True(Math.Abs(2.0f - retrievedPos[1]) < 0.01f);
        Assert.True(Math.Abs(3.0f - retrievedPos[2]) < 0.01f);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetVelocity()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        Span<float> velocity = stackalloc float[] { 0.5f, 0.25f, 0.1f };
        source.SetVelocity(velocity);

        Span<float> retrievedVel = stackalloc float[3];
        AL.GetSourcefv(source.ID, AL.AL_VELOCITY, retrievedVel);

        Assert.True(Math.Abs(0.5f - retrievedVel[0]) < 0.01f);
        Assert.True(Math.Abs(0.25f - retrievedVel[1]) < 0.01f);
        Assert.True(Math.Abs(0.1f - retrievedVel[2]) < 0.01f);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetDirection()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        Span<float> direction = stackalloc float[] { 1.0f, 0.0f, 0.0f };
        source.SetDirection(direction);

        Span<float> retrievedDir = stackalloc float[3];
        AL.GetSourcefv(source.ID, AL.AL_DIRECTION, retrievedDir);

        Assert.True(Math.Abs(1.0f - retrievedDir[0]) < 0.01f);
        Assert.True(Math.Abs(0.0f - retrievedDir[1]) < 0.01f);
        Assert.True(Math.Abs(0.0f - retrievedDir[2]) < 0.01f);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetLooping()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        source.SetLooping(true);
        Assert.True(source.looping);
        var looping = AL.GetSourcei(source.ID, AL.AL_LOOPING);
        Assert.Equal(1, looping);

        source.SetLooping(false);
        Assert.False(source.looping);
        looping = AL.GetSourcei(source.ID, AL.AL_LOOPING);
        Assert.Equal(0, looping);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetSecOffset()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);
        var bufferID = CreateTestBuffer();

        source.SetBuffer(bufferID);
        source.Play(); // Source must be playing for offset to be maintained
        source.SetSecOffset(0.5f);

        Assert.Equal(0.5f, source.secOffset);

        var offset = AL.GetSourcef(source.ID, AL.AL_SEC_OFFSET);
        Assert.True(Math.Abs(0.5f - offset) < 0.01f);

        source.Stop();
        source.Dispose();
        AL.DeleteBuffer(bufferID);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void FinishedReturnsFalseWhenLooping()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);
        var bufferID = CreateTestBuffer();

        source.SetBuffer(bufferID);
        source.SetLooping(true);
        source.Stop(); // Ensure it's stopped

        // Source should not be finished when looping, even if stopped
        Assert.False(source.Finished());

        source.Dispose();
        AL.DeleteBuffer(bufferID);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetFilterWithoutEFX()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        // Should not throw even if EFX is not available
        source.SetFilter(null, null, null);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void CreateMultipleSources()
    {
        var (device, context) = SetupDeviceAndContext();

        var source1 = new ALSource(AL.GenSource());
        var source2 = new ALSource(AL.GenSource());
        var source3 = new ALSource(AL.GenSource());

        // Each source should have a unique ID
        Assert.NotEqual(0u, source1.ID);
        Assert.NotEqual(0u, source2.ID);
        Assert.NotEqual(0u, source3.ID);
        Assert.NotEqual(source1.ID, source2.ID);
        Assert.NotEqual(source2.ID, source3.ID);
        Assert.NotEqual(source1.ID, source3.ID);

        source1.Dispose();
        source2.Dispose();
        source3.Dispose();

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void GenSourceReturnsValidID()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();

        Assert.NotEqual(0u, sourceID);

        AL.DeleteSource(sourceID);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void GenMultipleSources()
    {
        var (device, context) = SetupDeviceAndContext();

        var sources = AL.GenSources(5);

        Assert.Equal(5, sources.Length);

        foreach (var sourceID in sources)
        {
            Assert.NotEqual(0u, sourceID);
        }

        // All IDs should be unique
        var uniqueIDs = new HashSet<uint>(sources);
        Assert.Equal(5, uniqueIDs.Count);

        AL.DeleteSources(sources);
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void DeleteMultipleSources()
    {
        var (device, context) = SetupDeviceAndContext();

        var sources = AL.GenSources(3);

        // Should not throw
        AL.DeleteSources(sources);

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SourcePlayv()
    {
        var (device, context) = SetupDeviceAndContext();

        var bufferID = CreateTestBuffer();

        var source1 = new ALSource(AL.GenSource());
        var source2 = new ALSource(AL.GenSource());
        var source3 = new ALSource(AL.GenSource());

        source1.SetBuffer(bufferID);
        source2.SetBuffer(bufferID);
        source3.SetBuffer(bufferID);

        var sourceIDs = new uint[] { source1.ID, source2.ID, source3.ID };

        // Play all sources at once
        AL.SourcePlayv(sourceIDs);

        // All should be playing
        Assert.Equal(AL.AL_PLAYING, AL.GetSourcei(source1.ID, AL.AL_SOURCE_STATE));
        Assert.Equal(AL.AL_PLAYING, AL.GetSourcei(source2.ID, AL.AL_SOURCE_STATE));
        Assert.Equal(AL.AL_PLAYING, AL.GetSourcei(source3.ID, AL.AL_SOURCE_STATE));

        source1.Dispose();
        source2.Dispose();
        source3.Dispose();
        AL.DeleteBuffer(bufferID);

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SourceStopv()
    {
        var (device, context) = SetupDeviceAndContext();

        var bufferID = CreateTestBuffer();

        var source1 = new ALSource(AL.GenSource());
        var source2 = new ALSource(AL.GenSource());
        var source3 = new ALSource(AL.GenSource());

        source1.SetBuffer(bufferID);
        source2.SetBuffer(bufferID);
        source3.SetBuffer(bufferID);

        var sourceIDs = new uint[] { source1.ID, source2.ID, source3.ID };

        // Play then stop all sources at once
        AL.SourcePlayv(sourceIDs);
        AL.SourceStopv(sourceIDs);

        // All should be stopped
        Assert.Equal(AL.AL_STOPPED, AL.GetSourcei(source1.ID, AL.AL_SOURCE_STATE));
        Assert.Equal(AL.AL_STOPPED, AL.GetSourcei(source2.ID, AL.AL_SOURCE_STATE));
        Assert.Equal(AL.AL_STOPPED, AL.GetSourcei(source3.ID, AL.AL_SOURCE_STATE));

        source1.Dispose();
        source2.Dispose();
        source3.Dispose();
        AL.DeleteBuffer(bufferID);

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetVector3()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        Span<float> testVector = stackalloc float[] { 5.0f, 10.0f, 15.0f };
        source.SetVector3(AL.AL_POSITION, testVector);

        Span<float> retrievedVector = stackalloc float[3];
        AL.GetSourcefv(source.ID, AL.AL_POSITION, retrievedVector);

        Assert.True(Math.Abs(5.0f - retrievedVector[0]) < 0.01f);
        Assert.True(Math.Abs(10.0f - retrievedVector[1]) < 0.01f);
        Assert.True(Math.Abs(15.0f - retrievedVector[2]) < 0.01f);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SourceIDValidAfterCreation()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        // ID should be valid immediately after creation
        Assert.NotEqual(0u, source.ID);
        Assert.False(source.IsDisposed());

        source.Dispose();

        // ID should be zero after disposal
        Assert.Equal(0u, source.ID);
        Assert.True(source.IsDisposed());

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void SetMultiplePropertiesInSequence()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var source = new ALSource(sourceID);

        // Set multiple properties
        source.SetGain(0.8f);
        source.SetPitch(1.2f);
        source.SetRelative(true);
        source.SetLooping(true);
        Span<float> pos = stackalloc float[] { 1.0f, 2.0f, 3.0f };
        source.SetPosition(pos);

        // Verify all properties
        Assert.True(Math.Abs(0.8f - AL.GetSourcef(source.ID, AL.AL_GAIN)) < 0.01f);
        Assert.True(Math.Abs(1.2f - AL.GetSourcef(source.ID, AL.AL_PITCH)) < 0.01f);
        Assert.Equal(1, AL.GetSourcei(source.ID, AL.AL_SOURCE_RELATIVE));
        Assert.Equal(1, AL.GetSourcei(source.ID, AL.AL_LOOPING));
        Assert.True(source.looping);

        source.Dispose();
        CleanupDeviceAndContext(device, context);
    }
}
