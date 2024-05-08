using BotDiscordCore.Locales;
using BotDiscordCore.Utils;
using Discord;
using Discord.Commands;

namespace BotDiscordCore.Commands.Attributes
{
    public class UserPermissionAttribute : PreconditionAttribute
    {
        public GuildPermission? GuildPermission { get; }
        public ChannelPermission? ChannelPermission { get; }

        public UserPermissionAttribute(GuildPermission guildPermission)
        {
            GuildPermission = guildPermission;
        }

        public UserPermissionAttribute(ChannelPermission channelPermission)
        {
            ChannelPermission = channelPermission;
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            IGuildUser guildUser = context.User as IGuildUser;
            if (GuildPermission.HasValue)
            {
                if (guildUser == null)
                {
                    return PreconditionResult.FromError(Resources.NOT_GUILD_ERROR_MESSAGE);
                }

                if (!guildUser.GuildPermissions.Has(GuildPermission.Value))
                {
                    return PreconditionResult.FromError(ErrorMessage ?? $"{Resources.NOT_PERMISSION_USER} {MarkdownText.BoldText(MarkdownText.HighlightText(GuildPermission.Value.ToString()))}.");
                }
            }
            if (ChannelPermission.HasValue)
            {
                ChannelPermissions perms;
                if (context.Channel is IGuildChannel guildChannel)
                    perms = guildUser.GetPermissions(guildChannel);
                else
                    perms = ChannelPermissions.All(context.Channel);

                if (!perms.Has(ChannelPermission.Value))
                    return PreconditionResult.FromError(ErrorMessage ?? $"{Resources.USER_NOT_PERMISSION_CHANNEL}  {MarkdownText.BoldText(MarkdownText.HighlightText(ChannelPermission.Value.ToString()))}.");
            }

            return PreconditionResult.FromSuccess();
        }
    }
}
