global using System;
global using System.Diagnostics;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Collections.Generic;

namespace OpenAL;

public enum ALDistanceModel
{
    None = AL.AL_NONE,
    InverseDistance = AL.AL_INVERSE_DISTANCE,
    InverseDistanceClamped = AL.AL_INVERSE_DISTANCE_CLAMPED,
    LinearDistance = AL.AL_LINEAR_DISTANCE,
    LinearDistanceClamped = AL.AL_LINEAR_DISTANCE_CLAMPED,
    ExponentDistance = AL.AL_EXPONENT_DISTANCE,
    ExponentDistanceClamped = AL.AL_EXPONENT_DISTANCE_CLAMPED,
}