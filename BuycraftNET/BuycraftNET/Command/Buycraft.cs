using MiNET;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using System;
using System.Runtime.Remoting.Contexts;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Worlds;
using Newtonsoft.Json;

using System.Reflection;

using System.Linq;

using Newtonsoft.Json.Linq;

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

            string command = "xp 100 Steve";
            
            var pm = Plugin.GetPluginManager();
            var commands = pm.Commands;
            
            foreach (var pmCommand in commands)
            {
                if (command.StartsWith(pmCommand.Key))
                {
                    command = command.Replace(pmCommand.Key, "").Trim();
                    var commandParams = command.Split(null);
                    
                    Console.WriteLine(commandParams.Length);
                    
                    Overload overload = pmCommand.Value.Versions.First().Overloads["default"];
                    
                    MethodInfo method = overload.Method;


                    Console.WriteLine("method:" + method.ToString());
                    Console.WriteLine("sender:" + sender.ToString());
                    Console.WriteLine("CommandParmas::" + commandParams.ToString());
                    
                    Console.WriteLine("Invoke:");
                    pm.GetType().GetTypeInfo().GetDeclaredMethod("ExecuteCommand").Invoke(pm, new object[] {method, sender, commandParams} );
                    //execCommand.Invoke(pm, );
                    /*
                    dynamic commandInputJson = JsonConvert.DeserializeObject("{}");

                    int x = 0;
                    foreach (ParameterInfo parameter in method.GetParameters())
                    {
                        if (typeof (Player).IsAssignableFrom(parameter.ParameterType)) continue;

                        if (commandParams.Length > x)
                        {
                            string argName = char.ToUpper(parameter.Name[0]) + parameter.Name.Substring(1);
                            
                            commandInputJson[argName] = commandParams[x];    
                        }                        
                        x++;
                    }

                    dynamic commandInput = JObject.FromObject(commandInputJson);
                    
                    Console.WriteLine(commandInput.ToString());
                    
                    
                    
                    JObject tobj = commandInput;
                    if (commandInput != null)
                    {
                        if (tobj.Property("Time") != null)
                        {
                            Plugin.LogInfo($"Parameter: {commandInput["Time"].ToString()}");                            
                        }                        
                    }

                    
                    pm.HandleCommand(sender, pmCommand.Key, "default", tobj);
                    */
                    
                    Console.WriteLine("Matching Command:" + pmCommand.Key);
                    break;
                }
                
            }                      
        }
        
    }
}
