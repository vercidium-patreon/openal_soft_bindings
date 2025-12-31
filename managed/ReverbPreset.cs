namespace OpenAL;

public class ReverbPreset
{
    public string name;

    /// <summary>
    /// Controls the modal density of the reverb tail. Higher values produce a smoother, more continuous reverb tail
    /// by increasing the density of late reflections. Lower values create a more sparse, granular reverb with greater
    /// separation between individual reflections. Higher density is typically preferred for percussive sounds, while
    /// lower density works better for vocals and sustained instruments.
    /// Range: 0.0 to 1.0
    /// </summary>
    public float density;

    /// <summary>
    /// Controls how quickly sound energy is scattered and dispersed throughout the reverb field. Higher values
    /// produce a smoother, more homogeneous reverb tail with less distinct echo separation. Lower values result
    /// in clearer, more distinguishable discrete reflections with a thinner overall character.
    /// Range: 0.0 to 1.0
    /// </summary>
    public float diffusion;

    /// <summary>
    /// Master output level for the overall reverb effect.
    /// Range: 0.0 to 1.0
    /// </summary>
    public float gain;

    /// <summary>
    /// High-frequency damping factor applied to the reverb tail. Lower values simulate softer, more absorptive
    /// surfaces that attenuate high frequencies on each reflection, producing a warmer reverb character.
    /// Higher values preserve high frequencies, simulating harder, more reflective surfaces.
    /// Range: 0.0 to 1.0
    /// </summary>
    public float gainHF;

    /// <summary>
    /// Low-frequency gain adjustment for the reverb effect. Controls the amplitude of low-frequency content
    /// in the reverb tail relative to mid-range frequencies.
    /// Range: 0.0 to 1.0
    /// </summary>
    public float gainLF;

    /// <summary>
    /// Reverb decay time in seconds. Defines how long it takes for the reverb tail to decay by 60 dB (RT60).
    /// Longer values simulate larger spaces or more reflective surfaces. Decay time should be proportional
    /// to the implied room size for realistic results.
    /// Range: 0.1 to 20.0 seconds
    /// </summary>
    public float decayTime;

    /// <summary>
    /// Ratio of high-frequency decay time to the base decay time. Values greater than 1.0 cause high frequencies
    /// to decay more slowly than mid frequencies, creating a brighter reverb tail. Values less than 1.0 produce
    /// faster high-frequency decay for a darker, warmer character.
    /// Range: 0.1 to 2.0
    /// </summary>
    public float decayHFRatio;

    /// <summary>
    /// Ratio of low-frequency decay time to the base decay time. Values greater than 1.0 cause low frequencies
    /// to decay more slowly, which can add warmth but may result in muddiness if overused. Values less than 1.0
    /// produce faster low-frequency decay.
    /// Range: 0.1 to 2.0
    /// </summary>
    public float decayLFRatio;

    /// <summary>
    /// Output level for early reflections, which represent the initial discrete echoes before the dense
    /// reverb tail develops.
    /// Range: 0.0 to 3.16 (linear gain)
    /// </summary>
    public float reflectionsGain;

    /// <summary>
    /// Time delay in seconds between the direct sound and the arrival of the first early reflection.
    /// Represents the initial time gap in the impulse response.
    /// Range: 0.0 to 0.3 seconds
    /// </summary>
    public float reflectionsDelay;

    /// <summary>
    /// Three-dimensional directional vector that biases the spatial distribution of early reflections.
    /// Allows early reflections to be weighted toward a specific direction in 3D space.
    /// Format: [X, Y, Z]
    /// </summary>
    public float[] reflectionsPan = new float[3];

    /// <summary>
    /// Output level for late reverberation, which represents the dense tail of reflections that follows
    /// the early reflections.
    /// Range: 0.0 to 10.0 (linear gain)
    /// </summary>
    public float lateReverbGain;

    /// <summary>
    /// Time delay in seconds between the direct sound and the start of the late reverb tail.
    /// Defines when the dense reverberation begins in the impulse response.
    /// Range: 0.0 to 0.1 seconds
    /// </summary>
    public float lateReverbDelay;

    /// <summary>
    /// Three-dimensional directional vector that biases the spatial distribution of late reverberation.
    /// Allows the reverb tail to be weighted toward a specific direction in 3D space.
    /// Format: [X, Y, Z]
    /// </summary>
    public float[] lateReverbPan = new float[3];

    /// <summary>
    /// Delay time in seconds for the echo effect within the reverb. Controls the periodicity of
    /// repeating echo patterns in the reverb tail.
    /// Range: 0.075 to 0.25 seconds
    /// </summary>
    public float echoTime;

    /// <summary>
    /// Modulation depth of the echo effect. Controls how pronounced the echo repetition pattern is
    /// within the reverb tail.
    /// Range: 0.0 to 1.0
    /// </summary>
    public float echoDepth;

    /// <summary>
    /// Rate of periodic modulation applied to the reverb in seconds. Controls how quickly the modulation
    /// effect cycles, adding time-varying characteristics to the reverb.
    /// Range: 0.04 to 4.0 seconds
    /// </summary>
    public float modulationTime;

    /// <summary>
    /// Depth of pitch modulation applied to the reverb. Introduces subtle time-varying changes to the
    /// reverb characteristics, simulating natural variations in real acoustic spaces.
    /// Range: 0.0 to 1.0
    /// </summary>
    public float modulationDepth;

    /// <summary>
    /// High-frequency attenuation due to air absorption. Simulates the frequency-dependent absorption
    /// of sound traveling through air, which increases with distance and humidity. Higher values produce
    /// more damping of high frequencies.
    /// Range: 0.892 to 1.0 (linear gain)
    /// </summary>
    public float airAbsorptionGainHF;

    /// <summary>
    /// Reference frequency in Hz for high-frequency attenuation and decay ratio calculations. Defines
    /// the frequency at which gainHF and decayHFRatio parameters take effect. Typical values range
    /// from 2000 Hz to 8000 Hz depending on the desired tonal character.
    /// Range: 1000.0 to 20000.0 Hz
    /// </summary>
    public float hfReference;

