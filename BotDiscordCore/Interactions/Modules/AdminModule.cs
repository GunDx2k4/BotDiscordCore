using BotDiscordCore.Interactions.Attributes;
using Discord;
using Discord.Interactions;

namespace BotDiscordCore.Interactions.Modules
{
    [BotPermission(GuildPermission.Administrator)]
    [UserPermission(GuildPermission.Administrator)]
    public class AdminModule : BaseInteraction
    {

        [SlashCommand("ping", "Test Bot")]
        public async Task HandlePingCommand()
        {
            await RespondAsync($"Pong!");
        }
    }
}
