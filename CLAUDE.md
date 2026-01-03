This repository contains C# bindings for the soft_oal.dll OpenAL Soft C DLL.

The managed folder contains managed wrappers for AL objects, like devices, contexts, sources, filters and reverb effects.

The internal folder contains the P/Invoke calls to soft_oal.dll, and the public folder contains C#-developer friendly functions for using these P/Invoke calls (converts strings, handles IntPtr with Span<T>, etc).

When writing XML documentation, don't use an 's' prefix on the first word, i.e. prefer 'Create a reverb effect' over 'Creates a reverb effect' when describing a function that creates a reverb effect. Also omit the word 'The' at the start of the sentence when describing properties on a class.