using System;
using System.Configuration;
using System.IO;

namespace SongListScraper.Settings
{
    public class SettingsConfig : ConfigurationSection
    {
        public SettingsConfig()
        {
            SavedSongsFile = $"{AppDomain.CurrentDomain.BaseDirectory}SavedSongs.txt";
        }

        [ConfigurationProperty("StorageType", DefaultValue = SongStorage.FILE)]
        public SongStorage StorageType
        {
            get { return (SongStorage)this["StorageType"]; }
            set { this["StorageType"] = value; }
        }

        [ConfigurationProperty("SavedSongsFile", DefaultValue = null)]
        public string SavedSongsFile
        {
            get { return (string)this["SavedSongsFile"]; }
            set { this["SavedSongsFile"] = value; }
        }

        [ConfigurationProperty("Delimiter", DefaultValue = "##")]
        public string Delimiter
        {
            get { return (string)this["Delimiter"]; }
            set { this["Delimiter"] = value; }
        }

        [ConfigurationProperty("UpdateInterval", DefaultValue = "15")]
        [IntegerValidator(MinValue = 10)]
        public int UpdateInterval
        {
            get { return (int)this["UpdateInterval"]; }
            set { this["UpdateInterval"] = value; }
        }

        public static void Save(SettingsConfig settings, string path = null)
        {
            if (path == null)
                path = $"{AppDomain.CurrentDomain.BaseDirectory}Settings.xml";

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.Sections.Add("SettingsConfig", settings);
            config.SaveAs(path, ConfigurationSaveMode.Full);
        }
        
        public static void Load(ref SettingsConfig settings, string path = null)
        {
            if (path == null)
                path = $"{AppDomain.CurrentDomain.BaseDirectory}Settings.xml";

            if (!File.Exists(path))
                SettingsConfig.Save(settings, path);

            ConfigurationFileMap fileMap = new ConfigurationFileMap(path);
            Configuration config = ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
            settings = (SettingsConfig)config.GetSection("SettingsConfig");
        }
    }
}