    /// <summary>
    /// Reference frequency in Hz for low-frequency gain and decay ratio calculations. Defines the
    /// frequency at which gainLF and decayLFRatio parameters take effect. Used to control the behavior
    /// of low frequencies in the reverb. Typical values range from 100 Hz to 200 Hz.
    /// Range: 20.0 to 1000.0 Hz
    /// </summary>
    public float lfReference;

    /// <summary>
    /// Multiplier for the room effect distance attenuation. Controls how quickly the reverb effect
    /// diminishes with distance from the source. A value of 0.0 disables distance-based rolloff.
    /// Values between 0.4 and 0.6 provide natural distance cues, though many presets use 0.0.
    /// Range: 0.0 to 10.0
    /// </summary>
    public float roomRolloffFactor;

    /// <summary>
    /// Boolean flag (0 or 1) that controls whether high-frequency decay is automatically limited
    /// based on air absorption. When enabled (1), prevents high-frequency decay from exceeding
    /// physically realistic values based on the air absorption setting.
    /// Values: 0 (disabled) or 1 (enabled)
    /// </summary>
    public int decayHFLimit;

    public ReverbPreset(string name, float density, float diffusion, float gain, float gainHF, float gainLF, float decayTime, float decayHFRatio, float decayLFRatio,
                        float reflectionsGain, float reflectionsDelay, float[] reflectionsPan, float lateReverbGain, float lateReverbDelay, float[] lateReverbPan,
                        float echoTime, float echoDepth, float modulationTime, float modulationDepth, float airAbsorptionGainHF, float hfReference, float lfReference,
                        float roomRolloffFactor, int decayHFLimit)
    {
        this.name = name;
        this.density = density;
        this.diffusion = diffusion;
        this.gain = gain;
        this.gainHF = gainHF;
        this.gainLF = gainLF;
        this.decayTime = decayTime;
        this.decayHFRatio = decayHFRatio;
        this.decayLFRatio = decayLFRatio;
        this.reflectionsGain = reflectionsGain;
        this.reflectionsDelay = reflectionsDelay;
        this.reflectionsPan = reflectionsPan;
        this.lateReverbGain = lateReverbGain;
        this.lateReverbDelay = lateReverbDelay;
        this.lateReverbPan = lateReverbPan;
        this.echoTime = echoTime;
        this.echoDepth = echoDepth;
        this.modulationTime = modulationTime;
        this.modulationDepth = modulationDepth;
        this.airAbsorptionGainHF = airAbsorptionGainHF;
        this.hfReference = hfReference;
        this.lfReference = lfReference;
        this.roomRolloffFactor = roomRolloffFactor;
        this.decayHFLimit = decayHFLimit;
    }

