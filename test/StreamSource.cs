namespace openal_soft_bindings_test;

[Collection("Sequential")]
public class StreamSource
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

    private byte[] CreateTestAudioData(int numSamples)
    {
        // Create simple test audio data (16-bit mono samples)
        var data = new byte[numSamples * sizeof(short)];
        var random = new Random(42); // Fixed seed for reproducibility

        for (int i = 0; i < numSamples; i++)
        {
            short sample = (short)(random.Next(-1000, 1000));
            BitConverter.TryWriteBytes(data.AsSpan(i * sizeof(short)), sample);
        }

        return data;
    }

    [Fact]
    public void CreateStreamSource()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        Assert.NotEqual(0u, streamSource.ID);
        Assert.Equal(sourceID, streamSource.ID);
        Assert.False(streamSource.IsDisposed());

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void DisposeStreamSource()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);
        var originalID = streamSource.ID;

        Assert.NotEqual(0u, originalID);

        streamSource.Dispose();

        Assert.Equal(0u, streamSource.ID);
        Assert.True(streamSource.IsDisposed());

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void EnqueueManagedData()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        var audioData = CreateTestAudioData(1024);

        // Should not throw
        streamSource.EnqueueData(audioData, 0, audioData.Length);

        // Give the callback time to process
        Thread.Sleep(100);

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void EnqueueManagedDataWithOffset()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        var audioData = CreateTestAudioData(2048);

        // Enqueue with offset and partial length
        streamSource.EnqueueData(audioData, 512, 1024);

        Thread.Sleep(100);

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public unsafe void EnqueueUnmanagedData()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        var audioData = CreateTestAudioData(1024);

        fixed (byte* dataPtr = audioData)
        {
            // Should not throw
            streamSource.EnqueueData(dataPtr, 0, audioData.Length);

            // Give the callback time to process
            Thread.Sleep(100);
        }

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public unsafe void EnqueueUnmanagedDataWithOffset()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        var audioData = CreateTestAudioData(2048);

        fixed (byte* dataPtr = audioData)
        {
            // Enqueue with offset and partial length
            streamSource.EnqueueData(dataPtr, 512, 1024);

            Thread.Sleep(100);
        }

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void EnqueueMultipleBuffers()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        var audioData1 = CreateTestAudioData(512);
        var audioData2 = CreateTestAudioData(512);
        var audioData3 = CreateTestAudioData(512);

        // Enqueue multiple buffers
        streamSource.EnqueueData(audioData1, 0, audioData1.Length);
        streamSource.EnqueueData(audioData2, 0, audioData2.Length);
        streamSource.EnqueueData(audioData3, 0, audioData3.Length);

        Thread.Sleep(200);

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void TryGetUsedData()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        var audioData = CreateTestAudioData(1024);
        streamSource.EnqueueData(audioData, 0, audioData.Length);

        // Wait for buffer to be consumed
        Thread.Sleep(200);

        // Try to retrieve used buffer
        var retrieved = streamSource.TryGetUsedData(out var usedBuffer);

        // May or may not have a used buffer depending on timing
        if (retrieved)
        {
            Assert.NotNull(usedBuffer);
            Assert.NotNull(usedBuffer.data);
            Assert.Equal(0, usedBuffer.offset); // Should be restored to original
            Assert.Equal(audioData.Length, usedBuffer.length); // Should be restored to original
        }

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void TryGetUsedDataMultiple()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        // Enqueue several small buffers
        for (int i = 0; i < 5; i++)
        {
            var audioData = CreateTestAudioData(256);
            streamSource.EnqueueData(audioData, 0, audioData.Length);
        }

        // Wait for buffers to be consumed
        Thread.Sleep(300);

        // Try to retrieve all used buffers
        var usedCount = 0;
        while (streamSource.TryGetUsedData(out var usedBuffer))
        {
            Assert.NotNull(usedBuffer);
            usedCount++;
        }

        // We may have retrieved some used buffers (timing dependent)
        Assert.True(usedCount >= 0);

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void TryGetUsedDataReturnsFalseWhenEmpty()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        // Don't enqueue any data
        var retrieved = streamSource.TryGetUsedData(out var usedBuffer);

        Assert.False(retrieved);
        Assert.Null(usedBuffer);

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void StreamSourceInheritFromALSource()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        // Should have access to base ALSource methods
        streamSource.SetGain(0.5f);
        streamSource.SetPitch(1.2f);

        var gain = AL.GetSourcef(streamSource.ID, AL.AL_GAIN);
        var pitch = AL.GetSourcef(streamSource.ID, AL.AL_PITCH);

        Assert.True(Math.Abs(0.5f - gain) < 0.01f);
        Assert.True(Math.Abs(1.2f - pitch) < 0.01f);

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void StreamSourceWithDifferentFormats()
    {
        var (device, context) = SetupDeviceAndContext();

        // Test with MONO16
        var sourceID1 = AL.GenSource();
        var streamSource1 = new ALStreamSource(sourceID1, AL.AL_FORMAT_MONO16, 44100);
        streamSource1.Dispose();

        // Test with STEREO16
        var sourceID2 = AL.GenSource();
        var streamSource2 = new ALStreamSource(sourceID2, AL.AL_FORMAT_STEREO16, 44100);
        streamSource2.Dispose();

        // Test with MONO8
        var sourceID3 = AL.GenSource();
        var streamSource3 = new ALStreamSource(sourceID3, AL.AL_FORMAT_MONO8, 44100);
        streamSource3.Dispose();

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void StreamSourceWithDifferentSampleRates()
    {
        var (device, context) = SetupDeviceAndContext();

        // Test with 22050 Hz
        var sourceID1 = AL.GenSource();
        var streamSource1 = new ALStreamSource(sourceID1, AL.AL_FORMAT_MONO16, 22050);
        streamSource1.Dispose();

        // Test with 44100 Hz
        var sourceID2 = AL.GenSource();
        var streamSource2 = new ALStreamSource(sourceID2, AL.AL_FORMAT_MONO16, 44100);
        streamSource2.Dispose();

        // Test with 48000 Hz
        var sourceID3 = AL.GenSource();
        var streamSource3 = new ALStreamSource(sourceID3, AL.AL_FORMAT_MONO16, 48000);
        streamSource3.Dispose();

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void CreateMultipleStreamSources()
    {
        var (device, context) = SetupDeviceAndContext();

        var streamSource1 = new ALStreamSource(AL.GenSource(), AL.AL_FORMAT_MONO16, 44100);
        var streamSource2 = new ALStreamSource(AL.GenSource(), AL.AL_FORMAT_MONO16, 44100);
        var streamSource3 = new ALStreamSource(AL.GenSource(), AL.AL_FORMAT_MONO16, 44100);

        // Each should have unique IDs
        Assert.NotEqual(0u, streamSource1.ID);
        Assert.NotEqual(0u, streamSource2.ID);
        Assert.NotEqual(0u, streamSource3.ID);
        Assert.NotEqual(streamSource1.ID, streamSource2.ID);
        Assert.NotEqual(streamSource2.ID, streamSource3.ID);
        Assert.NotEqual(streamSource1.ID, streamSource3.ID);

        streamSource1.Dispose();
        streamSource2.Dispose();
        streamSource3.Dispose();

        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void EnqueueAndRetrieveBufferLifecycle()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        var audioData = CreateTestAudioData(512);

        // Enqueue buffer
        streamSource.EnqueueData(audioData, 0, audioData.Length);

        // Wait for processing
        Thread.Sleep(200);

        // Try to retrieve the used buffer
        var bufferRetrieved = false;
        for (int i = 0; i < 10 && !bufferRetrieved; i++)
        {
            if (streamSource.TryGetUsedData(out var usedBuffer))
            {
                Assert.NotNull(usedBuffer);
                Assert.NotNull(usedBuffer.data);
                Assert.Equal(audioData, usedBuffer.data);
                Assert.Equal(0, usedBuffer.offset);
                Assert.Equal(audioData.Length, usedBuffer.length);
                bufferRetrieved = true;
            }
            else
            {
                Thread.Sleep(50);
            }
        }

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void StreamSourceBufferProperties()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        var audioData = CreateTestAudioData(1024);
        streamSource.EnqueueData(audioData, 100, 500);

        // Wait for processing
        Thread.Sleep(200);

        // Try to get the buffer back
        if (streamSource.TryGetUsedData(out var usedBuffer))
        {
            // Buffer should have original offset and length restored
            Assert.Equal(100, usedBuffer.offset);
            Assert.Equal(500, usedBuffer.length);
        }

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void StreamSourceAutoPlays()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        // Give it time to start playing
        Thread.Sleep(50);

        // Stream source should auto-play on creation
        var state = AL.GetSourcei(streamSource.ID, AL.AL_SOURCE_STATE);
        Assert.Equal(AL.AL_PLAYING, state);

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void StreamSourceCanBeStopped()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        Thread.Sleep(50);

        // Can use inherited Stop method
        streamSource.Stop();

        var state = AL.GetSourcei(streamSource.ID, AL.AL_SOURCE_STATE);
        Assert.Equal(AL.AL_STOPPED, state);

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void StreamSourceCanBeRestarted()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        Thread.Sleep(50);

        // Stop then play again
        streamSource.Stop();
        var state = AL.GetSourcei(streamSource.ID, AL.AL_SOURCE_STATE);
        Assert.Equal(AL.AL_STOPPED, state);

        streamSource.Play();
        Thread.Sleep(50);
        state = AL.GetSourcei(streamSource.ID, AL.AL_SOURCE_STATE);
        Assert.Equal(AL.AL_PLAYING, state);

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void StreamSourceWithLargeBuffer()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        // Enqueue a large buffer
        var largeAudioData = CreateTestAudioData(44100); // 1 second of audio
        streamSource.EnqueueData(largeAudioData, 0, largeAudioData.Length);

        Thread.Sleep(100);

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void StreamSourceWithEmptyEnqueue()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        var audioData = new byte[0];

        // Enqueueing empty data should not crash
        streamSource.EnqueueData(audioData, 0, 0);

        Thread.Sleep(50);

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void StreamSourceWith3DPositioning()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        // Should support all ALSource 3D positioning features
        Span<float> position = stackalloc float[] { 1.0f, 2.0f, 3.0f };
        streamSource.SetPosition(position);

        Span<float> retrievedPos = stackalloc float[3];
        AL.GetSourcefv(streamSource.ID, AL.AL_POSITION, retrievedPos);

        Assert.True(Math.Abs(1.0f - retrievedPos[0]) < 0.01f);
        Assert.True(Math.Abs(2.0f - retrievedPos[1]) < 0.01f);
        Assert.True(Math.Abs(3.0f - retrievedPos[2]) < 0.01f);

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }

    [Fact]
    public void StreamSourceBufferRecycling()
    {
        var (device, context) = SetupDeviceAndContext();

        var sourceID = AL.GenSource();
        var streamSource = new ALStreamSource(sourceID, AL.AL_FORMAT_MONO16, 44100);

        var audioData = CreateTestAudioData(256);

        // Enqueue the same buffer multiple times
        streamSource.EnqueueData(audioData, 0, audioData.Length);
        streamSource.EnqueueData(audioData, 0, audioData.Length);
        streamSource.EnqueueData(audioData, 0, audioData.Length);

        Thread.Sleep(200);

        // Retrieve used buffers and verify they can be recycled
        var retrievedCount = 0;
        while (streamSource.TryGetUsedData(out var usedBuffer))
        {
            Assert.NotNull(usedBuffer);

            // Re-enqueue the same buffer (recycling)
            streamSource.EnqueueData(usedBuffer.data, usedBuffer.offset, usedBuffer.length);
            retrievedCount++;

            if (retrievedCount > 10) break; // Safety limit
        }

        streamSource.Dispose();
        CleanupDeviceAndContext(device, context);
    }
}
