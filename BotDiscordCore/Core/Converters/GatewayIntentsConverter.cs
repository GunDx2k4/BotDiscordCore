using BotDiscordCore.Core.Base;
using Discord;

namespace BotDiscordCore.Core.Converters
{
    public class GatewayIntentsConverter : BaseConverter<GatewayIntents>
    {
        public GatewayIntentsConverter(bool useQuotes) : base(useQuotes) { }

        public GatewayIntentsConverter() : base(true) { }

        protected override List<KeyValuePair<GatewayIntents, string>> Mapping =>
            new List<KeyValuePair<GatewayIntents, string>>
                {
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.None, "None"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.Guilds, "Guilds"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.GuildMembers, "GuildMembers"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.GuildBans, "GuildBans"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.GuildEmojis, "GuildEmojis"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.GuildIntegrations, "GuildIntegrations"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.GuildWebhooks, "GuildWebhooks"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.GuildInvites, "GuildInvites"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.GuildVoiceStates, "GuildVoiceStates"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.GuildPresences, "GuildPresences"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.GuildMessages, "GuildMessages"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.GuildMessageReactions, "GuildMessageReactions"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.GuildMessageTyping, "GuildMessageTyping"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.DirectMessages, "DirectMessages"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.DirectMessageReactions, "DirectMessageReactions"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.DirectMessageTyping, "DirectMessageTyping"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.MessageContent, "MessageContent"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.GuildScheduledEvents, "GuildScheduledEvents"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.AutoModerationConfiguration, "AutoModerationConfiguration"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.AutoModerationActionExecution, "AutoModerationActionExecution"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.GuildMessagePolls, "GuildMessagePolls"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.DirectMessagePolls, "DirectMessagePolls"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.AllUnprivileged, "AllUnprivileged"),
                    new KeyValuePair<GatewayIntents, string>(GatewayIntents.All, "All")
                };
    }
}
