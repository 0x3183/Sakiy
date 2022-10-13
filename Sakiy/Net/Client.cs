using Newtonsoft.Json;
using Sakiy.Api;
using Sakiy.Util;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;

namespace Sakiy.Net
{
    public sealed class Connection
    {
        public readonly IPEndPoint Listener;

        private readonly Decoder IN;
        private readonly Encoder OUT;
        internal Socket Net;

        public event Action<string, ushort> Handshake = (ServerAddress, ServerPort) => { };
        public event Action<StatusResponse> Status = (Response) => { };
        internal Connection(Socket socket, IPEndPoint listener)
        {
            Net = socket;
            Net.Blocking = true; //true by default already
            IN = new(new NetworkStream(Net));
            OUT = new(new NetworkStream(Net)); //TODO: dispose
            Listener = listener;
        }
        internal void Read()
        {
            try
            {
                Decoder data = new(new MemoryStream(IN.ReadBuffer(IN.ReadVarInt(), false)));
                if (data.ReadVarInt() != 0x00)
                {
                    Connections.Remove(this, null);
                    return;
                }
                if (data.ReadVarInt() != 760)
                {
                    Connections.Remove(this, null);
                    return;
                }
                Handshake(data.ReadString(), data.ReadUShort());
                int next = data.ReadVarInt();
                if (next != 1 && next != 2)
                {
                    Connections.Remove(this, null);
                    return;
                }
                if (next == 1)
                {
                    data.Dispose();
                    data = new(new MemoryStream(IN.ReadBuffer(IN.ReadVarInt(), false)));
                    if(data.ReadVarInt() != 0x00)
                    {
                        Connections.Remove(this, null);
                        return;
                    }
                    MemoryStream ms = new();
                    Encoder encoder = new(ms);
                    StatusResponse response = new();
                    Status(response);
                    encoder.WriteString(JsonConvert.SerializeObject(response, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
                    Send(0x00, ms.ToArray());
                    encoder.Dispose();
                    data.Dispose();
                    data = new(new MemoryStream(IN.ReadBuffer(IN.ReadVarInt(), false)));
                    if (data.ReadVarInt() != 0x01)
                    {
                        Connections.Remove(this, null);
                        return;
                    }
                    Send(0x01, data.ReadBuffer(8, false));
                }
                if (next == 2)
                {
                    data.Dispose();
                    data = new(new MemoryStream(IN.ReadBuffer(IN.ReadVarInt(), false)));
                    if (data.ReadVarInt() != 0x00)
                    {
                        Connections.Remove(this, null);
                        return;
                    }
                    string name = data.ReadString();
                    if(name.Length > 16)
                    {
                        Connections.Remove(this, null);
                        return;
                    }
                    SignatureData? sign = null;
                    Guid guid = Guid.Empty;
                    bool authenticated = false;
                    SaltedSignatureData? salt = null;
                    AuthenticationResponse.Property[]? properties = null;
                    AuthSkin? skin = null;
                    if (data.ReadBool())
                    {
                        sign = new(data.ReadLong(), data.ReadBuffer(data.ReadVarInt(), false), data.ReadBuffer(data.ReadVarInt(), false));
                    }
                    MemoryStream ms;
                    Encoder encoder;
                    if (data.ReadBool())
                    {
                        guid = data.ReadGuid();
                        RSA rsa = RSA.Create();
                        byte[] key = rsa.ExportSubjectPublicKeyInfo();
                        byte[] verify = RandomNumberGenerator.GetBytes(8);
                        ms = new();
                        encoder = new(ms);
                        encoder.WriteString(new(' ', 20));
                        encoder.WriteVarInt(key.Length);
                        encoder.WriteBuffer(key, false);
                        encoder.WriteVarInt(verify.Length);
                        encoder.WriteBuffer(verify, false);
                        Send(0x01, ms.ToArray());
                        encoder.Dispose();
                        data = new(new MemoryStream(IN.ReadBuffer(IN.ReadVarInt(), false)));
                        if (data.ReadVarInt() != 0x01)
                        {
                            Connections.Remove(this, null);
                            return;
                        }
                        byte[] sharedSecret = data.ReadBuffer(data.ReadVarInt(), false);
                        if (data.ReadBool())
                        {
                            if (!rsa.Decrypt(data.ReadBuffer(data.ReadVarInt(), false), RSAEncryptionPadding.Pkcs1).SequenceEqual(verify))
                            {
                                Connections.Remove(this, null);
                                return;
                            }
                        }
                        else
                        {
                            salt = new(data.ReadLong(), data.ReadBuffer(data.ReadVarInt(), false));
                        }
                        byte[] decrypted = rsa.Decrypt(sharedSecret, RSAEncryptionPadding.Pkcs1);
                        OUT.Encrypt(decrypted);
                        IN.Decrypt(decrypted);
                        ms = new MemoryStream();
                        ms.Write(System.Text.Encoding.ASCII.GetBytes(new string(' ', 20)));
                        ms.Write(decrypted, 0, 16);
                        ms.Write(rsa.ExportSubjectPublicKeyInfo());
                        BigInteger b = new BigInteger(SHA1.Create().ComputeHash(ms.ToArray()).Reverse().ToArray());
                        ms.Dispose();
                        Task<HttpResponseMessage> tska = new HttpClient().GetAsync("https://sessionserver.mojang.com/session/minecraft/hasJoined?username=" + name + "&serverId=" + (b < 0 ? "-" + (-b).ToString("x").TrimStart('0') : b.ToString("x").TrimStart('0')));//&ip={(client.BaseSocket.RemoteEndPoint as IPEndPoint).Address}");
                        tska.Wait();
                        if (tska.Result.StatusCode != HttpStatusCode.OK)
                        {
                            Connections.Remove(this, null);
                            return;
                        }
                        authenticated = true;
                        Task<string> tskb = tska.Result.Content.ReadAsStringAsync();
                        AuthenticationResponse? response = JsonConvert.DeserializeObject<AuthenticationResponse>(tskb.Result);
                        if(response == null)
                        {
                            Connections.Remove(this, null);
                            return;
                        }
                        if (!Guid.TryParse(response.ID, out Guid parsed))
                        {
                            Connections.Remove(this, null);
                            return;
                        }
                        if(guid != parsed)
                        {
                            Connections.Remove(this, null);
                            return;
                        } //TODO: just get the skin 
                        properties = response.Properties;
                        skin = JsonConvert.DeserializeObject<AuthSkin>(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(response.Properties[0].Value)));
                    }
                    else
                    {
                        byte[] hash = MD5.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes($"OfflinePlayer:" + name));
                        hash[6] &= 0x0f;
                        hash[6] |= 0x30;
                        hash[8] &= 0x3f;
                        hash[8] |= 0x80;
                        string hex = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
                        guid = Guid.Parse(hex.Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-"));
                    }
                    ms = new();
                    encoder = new(ms);
                    encoder.WriteGuid(guid);
                    encoder.WriteString(name);
                    encoder.WriteVarInt(properties?.Length ?? 0);
                    if(properties != null)
                    {
                        for (int i = 0; i < properties.Length; i++)
                        {
                            encoder.WriteString(properties[i].Name);
                            encoder.WriteString(properties[i].Value);
                            encoder.WriteBool(properties[i].Signature != null);
                            if (properties[i].Signature != null) encoder.WriteString(properties[i].Signature ?? string.Empty);
                        }
                    }
                    Send(0x02, ms.ToArray());
                    encoder.Dispose();
                    //create playstate client
                    while (Net.Connected)
                    {
                        data = new(new MemoryStream(IN.ReadBuffer(IN.ReadVarInt(), false)));
                        int id = data.ReadVarInt();
                        //
                    }
                }
            }
            catch(EndOfStreamException)
            {
                Connections.Remove(this, null);
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
                encoder.Dispose();
                return true;
            }
            catch(IOException)
            {
                Connections.Remove(this, null);
            }
            Monitor.Exit(OUT);
            return false;
        }
    }
}
