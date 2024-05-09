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

        [Command("ban")]
        public async Task HandleBanCommand(IUser user ,string reason)
        {
            try
            {
                await Context.Guild.AddBanAsync(user, reason: reason);
                await ReplyAsync($"Complated ban user {user.Mention} [{user.Id}]!");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"ERROR {ex}");
            }
        }
    }
}
