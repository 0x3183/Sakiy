using Sakiy.Net;

namespace Sakiy.Api
{
    public static class Clients
    {
        private static readonly List<Client> ClientList = new();
        public static event Action<Client> Added = (d) => { };
        public static event Action<Client> Removed = (d) => { };
        public static void Add(Client data)
        {
            Monitor.Enter(ClientList);
            ClientList.Add(data);
            Monitor.Exit(ClientList);
            Thread thread1 = new(data.Read)
            {
                IsBackground = false,
                Name = $"{data.Net?.RemoteEndPoint ?? throw new ObjectDisposedException("Client.Net")} Reader",
            };
            thread1.Start();
            Added(data);
        }
        public static void Remove(Client data)
        {
            Monitor.Enter(ClientList);
            ClientList.Remove(data);
            Monitor.Exit(ClientList);
            data.Net.Close();
            Removed(data);
        }
        public static IReadOnlyList<Client> List()
        {
            return ClientList.AsReadOnly();
        }
    }
}
