using MiNET;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using System;
using MiNET.Plugins;

namespace BuycraftNET.Command
{
    public class Buycraft
    {
        private BuycraftNET Plugin;

        public Buycraft(BuycraftNET plugin)
        {
            Plugin = plugin;
        }
        
        [Command(Name = "buycraft secret", Permission = "op")]
        public void Secret(Player sender, string secret)
        {
            Plugin.LogInfo("Checking Key");
            Plugin.SetConfig("secret", secret);
            var storeInfo = Plugin.GetApiClient().GetInformation();

            if (storeInfo.Result == null)
            {
                sender.SendMessage(ChatColors.Red + "This is not a valid secret key");
                Plugin.SetConfig("secret", "");
            }
            else
            {
                sender.SendMessage(ChatColors.Green + "Secret key has been added!");
                
            }
        }

        [Command(Name = "buycraft refresh", Permission = "op")]
        public void Refresh(Player sender)
        {
            NotImplemented(sender);
            //TODO
        }
        
        [Command(Name = "buycraft forcecheck", Permission = "op")]
        public void Forcecheck(Player sender)
        {
            NotImplemented(sender);
            //TODO
        }        
        
        [Command(Name = "buycraft report", Permission = "op")]
        public void Report(Player sender)
        {
            NotImplemented(sender);
            //TODO
        }        
        
        [Command(Name = "buycraft signupdate", Permission = "op")]
        public void Signupdate(Player sender)
        {
            NotImplemented(sender);
            //TODO
        }        
        
        [Command(Name = "buycraft coupon list", Permission = "op")]
        public void Coupon(Player sender)
        {
            NotImplemented(sender);
            //TODO
        }         
        
        [Command(Name = "buy")]
        public void Refresh(Player sender)
        {
            NotImplemented(sender);
            //TODO
        }        

        private void NotImplemented(Player sender)
        {
            sender.SendMessage(ChatColors.Red + "This command has not been implemented yet");
        }
    }
}
