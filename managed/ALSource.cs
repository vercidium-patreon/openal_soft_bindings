
namespace OpenAL.managed;

public unsafe class ALSource
{
    public uint ID;

    public ALSource(uint ID)
    {
        Debug.Assert(ID > 0);
        this.ID = ID;
    }

    public virtual void Play() => AL.SourcePlay(ID);
    public virtual void Stop() => AL.SourceStop(ID);

    public virtual bool Finished() => !looping && AL.GetSourcei(ID, AL.AL_SOURCE_STATE) == AL.AL_STOPPED;

    public void SetBuffer(uint bufferID) => AL.Sourcei(ID, AL.AL_BUFFER, (int)bufferID);
    public void SetGain(float gain) => AL.Sourcef(ID, AL.AL_GAIN, gain);    
    public void SetPitch(float pitch) => AL.Sourcef(ID, AL.AL_PITCH, pitch);
    public void SetRelative(bool v) => AL.Sourcei(ID, AL.AL_SOURCE_RELATIVE, v ? 1 : 0);
    public void SetSpatialise(bool v) => AL.Sourcei(ID, AL.AL_SOURCE_SPATIALIZE_SOFT, v ? 1 : 0);
    public void SetReferenceDistance(float v) => AL.Sourcef(ID, AL.AL_REFERENCE_DISTANCE, v);
    public void SetMaxDistance(float v) => AL.Sourcef(ID, AL.AL_MAX_DISTANCE, v);
    public void SetAirAbsorptionFactor(float v) => AL.Sourcef(ID, AL.AL_AIR_ABSORPTION_FACTOR, v);
    public void SetRolloff(float v) => AL.Sourcef(ID, AL.AL_ROLLOFF_FACTOR, v);

    public void SetPosition(ReadOnlySpan<float> v) => AL.Sourcefv(ID, AL.AL_POSITION, v);
    public void SetVelocity(ReadOnlySpan<float> v) => AL.Sourcefv(ID, AL.AL_VELOCITY, v);
    public void SetDirection(ReadOnlySpan<float> v) => AL.Sourcefv(ID, AL.AL_DIRECTION, v);
    public void SetVector3(int param, ReadOnlySpan<float> v) => AL.Sourcefv(ID, param, v);

    public bool looping;
    public void SetLooping(bool v)
    {
        looping = v;
        AL.Sourcei(ID, AL.AL_LOOPING, v ? 1 : 0);
    }

    public float secOffset;
    public void SetSecOffset(float seconds)
    {
        secOffset = seconds;
        AL.Sourcef(ID, AL.AL_SEC_OFFSET, seconds);
    }

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

    public void Dispose()
    {
        // Ensure we're not double-disposing
        Debug.Assert(!IsDisposed());

        AL.DeleteSource(ID);
        ID = 0;
    }

    public bool IsDisposed() => ID == 0;
}