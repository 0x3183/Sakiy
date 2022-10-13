using Sakiy.Api;
using Sakiy.Util;

public class Extension
{
    public Extension()
    {
        Listeners.Status += (local, remote, status) =>
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
    }
}