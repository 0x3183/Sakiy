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
        DimensionType type = new(new("sakiy_types", "hub"), 7, "minecraft:the_end", null, false, 256, 0, false);
        World.DimensionTypes.Add(type);
        Logs.Log(World.DimensionTypes.Count);
        World.Dimensions.Add(new(new("sakiy", "hub"), type));
        World.Biomes.Add(new(new("sakiy", "space"), "snow", null, 0.0f, null, 1.0f, "none", null, new(255, 0, 0), new(255, 255, 255), new(0, 255, 0), new(0, 255, 255), null, null, null, null, null, null, null, null));
        Player.NewPlayer += (plr) =>
        {
            Logs.Log(plr.Name);
        };
    }
}