# Version 1.0.5
Added an effectSlotGain parameter to ALReverbEffect

# Version 1.0.4
Added XML comments to all managed classes.
Added destructor asserts to all managed classes (in DEBUG mode only) to alert when they are collected by the GC, but their AL resources have not been freed.
Each ALContext now has its own debug callback function.
ALDevice.Reopen() now functions correctly.

# Version 1.0.3
Created a new ALStreamSource class for streaming data to an OpenAL source (e.g. microphone input)

# Version 1.0.2
ALCaptureDevice now supports all sound formats.
Removed the InputThreshold property from ALCaptureDevice.
Created a new ReverbPreset class that holds EAX parameters and many default EAX presets.
Created a new ALReverbEffect.CopyFromPreset() function.

# Version 1.0.1
Renamed the ALFilter.gainLF field to ALFilter.gain, to correctly match the underlying OpenAL filter property.