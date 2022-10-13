using Newtonsoft.Json;

namespace Sakiy.Game.Login
{
    public sealed class StatusResponse
    {
        public sealed class ServerVersion
        {
            [JsonProperty("name")]
            public string Name = "1.19.2";
            [JsonProperty("protocol")]
            public int Protocol = 760;
        }
        public sealed class ServerPlayers
        {
            [JsonProperty("max")]
            public int? Maximum = 0;
            [JsonProperty("online")]
            public int? Online = 0; //TODO: connected playstate players
            [JsonProperty("sample")]
            public PlayerSample[]? Samples = null; //TODO: same as above
        }
        public sealed class PlayerSample
        {
            [JsonProperty("name")]
            public readonly string Name;
            [JsonProperty("id")]
            public readonly Guid Id;
            public PlayerSample(string name, Guid id)
            {
                Name = name;
                Id = id;
            }
        }
        [JsonProperty("version")]
        public ServerVersion? Version = new();
        [JsonProperty("players")]
        public ServerPlayers? Players = null;
        [JsonProperty("description")]
        public ChatComponent? Description = null;
        [JsonProperty("favicon")]
        public string? Favicon = null;
        [JsonProperty("previewsChat")]
        public bool? PreventProxies = null;
        [JsonProperty("enforcesSecureChat")]
        public bool? ChatReporting = null;
    }
}
