using Sakiy.Util;
using System.Reflection;

namespace Sakiy.Api
{
    public static class Config
    {
        public static Decoder Read(string name)
        {
            return new Decoder(File.Open(Path.Combine(Environment.CurrentDirectory, "Config", Assembly.GetCallingAssembly().GetName().Name ?? "$Global", name), FileMode.OpenOrCreate, FileAccess.Read, FileShare.None));
        }
        public static Encoder Write(string name)
        {
            return new Encoder(File.Open(Path.Combine(Environment.CurrentDirectory, "Config", Assembly.GetCallingAssembly().GetName().Name ?? "$Global", name), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None));
        }
        public static void Reset()
        {
            string? con = Assembly.GetCallingAssembly().GetName().Name;
            if (con == null) return;
            string dir = Path.Combine(Environment.CurrentDirectory, "Config", con);
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
        }
        public static void Reset(string name)
        {
            string? con = Assembly.GetCallingAssembly().GetName().Name;
            if (con == null) return;
            string file = Path.Combine(Environment.CurrentDirectory, "Config", con, name);
            if (File.Exists(file)) File.Delete(file);
        }
    }
}
