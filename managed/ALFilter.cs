namespace OpenAL.managed;

public class ALFilter
{
    public uint ID;
    public float gainLF;
    public float gainHF;

    public ALFilter(float gainLF, float gainHF)
    {
        ID = AL.GenFilter();
        AL.Filteri(ID, AL.AL_FILTER_TYPE, AL.AL_FILTER_LOWPASS);

        SetGain(gainLF, gainHF);
    }

    public void SetGain(float gainLF, float gainHF)
    {
        this.gainLF = gainLF;
        this.gainHF = MathF.Min(1, gainHF / MathF.Max(0.01f, gainLF)); // gainHF is relative to gain

        AL.Filterf(ID, AL.AL_LOWPASS_GAIN, gainLF);
        AL.Filterf(ID, AL.AL_LOWPASS_GAINHF, gainHF);
    }

    public void Delete()
    {
        AL.DeleteFilter(ID);
        ID = 0;
    }

#if DEBUG
    // Ensure we're disposed correctly
    ~ALFilter()
    {
        Debug.Assert(ID == 0);
    }
#endif
}