using Newtonsoft.Json;

namespace Sakiy.Util
{
    public sealed class AuthenticationResponse
    {
        [JsonProperty("id")]
        public string ID = string.Empty;
        [JsonProperty("name")]
        public string Name = string.Empty;
        [JsonProperty("properties")]
        public Property[] Properties = Array.Empty<Property>();
        public sealed class Property
        {
            [JsonProperty("name")]
            public string Name = string.Empty;
            [JsonProperty("value")]
            public string Value = string.Empty;
            [JsonProperty("signature")]
            public string? Signature = null;
        }
    }
}
