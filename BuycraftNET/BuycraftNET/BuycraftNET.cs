using BuycraftNET.Command;
//using BuycraftNET.Command.Buycraft;
//using BuycraftNET.Command.Buy;

using MiNET;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;

using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Reflection;

using IniParser;
using IniParser.Model;

namespace BuycraftNET
{
    [Plugin(PluginName = "BuycraftNET", Description = "MiNET Buycraft plugin.", PluginVersion = "1.0", Author = "Buycraft")]
    public class BuycraftNET : Plugin
    {
        private FileIniDataParser _parser;
        private IniData _config;
        private static readonly HttpClient _httpClient = new HttpClient();
        
        protected override void OnEnable()
        {

            this._parser = new FileIniDataParser();
            this._config = this._parser.ReadFile(this.GetConfigFile());
            
            Context.PluginManager.LoadCommands(new Buycraft(this));
//            Context.PluginManager.LoadCommands(new Buy(this));            

            var commandChecker = new CommandChecker(this);
            var autoEvent = new AutoResetEvent(false);

            var stateTimer = new Timer(commandChecker.CheckCommands,
                                   autoEvent, 10000, 300000);
        }

        public string GetDataFolder() => Path.Combine(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath, "BuycraftNET");

        public string GetConfigFile()
        {
            DirectoryInfo buycraftDataDirectory = new DirectoryInfo(GetDataFolder());
            if (!buycraftDataDirectory.Exists)
            {
                buycraftDataDirectory.Create();
            }
            
            FileInfo buycraftConfigFile = new FileInfo(GetDataFolder() + "config.txt" );
            if (!buycraftConfigFile.Exists)
            {
                FileStream fs = buycraftConfigFile.Create();
                fs.Close();
            }

            return GetDataFolder() + "config.txt";
        }

        public string GetConfig(string key)
        {
            return this._config["buycraft"][key];
        }

        public void SetConfig(string key, string value)
        {
            this._config["buycraft"][key] = value;
            this._parser.WriteFile(this.GetConfigFile(), this._config);
        }

        public HttpClient GetHttpClient()
        {
            return _httpClient;
        }
        
    }

    public class CommandChecker
    {
        private BuycraftNET Plugin;

        public CommandChecker (BuycraftNET plugin)
        {
            Plugin = plugin;
        }

        public void CheckCommands(Object stateInfo)
        {
            Console.WriteLine("Config:" + Plugin.GetConfigFile());
            Plugin.SetConfig("secret", "blah");
            Console.WriteLine(Plugin.GetConfig("secret"));
            Console.WriteLine("Do Command Check!");

            var httpClient = Plugin.GetHttpClient();
            
        }
    }
}