    public static Dictionary<string, ReverbPreset> Presets = new()
    {
        // Generic
        { "none", new("none", 1.0000f, 1.0000f, 0, 0, 1.0000f, 1.4900f, 0.8300f, 1.0000f, 1, 0, [0.0000f, 0.0000f, 0.0000f], 0, 0.0110f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "generic", new("generic", 1.0000f, 1.0000f, 0.3162f, 0.8913f, 1.0000f, 1.4900f, 0.8300f, 1.0000f, 0.0500f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 1.2589f, 0.0110f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "paddedcell", new("paddedcell", 0.1715f, 1.0000f, 0.3162f, 0.0010f, 1.0000f, 0.1700f, 0.1000f, 1.0000f, 0.2500f, 0.0010f, [0.0000f, 0.0000f, 0.0000f], 1.2691f, 0.0020f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "room", new("room", 0.4287f, 1.0000f, 0.3162f, 0.5929f, 1.0000f, 0.4000f, 0.8300f, 1.0000f, 0.1503f, 0.0020f, [0.0000f, 0.0000f, 0.0000f], 1.0629f, 0.0030f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "bathroom", new("bathroom", 0.1715f, 1.0000f, 0.3162f, 0.2512f, 1.0000f, 1.4900f, 0.5400f, 1.0000f, 0.6531f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 3.2734f, 0.0110f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "livingroom", new("livingroom", 0.9766f, 1.0000f, 0.3162f, 0.0010f, 1.0000f, 0.5000f, 0.1000f, 1.0000f, 0.2051f, 0.0030f, [0.0000f, 0.0000f, 0.0000f], 0.2805f, 0.0040f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "stoneroom", new("stoneroom", 1.0000f, 1.0000f, 0.3162f, 0.7079f, 1.0000f, 2.3100f, 0.6400f, 1.0000f, 0.4411f, 0.0120f, [0.0000f, 0.0000f, 0.0000f], 1.1003f, 0.0170f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "auditorium", new("auditorium", 1.0000f, 1.0000f, 0.3162f, 0.5781f, 1.0000f, 4.3200f, 0.5900f, 1.0000f, 0.4032f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 0.7170f, 0.0300f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "concerthall", new("concerthall", 1.0000f, 1.0000f, 0.3162f, 0.5623f, 1.0000f, 3.9200f, 0.7000f, 1.0000f, 0.2427f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 0.9977f, 0.0290f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "cave", new("cave", 1.0000f, 1.0000f, 0.3162f, 1.0000f, 1.0000f, 2.9100f, 1.3000f, 1.0000f, 0.5000f, 0.0150f, [0.0000f, 0.0000f, 0.0000f], 0.7063f, 0.0220f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },
        { "arena", new("arena", 1.0000f, 1.0000f, 0.3162f, 0.4477f, 1.0000f, 7.2400f, 0.3300f, 1.0000f, 0.2612f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 1.0186f, 0.0300f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "hangar", new("hangar", 1.0000f, 1.0000f, 0.3162f, 0.3162f, 1.0000f, 10.0500f, 0.2300f, 1.0000f, 0.5000f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 1.2560f, 0.0300f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "carpetedhallway", new("carpetedhallway", 0.4287f, 1.0000f, 0.3162f, 0.0100f, 1.0000f, 0.3000f, 0.1000f, 1.0000f, 0.1215f, 0.0020f, [0.0000f, 0.0000f, 0.0000f], 0.1531f, 0.0300f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "hallway", new("hallway", 0.3645f, 1.0000f, 0.3162f, 0.7079f, 1.0000f, 1.4900f, 0.5900f, 1.0000f, 0.2458f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 1.6615f, 0.0110f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "stonecorridor", new("stonecorridor", 1.0000f, 1.0000f, 0.3162f, 0.7612f, 1.0000f, 2.7000f, 0.7900f, 1.0000f, 0.2472f, 0.0130f, [0.0000f, 0.0000f, 0.0000f], 1.5758f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "sewerpipe", new("sewerpipe", 0.3071f, 0.8000f, 0.3162f, 0.3162f, 1.0000f, 2.8100f, 0.1400f, 1.0000f, 1.6387f, 0.0140f, [0.0000f, 0.0000f, 0.0000f], 3.2471f, 0.0210f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "underwater", new("underwater", 0.3645f, 1.0000f, 0.3162f, 0.0100f, 1.0000f, 1.4900f, 0.1000f, 1.0000f, 0.5963f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 7.0795f, 0.0110f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 1.1800f, 0.3480f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "drugged", new("drugged", 0.4287f, 0.5000f, 0.3162f, 1.0000f, 1.0000f, 8.3900f, 1.3900f, 1.0000f, 0.8760f, 0.0020f, [0.0000f, 0.0000f, 0.0000f], 3.1081f, 0.0300f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 1.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },
        { "dizzy", new("dizzy", 0.3645f, 0.6000f, 0.3162f, 0.6310f, 1.0000f, 17.2300f, 0.5600f, 1.0000f, 0.1392f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 0.4937f, 0.0300f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 1.0000f, 0.8100f, 0.3100f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },
        { "psychotic", new("psychotic", 0.0625f, 0.5000f, 0.3162f, 0.8404f, 1.0000f, 7.5600f, 0.9100f, 1.0000f, 0.4864f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 2.4378f, 0.0300f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 4.0000f, 1.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },
        
        // Castle   
        { "castle_smallroom", new("castle_smallroom", 1.0000f, 0.8900f, 0.3162f, 0.3981f, 0.1000f, 1.2200f, 0.8300f, 0.3100f, 0.8913f, 0.0220f, [0.0000f, 0.0000f, 0.0000f], 1.9953f, 0.0110f, [0.0000f, 0.0000f, 0.0000f], 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1) },
        { "castle_shortpassage", new("castle_shortpassage", 1.0000f, 0.8900f, 0.3162f, 0.3162f, 0.1000f, 2.3200f, 0.8300f, 0.3100f, 0.8913f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 1.2589f, 0.0230f, [0.0000f, 0.0000f, 0.0000f], 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1) },
        { "castle_mediumroom", new("castle_mediumroom", 1.0000f, 0.9300f, 0.3162f, 0.2818f, 0.1000f, 2.0400f, 0.8300f, 0.4600f, 0.6310f, 0.0220f, [0.0000f, 0.0000f, 0.0000f], 1.5849f, 0.0110f, [0.0000f, 0.0000f, 0.0000f], 0.1550f, 0.0300f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1) },
        { "castle_largeroom", new("castle_largeroom", 1.0000f, 0.8200f, 0.3162f, 0.2818f, 0.1259f, 2.5300f, 0.8300f, 0.5000f, 0.4467f, 0.0340f, [0.0000f, 0.0000f, 0.0000f], 1.2589f, 0.0160f, [0.0000f, 0.0000f, 0.0000f], 0.1850f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1) },
        { "castle_longpassage", new("castle_longpassage", 1.0000f, 0.8900f, 0.3162f, 0.3981f, 0.1000f, 3.4200f, 0.8300f, 0.3100f, 0.8913f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 1.4125f, 0.0230f, [0.0000f, 0.0000f, 0.0000f], 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1) },
        { "castle_hall", new("castle_hall", 1.0000f, 0.8100f, 0.3162f, 0.2818f, 0.1778f, 3.1400f, 0.7900f, 0.6200f, 0.1778f, 0.0560f, [0.0000f, 0.0000f, 0.0000f], 1.1220f, 0.0240f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1) },
        { "castle_cupboard", new("castle_cupboard", 1.0000f, 0.8900f, 0.3162f, 0.2818f, 0.1000f, 0.6700f, 0.8700f, 0.3100f, 1.4125f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 3.5481f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1) },
        { "castle_courtyard", new("castle_courtyard", 1.0000f, 0.4200f, 0.3162f, 0.4467f, 0.1995f, 2.1300f, 0.6100f, 0.2300f, 0.2239f, 0.1600f, [0.0000f, 0.0000f, 0.0000f], 0.7079f, 0.0360f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.3700f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },
        { "castle_alcove", new("castle_alcove", 1.0000f, 0.8900f, 0.3162f, 0.5012f, 0.1000f, 1.6400f, 0.8700f, 0.3100f, 1.0000f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 1.4125f, 0.0340f, [0.0000f, 0.0000f, 0.0000f], 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1) },

        // Factory  
        { "factory_smallroom", new("factory_smallroom", 0.3645f, 0.8200f, 0.3162f, 0.7943f, 0.5012f, 1.7200f, 0.6500f, 1.3100f, 0.7079f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 1.7783f, 0.0240f, [0.0000f, 0.0000f, 0.0000f], 0.1190f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1) },
        { "factory_shortpassage", new("factory_shortpassage", 0.3645f, 0.6400f, 0.2512f, 0.7943f, 0.5012f, 2.5300f, 0.6500f, 1.3100f, 1.0000f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 1.2589f, 0.0380f, [0.0000f, 0.0000f, 0.0000f], 0.1350f, 0.2300f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1) },
        { "factory_mediumroom", new("factory_mediumroom", 0.4287f, 0.8200f, 0.2512f, 0.7943f, 0.5012f, 2.7600f, 0.6500f, 1.3100f, 0.2818f, 0.0220f, [0.0000f, 0.0000f, 0.0000f], 1.4125f, 0.0230f, [0.0000f, 0.0000f, 0.0000f], 0.1740f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1) },
        { "factory_largeroom", new("factory_largeroom", 0.4287f, 0.7500f, 0.2512f, 0.7079f, 0.6310f, 4.2400f, 0.5100f, 1.3100f, 0.1778f, 0.0390f, [0.0000f, 0.0000f, 0.0000f], 1.1220f, 0.0230f, [0.0000f, 0.0000f, 0.0000f], 0.2310f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1) },
        { "factory_longpassage", new("factory_longpassage", 0.3645f, 0.6400f, 0.2512f, 0.7943f, 0.5012f, 4.0600f, 0.6500f, 1.3100f, 1.0000f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 1.2589f, 0.0370f, [0.0000f, 0.0000f, 0.0000f], 0.1350f, 0.2300f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1) },
        { "factory_hall", new("factory_hall", 0.4287f, 0.7500f, 0.3162f, 0.7079f, 0.6310f, 7.4300f, 0.5100f, 1.3100f, 0.0631f, 0.0730f, [0.0000f, 0.0000f, 0.0000f], 0.8913f, 0.0270f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1) },
        { "factory_cupboard", new("factory_cupboard", 0.3071f, 0.6300f, 0.2512f, 0.7943f, 0.5012f, 0.4900f, 0.6500f, 1.3100f, 1.2589f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 1.9953f, 0.0320f, [0.0000f, 0.0000f, 0.0000f], 0.1070f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1) },
        { "factory_courtyard", new("factory_courtyard", 0.3071f, 0.5700f, 0.3162f, 0.3162f, 0.6310f, 2.3200f, 0.2900f, 0.5600f, 0.2239f, 0.1400f, [0.0000f, 0.0000f, 0.0000f], 0.3981f, 0.0390f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.2900f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1) },
        { "factory_alcove", new("factory_alcove", 0.3645f, 0.5900f, 0.2512f, 0.7943f, 0.5012f, 3.1400f, 0.6500f, 1.3100f, 1.4125f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 1.0000f, 0.0380f, [0.0000f, 0.0000f, 0.0000f], 0.1140f, 0.1000f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1) },
        
        // Ice Palace
        { "icepalace_smallroom", new("icepalace_smallroom", 1.0000f, 0.8400f, 0.3162f, 0.5623f, 0.2818f, 1.5100f, 1.5300f, 0.2700f, 0.8913f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 1.4125f, 0.0110f, [0.0000f, 0.0000f, 0.0000f], 0.1640f, 0.1400f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1) },
        { "icepalace_shortpassage", new("icepalace_shortpassage", 1.0000f, 0.7500f, 0.3162f, 0.5623f, 0.2818f, 1.7900f, 1.4600f, 0.2800f, 0.5012f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 1.1220f, 0.0190f, [0.0000f, 0.0000f, 0.0000f], 0.1770f, 0.0900f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1) },
        { "icepalace_mediumroom", new("icepalace_mediumroom", 1.0000f, 0.8700f, 0.3162f, 0.5623f, 0.4467f, 2.2200f, 1.5300f, 0.3200f, 0.3981f, 0.0390f, [0.0000f, 0.0000f, 0.0000f], 1.1220f, 0.0270f, [0.0000f, 0.0000f, 0.0000f], 0.1860f, 0.1200f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1) },
        { "icepalace_largeroom", new("icepalace_largeroom", 1.0000f, 0.8100f, 0.3162f, 0.5623f, 0.4467f, 3.1400f, 1.5300f, 0.3200f, 0.2512f, 0.0390f, [0.0000f, 0.0000f, 0.0000f], 1.0000f, 0.0270f, [0.0000f, 0.0000f, 0.0000f], 0.2140f, 0.1100f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1) },
        { "icepalace_longpassage", new("icepalace_longpassage", 1.0000f, 0.7700f, 0.3162f, 0.5623f, 0.3981f, 3.0100f, 1.4600f, 0.2800f, 0.7943f, 0.0120f, [0.0000f, 0.0000f, 0.0000f], 1.2589f, 0.0250f, [0.0000f, 0.0000f, 0.0000f], 0.1860f, 0.0400f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1) },
        { "icepalace_hall", new("icepalace_hall", 1.0000f, 0.7600f, 0.3162f, 0.4467f, 0.5623f, 5.4900f, 1.5300f, 0.3800f, 0.1122f, 0.0540f, [0.0000f, 0.0000f, 0.0000f], 0.6310f, 0.0520f, [0.0000f, 0.0000f, 0.0000f], 0.2260f, 0.1100f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1) },
        { "icepalace_cupboard", new("icepalace_cupboard", 1.0000f, 0.8300f, 0.3162f, 0.5012f, 0.2239f, 0.7600f, 1.5300f, 0.2600f, 1.1220f, 0.0120f, [0.0000f, 0.0000f, 0.0000f], 1.9953f, 0.0160f, [0.0000f, 0.0000f, 0.0000f], 0.1430f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1) },
        { "icepalace_courtyard", new("icepalace_courtyard", 1.0000f, 0.5900f, 0.3162f, 0.2818f, 0.3162f, 2.0400f, 1.2000f, 0.3800f, 0.3162f, 0.1730f, [0.0000f, 0.0000f, 0.0000f], 0.3162f, 0.0430f, [0.0000f, 0.0000f, 0.0000f], 0.2350f, 0.4800f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1) },
        { "icepalace_alcove", new("icepalace_alcove", 1.0000f, 0.8400f, 0.3162f, 0.5623f, 0.2818f, 2.7600f, 1.4600f, 0.2800f, 1.1220f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 0.8913f, 0.0300f, [0.0000f, 0.0000f, 0.0000f], 0.1610f, 0.0900f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1) },

        // Space Station
        { "spacestation_smallroom", new("spacestation_smallroom", 0.2109f, 0.7000f, 0.3162f, 0.7079f, 0.8913f, 1.7200f, 0.8200f, 0.5500f, 0.7943f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 1.4125f, 0.0130f, [0.0000f, 0.0000f, 0.0000f], 0.1880f, 0.2600f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1) },
        { "spacestation_shortpassage", new("spacestation_shortpassage", 0.2109f, 0.8700f, 0.3162f, 0.6310f, 0.8913f, 3.5700f, 0.5000f, 0.5500f, 1.0000f, 0.0120f, [0.0000f, 0.0000f, 0.0000f], 1.1220f, 0.0160f, [0.0000f, 0.0000f, 0.0000f], 0.1720f, 0.2000f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1) },
        { "spacestation_mediumroom", new("spacestation_mediumroom", 0.2109f, 0.7500f, 0.3162f, 0.6310f, 0.8913f, 3.0100f, 0.5000f, 0.5500f, 0.3981f, 0.0340f, [0.0000f, 0.0000f, 0.0000f], 1.1220f, 0.0350f, [0.0000f, 0.0000f, 0.0000f], 0.2090f, 0.3100f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1) },
        { "spacestation_largeroom", new("spacestation_largeroom", 0.3645f, 0.8100f, 0.3162f, 0.6310f, 0.8913f, 3.8900f, 0.3800f, 0.6100f, 0.3162f, 0.0560f, [0.0000f, 0.0000f, 0.0000f], 0.8913f, 0.0350f, [0.0000f, 0.0000f, 0.0000f], 0.2330f, 0.2800f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1) },
        { "spacestation_longpassage", new("spacestation_longpassage", 0.4287f, 0.8200f, 0.3162f, 0.6310f, 0.8913f, 4.6200f, 0.6200f, 0.5500f, 1.0000f, 0.0120f, [0.0000f, 0.0000f, 0.0000f], 1.2589f, 0.0310f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.2300f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1) },
        { "spacestation_hall", new("spacestation_hall", 0.4287f, 0.8700f, 0.3162f, 0.6310f, 0.8913f, 7.1100f, 0.3800f, 0.6100f, 0.1778f, 0.1000f, [0.0000f, 0.0000f, 0.0000f], 0.6310f, 0.0470f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.2500f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1) },
        { "spacestation_cupboard", new("spacestation_cupboard", 0.1715f, 0.5600f, 0.3162f, 0.7079f, 0.8913f, 0.7900f, 0.8100f, 0.5500f, 1.4125f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 1.7783f, 0.0180f, [0.0000f, 0.0000f, 0.0000f], 0.1810f, 0.3100f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1) },
        { "spacestation_alcove", new("spacestation_alcove", 0.2109f, 0.7800f, 0.3162f, 0.7079f, 0.8913f, 1.1600f, 0.8100f, 0.5500f, 1.4125f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 1.0000f, 0.0180f, [0.0000f, 0.0000f, 0.0000f], 0.1920f, 0.2100f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1) },
        
        // Wood
        { "wooden_smallroom", new("wooden_smallroom", 1.0000f, 1.0000f, 0.3162f, 0.1122f, 0.3162f, 0.7900f, 0.3200f, 0.8700f, 1.0000f, 0.0320f, [0.0000f, 0.0000f, 0.0000f], 0.8913f, 0.0290f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1) },
        { "wooden_shortpassage", new("wooden_shortpassage", 1.0000f, 1.0000f, 0.3162f, 0.1259f, 0.3162f, 1.7500f, 0.5000f, 0.8700f, 0.8913f, 0.0120f, [0.0000f, 0.0000f, 0.0000f], 0.6310f, 0.0240f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1) },
        { "wooden_mediumroom", new("wooden_mediumroom", 1.0000f, 1.0000f, 0.3162f, 0.1000f, 0.2818f, 1.4700f, 0.4200f, 0.8200f, 0.8913f, 0.0490f, [0.0000f, 0.0000f, 0.0000f], 0.8913f, 0.0290f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1) },
        { "wooden_largeroom", new("wooden_largeroom", 1.0000f, 1.0000f, 0.3162f, 0.0891f, 0.2818f, 2.6500f, 0.3300f, 0.8200f, 0.8913f, 0.0660f, [0.0000f, 0.0000f, 0.0000f], 0.7943f, 0.0490f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1) },
        { "wooden_longpassage", new("wooden_longpassage", 1.0000f, 1.0000f, 0.3162f, 0.1000f, 0.3162f, 1.9900f, 0.4000f, 0.7900f, 1.0000f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 0.4467f, 0.0360f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1) },
        { "wooden_hall", new("wooden_hall", 1.0000f, 1.0000f, 0.3162f, 0.0794f, 0.2818f, 3.4500f, 0.3000f, 0.8200f, 0.8913f, 0.0880f, [0.0000f, 0.0000f, 0.0000f], 0.7943f, 0.0630f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1) },
        { "wooden_cupboard", new("wooden_cupboard", 1.0000f, 1.0000f, 0.3162f, 0.1413f, 0.3162f, 0.5600f, 0.4600f, 0.9100f, 1.1220f, 0.0120f, [0.0000f, 0.0000f, 0.0000f], 1.1220f, 0.0280f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1) },
        { "wooden_courtyard", new("wooden_courtyard", 1.0000f, 0.6500f, 0.3162f, 0.0794f, 0.3162f, 1.7900f, 0.3500f, 0.7900f, 0.5623f, 0.1230f, [0.0000f, 0.0000f, 0.0000f], 0.1000f, 0.0320f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1) },
        { "wooden_alcove", new("wooden_alcove", 1.0000f, 1.0000f, 0.3162f, 0.1259f, 0.3162f, 1.2200f, 0.6200f, 0.9100f, 1.1220f, 0.0120f, [0.0000f, 0.0000f, 0.0000f], 0.7079f, 0.0240f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1) },

        // Sports
        { "sport_emptystadium", new("sport_emptystadium", 1.0000f, 1.0000f, 0.3162f, 0.4467f, 0.7943f, 6.2600f, 0.5100f, 1.1000f, 0.0631f, 0.1830f, [0.0000f, 0.0000f, 0.0000f], 0.3981f, 0.0380f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "sport_squashcourt", new("sport_squashcourt", 1.0000f, 0.7500f, 0.3162f, 0.3162f, 0.7943f, 2.2200f, 0.9100f, 1.1600f, 0.4467f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 0.7943f, 0.0110f, [0.0000f, 0.0000f, 0.0000f], 0.1260f, 0.1900f, 0.2500f, 0.0000f, 0.9943f, 7176.8999f, 211.2000f, 0.0000f, 0x1) },
        { "sport_smallswimmingpool", new("sport_smallswimmingpool", 1.0000f, 0.7000f, 0.3162f, 0.7943f, 0.8913f, 2.7600f, 1.2500f, 1.1400f, 0.6310f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 0.7943f, 0.0300f, [0.0000f, 0.0000f, 0.0000f], 0.1790f, 0.1500f, 0.8950f, 0.1900f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },
        { "sport_largeswimmingpool", new("sport_largeswimmingpool", 1.0000f, 0.8200f, 0.3162f, 0.7943f, 1.0000f, 5.4900f, 1.3100f, 1.1400f, 0.4467f, 0.0390f, [0.0000f, 0.0000f, 0.0000f], 0.5012f, 0.0490f, [0.0000f, 0.0000f, 0.0000f], 0.2220f, 0.5500f, 1.1590f, 0.2100f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },
        { "sport_gymnasium", new("sport_gymnasium", 1.0000f, 0.8100f, 0.3162f, 0.4467f, 0.8913f, 3.1400f, 1.0600f, 1.3500f, 0.3981f, 0.0290f, [0.0000f, 0.0000f, 0.0000f], 0.5623f, 0.0450f, [0.0000f, 0.0000f, 0.0000f], 0.1460f, 0.1400f, 0.2500f, 0.0000f, 0.9943f, 7176.8999f, 211.2000f, 0.0000f, 0x1) },
        { "sport_fullstadium", new("sport_fullstadium", 1.0000f, 1.0000f, 0.3162f, 0.0708f, 0.7943f, 5.2500f, 0.1700f, 0.8000f, 0.1000f, 0.1880f, [0.0000f, 0.0000f, 0.0000f], 0.2818f, 0.0380f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "sport_stadiumtannoy", new("sport_stadiumtannoy", 1.0000f, 0.7800f, 0.3162f, 0.5623f, 0.5012f, 2.5300f, 0.8800f, 0.6800f, 0.2818f, 0.2300f, [0.0000f, 0.0000f, 0.0000f], 0.5012f, 0.0630f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.2000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        
        // Prefab
        { "prefab_workshop", new("prefab_workshop", 0.4287f, 1.0000f, 0.3162f, 0.1413f, 0.3981f, 0.7600f, 1.0000f, 1.0000f, 1.0000f, 0.0120f, [0.0000f, 0.0000f, 0.0000f], 1.1220f, 0.0120f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },
        { "prefab_practiseroom", new("prefab_practiseroom", 0.4022f, 0.8700f, 0.3162f, 0.3981f, 0.5012f, 1.1200f, 0.5600f, 0.1800f, 1.2589f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 1.4125f, 0.0110f, [0.0000f, 0.0000f, 0.0000f], 0.0950f, 0.1400f, 0.2500f, 0.0000f, 0.9943f, 7176.8999f, 211.2000f, 0.0000f, 0x1) },
        { "prefab_outhouse", new("prefab_outhouse", 1.0000f, 0.8200f, 0.3162f, 0.1122f, 0.1585f, 1.3800f, 0.3800f, 0.3500f, 0.8913f, 0.0240f, [0.0000f, 0.0000f, -0.0000f], 0.6310f, 0.0440f, [0.0000f, 0.0000f, 0.0000f], 0.1210f, 0.1700f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 107.5000f, 0.0000f, 0x0) },
        { "prefab_caravan", new("prefab_caravan", 1.0000f, 1.0000f, 0.3162f, 0.0891f, 0.1259f, 0.4300f, 1.5000f, 1.0000f, 1.0000f, 0.0120f, [0.0000f, 0.0000f, 0.0000f], 1.9953f, 0.0120f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },
        
        // Dome and Pipe
        { "dome_tomb", new("dome_tomb", 1.0000f, 0.7900f, 0.3162f, 0.3548f, 0.2239f, 4.1800f, 0.2100f, 0.1000f, 0.3868f, 0.0300f, [0.0000f, 0.0000f, 0.0000f], 1.6788f, 0.0220f, [0.0000f, 0.0000f, 0.0000f], 0.1770f, 0.1900f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, 0x0) },
        { "dome_saintpauls", new("dome_saintpauls", 1.0000f, 0.8700f, 0.3162f, 0.3548f, 0.2239f, 10.4800f, 0.1900f, 0.1000f, 0.1778f, 0.0900f, [0.0000f, 0.0000f, 0.0000f], 1.2589f, 0.0420f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.1200f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, 0x1) },
        { "pipe_small", new("pipe_small", 1.0000f, 1.0000f, 0.3162f, 0.3548f, 0.2239f, 5.0400f, 0.1000f, 0.1000f, 0.5012f, 0.0320f, [0.0000f, 0.0000f, 0.0000f], 2.5119f, 0.0150f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, 0x1) },
        { "pipe_longthin", new("pipe_longthin", 0.2560f, 0.9100f, 0.3162f, 0.4467f, 0.2818f, 9.2100f, 0.1800f, 0.1000f, 0.7079f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 0.7079f, 0.0220f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, 0x0) },
        { "pipe_large", new("pipe_large", 1.0000f, 1.0000f, 0.3162f, 0.3548f, 0.2239f, 8.4500f, 0.1000f, 0.1000f, 0.3981f, 0.0460f, [0.0000f, 0.0000f, 0.0000f], 1.5849f, 0.0320f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, 0x1) },
        { "pipe_resonant", new("pipe_resonant", 0.1373f, 0.9100f, 0.3162f, 0.4467f, 0.2818f, 6.8100f, 0.1800f, 0.1000f, 0.7079f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 1.0000f, 0.0220f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, 0x0) },

        // Outside
        { "backyard", new("backyard", 1.0000f, 0.4500f, 0.3162f, 0.2512f, 0.5012f, 1.1200f, 0.3400f, 0.4600f, 0.4467f, 0.0690f, [0.0000f, 0.0000f, -0.0000f], 0.7079f, 0.0230f, [0.0000f, 0.0000f, 0.0000f], 0.2180f, 0.3400f, 0.2500f, 0.0000f, 0.9943f, 4399.1001f, 242.9000f, 0.0000f, 0x0) },
        { "rollingplains", new("rollingplains", 1.0000f, 0.0000f, 0.3162f, 0.0112f, 0.6310f, 2.1300f, 0.2100f, 0.4600f, 0.1778f, 0.3000f, [0.0000f, 0.0000f, -0.0000f], 0.4467f, 0.0190f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 4399.1001f, 242.9000f, 0.0000f, 0x0) },
        { "deepcanyon", new("deepcanyon", 1.0000f, 0.7400f, 0.3162f, 0.1778f, 0.6310f, 3.8900f, 0.2100f, 0.4600f, 0.3162f, 0.2230f, [0.0000f, 0.0000f, -0.0000f], 0.3548f, 0.0190f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 4399.1001f, 242.9000f, 0.0000f, 0x0) },
        { "creek", new("creek", 1.0000f, 0.3500f, 0.3162f, 0.1778f, 0.5012f, 2.1300f, 0.2100f, 0.4600f, 0.3981f, 0.1150f, [0.0000f, 0.0000f, -0.0000f], 0.1995f, 0.0310f, [0.0000f, 0.0000f, 0.0000f], 0.2180f, 0.3400f, 0.2500f, 0.0000f, 0.9943f, 4399.1001f, 242.9000f, 0.0000f, 0x0) },
        { "valley", new("valley", 1.0000f, 0.2800f, 0.3162f, 0.0282f, 0.1585f, 2.8800f, 0.2600f, 0.3500f, 0.1413f, 0.2630f, [0.0000f, 0.0000f, -0.0000f], 0.3981f, 0.1000f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.3400f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 107.5000f, 0.0000f, 0x0) },
        { "alley", new("alley", 1.0000f, 0.3000f, 0.3162f, 0.7328f, 1.0000f, 1.4900f, 0.8600f, 1.0000f, 0.2500f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 0.9954f, 0.0110f, [0.0000f, 0.0000f, 0.0000f], 0.1250f, 0.9500f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "forest", new("forest", 1.0000f, 0.3000f, 0.3162f, 0.0224f, 1.0000f, 1.4900f, 0.5400f, 1.0000f, 0.0525f, 0.1620f, [0.0000f, 0.0000f, 0.0000f], 0.7682f, 0.0880f, [0.0000f, 0.0000f, 0.0000f], 0.1250f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "city", new("city", 1.0000f, 0.5000f, 0.3162f, 0.3981f, 1.0000f, 1.4900f, 0.6700f, 1.0000f, 0.0730f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 0.1427f, 0.0110f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "mountains", new("mountains", 1.0000f, 0.2700f, 0.3162f, 0.0562f, 1.0000f, 1.4900f, 0.2100f, 1.0000f, 0.0407f, 0.3000f, [0.0000f, 0.0000f, 0.0000f], 0.1919f, 0.1000f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },
        { "quarry", new("quarry", 1.0000f, 1.0000f, 0.3162f, 0.3162f, 1.0000f, 1.4900f, 0.8300f, 1.0000f, 0.0000f, 0.0610f, [0.0000f, 0.0000f, 0.0000f], 1.7783f, 0.0250f, [0.0000f, 0.0000f, 0.0000f], 0.1250f, 0.7000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "plain", new("plain", 1.0000f, 0.2100f, 0.3162f, 0.1000f, 1.0000f, 1.4900f, 0.5000f, 1.0000f, 0.0585f, 0.1790f, [0.0000f, 0.0000f, 0.0000f], 0.1089f, 0.1000f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "parkinglot", new("parkinglot", 1.0000f, 1.0000f, 0.3162f, 1.0000f, 1.0000f, 1.6500f, 1.5000f, 1.0000f, 0.2082f, 0.0080f, [0.0000f, 0.0000f, 0.0000f], 0.2652f, 0.0120f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },
        
        // Mood
        { "mood_heaven", new("mood_heaven", 1.0000f, 0.9400f, 0.3162f, 0.7943f, 0.4467f, 5.0400f, 1.1200f, 0.5600f, 0.2427f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 1.2589f, 0.0290f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0800f, 2.7420f, 0.0500f, 0.9977f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "mood_hell", new("mood_hell", 1.0000f, 0.5700f, 0.3162f, 0.3548f, 0.4467f, 3.5700f, 0.4900f, 2.0000f, 0.0000f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 1.4125f, 0.0300f, [0.0000f, 0.0000f, 0.0000f], 0.1100f, 0.0400f, 2.1090f, 0.5200f, 0.9943f, 5000.0000f, 139.5000f, 0.0000f, 0x0) },
        { "mood_memory", new("mood_memory", 1.0000f, 0.8500f, 0.3162f, 0.6310f, 0.3548f, 4.0600f, 0.8200f, 0.5600f, 0.0398f, 0.0000f, [0.0000f, 0.0000f, 0.0000f], 1.1220f, 0.0000f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.4740f, 0.4500f, 0.9886f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },

        // Driving
        { "driving_commentator", new("driving_commentator", 1.0000f, 0.0000f, 3.1623f, 0.5623f, 0.5012f, 2.4200f, 0.8800f, 0.6800f, 0.1995f, 0.0930f, [0.0000f, 0.0000f, 0.0000f], 0.2512f, 0.0170f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9886f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "driving_pitgarage", new("driving_pitgarage", 0.4287f, 0.5900f, 0.3162f, 0.7079f, 0.5623f, 1.7200f, 0.9300f, 0.8700f, 0.5623f, 0.0000f, [0.0000f, 0.0000f, 0.0000f], 1.2589f, 0.0160f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.1100f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },
        { "driving_incar_racer", new("driving_incar_racer", 0.0832f, 0.8000f, 0.3162f, 1.0000f, 0.7943f, 0.1700f, 2.0000f, 0.4100f, 1.7783f, 0.0070f, [0.0000f, 0.0000f, 0.0000f], 0.7079f, 0.0150f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10268.2002f, 251.0000f, 0.0000f, 0x1) },
        { "driving_incar_sports", new("driving_incar_sports", 0.0832f, 0.8000f, 0.3162f, 0.6310f, 1.0000f, 0.1700f, 0.7500f, 0.4100f, 1.0000f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 0.5623f, 0.0000f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10268.2002f, 251.0000f, 0.0000f, 0x1) },
        { "driving_incar_luxury", new("driving_incar_luxury", 0.2560f, 1.0000f, 0.3162f, 0.1000f, 0.5012f, 0.1300f, 0.4100f, 0.4600f, 0.7943f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 1.5849f, 0.0100f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10268.2002f, 251.0000f, 0.0000f, 0x1) },
        { "driving_fullgrandstand", new("driving_fullgrandstand", 1.0000f, 1.0000f, 0.3162f, 0.2818f, 0.6310f, 3.0100f, 1.3700f, 1.2800f, 0.3548f, 0.0900f, [0.0000f, 0.0000f, 0.0000f], 0.1778f, 0.0490f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10420.2002f, 250.0000f, 0.0000f, 0x0) },
        { "driving_emptygrandstand", new("driving_emptygrandstand", 1.0000f, 1.0000f, 0.3162f, 1.0000f, 0.7943f, 4.6200f, 1.7500f, 1.4000f, 0.2082f, 0.0900f, [0.0000f, 0.0000f, 0.0000f], 0.2512f, 0.0490f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10420.2002f, 250.0000f, 0.0000f, 0x0) },
        { "driving_tunnel", new("driving_tunnel", 1.0000f, 0.8100f, 0.3162f, 0.3981f, 0.8913f, 3.4200f, 0.9400f, 1.3100f, 0.7079f, 0.0510f, [0.0000f, 0.0000f, 0.0000f], 0.7079f, 0.0470f, [0.0000f, 0.0000f, 0.0000f], 0.2140f, 0.0500f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 155.3000f, 0.0000f, 0x1) },

        // City
        { "city_streets", new("city_streets", 1.0000f, 0.7800f, 0.3162f, 0.7079f, 0.8913f, 1.7900f, 1.1200f, 0.9100f, 0.2818f, 0.0460f, [0.0000f, 0.0000f, 0.0000f], 0.1995f, 0.0280f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.2000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "city_subway", new("city_subway", 1.0000f, 0.7400f, 0.3162f, 0.7079f, 0.8913f, 3.0100f, 1.2300f, 0.9100f, 0.7079f, 0.0460f, [0.0000f, 0.0000f, 0.0000f], 1.2589f, 0.0280f, [0.0000f, 0.0000f, 0.0000f], 0.1250f, 0.2100f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "city_museum", new("city_museum", 1.0000f, 0.8200f, 0.3162f, 0.1778f, 0.1778f, 3.2800f, 1.4000f, 0.5700f, 0.2512f, 0.0390f, [0.0000f, 0.0000f, -0.0000f], 0.8913f, 0.0340f, [0.0000f, 0.0000f, 0.0000f], 0.1300f, 0.1700f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 107.5000f, 0.0000f, 0x0) },
        { "city_library", new("city_library", 1.0000f, 0.8200f, 0.3162f, 0.2818f, 0.0891f, 2.7600f, 0.8900f, 0.4100f, 0.3548f, 0.0290f, [0.0000f, 0.0000f, -0.0000f], 0.8913f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 0.1300f, 0.1700f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 107.5000f, 0.0000f, 0x0) },
        { "city_underpass", new("city_underpass", 1.0000f, 0.8200f, 0.3162f, 0.4467f, 0.8913f, 3.5700f, 1.1200f, 0.9100f, 0.3981f, 0.0590f, [0.0000f, 0.0000f, 0.0000f], 0.8913f, 0.0370f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.1400f, 0.2500f, 0.0000f, 0.9920f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "city_abandoned", new("city_abandoned", 1.0000f, 0.6900f, 0.3162f, 0.7943f, 0.8913f, 3.2800f, 1.1700f, 0.9100f, 0.4467f, 0.0440f, [0.0000f, 0.0000f, 0.0000f], 0.2818f, 0.0240f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.2000f, 0.2500f, 0.0000f, 0.9966f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        
        // Miscellaneous
        { "dustyroom", new("dustyroom", 0.3645f, 0.5600f, 0.3162f, 0.7943f, 0.7079f, 1.7900f, 0.3800f, 0.2100f, 0.5012f, 0.0020f, [0.0000f, 0.0000f, 0.0000f], 1.2589f, 0.0060f, [0.0000f, 0.0000f, 0.0000f], 0.2020f, 0.0500f, 0.2500f, 0.0000f, 0.9886f, 13046.0000f, 163.3000f, 0.0000f, 0x1) },
        { "chapel", new("chapel", 1.0000f, 0.8400f, 0.3162f, 0.5623f, 1.0000f, 4.6200f, 0.6400f, 1.2300f, 0.4467f, 0.0320f, [0.0000f, 0.0000f, 0.0000f], 0.7943f, 0.0490f, [0.0000f, 0.0000f, 0.0000f], 0.2500f, 0.0000f, 0.2500f, 0.1100f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1) },
        { "smallwaterroom", new("smallwaterroom", 1.0000f, 0.7000f, 0.3162f, 0.4477f, 1.0000f, 1.5100f, 1.2500f, 1.1400f, 0.8913f, 0.0200f, [0.0000f, 0.0000f, 0.0000f], 1.4125f, 0.0300f, [0.0000f, 0.0000f, 0.0000f], 0.1790f, 0.1500f, 0.8950f, 0.1900f, 0.9920f, 5000.0000f, 250.0000f, 0.0000f, 0x0) },
    };
}
