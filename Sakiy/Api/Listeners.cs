using System.Net;
using System.Net.Sockets;

namespace Sakiy.Api
{
    public static class Listeners
    {
        private static readonly Dictionary<IPEndPoint, Socket> Sockets = new();
        public static void Add(IPEndPoint endpoint)
        {
            Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endpoint);
            socket.Listen();
            Thread thread0 = new(Listen)
            {
                IsBackground = false,
                Name = $"{endpoint} Listener",
            };
            thread0.Start(endpoint);
            Monitor.Enter(Sockets);
            Sockets.Add(endpoint, socket);
            Monitor.Exit(Sockets);
        }
        private static void Listen(object? obj)
        {
            IPEndPoint endpoint = obj as IPEndPoint ?? throw new InvalidCastException();
            Monitor.Enter(Sockets);
            Socket socket = Sockets.Where(kvp => kvp.Key == endpoint).First().Value;
            Monitor.Exit(Sockets);
            try
            {
                while (true)
                {
                    Socket client = socket.Accept();
                    Monitor.Enter(Sockets);
                    Connections.Add(new(client, endpoint));
                    Monitor.Exit(Sockets);
                }
            }
            catch
            {

            }
        }
        public static void Remove(IPEndPoint endpoint)
        {
            Monitor.Enter(Sockets);
            if (!Sockets.ContainsKey(endpoint))
            {
                Monitor.Exit(Sockets);
                return;
            }
            Sockets.Where(kvp => kvp.Key == endpoint).First().Value.Close();
            Sockets.Remove(endpoint);
            Monitor.Exit(Sockets);
        }
    }
}
