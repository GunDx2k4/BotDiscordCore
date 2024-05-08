using Discord;
using Discord.Interactions;

namespace BotDiscordCore.Interactions
{
    public class BaseInteraction : InteractionModuleBase<SocketInteractionContext>
    {
        protected override Task RespondAsync(string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = true, AllowedMentions allowedMentions = null, RequestOptions options = null, MessageComponent components = null, Embed embed = null)
        {
            return base.RespondAsync(text, embeds, isTTS, ephemeral, allowedMentions, options, components, embed);
        }
    }
}
