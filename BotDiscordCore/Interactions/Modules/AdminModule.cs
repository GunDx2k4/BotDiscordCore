using BotDiscordCore.Bot;
using BotDiscordCore.Interactions.Attributes;
using BotDiscordCore.Utils;
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

        [SlashCommand("ban", "Ban user")]
        public async Task HandleBanCommand(IUser user, string reason)
        {
            try
            {
                await Context.Guild.AddBanAsync(user, reason: reason);
                await RespondAsync($"Complated ban user {user.Mention} [{user.Id}]!");
            }
            catch (Exception ex)
            {
                await RespondAsync($"ERROR {ex}");
            }
        }

        [UserCommand("Unban")]
        public async Task HandleUnbanUser(IUser user)
        {
            try
            {
                await Context.Guild.RemoveBanAsync(user);
                await RespondAsync($"Complated unban user {user.Mention} [{user.Id}]!");
            }
            catch (Exception ex)
            {
                await RespondAsync($"ERROR {ex}");
            }
        }

        [SlashCommand("listban","Get list ban user")]
        public async Task HandleListBanCommand()
        {
            try
            {
                var listUserBans = new List<EmbedFieldBuilder>();

                var list = await Context.Guild.GetBansAsync().FlattenAsync();
                if (list.Count() > 0)
                {
                    int count = 1;
                    var userBan = new EmbedFieldBuilder();
                    foreach (var user in list)
                    {
                        userBan.WithName($"{count}. User {user.User}");
                        userBan.WithValue($"{user.User.Mention} reason ban : {MarkdownText.BoldText(MarkdownText.HighlightText(user.Reason))}");
                        listUserBans.Add(userBan);
                        count++;
                    }
                }
                else
                {
                    listUserBans.Add(new EmbedFieldBuilder
                    {
                        Name = "List ban clean",
                        Value = $"{MarkdownText.BoldText(MarkdownText.HighlightText("Nobody was banned."))}",
                        IsInline = true
                    });
                }
                

                var embedUserBans = new EmbedBuilder
                {
                    Author = new EmbedAuthorBuilder
                    {
                        Name = BotDiscord.Instance.ClientBot.CurrentUser.Username,
                        IconUrl = string.IsNullOrEmpty(BotDiscord.Instance.ClientBot.CurrentUser.GetAvatarUrl()) ? BotDiscord.Instance.ClientBot.CurrentUser.GetDefaultAvatarUrl() : BotDiscord.Instance.ClientBot.CurrentUser.GetAvatarUrl(),
                    },
                    Title = $"List ban user in server {Context.Guild.Name}",
                    Color = Color.Blue,
                    Timestamp = DateTimeOffset.UtcNow,
                    ThumbnailUrl = Context.Guild.IconUrl,
                    Fields = listUserBans,
                    Footer = new EmbedFooterBuilder
                    {
                        Text = $"From {Context.Guild.Name}",
                        IconUrl = Context.Guild.IconUrl,
                    }
                };
                await RespondAsync(embed: embedUserBans.Build(), ephemeral: false);
            }
            catch (Exception ex)
            {
                await RespondAsync($"ERROR {ex}");
            }
        }
    }
}
