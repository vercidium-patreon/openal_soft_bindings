namespace OpenAL.managed;

/// <summary>
/// Represents an OpenAL audio source
/// </summary>
public unsafe class ALSource
{
    /// <summary>
    /// The OpenAL source ID
    /// </summary>
    public uint ID;

    /// <summary>
    /// Creates a new audio source wrapper
    /// </summary>
    /// <param name="ID">The OpenAL source ID</param>
    public ALSource(uint ID)
    {
        Debug.Assert(ID > 0);
        this.ID = ID;
    }

    /// <summary>Starts playing the source</summary>
    public void Play() => AL.SourcePlay(ID);
    /// <summary>Stops playing the source</summary>
    public void Stop() => AL.SourceStop(ID);

    /// <summary>Checks if the source has finished playing</summary>
    public bool Finished() => !looping && AL.GetSourcei(ID, AL.AL_SOURCE_STATE) == AL.AL_STOPPED;

    /// <summary>Sets the audio buffer to play</summary>
    public void SetBuffer(uint bufferID) => AL.Sourcei(ID, AL.AL_BUFFER, (int)bufferID);
    /// <summary>Sets the source gain (volume)</summary>
    public void SetGain(float gain) => AL.Sourcef(ID, AL.AL_GAIN, gain);
    /// <summary>Sets the source pitch multiplier</summary>
    public void SetPitch(float pitch) => AL.Sourcef(ID, AL.AL_PITCH, pitch);
    /// <summary>Sets whether the source is relative to the listener</summary>
    public void SetRelative(bool v) => AL.Sourcei(ID, AL.AL_SOURCE_RELATIVE, v ? 1 : 0);
    /// <summary>Sets whether the source should be spatialized</summary>
    public void SetSpatialise(bool v) => AL.Sourcei(ID, AL.AL_SOURCE_SPATIALIZE_SOFT, v ? 1 : 0);
    /// <summary>Sets the reference distance for attenuation</summary>
    public void SetReferenceDistance(float v) => AL.Sourcef(ID, AL.AL_REFERENCE_DISTANCE, v);
    /// <summary>Sets the maximum distance for attenuation</summary>
    public void SetMaxDistance(float v) => AL.Sourcef(ID, AL.AL_MAX_DISTANCE, v);
    /// <summary>Sets the air absorption factor</summary>
    public void SetAirAbsorptionFactor(float v) => AL.Sourcef(ID, AL.AL_AIR_ABSORPTION_FACTOR, v);
    /// <summary>Sets the rolloff factor for distance attenuation</summary>
    public void SetRolloff(float v) => AL.Sourcef(ID, AL.AL_ROLLOFF_FACTOR, v);

    /// <summary>Sets the 3D position of the source</summary>
    public void SetPosition(ReadOnlySpan<float> v) => AL.Sourcefv(ID, AL.AL_POSITION, v);
    /// <summary>Sets the velocity vector for doppler effect</summary>
    public void SetVelocity(ReadOnlySpan<float> v) => AL.Sourcefv(ID, AL.AL_VELOCITY, v);
    /// <summary>Sets the direction vector for directional sources</summary>
    public void SetDirection(ReadOnlySpan<float> v) => AL.Sourcefv(ID, AL.AL_DIRECTION, v);
    /// <summary>Sets a 3D vector parameter</summary>
    public void SetVector3(int param, ReadOnlySpan<float> v) => AL.Sourcefv(ID, param, v);

    /// <summary>Whether this source is looping</summary>
    public bool looping;
    /// <summary>Sets whether the source should loop</summary>
    public void SetLooping(bool v)
    {
        looping = v;
        AL.Sourcei(ID, AL.AL_LOOPING, v ? 1 : 0);
    }

    /// <summary>Current playback offset in seconds</summary>
    public float secOffset;
    /// <summary>Sets the playback position in seconds</summary>
    public void SetSecOffset(float seconds)
    {
        secOffset = seconds;
        AL.Sourcef(ID, AL.AL_SEC_OFFSET, seconds);
    }

    /// <summary>
    /// Applies reverb and filter effects to the source
    /// </summary>
    /// <param name="reverbEffect">The reverb effect to apply</param>
    /// <param name="directFilter">Filter for the direct (dry) signal</param>
    /// <param name="reverbFilter">Filter for the reverb (wet) signal</param>
    public void SetFilter(ALReverbEffect reverbEffect, ALFilter directFilter, ALFilter reverbFilter)
    {
        // Get IDs
        var directFilterID = (int)(directFilter?.ID ?? 0);
        var reverbFilterID = (int)(reverbFilter?.ID ?? 0);
        var effectSlotID = (int)(reverbEffect?.effectSlotID ?? 0);


        // Set direct filter
        AL.Sourcei(ID, AL.AL_DIRECT_FILTER, directFilterID);

        // Apply effect, with filter
        AL.Source3i(ID, AL.AL_AUXILIARY_SEND_FILTER, effectSlotID, 0, reverbFilterID);
    }

    /// <summary>
    /// Disposes the source and releases its resources
    /// </summary>
    public virtual void Dispose()
    {
        // Ensure we're not double-disposing
        Debug.Assert(!IsDisposed());

        AL.DeleteSource(ID);
        ID = 0;
    }

    /// <summary>
    /// Checks if the source has been disposed
    /// </summary>
    public bool IsDisposed() => ID == 0;
}