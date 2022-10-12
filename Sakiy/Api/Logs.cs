using System.Reflection;

namespace Sakiy.Api
{
    public static class Logs
    {
        private static readonly FileStream Logfile;
        static Logs()
        {
            Logfile = new("log.txt", FileMode.Create);
            Events.Exit += Logfile.Close;
        }
        public static void Log(object message)
        {
            Monitor.Enter(Logfile);
            string line = $"[{Assembly.GetCallingAssembly().GetName().Name}] {message}\n";
            Logfile.Write(System.Text.Encoding.UTF8.GetBytes(line));
            Console.Write(line);
            Monitor.Exit(Logfile);
        }
    }
}
