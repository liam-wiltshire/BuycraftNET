using MiNET;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using System;
using System.Runtime.Remoting.Contexts;
using MiNET.Net;
using MiNET.Plugins;
using Newtonsoft.Json;

using System.Reflection;

using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;

namespace BuycraftNET.Command
{
    public class Executor
    {
        
        private BuycraftNET Plugin;

        public Executor(BuycraftNET plugin)
        {
            Plugin = plugin;
        }
        
        public void ExecuteCommand(string command)
        {
            Player testsender = Plugin.GetServer().LevelManager.Levels.First().Players.First().Value;
            
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                Formatting = Formatting.Indented,
            };
            
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
                    
                    int x = 0;
                    foreach (ParameterInfo parameter in method.GetParameters())
                    {
                        if (typeof (Player).IsAssignableFrom(parameter.ParameterType)) continue;

                        if (commandParams.Length > x)
                        {
                            if (parameter.ParameterType == typeof(Target))
                            {
                                Target target = new Target();

                                if (commandParams[x] == "@a")
                                {
                                    target.Selector = "allPlayers";
                                }
                                else if (commandParams[x] == "@e")
                                {
                                    target.Selector = "allEntities";
                                }
                                else if (commandParams[x] == "@p")
                                {
                                    target.Selector = "nearestPlayer";
                                    target.Rules = null;
                                }
                                else if (commandParams[x] == "@r")
                                {
                                    target.Selector = "randomPlayer";
                                }
                                else
                                {
                                    target.Selector = "nearestPlayer";
                                    Target.Rule rule = new Target.Rule();
                                    rule.Name = "rule";
                                    rule.Value = commandParams[x];
                                    target.Rules = new[] {rule};                                    
                                }
                                
                                commandParams[x] = JsonConvert.SerializeObject(target, jsonSerializerSettings);
                            }    
                        }                        
                        x++;
                    }                    


                    Console.WriteLine("method:" + method.ToString());
                    Console.WriteLine("sender:" + testsender.ToString());
                    Console.WriteLine("CommandParmas::" + commandParams.ToString());
                    
                    Console.WriteLine("Invoke:");
                    pm.GetType().GetTypeInfo().GetDeclaredMethod("ExecuteCommand").Invoke(pm, new object[] {method, testsender, commandParams} );

                    Console.WriteLine("Matching Command:" + pmCommand.Key);
                    break;
                }
                
            }                      
            
        }
    }
}