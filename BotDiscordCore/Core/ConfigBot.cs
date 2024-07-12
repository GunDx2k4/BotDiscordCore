using BotDiscordCore.Core.Converters;
using Discord;
using Newtonsoft.Json;

namespace BotDiscordCore.Core
{
    public class ConfigBot
    {
        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string TokenBot {  get; set; }

        [JsonProperty("prefix", NullValueHandling = NullValueHandling.Ignore)]
        public string Prefix { get; set; }

        [JsonProperty("gatewayIntents"), JsonConverter(typeof(GatewayIntentsConverter))]
        public GatewayIntents GatewayIntents { get; set; }

        [JsonProperty("messageCacheSize")]
        public int MessageCacheSize { get; set; } = 100;

        [JsonProperty("alwaysDownloadUsers")]
        public bool AlwaysDownloadUsers { get; set; } = true;
    }
}
