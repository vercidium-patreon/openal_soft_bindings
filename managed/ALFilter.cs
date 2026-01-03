namespace OpenAL.managed;

/// <summary>
/// Represents an OpenAL lowpass filter
/// </summary>
public class ALFilter
{
    /// <summary>
    /// OpenAL filter ID
    /// </summary>
    public uint ID;

    /// <summary>
    /// Overall gain applied by the filter
    /// </summary>
    public float gain;

    /// <summary>
    /// High-frequency gain multiplier
    /// </summary>
    public float gainHF;

    /// <summary>
    /// Create a new lowpass filter
    /// </summary>
    /// <param name="gain">The overall gain</param>
    /// <param name="gainHF">The high-frequency gain</param>
    public ALFilter(float gain, float gainHF)
    {
        ID = AL.GenFilter();
        AL.Filteri(ID, AL.AL_FILTER_TYPE, AL.AL_FILTER_LOWPASS);

        SetGain(gain, gainHF);
    }

    /// <summary>
    /// Update the filter's gain parameters
    /// </summary>
    /// <param name="gain">The overall gain</param>
    /// <param name="gainHF">The high-frequency gain</param>
    public void SetGain(float gain, float gainHF)
    {
        this.gain = gain;
        this.gainHF = MathF.Min(1, gainHF / MathF.Max(0.01f, gain)); // gainHF is relative to gain

        AL.Filterf(ID, AL.AL_LOWPASS_GAIN, gain);
        AL.Filterf(ID, AL.AL_LOWPASS_GAINHF, gainHF);
    }

    /// <summary>
    /// Delete the filter and release its resources
    /// </summary>
    public void Delete()
    {
        AL.DeleteFilter(ID);
        ID = 0;
    }

#if DEBUG
    ~ALFilter()
    {
        Debug.Assert(ID == 0);
    }
#endif
}