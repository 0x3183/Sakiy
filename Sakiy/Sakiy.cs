using Sakiy.Api;
using System.Net;
using System.Reflection;

namespace Sakiy
{
    public static class Sakiy
    {
        private static int Main(string[] arguments)
        {
            string[] plugindir = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "plugins"));
            for (int i = 0; i < plugindir.Length; i++) if (plugindir[i].EndsWith(".dll")) Assembly.LoadFile(plugindir[i]);
            for (int i = 0; i < plugindir.Length; i++) if (plugindir[i].EndsWith(".dll")) Assembly.LoadFile(plugindir[i]).CreateInstance("Extension");
            Logs.Log($"Initializing Listeners.");
            Endpoints.Add(new IPEndPoint(IPAddress.Any, 8192)); //TODO: load from config
            //TODO: api for storing session and saved and cached data on players
            //TODO: reference worlds which allow per player and global / shared
            //TODO: seperate manager for clients whcih are not just connections
            //TODO: create plugin folders and shit
            return 0;
        }
    }
}