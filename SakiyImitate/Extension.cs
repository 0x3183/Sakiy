using Sakiy.Api;
using Sakiy.Game;
using Sakiy.Game.World;

public class Extension
{
    public Extension()
    {
        Endpoints.Status += (local, remote, status) =>
        {
            status.Description = new ChatComponent.Text("")
            {
                Color = "dark_gray",
                Extra = new ChatComponent[]
                    {
                        new ChatComponent.Text("Press "),
                        new ChatComponent.Keybind("key.jump")
                        {
                            Color = "red",
                        },
                        new ChatComponent.Text(" to jump!"),
                    },
            };
        };
        World.DimensionTypes.Add(new(new("minecraft", "overworld"), 0.0f, "minecraft:overworld", null, true, 256, 0, true));
        World.DimensionTypes.Add(new(new("minecraft", "overworld_caves"), 0.0f, "minecraft:overworld", null, true, 256, 0, true));
        World.DimensionTypes.Add(new(new("minecraft", "the_nether"), 0.0f, "minecraft:overworld", null, true, 256, 0, true));
        World.DimensionTypes.Add(new(new("minecraft", "the_end"), 0.0f, "minecraft:overworld", null, true, 256, 0, true));
        //DimensionType type = new(new("sakiy_types", "hub"), 7, "minecraft:the_end", null, false, 256, 0, false);
        //World.DimensionTypes.Add(type);
        //World.Dimensions.Add(new(new("sakiy", "hub"), World.DimensionTypes.First()));
        World.Dimensions.Add(new(new("minecraft", "overworld"), World.DimensionTypes.First()));
        World.Biomes.Add(new(new("minecraft", "plains"), "rain", 0.125f, 0.800000011920929f, 0.05000000074505806f, 0.4000000059604645f, "plains", null, new(0x78, 0xA7, 0xFF), new(0x05, 0x05, 0x33), new(0xC0, 0xD8, 0xFF), new(0x00, 0x00, 0x00), null, null, null, null, null, null, new(new("minecraft", "ambient.cave"), 6000, 2.0, 8), null));
        //World.Biomes.Add(new(new("sakiy", "space"), "snow", null, 0.0f, null, 1.0f, "none", null, new(255, 0, 0), new(255, 255, 255), new(0, 255, 0), new(0, 255, 255), null, null, null, null, null, null, null, null));
        Player.NewPlayer += (plr) =>
        {
            Logs.Log(plr.Name);
        };
    }
}