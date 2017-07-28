using MiNET;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using System;

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
        public void Execute(Player sender, string command, string param=null)
        {
            Console.WriteLine(sender.PermissionLevel.ToString());
            sender.SendMessage(ChatColors.Green + "You asked for " + command + " " + param );
            switch (command)
            {
                case "secret":
                    Secret(sender, param);
                    break;       
                case "test":
                    Test(sender);
                    break;
            }
        }

        private void Secret(Player sender, string param)
        {
            Plugin.LogInfo("Checking Key");
            Plugin.SetConfig("secret", param);
            var storeInfo = Plugin.GetApiClient().GetInformation();

            if (storeInfo.Result == null)
            {
                sender.SendMessage(ChatColors.Red + "This is not a valid secret key");                
            }
            else
            {
                sender.SendMessage(ChatColors.Green + "Secret key has been added!");
                
            }
        }

        private void Test(Player sender)
        {
            Executor executor = new Executor(Plugin);
            executor.ExecuteCommand("xp 1000 Steve");
        }
        
    }
}
