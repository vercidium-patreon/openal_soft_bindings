namespace OpenAL;

public static unsafe partial class AL
{
    public delegate void ALDebugProc(int source, int type, int id, int severity, int length, IntPtr message, IntPtr userParam);
    public delegate int ALBufferCallbackTypeSoft(void* userData, void* sampleData, int numBytes);
}
