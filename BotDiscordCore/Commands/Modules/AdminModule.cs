using BotDiscordCore.Commands.Attributes;
using Discord;
using Discord.Commands;

namespace BotDiscordCore.Commands.Modules
{
    [BotPermission(GuildPermission.Administrator)]
    [UserPermission(GuildPermission.Administrator)]
    public class AdminModule : BaseCommand
    {
        [Command("ping")]
        public async Task HandlePingCommand()
        {
            await ReplyAsync("Pong!");
        }
    }
}
