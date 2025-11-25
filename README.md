# OpenAL Soft Bindings for C#

Modern, high-performance C# bindings for OpenAL Soft, with managed wrappers for ease of use.

## Features

- **Modern C# Interop**: Uses `LibraryImport` and `Span<T>` for optimal performance
- **Zero-Allocation Design**: Leverages stack allocation and spans to minimize heap allocations
- **Complete Coverage**: Supports OpenAL 1.0, OpenAL 1.1, and:
  - Effects Extensions (EFX) for audio effects and filters
  - OpenAL Extensions (ALEXT, SOFT) for latest functionality
  - Debug extensions for development
- **Managed Wrappers**: High-level managed classes for common scenarios:
  - `ALDevice` for device management
  - `ALContext` for context creation and management
  - `ALCaptureDevice` for microphone input
  - `ALSource` for sources
  - `ALFilter` and `ALReverbEffect` for filters and effects

## Requirements

- .NET 8.0 or later
- OpenAL Soft library:
  - soft_oal.dll on Windows
  - libopenal.so on Linux
  - libopenal.1.dylib on OSX

## Quick Start

### Basic Setup

```csharp
using OpenAL;
using OpenAL.managed;

// Get all devices
var deviceNames = AL.GetStringList(IntPtr.Zero, AL.ALC_ALL_DEVICES_SPECIFIER);

// Open a device
var device = new ALDevice(deviceNames[0]);

// Create a context
var context = new ALContext(device, new ALContextSettings()
{
    HRTFEnabled = true,
    SampleRate = 48000
});

// Make the context current
context.MakeCurrent();

// Generate a buffer and source
uint buffer = AL.GenBuffer();
uint source = AL.GenSource();

// Load PCM data into the buffer
AL.BufferData(buffer, AL.AL_FORMAT_MONO16, pcmData, sampleRate);

// Configure and play the source
AL.Sourcei(source, AL.AL_BUFFER, (int)buffer);
AL.SourcePlay(source);

// Cleanup
AL.DeleteSource(source);
AL.DeleteBuffer(buffer);

context.Destroy();
device.Close();
```

### Effects (EFX)

```csharp
// Generate an effect and auxiliary send
uint effect = AL.GenEffect();
uint auxSlot = AL.GenAuxiliaryEffectSlot();

// Configure reverb effect
AL.Effecti(effect, AL.AL_EFFECT_TYPE, AL.AL_EFFECT_REVERB);
AL.Effectf(effect, AL.AL_REVERB_GAIN, 0.7f);
AL.Effectf(effect, AL.AL_REVERB_DECAY_TIME, 2.5f);

// Attach effect to auxiliary slot
AL.AuxiliaryEffectSloti(auxSlot, AL.AL_EFFECTSLOT_EFFECT, (int)effect);

// Connect source to effect slot
AL.Source3i(source, AL.AL_AUXILIARY_SEND_FILTER, (int)auxSlot, 0, AL.AL_FILTER_NULL);
```

### 3D Spatialised Audio

```csharp
// Set listener orientation using span
Span<float> orientation = [ 0f, 0f, -1f, 0f, 1f, 0f ];
AL.Listenerfv(AL.AL_ORIENTATION, orientation);

// Position a source in 3D space
AL.Source3f(source, AL.AL_POSITION, 5f, 0f, -10f);
AL.Source3f(source, AL.AL_VELOCITY, 0f, 0f, 1f);

// Configure distance model
AL.DistanceModel(AL.AL_INVERSE_DISTANCE_CLAMPED);
AL.Sourcef(source, AL.AL_REFERENCE_DISTANCE, 1f);
AL.Sourcef(source, AL.AL_MAX_DISTANCE, 100f);
```

## Architecture

The library is organized into several layers:

### Internal Bindings (`internal/`)

Low-level P/Invoke declarations that directly wrap OpenAL native functions:

- `ALBindings.cs` - Core OpenAL
- `ALCBindings.cs` - Context management
- `EFXBindings.cs` - Effects and filters
- `ALEXTBindings.cs` - OpenAL extensions
- `DebugBindings.cs` - Debug callback

### Public API (`public/`)

Clean C# wrappers for functions and constants:

- `AL.cs` - Core OpenAL
- `ALC.cs` - Context management
- `EFX.cs` - Effects and filters
- `ALEXT.cs` - OpenAL extensions
- `*Constants.cs` - OpenAL constants

### Managed Wrappers (`managed/`)

High-level classes for common use cases:

- `ALDevice.cs` / `ALCaptureDevice.cs` - Device lifecycle management
- `ALContext.cs` - Context creation with HRTF support
- `ALSource.cs` - Audio source management
- `ALFilter.cs` - Filter management
- `ALReverbEffect.cs` - Reverb effect helpers

## Thread Safety

OpenAL's error state is per-context rather than per-thread. For multithreaded applications:

- Create separate contexts for different threads
- Use synchronization when sharing contexts
- Consider using thread-local contexts for audio processing threads

## License

MIT No Attribution

Copyright 2025 Vercidium Pty Ltd

See [license.txt](license.txt) for full license text.

## Contributing

Contributions are welcome! The project follows modern C# conventions:

- Use of `LibraryImport` over `DllImport`
- Span-based APIs where appropriate

## Resources

- [OpenAL Soft Repository](https://github.com/kcat/openal-soft)
- [Extensions and PDF Specification](https://github.com/Raulshc/OpenAL-EXT-Repository/tree/master)

## Status

This is an actively developed project. Current focus areas:

- Improving managed wrapper APIs
- Adding comprehensive examples
