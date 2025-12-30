namespace OpenAL.managed;

public class ALFilter
{
    public uint ID;
    public float gain;
    public float gainHF;

    public ALFilter(float gain, float gainHF)
    {
        ID = AL.GenFilter();
        AL.Filteri(ID, AL.AL_FILTER_TYPE, AL.AL_FILTER_LOWPASS);

        SetGain(gain, gainHF);
    }

    public void SetGain(float gain, float gainHF)
    {
        this.gain = gain;
        this.gainHF = MathF.Min(1, gainHF / MathF.Max(0.01f, gain)); // gainHF is relative to gain

        AL.Filterf(ID, AL.AL_LOWPASS_GAIN, gain);
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