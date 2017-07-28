using BuycraftNET.Command;

using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Worlds;

using System;
using System.Collections.Concurrent;
using System.Timers;
using System.IO;
using System.Reflection;

using IniParser;
using IniParser.Model;

using BuycraftNET.Api;
using MiNET;
using MiNET.Plugins.Commands;

using System.Linq;
using System.Net;

namespace BuycraftNET
{
    [Plugin(PluginName = "BuycraftNET", Description = "MiNET Buycraft plugin.", PluginVersion = "1.0", Author = "Buycraft")]
    public class BuycraftNET : Plugin
    {
        private FileIniDataParser _parser = new FileIniDataParser();
        private IniData _config;
        private ApiClient _apiClient;
        
        protected override void OnEnable()
        {
            LogInfo("Buycraft has started");

            //@TODO - This Is only in so we have commands to test with - needs removing at some point.
            Context.PluginManager.LoadCommands(new VanillaCommands(Context.Server.PluginManager));
            
            this._config = this._parser.ReadFile(this.GetConfigFile());
            
            _apiClient = new ApiClient(this);
            
            Context.PluginManager.LoadCommands(new Buycraft(this));
            Context.PluginManager.LoadCommands(new Buy(this));            

            var commandChecker = new CommandChecker(this);
            

            //Start the timer for checking commands
            var stateTimer = new Timer(5000);
            stateTimer.Elapsed += commandChecker.CheckCommands;
            stateTimer.AutoReset = true;
            stateTimer.Enabled = true;
            
            //Do we have a secret key
            if (GetConfig("secret") == null || GetConfig("secret") == "")
            {
                LogError("It looks like this is a new installation. Specify your secret key by entering /buycraft secret [key]");
            }
            
        }

        public string GetDataFolder() => Path.Combine(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath, "BuycraftNET");

        public string GetConfigFile()
        {
            DirectoryInfo buycraftDataDirectory = new DirectoryInfo(GetDataFolder());
            if (!buycraftDataDirectory.Exists)
            {
                buycraftDataDirectory.Create();
            }
            
            FileInfo buycraftConfigFile = new FileInfo(GetDataFolder() + "/config.txt" );
            if (!buycraftConfigFile.Exists)
            {
                FileStream fs = buycraftConfigFile.Create();
                fs.Close();
            }

            return GetDataFolder() + "/config.txt";
        }

        public string GetConfig(string key)
        {
            return _config["buycraft"][key];
        }

        public void SetConfig(string key, string value)
        {
            _config["buycraft"][key] = value;
            _parser.WriteFile(this.GetConfigFile(), _config);
        }

        public void LogInfo(string message)
        {
            Console.WriteLine("INFO [BuycraftNET] "+ message);
        }

        public void LogError(string message)
        {
            Console.WriteLine("ERROR [BuycraftNET] "+ message);
        }

        public ApiClient GetApiClient()
        {
            return _apiClient;
        }

        public PluginManager GetPluginManager()
        {
            return Context.PluginManager;
        }

        public MiNetServer GetServer()
        {
            return Context.Server;
        }
    }

    public class CommandChecker
    {
        private BuycraftNET _plugin;

        public CommandChecker (BuycraftNET plugin)
        {
            _plugin = plugin;
        }

        public void CheckCommands(Object source, ElapsedEventArgs e)
        {
//            var result = Plugin.GetApiClient().Get("https://plugin.buycraft.net/versions/sponge");
//            Console.WriteLine(result.Result.ToString());
        }

    }
}
