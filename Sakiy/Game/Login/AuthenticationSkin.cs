using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakiy.Game.Login
{
    public sealed class AuthSkin
    {
        [JsonProperty("timestamp")]
        public ulong Timestamp = 0UL;
        [JsonProperty("profileId")]
        public string ProfileId = string.Empty;
        [JsonProperty("profileName")]
        public string ProfileName = string.Empty;
        [JsonProperty("signatureRequired")]
        public bool SignatureRequired = false;
        public Dictionary<string, SkinTexture> Textures = new();
        public struct SkinTexture
        {
            [JsonProperty("url")]
            public string URL { get; set; }
        }
    }
}
