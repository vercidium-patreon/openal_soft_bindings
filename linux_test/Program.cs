using OpenAL;
using System;

namespace linux_test;

internal class Program
{
    static void Main(string[] args)
    {
        OpenAL.Logger.Log = Console.WriteLine;
        OpenAL.Logger.Error = Console.Error.WriteLine;
        var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);
        Console.WriteLine($"{deviceNames.Count} devices found");
    }
}
