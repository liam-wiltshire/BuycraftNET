using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;

using MiNET;

using BuycraftNET.Models;
using MiNET.Blocks;
using MiNET.Plugins;
using MiNET.Worlds;

namespace BuycraftNET.Services
{
    public class CommandChecker
    {
        private BuycraftNET _plugin;

        public CommandChecker (BuycraftNET plugin)
        {
            _plugin = plugin;
        }

        public void CheckCommands(Object source, ElapsedEventArgs e)
        {

            List<BCPlayer> playerlist = new List<BCPlayer>();
            
            _plugin.LogInfo("Fetching all due players...");
            
            var players = _plugin.GetApiClient().GetDuePlayers();
            if (players.Result == null)
            {
                _plugin.LogError("Unable to fetch due players");
            }
            else
            {
                foreach (var playerObj in players.Result["players"])
                {
                    BCPlayer player = new BCPlayer(
                        (int)playerObj["id"],
                        (string)playerObj["name"],
                        (string)playerObj["uuid"]
                    );
                    playerlist.Add(player);
                }
                
                _plugin.LogInfo("Fetched due players ("+playerlist.Count+" found)");

                if ((bool) players.Result["meta"]["execute_offline"])
                {
                    _plugin.LogInfo("Executing commands that can be completed now");
                    //platform.executeAsync(new ImmediateCommandExecutor(platform));
                }
                else
                {
                    _plugin.LogInfo("Process online players");
                     ProcessOnlinePlayers(playerlist);
                }
            }
            
        }
        
        public async Task ProcessOnlinePlayers(List<BCPlayer> playerlist)
        {
            await Task.Delay(100);
            foreach (var player in playerlist)
            {
                //Is the player online?
                Level level = (Level) _plugin.GetServer().LevelManager.Levels.First();
                
                if (level.GetSpawnedPlayers().Where(p => p.Username == player.getIgn()).ToArray().Length == 0)
                {
                    Player pluginPlayer = level.GetSpawnedPlayers().First(p => p.Username == player.getIgn());
                    Console.WriteLine(player.getIgn() + " is online");
                    //Fetch commands for player
                    var commands = _plugin.GetApiClient().GetPlayerQueue(player.getId());
                    Console.WriteLine(commands.Result.ToString());
                    _plugin.LogInfo("Fetched " + commands.Result["commands"].ToArray().Length + " commands for player " + player.getIgn());
                    //Execute commands
                    foreach (var command in commands.Result["commands"])
                    {
                        if ((int) command["conditions"]["slots"] > 0)
                        {
                            int slots = pluginPlayer.Inventory.Slots.Where(i => i.GetType() == typeof(Air)).Count();
                            Console.WriteLine("Player has " + slots + " free slots");
                            if (slots < (int) command["conditions"]["slots"])
                            {
                                continue;
                            }
                        }
                        
                        //command["command"]
                        //command["conditions"]["delay"]
                        //command["conditions"]["slots"]
                    }
                }
                else
                {
                    Console.WriteLine(player.getIgn() + " is not online");
                }
            }
        }
    }
}