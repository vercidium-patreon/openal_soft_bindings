This repository contains C# bindings for the soft_oal.dll OpenAL Soft C DLL.

The managed folder contains managed wrappers for AL objects, like devices, contexts, sources, filters and reverb effects.

The internal folder contains the P/Invoke calls to soft_oal.dll, and the public folder contains C#-developer friendly functions for using these P/Invoke calls (converts strings, handles IntPtr with Span<T>, etc).