using BuycraftNET.Command;
using BuycraftNET.Command.Buycraft;
//using BuycraftNET.Command.Buy;

using MiNET;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using YamlDotNet;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace BuycraftNET
{
    [Plugin(PluginName = "BuycraftNET", Description = "MiNET Buycraft plugin.", PluginVersion = "1.0", Author = "Buycraft")]
    public class BuycraftNET : Plugin
    {
        protected override void OnEnable()
        {
            Context.PluginManager.LoadCommands(new Buycraft(this));
//            Context.PluginManager.LoadCommands(new Buy(this));            
            EIO();
        }

        public string GetDataFolder() => Path.Combine(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath, "BuycraftNET");
        public void EIO()
        {
            CTDT(GetDataFolder());
            //TODO
        }
        public void CTDT(string name)
        {
            DirectoryInfo temp = new DirectoryInfo(name);
            if (!temp.Exists)
            {
                temp.Create();
            }
        }
        public void CTFL(string name)
        {
            FileInfo temp = new FileInfo(name);
            if (!temp.Exists)
            {
                FileStream fs = temp.Create();
                fs.Close();
            }
        }
    }
}
