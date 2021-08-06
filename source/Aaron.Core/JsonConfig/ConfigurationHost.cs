// Copyright (C) 2021 Aaron C. Willows (aaron@aaronwillows.com)
// 
// This program is free software; you can redistribute it and/or modify it under the terms of the
// GNU Lesser General Public License as published by the Free Software Foundation; either version
// 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See
// the GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston,
// MA 02111-1307 USA

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Aaron.Core.JsonConfig
{
    public class ConfigurationHost
    {
        private readonly Dictionary<string, IConfigurable> _configurations;
        private readonly Dictionary<string, IConfigurable> _defaults;
        private readonly JsonSerializerSettings _serializerSettings;

        public string ConfigurationBaseLocation { get; }
        public string DefaultBaseLocation { get; }

        public IConfigurable this[string name] => _configurations[name];

        public static void EnsureDirectory(string fileLocation)
        {
            string directory = Path.GetDirectoryName(fileLocation);

            Directory.CreateDirectory(directory);
        }

        public string GetDefaultFileLocation(string name)
        {
            return Path.Join(DefaultBaseLocation, GetFileName(name));
        }

        public string GetFileLocation(string name)
        {
            return Path.Join(ConfigurationBaseLocation, GetFileName(name));
        }

        public static string GetFileName(string name)
        {
            return $"{name}.json";
        }

        public bool HasConfiguration(string name)
        {
            return !string.IsNullOrEmpty(name) && _configurations.ContainsKey(name);
        }

        public void Load<T>(string name, IConfigurable defaults)
            where T : IConfigurable, new()
        {
            if (defaults is null) { throw new ArgumentNullException(nameof(defaults)); }

            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException(nameof(name)); }

            if (HasConfiguration(name)) { throw new DuplicateConfigurationNameException(name); }

            string fileLocation = GetFileLocation(name);

            IConfigurable instance = defaults.Copy();

            if (File.Exists(fileLocation))
            {
                string json = File.ReadAllText(fileLocation);
                instance = JsonConvert.DeserializeObject<T>(json);
            }

            _configurations.Add(name, instance);
            _defaults.Add(name, defaults.Copy());
        }

        public void Load<T>(string name)
            where T : IConfigurable, new()
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException(nameof(name)); }

            if (HasConfiguration(name)) { throw new DuplicateConfigurationNameException(name); }

            string defaultFileLocation = GetDefaultFileLocation(name);

            string json = File.ReadAllText(defaultFileLocation);
            IConfigurable defaultConfiguration = JsonConvert.DeserializeObject<T>(json);

            Load<T>(name, defaultConfiguration);
        }

        public void RestoreDefaults(string name)
        {
            IConfigurable newConfiguration = _defaults[name].Copy();

            _configurations[name] = newConfiguration;
        }

        public void Save(string name)
        {
            Save(name, false);
        }

        public void Save()
        {
            foreach (string name in _configurations.Keys) { Save(name); }
        }

        public void SaveDefault(string name)
        {
            Save(name, true);
        }

        private void Save(string name, bool isDefault)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException(nameof(name)); }

            IConfigurable configurable = this[name];
            string fileLocation = isDefault
                ? GetDefaultFileLocation(name)
                : GetFileLocation(name);

            EnsureDirectory(fileLocation);

            string json = JsonConvert.SerializeObject(configurable, _serializerSettings);

            File.WriteAllText(fileLocation, json);
        }


        #region Singleton

        // ReSharper disable once InconsistentNaming
        private static readonly ConfigurationHost _instance = new ConfigurationHost();

        static ConfigurationHost() { }

        private ConfigurationHost()
        {
            ConfigurationBaseLocation =
                Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "aaron");

            DefaultBaseLocation = AppDomain.CurrentDomain.BaseDirectory;


            _defaults = new Dictionary<string, IConfigurable>();
            _configurations = new Dictionary<string, IConfigurable>();

            _serializerSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };

            Directory.CreateDirectory(ConfigurationBaseLocation);
        }

        // ReSharper disable once ConvertToAutoProperty
        // ReSharper disable once ReplaceAutoPropertyWithComputedProperty
        public static ConfigurationHost Instance { get; } = _instance;

        #endregion
    }
}