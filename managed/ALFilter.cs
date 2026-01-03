namespace OpenAL.managed;

/// <summary>
/// Represents an OpenAL lowpass filter
/// </summary>
public class ALFilter
{
    /// <summary>
    /// The OpenAL filter ID
    /// </summary>
    public uint ID;

    /// <summary>
    /// The overall gain applied by the filter
    /// </summary>
    public float gain;

    /// <summary>
    /// The high-frequency gain attenuation
    /// </summary>
    public float gainHF;

    /// <summary>
    /// Creates a new lowpass filter with the specified gain parameters
    /// </summary>
    /// <param name="gain">The overall gain</param>
    /// <param name="gainHF">The high-frequency gain attenuation</param>
    public ALFilter(float gain, float gainHF)
    {
        ID = AL.GenFilter();
        AL.Filteri(ID, AL.AL_FILTER_TYPE, AL.AL_FILTER_LOWPASS);

        SetGain(gain, gainHF);
    }

    /// <summary>
    /// Updates the filter's gain parameters
    /// </summary>
    /// <param name="gain">The overall gain</param>
    /// <param name="gainHF">The high-frequency gain attenuation</param>
    public void SetGain(float gain, float gainHF)
    {
        this.gain = gain;
        this.gainHF = MathF.Min(1, gainHF / MathF.Max(0.01f, gain)); // gainHF is relative to gain

        AL.Filterf(ID, AL.AL_LOWPASS_GAIN, gain);
        AL.Filterf(ID, AL.AL_LOWPASS_GAINHF, gainHF);
    }

    /// <summary>
    /// Deletes the filter and releases its resources
    /// </summary>
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