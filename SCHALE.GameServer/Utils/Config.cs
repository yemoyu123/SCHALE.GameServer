using Serilog;
using System.Text.Json;

namespace SCHALE.GameServer.Utils
{
    public class Config : Singleton<Config>
    {
        public static string ConfigPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.json");
        
        public string Address { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 6667;

        public static void Load()
        {
            if (!File.Exists(ConfigPath))
                Save();

#if !DOCKER_BUILD
            string json = File.ReadAllText(ConfigPath);
            Instance = JsonSerializer.Deserialize<Config>(json);
#endif

            Log.Debug($"Config loaded");
        }

        public static void Save()
        {
#if !DOCKER_BUILD
            File.WriteAllText(ConfigPath, JsonSerializer.Serialize(Instance));
#endif

            Log.Debug($"Config saved");
        }
    }
}
