namespace OpenAL
{
    public static class Logger
    {
        // Disable logging by default. User can override this callback
        public static Action<string> Log = (_) => { };
        public static Action<string> Error = (_) => { };
    }
}
