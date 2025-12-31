namespace OpenAL;

public static partial class AL
{
    public static List<string> GetStringList(IntPtr device, int param)
    {
        var listPtr = GetStringPtr(device, param);
        var result = new List<string>();
 
        if (listPtr == IntPtr.Zero)
            return result;
 
        while (true)
        {
            var str = Marshal.PtrToStringUTF8(listPtr);
 
            if (str.Length == 0)
                break;
 
            result.Add(str);
            listPtr += str.Length + 1;
        }
 
        return result;
    }

    public static int GetSoundFormat(int channels, int bitDepth)
    {
        // Mono formats
        if (channels == 1)
        {
            if (bitDepth == 8)
                return AL_FORMAT_MONO8;
            else if (bitDepth == 16)
                return AL_FORMAT_MONO16;
            else if (bitDepth == 32)
                return AL_FORMAT_MONO_FLOAT32;
            else if (bitDepth == 64)
                return AL_FORMAT_MONO_DOUBLE_EXT;
        }
        // Stereo formats
        else if (channels == 2)
        {
            if (bitDepth == 8)
                return AL_FORMAT_STEREO8;
            else if (bitDepth == 16)
                return AL_FORMAT_STEREO16;
            else if (bitDepth == 32)
                return AL_FORMAT_STEREO_FLOAT32;
            else if (bitDepth == 64)
                return AL_FORMAT_STEREO_DOUBLE_EXT;
        }
        // Quad formats
        else if (channels == 4)
        {
            if (bitDepth == 8)
                return AL_FORMAT_QUAD8;
            else if (bitDepth == 16)
                return AL_FORMAT_QUAD16;
            else if (bitDepth == 32)
                return AL_FORMAT_QUAD32;
        }
        // 5.1 surround formats
        else if (channels == 6)
        {
            if (bitDepth == 8)
                return AL_FORMAT_51CHN8;
            else if (bitDepth == 16)
                return AL_FORMAT_51CHN16;
            else if (bitDepth == 32)
                return AL_FORMAT_51CHN32;
        }
        // 6.1 surround formats
        else if (channels == 7)
        {
            if (bitDepth == 8)
                return AL_FORMAT_61CHN8;
            else if (bitDepth == 16)
                return AL_FORMAT_61CHN16;
            else if (bitDepth == 32)
                return AL_FORMAT_61CHN32;
        }
        // 7.1 surround formats
        else if (channels == 8)
        {
            if (bitDepth == 8)
                return AL_FORMAT_71CHN8;
            else if (bitDepth == 16)
                return AL_FORMAT_71CHN16;
            else if (bitDepth == 32)
                return AL_FORMAT_71CHN32;
        }
        // B-Format 2D (3 channels: W, X, Y)
        else if (channels == 3)
        {
            if (bitDepth == 8)
                return AL_FORMAT_BFORMAT2D_8;
            else if (bitDepth == 16)
                return AL_FORMAT_BFORMAT2D_16;
            else if (bitDepth == 32)
                return AL_FORMAT_BFORMAT2D_FLOAT32;
        }

        // Default fallback
        return AL_FORMAT_MONO8;
    }

    public static int GetBytesPerSample(int format)
    {
        if (format == AL_FORMAT_MONO8 || format == AL_FORMAT_STEREO8 || format == AL_FORMAT_QUAD8 || format == AL_FORMAT_51CHN8 || format == AL_FORMAT_61CHN8 || format == AL_FORMAT_71CHN8 || format == AL_FORMAT_BFORMAT2D_8)
            return 1;

        if (format == AL_FORMAT_MONO16 || format == AL_FORMAT_STEREO16 || format == AL_FORMAT_QUAD16 || format == AL_FORMAT_51CHN16 || format == AL_FORMAT_61CHN16 || format == AL_FORMAT_71CHN16 || format == AL_FORMAT_BFORMAT2D_16)
            return 2;

        if (format == AL_FORMAT_MONO_FLOAT32 || format == AL_FORMAT_STEREO_FLOAT32 || format == AL_FORMAT_QUAD32 || format == AL_FORMAT_51CHN32 || format == AL_FORMAT_61CHN32 || format == AL_FORMAT_71CHN32 || format == AL_FORMAT_BFORMAT2D_FLOAT32)
            return 4;

        // Default fallback
        return 1;
    }

    public static int GetBytesPerFrame(int format)
    {
        int bytesPerSample = GetBytesPerSample(format);

        if (format == AL_FORMAT_MONO8 || format == AL_FORMAT_MONO16 || format == AL_FORMAT_MONO_FLOAT32 || format == AL_FORMAT_MONO_DOUBLE_EXT)
            return bytesPerSample * 1;

        if (format == AL_FORMAT_STEREO8 || format == AL_FORMAT_STEREO16 || format == AL_FORMAT_STEREO_FLOAT32 || format == AL_FORMAT_STEREO_DOUBLE_EXT)
            return bytesPerSample * 2;

        if (format == AL_FORMAT_BFORMAT2D_8 || format == AL_FORMAT_BFORMAT2D_16 || format == AL_FORMAT_BFORMAT2D_FLOAT32)
            return bytesPerSample * 3;

        if (format == AL_FORMAT_QUAD8 || format == AL_FORMAT_QUAD16 || format == AL_FORMAT_QUAD32)
            return bytesPerSample * 4;

        if (format == AL_FORMAT_51CHN8 || format == AL_FORMAT_51CHN16 || format == AL_FORMAT_51CHN32)
            return bytesPerSample * 6;

        if (format == AL_FORMAT_61CHN8 || format == AL_FORMAT_61CHN16 || format == AL_FORMAT_61CHN32)
            return bytesPerSample * 7;

        if (format == AL_FORMAT_71CHN8 || format == AL_FORMAT_71CHN16 || format == AL_FORMAT_71CHN32)
            return bytesPerSample * 8;

        return bytesPerSample;
    }
}
