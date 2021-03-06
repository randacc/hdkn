﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadouken.Configuration
{
    public class ApplicationConfigurationSection : ConfigurationSection, IConfiguration
    {
        [ConfigurationProperty("instanceName", IsRequired = false, DefaultValue = "Hadouken")]
        public string InstanceName
        {
            get { return this["instanceName"].ToString(); }
            set { this["instanceName"] = value; }
        }

        [ConfigurationProperty("dataPath", IsRequired = false, DefaultValue = "/")]
        public string ApplicationDataPath
        {
            get { return GetFullPath(this["dataPath"].ToString()); }
            set { this["dataPath"] = value; }
        }

        private string GetFullPath(string relativePath)
        {
            string path = Environment.ExpandEnvironmentVariables(relativePath);
            path =  Path.GetFullPath(path);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        [ConfigurationProperty("plugins", IsRequired = true)]
        [ConfigurationCollection(typeof(PluginsCollection), AddItemName = "plugin", CollectionType = ConfigurationElementCollectionType.BasicMap)]
        public PluginsCollection Plugins
        {
            get { return this["plugins"] as PluginsCollection; }
        }

        [ConfigurationProperty("http", IsRequired = true)]
        public HttpConfiguration Http
        {
            get { return this["http"] as HttpConfiguration; }
        }

        [ConfigurationProperty("rpc", IsRequired = true)]
        public RpcConfiguration Rpc
        {
            get { return this["rpc"] as RpcConfiguration; }
        }

        public void Save()
        {
            _configuration.Save();
        }

        private System.Configuration.Configuration _configuration;

        public static IConfiguration Load()
        {
            var cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var section = cfg.GetSection("hdkn") as ApplicationConfigurationSection;

            if (section == null)
                return null;

            section._configuration = cfg;

            return section;
        }
    }

    public sealed class RpcConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("gatewayUri", IsRequired = true)]
        public string GatewayUri
        {
            get { return this["gatewayUri"].ToString(); }
            set { this["gatewayUri"] = value; }
        }

        [ConfigurationProperty("pluginUri", IsRequired = true)]
        public string PluginUri
        {
            get { return this["pluginUri"].ToString(); }
            set { this["pluginUri"] = value; }
        }
    }
}
