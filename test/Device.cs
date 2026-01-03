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

        device.Close();
    }
}
