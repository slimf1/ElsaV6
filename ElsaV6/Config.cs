using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ElsaV6
{
    public class Config
    {
        public static Config LoadFromFile(string path)
        {
            Config config = null;
            using (StreamReader streamReader = File.OpenText(path))
            {
                config = (Config)JsonSerializer.Deserialize(streamReader.ReadToEnd(), typeof(Config));
            }
            return config;
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Log { get; set; }
        public string[] Rooms { get; set; }
        public string[] Whitelist { get; set; }
        public string Trigger { get; set; }
        public string[] Blacklist { get; set; }
        public string[] RoomBlacklist { get; set; }
    }
}
