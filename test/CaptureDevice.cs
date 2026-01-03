namespace openal_soft_bindings_test;

[Collection("Sequential")]
public class CaptureDevice
{
    [Fact]
    public void CreateCaptureDevice()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        int callbackInvoked = 0;

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            SampleRate = 44100,
            Format = AL.AL_FORMAT_MONO16,
            BufferSize = 1024,
            DataCallback = (data, sampleCount) => { callbackInvoked++; }
        };

        var captureDevice = new ALCaptureDevice(settings);

        captureDevice.Close();
    }

    [Fact]
    public void CreateCaptureDeviceWithoutDataCallback()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            DataCallback = null // Missing callback
        };

        // Should throw ArgumentException when DataCallback is null
        Assert.Throws<ArgumentException>(() => new ALCaptureDevice(settings));
    }

    [Fact]
    public void CreateCaptureDeviceWithInvalidName()
    {
        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = "INVALID_CAPTURE_DEVICE_NAME_12345",
            DataCallback = (data, count) => { }
        };

        // Should throw Exception when device name is invalid
        Assert.Throws<Exception>(() => new ALCaptureDevice(settings));
    }

    [Fact]
    public void CreateCaptureDeviceWithCustomSettings()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        var logMessages = new List<string>();

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            SampleRate = 48000,
            Format = AL.AL_FORMAT_MONO_FLOAT32, // Can fail on some devices if it's set to STEREO16
            BufferSize = 2048,
            LogCallback = msg => logMessages.Add(msg),
            DataCallback = (data, count) => { }
        };

        var captureDevice = new ALCaptureDevice(settings);

        captureDevice.Close();
    }

    [Fact]
    public void CaptureStartAndStop()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            DataCallback = (data, count) => { }
        };

        var captureDevice = new ALCaptureDevice(settings);

        // Should not throw
        captureDevice.CaptureStart();
        captureDevice.CaptureStop();

        captureDevice.Close();
    }

    [Fact]
    public void CaptureUpdate()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        int callbackInvoked = 0;
        int totalSamples = 0;

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            SampleRate = 44100,
            Format = AL.AL_FORMAT_MONO16,
            BufferSize = 1024,
            DataCallback = (data, sampleCount) =>
            {
                callbackInvoked++;
                totalSamples += sampleCount;
                Assert.NotEqual(IntPtr.Zero, data);
                Assert.True(sampleCount > 0);
                Assert.True(sampleCount <= 1024); // Should not exceed buffer size
            }
        };

        var captureDevice = new ALCaptureDevice(settings);
        captureDevice.CaptureStart();

        // Call Update multiple times
        // Note: Callback may not be invoked if no samples are captured
        for (int i = 0; i < 10; i++)
        {
            captureDevice.Update();
            Thread.Sleep(10); // Small delay to allow capture
        }

        captureDevice.CaptureStop();
        captureDevice.Close();

        // We can't guarantee the callback was invoked (depends on actual audio capture)
        // but if it was, verify the constraints
        if (callbackInvoked > 0)
        {
            Assert.True(totalSamples > 0);
        }
    }

    [Fact]
    public void UpdateWithoutCapturing()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        int callbackInvoked = 0;

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            DataCallback = (data, count) => { callbackInvoked++; }
        };

        var captureDevice = new ALCaptureDevice(settings);

        // Update without starting capture - callback should not be invoked
        captureDevice.Update();
        captureDevice.Update();

        Assert.Equal(0, callbackInvoked);

        captureDevice.Close();
    }

    [Fact]
    public void CloseDevice()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            DataCallback = (data, count) => { }
        };

        var captureDevice = new ALCaptureDevice(settings);

        // Should not throw
        captureDevice.Close();
    }

    [Fact]
    public void CaptureStartFailsAfterClose()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            DataCallback = (data, count) => { }
        };

        var captureDevice = new ALCaptureDevice(settings);
        captureDevice.Close();

        // Should throw after closing
        Assert.Throws<Exception>(() => captureDevice.CaptureStart());
    }

    [Fact]
    public void CaptureStopFailsAfterClose()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            DataCallback = (data, count) => { }
        };

        var captureDevice = new ALCaptureDevice(settings);
        captureDevice.Close();

        // Should throw after closing
        Assert.Throws<Exception>(() => captureDevice.CaptureStop());
    }

    [Fact]
    public void UpdateFailsAfterClose()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            DataCallback = (data, count) => { }
        };

        var captureDevice = new ALCaptureDevice(settings);
        captureDevice.Close();

        // Should throw after closing
        Assert.Throws<Exception>(() => captureDevice.Update());
    }

    [Fact]
    public void CloseFailsWhenAlreadyClosed()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            DataCallback = (data, count) => { }
        };

        var captureDevice = new ALCaptureDevice(settings);
        captureDevice.Close();

        // Should throw when trying to close again
        Assert.Throws<Exception>(() => captureDevice.Close());
    }

    [Fact]
    public void AllFunctionsFailAfterClose()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            DataCallback = (data, count) => { }
        };

        var captureDevice = new ALCaptureDevice(settings);
        captureDevice.Close();

        // All methods should throw after close
        Assert.Throws<Exception>(captureDevice.CaptureStart);
        Assert.Throws<Exception>(captureDevice.CaptureStop);
        Assert.Throws<Exception>(captureDevice.Update);
        Assert.Throws<Exception>(captureDevice.Close);
    }

    [Fact]
    public void GetCaptureDeviceList()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        // Each device name should be valid
        foreach (var name in captureDeviceNames)
        {
            Assert.NotNull(name);
            Assert.NotEmpty(name);
        }
    }

    [Fact]
    public void DataCallbackReceivesCorrectData()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        var receivedData = new List<(nint data, int count)>();

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            SampleRate = 44100,
            Format = AL.AL_FORMAT_MONO16,
            BufferSize = 1024,
            DataCallback = (data, sampleCount) =>
            {
                receivedData.Add((data, sampleCount));
            }
        };

        var captureDevice = new ALCaptureDevice(settings);
        captureDevice.CaptureStart();

        // Try to capture some data
        for (int i = 0; i < 20; i++)
        {
            captureDevice.Update();
            Thread.Sleep(10);
        }

        captureDevice.CaptureStop();
        captureDevice.Close();

        // Verify that if data was captured, it meets our constraints
        foreach (var (data, count) in receivedData)
        {
            Assert.NotEqual(IntPtr.Zero, data);
            Assert.True(count > 0);
            Assert.True(count <= 1024); // Should not exceed buffer size
        }
    }

    [Fact]
    public void DifferentAudioFormats()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        // Test different audio formats
        var formats = new[]
        {
            AL.AL_FORMAT_MONO8,
            AL.AL_FORMAT_MONO16,
            AL.AL_FORMAT_STEREO8,
            AL.AL_FORMAT_STEREO16
        };

        foreach (var format in formats)
        {
            var settings = new ALCaptureDeviceSettings
            {
                DeviceName = captureDeviceNames[0],
                SampleRate = 44100,
                Format = format,
                BufferSize = 1024,
                DataCallback = (data, count) => { }
            };

            try
            {
                var captureDevice = new ALCaptureDevice(settings);
                captureDevice.Close();
            }
            catch
            {
                // Some formats may not be supported, which is acceptable
            }
        }
    }

    [Fact]
    public void DifferentSampleRates()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        // Test different sample rates
        var sampleRates = new[] { 8000, 22050, 44100, 48000 };

        foreach (var sampleRate in sampleRates)
        {
            var settings = new ALCaptureDeviceSettings
            {
                DeviceName = captureDeviceNames[0],
                SampleRate = sampleRate,
                Format = AL.AL_FORMAT_MONO16,
                BufferSize = 1024,
                DataCallback = (data, count) => { }
            };

            try
            {
                var captureDevice = new ALCaptureDevice(settings);
                captureDevice.Close();
            }
            catch
            {
                // Some sample rates may not be supported, which is acceptable
            }
        }
    }

    [Fact]
    public void MultipleStartStop()
    {
        var captureDeviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_CAPTURE_DEVICE_SPECIFIER);

        // Skip test if no capture devices are available
        if (captureDeviceNames.Count == 0)
            return;

        var settings = new ALCaptureDeviceSettings
        {
            DeviceName = captureDeviceNames[0],
            DataCallback = (data, count) => { }
        };

        var captureDevice = new ALCaptureDevice(settings);

        // Start and stop multiple times
        captureDevice.CaptureStart();
        captureDevice.CaptureStop();

        captureDevice.CaptureStart();
        captureDevice.CaptureStop();

        captureDevice.CaptureStart();
        captureDevice.CaptureStop();

        captureDevice.Close();
    }
}
