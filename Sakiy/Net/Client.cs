using Newtonsoft.Json;
using Sakiy.Api;
using Sakiy.Util;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace Sakiy.Net
{
    public sealed class Client
    {
        public readonly IPEndPoint Listener;

        private readonly Decoder IN;
        private readonly Encoder OUT;
        private ProtocolState Mode;
        internal Socket Net;

        public event Action<string, ushort> Handshake = (ServerAddress, ServerPort) => { };
        public event Action<StatusResponse> Status = (Response) => { };
        internal Client(Socket socket, IPEndPoint listener)
        {
            Mode = 0;
            Net = socket;
            Net.Blocking = true; //true by default already
            IN = new(new NetworkStream(Net));
            OUT = new(new NetworkStream(Net));
            Listener = listener;
        }
        internal void Read()
        {
            try
            {
                while(Net.Connected)
                {
                    Decoder data = new(new MemoryStream(IN.ReadBuffer(IN.ReadVarInt(), false)));
                    int id = data.ReadVarInt();
                    switch (Mode)
                    {
                        case ProtocolState.Handshake:
                            {
                                if (id == 0x00)
                                {
                                    int version = data.ReadVarInt();
                                    if (version != 760)
                                    {
                                        //TODO: disconnect message
                                        Clients.Remove(this);
                                        return;
                                    }
                                    Handshake(data.ReadString(), data.ReadUShort());
                                    int next = data.ReadVarInt();
                                    if (next != 1 && next != 2)
                                    {
                                        Clients.Remove(this);
                                        return;
                                    }
                                    if (next == 1) Mode = ProtocolState.Status;
                                    if (next == 2) Mode = ProtocolState.Login;
                                    break;
                                }
                                Clients.Remove(this);
                                return;
                            }
                        case ProtocolState.Login:
                            {
                                MemoryStream ms = new();
                                Encoder encoder = new(ms);
                                //encoder.WriteString(new Chat.ChatComponent.ChatText("lol") { Extra = new Chat.ChatComponent[] { new Chat.ChatComponent.ChatTranslation("chat.type.text") { Color = "red" } } }.ToString());
                                //Send(0x00, ms.ToArray());
                                break;
                            }
                        case ProtocolState.Status:
                            {
                                if (id == 0x00)
                                {
                                    MemoryStream ms = new();
                                    Encoder encoder = new(ms);
                                    StatusResponse response = new();
                                    Status(response);
                                    encoder.WriteString(JsonConvert.SerializeObject(response, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
                                    Send(0x00, ms.ToArray());
                                    break;
                                }
                                if (id == 0x01)
                                {
                                    Send(0x01, data.ReadBuffer(8, false));
                                    break;
                                }
                                Clients.Remove(this);
                                return;
                            }
                        default:
                            {
                                Clients.Remove(this);
                                return;
                            }
                    }
                }
            }
            catch(EndOfStreamException)
            {
                Clients.Remove(this);
            }
        }
        public bool Send(int id, byte[] data)
        {
            try
            {
                if (Net == null) return false;
                MemoryStream ms = new();
                Encoder encoder = new(ms);
                encoder.WriteVarInt(id);
                Monitor.Enter(OUT);
                OUT.WriteVarInt((int)ms.Length + data.Length);
                OUT.WriteVarInt(id);
                OUT.WriteBuffer(data, false);
                Monitor.Exit(OUT);
                return true;
            }
            catch(IOException)
            {
                Clients.Remove(this);
            }
            Monitor.Exit(OUT);
            return false;
        }
    }
}
