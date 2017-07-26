using MiNET;
using MiNET.Plugins.Attributes;
using MiNET.Utils;

namespace BuycraftNET.Command
{
    public class Buy
    {
        private BuycraftNET Plugin;

        public Buy(BuycraftNET plugin)
        {
            Plugin = plugin;
        }

        [Command(Name = "buy", Description = "Buycraft in game purchase")]
        public void Execute(Player sender, string command, string param)
        {
            sender.SendMessage(ChatColors.Green + "You asked for " + command);
        }
    }
}
