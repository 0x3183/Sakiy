using Newtonsoft.Json;
using Sakiy.Type;

namespace Sakiy.Game
{
    public abstract class ChatComponent
    {
        public abstract class ClickEvent
        {
            public sealed class URL : ClickEvent
            {
                [JsonProperty("value")]
                public string Value;
                public URL(string url) : base("open_url")
                {
                    Value = url;
                }
            }
            public sealed class Command : ClickEvent
            {
                [JsonProperty("value")]
                public string Value;
                public Command(string command) : base("run_command")
                {
                    Value = command;
                }
            }
            public sealed class Suggestion : ClickEvent
            {
                [JsonProperty("value")]
                public string Value;
                public Suggestion(string command) : base("suggest_command")
                {
                    Value = command;
                }
            }
            public sealed class Page : ClickEvent
            {
                [JsonProperty("value")]
                public int Value;
                public Page(int page) : base("change_page")
                {
                    Value = page;
                }
            }
            public sealed class Copy : ClickEvent
            {
                [JsonProperty("value")]
                public string Value;
                public Copy(string text) : base("copy_to_clipboard")
                {
                    Value = text;
                }
            }
            [JsonProperty("action")]
            public readonly string Action;
            private ClickEvent(string action)
            {
                Action = action;
            }
        }
        public abstract class HoverEvent
        {
            public sealed class Text : HoverEvent
            {
                [JsonProperty("value")]
                public string Value;
                public Text(string text) : base("show_text")
                {
                    Value = text;
                }
            }
            //TODO: show_item
            //TODO: shpw_entity
            [JsonProperty("action")]
            public readonly string Action;
            private HoverEvent(string action)
            {
                Action = action;
            }
        }
        public sealed class Text : ChatComponent
        {
            [JsonProperty("text")]
            public string Value;
            public Text(string text = "")
            {
                Value = text;
            }
        }
        public sealed class Translation : ChatComponent
        {
            [JsonProperty("translate")]
            public string Value;
            [JsonProperty("with")]
            public ChatComponent[]? With = null;
            public Translation(string translate = "")
            {
                Value = translate;
            }
        }
        public sealed class Keybind : ChatComponent
        {
            [JsonProperty("keybind")]
            public string Value;
            public Keybind(string keybind = "")
            {
                Value = keybind;
            }
        }
        public sealed class Score : ChatComponent
        {
            public sealed class Scores
            {
                [JsonProperty("name")]
                public string Name;
                [JsonProperty("objective")]
                public string Objective;
                [JsonProperty("value")]
                public string Value;
                public Scores(string name, string objective, string value)
                {
                    Name = name;
                    Objective = objective;
                    Value = value;
                }
            }
            [JsonProperty("score")]
            public Scores Value;
            public Score(Scores score)
            {
                Value = score;
            }
        }
        [JsonProperty("color")]
        public string? Color = null;
        [JsonProperty("bold")]
        public bool? Bold = null;
        [JsonProperty("italic")]
        public bool? Italic = null;
        [JsonProperty("underlined")]
        public bool? Underlined = null;
        [JsonProperty("obfuscated")]
        public bool? Obfuscated = null;
        [JsonProperty("font")]
        public Identifier? Font = null; //minecraft:uniform, minecraft:alt, minecraft:default
        [JsonProperty("insertion")]
        public string? Insertion = null;
        [JsonProperty("clickEvent")]
        public ClickEvent? Click = null;
        [JsonProperty("hoverEvent")]
        public HoverEvent? Hover = null;
        [JsonProperty("extra")]
        public ChatComponent[]? Extra = null;
        private ChatComponent()
        {

        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
