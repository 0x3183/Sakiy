namespace Sakiy.Api
{
    public static class Events
    {
        public static event Action Exit = () => { };
        public static event Action Tick = () => { };
        static Events()
        {
            AppDomain.CurrentDomain.ProcessExit += (s, e) => { Exit(); };
            AppDomain.CurrentDomain.UnhandledException += (sender, ex) =>
            {
                Logs.Log($"Unhandled Exception: {ex.ExceptionObject}");
                Environment.Exit(1);
            };
            Thread thread = new Thread(Loop)
            {
                IsBackground = false,
            };
            thread.Start();
        }
        private static void Loop()
        {
            while (true)
            {
                Thread.Sleep(50);
                Tick();
            }
        }
    }
}
