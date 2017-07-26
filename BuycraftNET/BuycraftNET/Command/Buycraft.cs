using MiNET;
using MiNET.Plugins.Attributes;
using MiNET.Utils;

namespace BuycraftNET.Command
{
    public class Buycraft
    {
        private BuycraftNET Plugin;

        public Buycraft(BuycraftNET plugin)
        {
            Plugin = plugin;
        }

        [Command(Name = "buycraft", Description = "Buycraft Management", Permission = "op")]
        public void Execute(Player sender, string command, string param)
        {
            sender.SendMessage(ChatColors.Green + "You asked for " + command + " " + param );
        }
    }
}
