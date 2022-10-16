using Sakiy.Game.Login;
using Sakiy.Type;
using Sakiy.Util;

namespace Sakiy.Game.World
{
    public sealed class Player : Entity
    {
        public static event Action<Player> NewPlayer = (plr) => { };

        public readonly Decoder IN;
        public readonly Encoder OUT;
        public readonly string ConnectionAddress;
        public readonly ushort ConnectionPort;
        public readonly string Name;
        public readonly Guid UUID;
        public readonly bool Authenticated;
        public readonly SaltedSignatureData? Salt = null;
        public readonly AuthenticationResponse.Property[]? Properties = null;
        public readonly AuthSkin? Skin = null;

        public Gamemode Gamemode;
        public Gamemode PreviousGamemode;
        public Identifier? DeathDimension;
        public Position? DeathLocation;

        public Player(int eid, Decoder netin, Encoder netout, string connectionAddress, ushort connectionPort, string name, Guid uuid, bool authenticated, SaltedSignatureData? salt, AuthenticationResponse.Property[]? properties, AuthSkin? skin, Gamemode gamemode) : base(eid)
        {
            IN = netin;
            OUT = netout;
            ConnectionAddress = connectionAddress;
            ConnectionPort = connectionPort;
            Name = name;
            UUID = uuid;
            Authenticated = authenticated;
            Salt = salt;
            Properties = properties;
            Skin = skin;
            Gamemode = gamemode;
            PreviousGamemode = gamemode;
            NewPlayer(this);
        }
    }
}
