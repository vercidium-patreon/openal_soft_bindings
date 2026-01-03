namespace openal_soft_bindings_test;

[Collection("Sequential")]
public class Context
{
    [Fact]
    public void CreateContext()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);
        var settings = new ALContextSettings();

        var context = new ALContext(device, settings);

        Assert.NotEqual(IntPtr.Zero, context.handle);
        Assert.True(context.IsCurrent);
        Assert.Equal(device, context.device);

        context.Destroy();
        device.Close();
    }

    [Fact]
    public void CreateContextWithCustomSettings()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        var settings = new ALContextSettings
        {
            SampleRate = 48000,
            MaximumAuxiliarySends = 2,
            MaximumMonoSources = 32,
            MaximumStereoSources = 128,
            HRTFEnabled = false,
            HRTFID = 1
        };

        var context = new ALContext(device, settings);

        Assert.NotEqual(IntPtr.Zero, context.handle);
        Assert.True(context.IsCurrent);

        context.Destroy();
        device.Close();
    }

    [Fact]
    public void CreateContextWithCustomLogFunction()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        var logMessages = new List<string>();
        var settings = new ALContextSettings
        {
            LogFunction = msg => logMessages.Add(msg)
        };

        var context = new ALContext(device, settings);

        // If HRTF extension is missing, we should have a log message
        if (!device.HasExtension("ALC_SOFT_HRTF"))
        {
            Assert.Contains(logMessages, msg => msg.Contains("Unable to enable HRTF"));
        }

        context.Destroy();
        device.Close();
    }

    [Fact]
    public void MakeContextCurrent()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);
        var settings = new ALContextSettings();

        var context1 = new ALContext(device, settings);
        Assert.True(context1.IsCurrent);

        // Create a second context (though we reuse the same device)
        // Note: This creates a second context on the same device
        var context2 = new ALContext(device, settings);
        Assert.True(context2.IsCurrent);
        Assert.False(context1.IsCurrent);

        // Make the first context current again
        context1.MakeCurrent();
        Assert.True(context1.IsCurrent);
        Assert.False(context2.IsCurrent);

        context1.Destroy();
        context2.Destroy();
        device.Close();
    }

    [Fact]
    public void ProcessContext()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);
        var settings = new ALContextSettings();

        var context = new ALContext(device, settings);

        // Process should not throw
        context.Process();

        context.Destroy();
        device.Close();
    }

    [Fact]
    public void DestroyContext()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);
        var settings = new ALContextSettings();

        var context = new ALContext(device, settings);
        var originalHandle = context.handle;

        Assert.NotEqual(IntPtr.Zero, originalHandle);

        context.Destroy();

        Assert.Equal(IntPtr.Zero, context.handle);

        device.Close();
    }

    [Fact]
    public void GetErrorALC()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);
        var settings = new ALContextSettings();

        var context = new ALContext(device, settings);

        // GetErrorALC should return no error initially
        var error = context.GetErrorALC();
        Assert.Equal(AL.ALC_NO_ERROR, error);

        context.Destroy();
        device.Close();
    }

    [Fact]
    public void GetAttribsWithoutHRTF()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        var settings = new ALContextSettings
        {
            SampleRate = 48000,
            MaximumAuxiliarySends = 3,
            MaximumMonoSources = 64,
            MaximumStereoSources = 256,
            HRTFEnabled = false
        };

        var context = new ALContext(device, settings);
        var attribs = context.GetAttribs(settings);

        // Check that the attributes contain our settings
        Assert.Contains(AL.ALC_FREQUENCY, attribs);
        var freqIndex = Array.IndexOf(attribs, AL.ALC_FREQUENCY);
        Assert.Equal(48000, attribs[freqIndex + 1]);

        Assert.Contains(AL.ALC_MAX_AUXILIARY_SENDS, attribs);
        var auxIndex = Array.IndexOf(attribs, AL.ALC_MAX_AUXILIARY_SENDS);
        Assert.Equal(3, attribs[auxIndex + 1]);

        Assert.Contains(AL.ALC_MONO_SOURCES, attribs);
        var monoIndex = Array.IndexOf(attribs, AL.ALC_MONO_SOURCES);
        Assert.Equal(64, attribs[monoIndex + 1]);

        Assert.Contains(AL.ALC_STEREO_SOURCES, attribs);
        var stereoIndex = Array.IndexOf(attribs, AL.ALC_STEREO_SOURCES);
        Assert.Equal(256, attribs[stereoIndex + 1]);

        // Array should be null-terminated
        Assert.Equal(0, attribs[^1]);
        Assert.Equal(0, attribs[^2]);

        context.Destroy();
        device.Close();
    }

    [Fact]
    public void GetAttribsWithHRTF()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        var settings = new ALContextSettings
        {
            HRTFEnabled = true,
            HRTFID = 2
        };

        var context = new ALContext(device, settings);
        var attribs = context.GetAttribs(settings);

        // Check for HRTF soft attribute
        Assert.Contains(AL.ALC_HRTF_SOFT, attribs);
        var hrtfIndex = Array.IndexOf(attribs, AL.ALC_HRTF_SOFT);

        if (device.HasExtension("ALC_SOFT_HRTF"))
        {
            // HRTF should be enabled (1)
            Assert.Equal(1, attribs[hrtfIndex + 1]);

            // Should have HRTF ID
            Assert.Contains(AL.ALC_HRTF_ID_SOFT, attribs);
            var hrtfIdIndex = Array.IndexOf(attribs, AL.ALC_HRTF_ID_SOFT);
            Assert.Equal(2, attribs[hrtfIdIndex + 1]);
        }
        else
        {
            // HRTF should be disabled (0) when extension is missing
            Assert.Equal(0, attribs[hrtfIndex + 1]);
        }

        context.Destroy();
        device.Close();
    }

    [Fact]
    public void DebugCallbackLogging()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        var logMessages = new List<string>();
        var settings = new ALContextSettings
        {
            LogFunction = msg => logMessages.Add(msg)
        };

        var context = new ALContext(device, settings);

        // Try to trigger a debug callback by doing something that might generate a notification
        // For example, querying debug info or generating invalid operations

        // Note: This test validates that the debug callback mechanism is set up,
        // but may not actually receive messages unless OpenAL Soft generates them.
        // The callback is registered in the constructor via DebugMessageCallback.Invoke

        // We can at least verify no exceptions are thrown during context operations
        context.MakeCurrent();
        context.Process();

        context.Destroy();
        device.Close();

        // If we got any debug messages, verify they follow the expected format
        if (logMessages.Count > 0)
        {
            foreach (var msg in logMessages)
            {
                // Debug messages should contain severity and source information
                // Format: "OpenAL [Severity] Source/Type: message"
                Assert.Contains("OpenAL", msg);
            }
        }
    }

    [Fact]
    public void CreateContextFailsWithNullDevice()
    {
        // Create an ALDevice wrapper with a null handle to simulate failure
        // This is tricky because ALDevice constructor validates the handle
        // Instead, we test the behavior when context creation might fail

        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        // Close the device first to invalidate it
        device.Close();

        var settings = new ALContextSettings();

        // Attempting to create a context on a closed device should throw
        Assert.Throws<Exception>(() => new ALContext(device, settings));
    }

    [Fact]
    public void FunctionsFailWhenDestroyed()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);
        var settings = new ALContextSettings();

        var context = new ALContext(device, settings);
        context.Destroy();

        // Should throw when invoking functions on a destroyed context
        Assert.Throws<Exception>(context.MakeCurrent);
        Assert.Throws<Exception>(context.Process);
        Assert.Throws<Exception>(context.Destroy);
        Assert.Throws<Exception>(() => context.GetErrorALC());

        device.Close();
    }

    [Fact]
    public void MultipleContextsOnSameDevice()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);
        var settings = new ALContextSettings();

        var context1 = new ALContext(device, settings);
        var context2 = new ALContext(device, settings);
        var context3 = new ALContext(device, settings);

        // Last created context should be current
        Assert.True(context3.IsCurrent);
        Assert.False(context2.IsCurrent);
        Assert.False(context1.IsCurrent);

        // Switch between contexts
        context1.MakeCurrent();
        Assert.True(context1.IsCurrent);

        context2.MakeCurrent();
        Assert.True(context2.IsCurrent);

        // Clean up in reverse order
        context3.Destroy();
        context2.Destroy();
        context1.Destroy();
        device.Close();
    }
}
