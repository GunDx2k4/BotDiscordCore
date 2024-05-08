using Discord;
using Discord.Commands;

namespace BotDiscordCore.Commands
{
    public class BaseCommand : ModuleBase<SocketCommandContext>
    {
        protected override Task<IUserMessage> ReplyAsync(string message = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageReference messageReference = null, MessageComponent components = null, ISticker[] stickers = null, Embed[] embeds = null, MessageFlags flags = MessageFlags.None)
        {
            allowedMentions ??= AllowedMentions.None;
            return Context.Message.ReplyAsync(message, isTTS, embed, allowedMentions, options, components, stickers, embeds, flags);
        }
    }
}
