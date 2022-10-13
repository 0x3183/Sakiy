using Sakiy.Net;
using Sakiy.Util;

namespace Sakiy.Api
{
    public static class Connections
    {
        private static readonly List<Connection> ConnectionsList = new();
        public static event Action<Connection> Added = (d) => { };
        public static event Action<Connection> Removed = (d) => { };
        public static void Add(Connection data)
        {
            Monitor.Enter(ConnectionsList);
            ConnectionsList.Add(data);
            Monitor.Exit(ConnectionsList);
            Thread thread1 = new(data.Read)
            {
                IsBackground = false,
                Name = $"{data.Net?.RemoteEndPoint ?? throw new ObjectDisposedException("Client.Net")} Reader",
            };
            thread1.Start();
            Added(data);
        }
        public static void Remove(Connection data, ChatComponent? reason)
        {
            Monitor.Enter(ConnectionsList);
            ConnectionsList.Remove(data);
            Monitor.Exit(ConnectionsList);
            if(reason != null)
            {
                //TODO: message
            }
            data.Net.Close();
            Removed(data);
        }
        public static IReadOnlyList<Connection> List()
        {
            return ConnectionsList.AsReadOnly();
        }
    }
}
