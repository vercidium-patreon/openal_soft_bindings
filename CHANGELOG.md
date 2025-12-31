# Version 1.0.3
Created a new ALStreamSource class for streaming data to an OpenAL source (e.g. microphone input)

# Version 1.0.2
ALCaptureDevice now supports all sound formats.
Removed the InputThreshold property from ALCaptureDevice.
Created a new ReverbPreset class that holds EAX parameters and many default EAX presets.
Created a new ALReverbEffect.CopyFromPreset() function.

# Version 1.0.1
Renamed the ALFilter.gainLF field to ALFilter.gain, to correctly match the underlying OpenAL filter property.