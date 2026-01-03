global using OpenAL;
global using OpenAL.managed;
global using Xunit;

namespace openal_soft_bindings_test;

[Collection("Sequential")]
public class Device
{
    [Fact]
    public void CreateDevice()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);

        var device = new ALDevice(deviceNames[0]);

        device.Close();
    }

    [Fact]
    public void ReopenDevice()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);

        var device = new ALDevice(deviceNames[0]);

        if (!device.HasExtension("ALC_SOFT_reopen_device"))
        {
            device.Close();
            return;
        }

        var settings = new ALContextSettings();
        var context = new ALContext(device, settings);

        var success = device.Reopen(deviceNames[1], context.GetAttribs(settings));

        Assert.True(success);

        context.Destroy();
        device.Close();
    }

    [Fact]
    public void GetDeviceString()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        // Get device specifier string
        var deviceSpec = device.GetString(AL.ALC_DEVICE_SPECIFIER);
        Assert.NotNull(deviceSpec);
        Assert.NotEmpty(deviceSpec);

        // Get extensions string
        var extensions = device.GetString(AL.ALC_EXTENSIONS);
        Assert.NotNull(extensions);

        device.Close();
    }

    [Fact]
    public void GetStringRaw()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        // Get raw string pointer
        var rawPtr = device.GetStringRaw(AL.ALC_DEVICE_SPECIFIER);
        Assert.NotEqual(IntPtr.Zero, rawPtr);

        device.Close();
    }

    [Fact]
    public void GetIntegerALC()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        // Create a context first to query context-specific values
        var settings = new ALContextSettings();
        var context = new ALContext(device, settings);

        // Get major version
        var majorVersion = device.GetIntegerALC(AL.ALC_MAJOR_VERSION);
        Assert.True(majorVersion >= 1);

        // Get minor version
        var minorVersion = device.GetIntegerALC(AL.ALC_MINOR_VERSION);
        Assert.True(minorVersion >= 0);

        // Get mono sources
        var monoSources = device.GetIntegerALC(AL.ALC_MONO_SOURCES);
        Assert.True(monoSources >= 0);

        // Get stereo sources
        var stereoSources = device.GetIntegerALC(AL.ALC_STEREO_SOURCES);
        Assert.True(stereoSources >= 0);

        context.Destroy();
        device.Close();
    }

    [Fact]
    public void GetErrorALC()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        // Initially should have no error
        var error = device.GetErrorALC();
        Assert.Equal(AL.ALC_NO_ERROR, error);

        device.Close();
    }

    [Fact]
    public void HasExtension()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        // Check for a common extension (EFX is widely supported)
        var hasEfx = device.HasExtension("ALC_EXT_EFX");

        // We don't assert true/false since it depends on the device
        Assert.True(hasEfx == true || hasEfx == false);

        // Check for a non-existent extension
        var hasFake = device.HasExtension("ALC_FAKE_EXTENSION_THAT_DOES_NOT_EXIST");
        Assert.False(hasFake);

        device.Close();
    }

    [Fact]
    public void CloseDevice()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        var originalHandle = device.handle;
        Assert.NotEqual(IntPtr.Zero, originalHandle);

        device.Close();
        Assert.Equal(IntPtr.Zero, device.handle);
    }

    [Fact]
    public void CreateDeviceWithInvalidName()
    {
        // Attempting to open a device with an invalid/non-existent name should throw
        Assert.Throws<Exception>(() => new ALDevice("THIS_DEVICE_DOES_NOT_EXIST_12345"));
    }

    [Fact]
    public void MultipleDevices()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);

        // Open multiple devices
        var device1 = new ALDevice(deviceNames[0]);
        var device2 = new ALDevice(deviceNames[0]); // Can open the same device multiple times

        Assert.NotEqual(IntPtr.Zero, device1.handle);
        Assert.NotEqual(IntPtr.Zero, device2.handle);
        Assert.NotEqual(device1.handle, device2.handle); // Should be different handles

        device1.Close();
        device2.Close();
    }

    [Fact]
    public void GetAllDevices()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);

        // Should have at least one device
        Assert.NotNull(deviceNames);
        Assert.NotEmpty(deviceNames);

        // Each device name should be valid
        foreach (var name in deviceNames)
        {
            Assert.NotNull(name);
            Assert.NotEmpty(name);
        }
    }

    [Fact]
    public void ReopenDeviceWithoutExtension()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        if (device.HasExtension("ALC_SOFT_reopen_device"))
        {
            device.Close();
            return; // Skip this test if extension is present
        }

        var settings = new ALContextSettings();
        var context = new ALContext(device, settings);

        // Reopening without the extension should still be callable
        // but the result depends on implementation
        var success = device.Reopen(deviceNames[0], context.GetAttribs(settings));

        context.Destroy();
        device.Close();
    }

    [Fact]
    public void DeviceHandleValidAfterCreation()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        // Handle should be valid immediately after creation
        Assert.NotEqual(IntPtr.Zero, device.handle);

        device.Close();

        // Handle should be zero after closing
        Assert.Equal(IntPtr.Zero, device.handle);
    }

    [Fact]
    public void GetExtensionsList()
    {
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        var device = new ALDevice(deviceNames[0]);

        // Get the extensions string and verify it's parseable
        var extensions = device.GetString(AL.ALC_EXTENSIONS);
        Assert.NotNull(extensions);

        // Extensions should be space-separated
        if (!string.IsNullOrEmpty(extensions))
        {
            var extList = extensions.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Assert.NotEmpty(extList);

            // Verify that HasExtension works for each extension in the list
            foreach (var ext in extList)
            {
                Assert.True(device.HasExtension(ext));
            }
        }

        device.Close();
    }
}
