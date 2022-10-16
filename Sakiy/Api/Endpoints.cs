using Newtonsoft.Json;
using Sakiy.Game;
using Sakiy.Game.Login;
using Sakiy.Game.NBT;
using Sakiy.Game.World;
using Sakiy.Util;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography;

namespace Sakiy.Api
{
    public static class Endpoints
    {
        private static readonly Dictionary<IPEndPoint, Socket> Sockets = new();

        public static event Action<IPEndPoint, IPEndPoint, StatusResponse> Status = (LocalEndpoint, RemoteEndpoint, Response) => { };
        public static void Add(IPEndPoint endpoint)
        {
            Thread thread = new(() =>
            {
                using Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket.Bind(endpoint);
                    socket.Listen();
                    Monitor.Enter(Sockets);
                    Sockets.Add(endpoint, socket);
                    Monitor.Exit(Sockets);
                    while (true)
                    {
                        Socket client = socket.Accept();
                        IPEndPoint remote = client.RemoteEndPoint as IPEndPoint ?? new IPEndPoint(IPAddress.Any, IPEndPoint.MinPort);
                        Thread thread1 = new(() =>
                        {
                            try
                            {
                                using Decoder IN = new(new NetworkStream(client));
                                using Encoder OUT = new(new NetworkStream(client));
                                string address;
                                ushort port;
                                int next;
                                using (Decoder handshake = new(new MemoryStream(IN.ReadBuffer(IN.ReadVarInt(), false))))
                                {
                                    if (handshake.ReadVarInt() != 0x00)
                                    {
                                        client.Close();
                                        return;
                                    }
                                    if (handshake.ReadVarInt() != 760)
                                    {
                                        client.Close();
                                        return;
                                    }
                                    address = handshake.ReadString();
                                    port = handshake.ReadUShort();
                                    next = handshake.ReadVarInt();
                                }
                                if (next == 1)
                                {
                                    using (Decoder request = new(new MemoryStream(IN.ReadBuffer(IN.ReadVarInt(), false))))
                                    {
                                        if (request.ReadVarInt() != 0x00)
                                        {
                                            client.Close();
                                            return;
                                        }
                                        using MemoryStream ms = new();
                                        using Encoder response = new(ms);
                                        StatusResponse data = new();
                                        Status(endpoint, remote, data);
                                        response.WriteVarInt(0x00);
                                        response.WriteString(JsonConvert.SerializeObject(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
                                        byte[] buffer = ms.ToArray();
                                        OUT.WriteVarInt(buffer.Length);
                                        OUT.WriteBuffer(buffer, false);
                                    }
                                    using (Decoder ping = new(new MemoryStream(IN.ReadBuffer(IN.ReadVarInt(), false))))
                                    {
                                        if (ping.ReadVarInt() != 0x01)
                                        {
                                            client.Close(); //TODO: just return or break?
                                            return;
                                        }
                                        using MemoryStream ms = new();
                                        using Encoder pong = new(ms);
                                        pong.WriteVarInt(0x01);
                                        pong.WriteBuffer(ping.ReadBuffer(8, false), false);
                                        byte[] buffer = ms.ToArray();
                                        OUT.WriteVarInt(buffer.Length);
                                        OUT.WriteBuffer(buffer, false);
                                    }
                                }
                                if (next == 2)
                                {
                                    string name;
                                    Guid guid = Guid.Empty;
                                    SignatureData? sign = null;
                                    SaltedSignatureData? salt = null;
                                    bool authenticated = false;
                                    AuthenticationResponse.Property[]? properties = null;
                                    AuthSkin? skin = null;
                                    bool tryauth = false;
                                    using (Decoder start = new(new MemoryStream(IN.ReadBuffer(IN.ReadVarInt(), false))))
                                    {
                                        if (start.ReadVarInt() != 0x00)
                                        {
                                            client.Close();
                                            return;
                                        }
                                        name = start.ReadString();
                                        if (name.Length > 16)
                                        {
                                            client.Close();
                                            return;
                                        }
                                        if (start.ReadBool())
                                        {
                                            sign = new(start.ReadLong(), start.ReadBuffer(start.ReadVarInt(), false), start.ReadBuffer(start.ReadVarInt(), false));
                                        }
                                        if (start.ReadBool())
                                        {
                                            guid = start.ReadGuid();
                                            tryauth = true;
                                        }
                                    }
                                    if (tryauth)
                                    {
                                        using RSA rsa = RSA.Create();
                                        byte[] key = rsa.ExportSubjectPublicKeyInfo();
                                        byte[] verify = RandomNumberGenerator.GetBytes(8);
                                        using (MemoryStream ms = new())
                                        {
                                            using Encoder request = new(ms);
                                            request.WriteVarInt(0x01);
                                            request.WriteString(new(' ', 20));
                                            request.WriteVarInt(key.Length);
                                            request.WriteBuffer(key, false);
                                            request.WriteVarInt(verify.Length);
                                            request.WriteBuffer(verify, false);
                                            byte[] buffer = ms.ToArray();
                                            OUT.WriteVarInt(buffer.Length);
                                            OUT.WriteBuffer(buffer, false);
                                        }
                                        byte[] decrypted;
                                        using (Decoder response = new(new MemoryStream(IN.ReadBuffer(IN.ReadVarInt(), false))))
                                        {
                                            if (response.ReadVarInt() != 0x01)
                                            {
                                                client.Close();
                                                return;
                                            }
                                            decrypted = rsa.Decrypt(response.ReadBuffer(response.ReadVarInt(), false), RSAEncryptionPadding.Pkcs1);
                                            OUT.Encrypt(decrypted);
                                            IN.Decrypt(decrypted);
                                            if (response.ReadBool())
                                            {
                                                if (!rsa.Decrypt(response.ReadBuffer(response.ReadVarInt(), false), RSAEncryptionPadding.Pkcs1).SequenceEqual(verify))
                                                {
                                                    client.Close();
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                salt = new(response.ReadLong(), response.ReadBuffer(response.ReadVarInt(), false));
                                            }
                                        }
                                        BigInteger num;
                                        using (MemoryStream hashraw = new())
                                        {
                                            hashraw.Write(System.Text.Encoding.ASCII.GetBytes(new string(' ', 20)));
                                            hashraw.Write(decrypted, 0, 16);
                                            hashraw.Write(rsa.ExportSubjectPublicKeyInfo());
                                            num = new BigInteger(SHA1.Create().ComputeHash(hashraw.ToArray()).Reverse().ToArray());
                                        }
                                        Task<HttpResponseMessage> http = new HttpClient().GetAsync("https://sessionserver.mojang.com/session/minecraft/hasJoined?username=" + name + "&serverId=" + (num < 0 ? "-" + (-num).ToString("x").TrimStart('0') : num.ToString("x").TrimStart('0')));//&ip={(client.BaseSocket.RemoteEndPoint as IPEndPoint).Address}");
                                        if (http.Result.StatusCode != HttpStatusCode.OK)
                                        {
                                            client.Close();
                                            return;
                                        }
                                        authenticated = true;
                                        AuthenticationResponse? auth = JsonConvert.DeserializeObject<AuthenticationResponse>(http.Result.Content.ReadAsStringAsync().Result);
                                        if (auth == null)
                                        {
                                            client.Close();
                                            return;
                                        }
                                        if (!Guid.TryParse(auth.ID, out Guid parsed))
                                        {
                                            client.Close();
                                            return;
                                        }
                                        if (guid != parsed)
                                        {
                                            client.Close();
                                            return;
                                        }
                                        properties = auth.Properties;
                                        skin = JsonConvert.DeserializeObject<AuthSkin>(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(auth.Properties[0].Value)));
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
                                    using (MemoryStream ms = new())
                                    {
                                        using Encoder success = new(ms);
                                        success.WriteVarInt(0x02);
                                        success.WriteGuid(guid);
                                        success.WriteString(name);
                                        success.WriteVarInt(properties?.Length ?? 0);
                                        if (properties != null)
                                        {
                                            for (int i = 0; i < properties.Length; i++)
                                            {
                                                success.WriteString(properties[i].Name);
                                                success.WriteString(properties[i].Value);
                                                success.WriteBool(properties[i].Signature != null);
                                                if (properties[i].Signature != null) success.WriteString(properties[i].Signature ?? string.Empty);
                                            }
                                        }
                                        byte[] buffer = ms.ToArray();
                                        OUT.WriteVarInt(buffer.Length);
                                        OUT.WriteBuffer(buffer, false);
                                    }
                                    Player player = new(Random.Shared.Next(), IN, OUT, address, port, name, guid, authenticated, salt, properties, skin, Gamemode.Creative);
                                    using (MemoryStream ms = new())
                                    {
                                        using Encoder initialize = new(ms);
                                        initialize.WriteVarInt(0x25);
                                        initialize.WriteInt(player.EID);
                                        initialize.WriteBool(World.Hardcore);
                                        initialize.WriteByte((byte)player.Gamemode);
                                        initialize.WriteByte((byte)player.PreviousGamemode);
                                        {
                                            Dimension[] dimensions = World.Dimensions.ToArray();
                                            initialize.WriteVarInt(dimensions.Length);
                                            for (int i = 0; i < dimensions.Length; i++) initialize.WriteString(dimensions[i].ID.ToString());
                                        }
                                        initialize.WriteUShort(0);
                                        {
                                            NbtCompound codec = new();
                                            {
                                                NbtCompound nbt = new();
                                                NbtList<NbtCompound> list = new();
                                                DimensionType[] dimensions = World.DimensionTypes.ToArray();
                                                for (int i = 0; i < dimensions.Length; i++)
                                                {
                                                    list.Value.Add(new(new Dictionary<string, NbtTag>()
                                                    {
                                                        { "element", dimensions[i].Nbt},
                                                        { "name", new NbtString(dimensions[i].ID.ToString())},
                                                        { "id", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(i) } })}
                                                    }));
                                                }
                                                nbt.Set("value", list);
                                                nbt.Set("type", new NbtString("minecraft:dimension_type"));
                                                codec.Set("minecraft:dimension_type", nbt);
                                            }
                                            {
                                                NbtCompound nbt = new();
                                                NbtList<NbtCompound> list = new();
                                                BiomeProperties[] biomes = World.Biomes.ToArray();
                                                for (int i = 0; i < biomes.Length; i++)
                                                {
                                                    list.Value.Add(new(new Dictionary<string, NbtTag>()
                                                    {
                                                        { "element", biomes[i].Nbt},
                                                        { "name", new NbtString(biomes[i].ID.ToString())},
                                                        { "id", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(i) } })}
                                                    }));
                                                }
                                                nbt.Set("value", list);
                                                nbt.Set("type", new NbtString("minecraft:worldgen/biome"));
                                                codec.Set("minecraft:worldgen/biome", nbt);
                                            }
                                            {
                                                NbtCompound nbt = new();
                                                nbt.Set("type", new NbtString("minecraft:chat_type"));
                                                {
                                                    NbtList<NbtCompound> list = new();
                                                    {
                                                        NbtCompound con = new();
                                                        con.Set("name", new NbtString("minecraft:chat"));
                                                        {
                                                            NbtCompound id = new();
                                                            id.Set("value", new NbtInt(0));
                                                            con.Set("id", id);
                                                        }
                                                        {
                                                            NbtCompound element = new();
                                                            {
                                                                NbtCompound cur = new();
                                                                {
                                                                    NbtCompound decoration = new();
                                                                    {
                                                                        decoration.Set("parameters", new NbtList<NbtString>(new List<NbtString>() { new NbtString("sender"), new NbtString("content") }));
                                                                        decoration.Set("translation_key", new NbtString("chat.type.text"));
                                                                    }
                                                                    cur.Set("decoration", decoration);
                                                                }
                                                                element.Set("chat", cur);
                                                            }
                                                            {
                                                                NbtCompound cur = new();
                                                                {
                                                                    NbtCompound decoration = new();
                                                                    {
                                                                        decoration.Set("parameters", new NbtList<NbtString>(new List<NbtString>() { new NbtString("sender"), new NbtString("content") }));
                                                                        decoration.Set("translation_key", new NbtString("chat.type.text.narrate"));
                                                                    }
                                                                    cur.Set("decoration", decoration);
                                                                }
                                                                cur.Set("priority", new NbtString("chat"));
                                                                element.Set("narration", cur);
                                                            }
                                                            con.Set("element", element);
                                                        }
                                                        list.Value.Add(con);
                                                    }
                                                    nbt.Set("value", list);
                                                }
                                                codec.Set("minecraft:chat_type", nbt);
                                            }
                                            codec.Serialize(initialize);
                                        }
                                        initialize.WriteString(World.Dimensions.First().Type.ID.ToString());
                                        initialize.WriteString(World.Dimensions.First().ID.ToString());
                                        initialize.WriteLong(0x0123456789ABCDEF); //TODO: hashed seed
                                        initialize.WriteVarInt(0);
                                        initialize.WriteVarInt(World.ViewDistance);
                                        initialize.WriteVarInt(World.SimulationDistance);
                                        initialize.WriteBool(World.ReducedDebugInfo);
                                        initialize.WriteBool(World.EnableRespawnScreen);
                                        initialize.WriteBool(false);
                                        initialize.WriteBool(World.Flat);
                                        {
                                            bool hasdeathlocation = player.DeathDimension.HasValue && player.DeathLocation.HasValue;
                                            initialize.WriteBool(hasdeathlocation);
                                            if (hasdeathlocation)
                                            {
                                                initialize.WriteString(player.DeathDimension.GetValueOrDefault().ToString());
                                                initialize.WriteULong(player.DeathLocation.GetValueOrDefault().Data);
                                            }
                                        }
                                        byte[] buffer = ms.ToArray();
                                        OUT.WriteVarInt(buffer.Length);
                                        OUT.WriteBuffer(buffer, false);
                                    }
                                    //DEBUG//
                                    using (MemoryStream ms = new())
                                    {
                                        Encoder disconnect = new(ms);
                                        disconnect.WriteVarInt(0x19);
                                        disconnect.WriteString(new ChatComponent.Text("debug").ToString());
                                        byte[] buffer = ms.ToArray();
                                        OUT.WriteVarInt(buffer.Length);
                                        OUT.WriteBuffer(buffer, false);
                                    }
                                    return;
                                    using (MemoryStream ms = new())
                                    {
                                        Encoder finish = new(ms);
                                        finish.WriteVarInt(0x21);
                                        finish.WriteInt(0);
                                        finish.WriteInt(0);
                                        finish.WriteUShort(0);
                                        {
                                            NbtCompound heightmaps = new();
                                            {
                                                NbtLongArray motionblocking = new(new long[32]);
                                                heightmaps.Set("MOTION_BLOCKING", motionblocking);
                                            }
                                            heightmaps.Serialize(finish);
                                        }
                                        finish.WriteByte(0);
                                        using (MemoryStream msb = new())
                                        {
                                            Encoder data = new(msb);
                                            for(int s = 0; s < 16; s++)
                                            {
                                                data.WriteShort(16); //TODO: block count
                                                data.WriteByte(8);
                                                data.WriteVarInt(256);
                                                for (int i = 0; i < 256; i++) data.WriteVarInt(i);
                                                data.WriteVarInt(512);
                                                for (int i = 0; i < 4096; i++)
                                                {
                                                    if (i % 16 == 0)
                                                    {
                                                        data.WriteByte(1);
                                                    }
                                                    else
                                                    {
                                                        data.WriteByte(0);
                                                    }
                                                }
                                                data.WriteByte(0);
                                                data.WriteVarInt(0);
                                                data.WriteVarInt(0);
                                            }
                                            byte[] databuf = ms.ToArray();
                                            finish.WriteVarInt(databuf.Length);
                                            finish.WriteBuffer(databuf, false);
                                        }
                                        finish.WriteVarInt(0);
                                        finish.WriteBool(false);
                                        finish.WriteVarInt(1);
                                        finish.WriteULong(0);
                                        finish.WriteVarInt(1);
                                        finish.WriteULong(0);
                                        finish.WriteVarInt(1);
                                        finish.WriteULong(255);
                                        finish.WriteVarInt(1);
                                        finish.WriteULong(255);
                                        finish.WriteVarInt(0);
                                        finish.WriteVarInt(0);
                                        byte[] buffer = ms.ToArray();
                                        OUT.WriteVarInt(buffer.Length);
                                        OUT.WriteBuffer(buffer, false);
                                    }
                                    /*using (MemoryStream ms = new())
                                    {
                                        Encoder finish = new(ms);
                                        finish.WriteVarInt(0x39);
                                        finish.WriteDouble(0.0);
                                        finish.WriteDouble(64.0);
                                        finish.WriteDouble(0.0);
                                        finish.WriteFloat(0.0f);
                                        finish.WriteFloat(0.0f);
                                        finish.WriteByte(0);
                                        finish.WriteVarInt(0x01234567);
                                        finish.WriteBool(true);
                                        byte[] buffer = ms.ToArray();
                                        OUT.WriteVarInt(buffer.Length);
                                        OUT.WriteBuffer(buffer, false);
                                    }*/
                                    //DEBUG//
                                    while (client.Connected)
                                    {
                                        using (Decoder packet = new(new MemoryStream(IN.ReadBuffer(IN.ReadVarInt(), false))))
                                        {
                                            int id = packet.ReadVarInt();
                                            //
                                        }
                                    }
                                }
                                client.Close();
                                return;
                            }
                            catch(Exception ex)
                            {
                                client.Close();
                            }
                        })
                        {
                            IsBackground = false,
                            Name = $"{client.RemoteEndPoint ?? new IPEndPoint(IPAddress.Any, IPEndPoint.MinPort)} Reader",
                        };
                        thread1.Start();
                    }
                }
                catch
                {
                    socket.Close();
                }
            })
            {
                IsBackground = false,
                Name = $"{endpoint} Listener",
            };
            thread.Start();
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
