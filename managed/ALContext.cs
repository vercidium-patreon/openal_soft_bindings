namespace OpenAL.managed;

/// <summary>
/// Configuration settings for creating an OpenAL context
/// </summary>
public class ALContextSettings
{
    /// <summary>
    /// Whether to enable HRTF (Head-Related Transfer Function) for spatial audio
    /// </summary>
    public bool HRTFEnabled = true;

    /// <summary>
    /// The HRTF profile ID to use (0 for default)
    /// </summary>
    public int HRTFID = 0;

    /// <summary>
    /// The audio output sample rate in Hz
    /// </summary>
    public int SampleRate = 44100;

    /// <summary>
    /// Maximum number of auxiliary effect sends per source
    /// </summary>
    public int MaximumAuxiliarySends = 1;

    /// <summary>
    /// Maximum number of mono audio sources
    /// </summary>
    public int MaximumMonoSources = 16;

    /// <summary>
    /// Maximum number of stereo audio sources
    /// </summary>
    public int MaximumStereoSources = 240;

    /// <summary>
    /// Custom logging function (defaults to Console.WriteLine)
    /// </summary>
    public Action<string> LogFunction;
}

/// <summary>
/// Represents an OpenAL audio context
/// </summary>
public class ALContext
{
    /// <summary>
    /// The device this context was created on
    /// </summary>
    public readonly ALDevice device;

    /// <summary>
    /// The native context handle
    /// </summary>
    public readonly IntPtr handle;

    readonly Action<string> Log;

    const string HRTF_EXTENSION = "ALC_SOFT_HRTF";

    /// <summary>
    /// Creates a new OpenAL context with the specified settings
    /// </summary>
    /// <param name="device">The device to create the context on</param>
    /// <param name="settings">Configuration settings for the context</param>
    /// <exception cref="Exception">Thrown if context creation fails</exception>
    public ALContext(ALDevice device, ALContextSettings settings)
    {
        this.device = device;

        Log = settings.LogFunction ?? Console.WriteLine;

        var attribs = GetAttribs(settings);

        // Initialise the context
        handle = AL.CreateContext(device.handle, attribs);

        if (handle == IntPtr.Zero)
            throw new Exception($"[OpenAL] Failed to create a context");

        MakeCurrent();

        AL.Enable(AL.AL_DEBUG_OUTPUT_EXT);
        device.SetupDebugMessageCallback(OpenALDebugCallback, IntPtr.Zero);
    }

    /// <summary>
    /// Builds the attribute list for context creation from settings
    /// </summary>
    /// <param name="settings">The settings to convert to attributes</param>
    /// <returns>Attribute array for context creation</returns>
    public int[] GetAttribs(ALContextSettings settings)
    {
        List<int> attribs =
        [
            AL.ALC_FREQUENCY,
            settings.SampleRate,
            AL.ALC_MAX_AUXILIARY_SENDS,
            settings.MaximumAuxiliarySends,
            AL.ALC_MONO_SOURCES,
            settings.MaximumMonoSources,
            AL.ALC_STEREO_SOURCES,
            settings.MaximumStereoSources
        ];

        // Attempt to enable HRTF
        var hasHrtfExtension = device.HasExtension(HRTF_EXTENSION);

        // Enable HRTF
        attribs.Add(AL.ALC_HRTF_SOFT);
        attribs.Add(hasHrtfExtension ? 1 : 0);

        if (hasHrtfExtension)
        {
            // Use the default HRTF specifier (0)
            attribs.Add(AL.ALC_HRTF_ID_SOFT);
            attribs.Add(settings.HRTFID);
        }
        else
            Log($"[OpenAL] Unable to enable HRTF as the {HRTF_EXTENSION} extension is missing");

        attribs.Add(0);
        attribs.Add(0);

        return attribs.ToArray();
    }


    private delegate void AlDebugMessageCallbackFunc(AL.ALDebugProc callback, IntPtr userParam);

    /// <summary>
    /// Makes this context the current OpenAL context
    /// </summary>
    /// <exception cref="Exception">Thrown if making the context current fails</exception>
    public void MakeCurrent()
    {
        if (!AL.MakeContextCurrent(handle))
            throw new Exception("[OpenAL] Failed to make the context current");

        Debug.Assert(IsCurrent);
    }

    /// <summary>
    /// Resumes processing on this context (use after Suspend)
    /// </summary>
    public void Process() => AL.ProcessContext(handle);

    /// <summary>
    /// Destroys this context and cleans up resources
    /// </summary>
    public void Destroy()
    {
        device.SetupDebugMessageCallback(null, IntPtr.Zero);
        AL.DestroyContext(handle);
    }

    /// <summary>
    /// Gets the current error state for this context's device
    /// </summary>
    /// <returns>The error code</returns>
    public int GetErrorALC() => device.GetErrorALC();

    /// <summary>
    /// Gets whether this context is the current active context
    /// </summary>
    public bool IsCurrent => handle == AL.GetCurrentContext();

    // Set up the callback
    void OpenALDebugCallback(int source, int type, int id, int severity, int length, IntPtr messagePtr, IntPtr userParam)
    {
        string message = Marshal.PtrToStringAnsi(messagePtr, length);

        var sourceStr = source switch
        {
            AL.AL_DEBUG_SOURCE_API_EXT => "API",
            AL.AL_DEBUG_SOURCE_AUDIO_SYSTEM_EXT => "Audio System",
            AL.AL_DEBUG_SOURCE_APPLICATION_EXT => "Application",
            AL.AL_DEBUG_SOURCE_THIRD_PARTY_EXT => "Third Party",
            AL.AL_DEBUG_SOURCE_OTHER_EXT  => "Other",
            _ => source.ToString()
        };

        var typeStr = type switch
        {
            AL.AL_DEBUG_TYPE_ERROR_EXT => "Error",
            AL.AL_DEBUG_TYPE_DEPRECATED_BEHAVIOR_EXT => "Deprecated behavior",
            AL.AL_DEBUG_TYPE_UNDEFINED_BEHAVIOR_EXT => "Undefined behavior",
            AL.AL_DEBUG_TYPE_PORTABILITY_EXT => "Portability",
            AL.AL_DEBUG_TYPE_PERFORMANCE_EXT => "Performance",
            AL.AL_DEBUG_TYPE_MARKER_EXT => "Marker",
            AL.AL_DEBUG_TYPE_PUSH_GROUP_EXT => "Push group",
            AL.AL_DEBUG_TYPE_POP_GROUP_EXT => "Pop group",
            AL.AL_DEBUG_TYPE_OTHER_EXT => "Other",
            _ => type.ToString()
        };

        var severityStr = severity switch
        {
            AL.AL_DEBUG_SEVERITY_HIGH_EXT => "High",
            AL.AL_DEBUG_SEVERITY_MEDIUM_EXT => "Medium",
            AL.AL_DEBUG_SEVERITY_LOW_EXT => "Low",
            AL.AL_DEBUG_SEVERITY_NOTIFICATION_EXT => "Notification",
            _ => severity.ToString()
        };

        Log($"OpenAL [{severityStr}] {sourceStr}/{typeStr}: {message}");
    }
}