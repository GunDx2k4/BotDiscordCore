using System.Text.Json;

namespace BotDiscordCore
{
    public class Config
    {
        public static string TokenBot {  get; private set; } = string.Empty;
        public static string StatusBot { get; private set; } = string.Empty; 

        static Config()
        {
            string jsonContent = File.ReadAllText($"config.json");

            JsonDocument jsonDoc = JsonDocument.Parse(jsonContent);
            JsonElement root = jsonDoc.RootElement;

            TokenBot = root.GetProperty("TokenBot").ToString();
            StatusBot = root.GetProperty("StatusBot").ToString();
        }
    }
}
