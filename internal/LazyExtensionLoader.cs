namespace OpenAL;

internal class LazyExtensionLoader<TDelegate> where TDelegate : Delegate
{
    private readonly Lazy<TDelegate> lazyDelegate;

    internal LazyExtensionLoader(string extensionName, string functionName, bool isAlcExtension)
    {
        lazyDelegate = new Lazy<TDelegate>(() =>
        {
            // Check if the extension exists
            bool extensionPresent = isAlcExtension
                ? AL.alcIsExtensionPresent(IntPtr.Zero, extensionName)
                : AL.alIsExtensionPresent(extensionName);

            if (!extensionPresent)
                return null;

            // Get the function pointer
            var ptr = isAlcExtension
                ? AL.alcGetProcAddress(IntPtr.Zero, functionName)
                : AL.GetProcAddress(IntPtr.Zero, functionName);

            if (ptr == IntPtr.Zero)
                return null;

            // Convert to delegate and cache it
            return Marshal.GetDelegateForFunctionPointer<TDelegate>(ptr);
        });
    }

    internal TDelegate Function => lazyDelegate.Value;

    internal bool IsAvailable => Function != null;
}
