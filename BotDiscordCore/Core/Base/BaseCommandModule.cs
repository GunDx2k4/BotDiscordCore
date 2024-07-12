using Discord;
using Discord.Commands;

namespace BotDiscordCore.Core.Base
{
    public class BaseCommandModule : ModuleBase<SocketCommandContext>
    {
        protected override Task<IUserMessage> ReplyAsync(string message = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageReference messageReference = null, MessageComponent components = null, ISticker[] stickers = null, Embed[] embeds = null, MessageFlags flags = MessageFlags.None)
        {
            return Context.Message.ReplyAsync(message, isTTS, embed, allowedMentions ??= AllowedMentions.None, options, components, stickers, embeds, flags);
        }
    }
}
