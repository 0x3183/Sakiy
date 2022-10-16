using System.Diagnostics;
using System.Text.Json;

internal static class BlockBuilder
{
    private static int Main(string[] args)
    {
        /*Process proc = new()
        {
            StartInfo = new()
            {
                FileName = "java",
                WorkingDirectory = Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "mcdata")).FullName,
                Arguments = "-DbundlerMainClass=net.minecraft.data.Main -jar ../server.jar --report",
                UseShellExecute = false,
            }
        };
        proc.Start();
        proc.WaitForExit();*/
        Directory.Delete("Z:/Code/Sakiy/Sakiy/Game/World/Blocks", true);
        Directory.Delete("Z:/Code/Sakiy/Sakiy/Game/World/BlockEnums", true);
        Directory.CreateDirectory("Z:/Code/Sakiy/Sakiy/Game/World/Blocks");
        Directory.CreateDirectory("Z:/Code/Sakiy/Sakiy/Game/World/BlockEnums");
        Directory.CreateDirectory("code");
        JsonDocument doc = JsonDocument.Parse(new FileStream(Path.Combine(Environment.CurrentDirectory, "mcdata", "generated", "reports", "blocks.json"), FileMode.Open, FileAccess.Read, FileShare.None));
        List<JsonProperty> list = doc.RootElement.EnumerateObject().ToList();
        Dictionary<string, JsonElement[]> propertytypes = new();
        foreach(JsonProperty prop in list)
        {
            char last = '-';
            string dir = string.Concat(prop.Name.Split(':').First().Select((c, i) => { char o = (last == '_' || last == '-') ? c.ToString().ToUpper().First() : c; last = c; return o; })).Replace("_", string.Empty).Replace("-", string.Empty);
            last = '-';
            string file = string.Concat(prop.Name.Split(':').Last().Select((c, i) => { char o = (last == '_' || last == '-') ? c.ToString().ToUpper().First() : c; last = c; return o; })).Replace("_", string.Empty).Replace("-", string.Empty);
            Directory.CreateDirectory(Path.Combine("Z:/Code/Sakiy/Sakiy/Game/World/Blocks", dir));
            string path = Path.Combine("Z:/Code/Sakiy/Sakiy/Game/World/Blocks", dir, file + ".cs");
            File.Create(path).Close();
            List<string> data = new();
            data.Add("using Sakiy.Game.World;");
            data.Add("using Sakiy.Game.World.BlockEnums;");
            data.Add("");
            data.Add("namespace Sakiy.Game.World.Blocks." + dir);
            data.Add("{");
            data.Add("    public sealed class " + file + " : Block");
            data.Add("    {");
            if(prop.Value.TryGetProperty("properties", out JsonElement temp))
            {
                List<JsonProperty> properties = temp.EnumerateObject().ToList();
                foreach (JsonProperty property in properties)
                {
                    bool duplicate = false;
                    last = '_';
                    string tfile = string.Concat(property.Name.Select((c, i) => { char o = (last == '_' || last == '-') ? c.ToString().ToUpper().First() : c; last = c; return o; })).Replace("_", string.Empty).Replace("-", string.Empty) + "Enum";
                    foreach (KeyValuePair<string, JsonElement[]> kvp in propertytypes)
                    {
                        bool equals = true;
                        JsonElement[] cur = property.Value.EnumerateArray().ToArray();
                        JsonElement[] com = kvp.Value;
                        if (cur.Length != com.Length) equals = false;
                        if (equals) for (int i = 0; i < com.Length; i++)
                        {
                            if(cur[i].GetString() != com[i].GetString())
                                {
                                equals = false;
                                break;
                            }
                        }
                        if (equals)
                        {
                            duplicate = true;
                            tfile = kvp.Key;
                            break;
                        }
                    }
                    byte[,] lol = new byte[1, 2]
                    {
                        {0, 1 },
                    };
                    if (!duplicate)
                    {
                        while (propertytypes.ContainsKey(tfile)) tfile += "_";
                        propertytypes.Add(tfile, property.Value.EnumerateArray().ToArray());
                        string tpath = Path.Combine("Z:/Code/Sakiy/Sakiy/Game/World/BlockEnums", tfile + ".cs");
                        File.Create(tpath).Close();
                        List<string> tdata = new();
                        tdata.Add("namespace Sakiy.Game.World.BlockEnums");
                        tdata.Add("{");
                        tdata.Add("    public enum " + tfile + " : int");
                        tdata.Add("    {");
                        JsonElement[] possible = property.Value.EnumerateArray().ToArray();
                        for (int i = 0; i < possible.Length; i++)
                        {
                            string ttname = possible[i].GetString();
                            if ("0123456789".Contains(ttname.First()))
                            {
                                ttname = "value_" + ttname;
                            }
                            if (ttname == "true") ttname = "value_" + ttname;
                            if (ttname == "false") ttname = "value_" + ttname;
                            tdata.Add("        " + ttname + " = " + i.ToString() + ",");
                        }
                        tdata.Add("    }");
                        tdata.Add("}");
                        File.WriteAllLines(tpath, tdata.ToArray());
                    }
                    data.Add("        public " + tfile + " " + property.Name + ";");
                }
            }
            data.Add("        public override int GetHashCode()");
            data.Add("        {");
            data.Add("            return table[" + lookup + "0];");
            data.Add("        }");
            data.Add("");
            data.Add("        private static int[" + table.Join(',').Where(c=>c==',').Join() + "] table = new;");
            data.Add("    }");
            data.Add("}");
            File.WriteAllLines(path, data.ToArray());
        }
        return 0;
    }
}
