using BotDiscordCore.Bot;
using BotDiscordCore.Interactions.Attributes;
using BotDiscordCore.Utils;
using Discord;
using Discord.Interactions;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Text.Json;
using System.Threading.Channels;

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
                var embedBan = new EmbedBuilder
                {
                    Author = new EmbedAuthorBuilder
                    {
                        Name = BotDiscord.Instance.ClientBot.CurrentUser.Username,
                        IconUrl = string.IsNullOrEmpty(BotDiscord.Instance.ClientBot.CurrentUser.GetAvatarUrl()) ? BotDiscord.Instance.ClientBot.CurrentUser.GetDefaultAvatarUrl() : BotDiscord.Instance.ClientBot.CurrentUser.GetAvatarUrl(),
                    },
                    Title = $"Bạn đã bị ban tại server {Context.Guild.Name}",
                    Description = $"Bạn đã bị ban tại server {MarkdownText.BoldText(MarkdownText.HighlightText(Context.Guild.Name))} " +
                    $"với lí do {MarkdownText.BoldText(MarkdownText.HighlightText(reason))}",
                    Color = Color.Red,
                    Timestamp = DateTimeOffset.UtcNow,
                    ThumbnailUrl = Context.Guild.IconUrl,
                    Footer = new EmbedFooterBuilder
                    {
                        Text = $"From {Context.Guild.Name}",
                        IconUrl = Context.Guild.IconUrl,
                    }
                };
                await user.SendMessageAsync(embed: embedBan.Build());
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
                var list = await Context.Guild.GetBansAsync().FlattenAsync();
                if (list.ToList().Find(u => u.User.Id == user.Id) == null)
                {
                    await RespondAsync($"Not found user {user.Mention} [{user.Id}] in list ban!");
                    return;
                };
                await Context.Guild.RemoveBanAsync(user);
                await RespondAsync($"Complated unban user {user.Mention} [{user.Id}]!");
                var embedUnban = new EmbedBuilder
                {
                    Author = new EmbedAuthorBuilder
                    {
                        Name = BotDiscord.Instance.ClientBot.CurrentUser.Username,
                        IconUrl = string.IsNullOrEmpty(BotDiscord.Instance.ClientBot.CurrentUser.GetAvatarUrl()) ? BotDiscord.Instance.ClientBot.CurrentUser.GetDefaultAvatarUrl() : BotDiscord.Instance.ClientBot.CurrentUser.GetAvatarUrl(),
                    },
                    Title = $"Bạn đã được mở ban tại server {Context.Guild.Name}",
                    Description = $"Bạn có thể quay lại server tại đây \n {MarkdownText.MaskedLinks(Context.Guild.Name, Context.Guild.DefaultChannel.CreateInviteAsync(maxUses: 1,maxAge: null).Result.Url)}",
                    Color = Color.Green,
                    Timestamp = DateTimeOffset.UtcNow,
                    ThumbnailUrl = Context.Guild.IconUrl,
                    Footer = new EmbedFooterBuilder
                    {
                        Text = $"From {Context.Guild.Name}",
                        IconUrl = Context.Guild.IconUrl,
                    }
                };
                await user.SendMessageAsync(embed: embedUnban.Build());
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

        private static CancellationTokenSource _cancellationTokenSource = new();
        private static string IdVideoLive = string.Empty;

        [SlashCommand("ytlive","Check Youtube livestream")]
        public async Task HandleYTLiveCommand(string apiKey, string channelId)
        {
            _cancellationTokenSource?.Cancel();

            _cancellationTokenSource = new();

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
            });

            var searchChannelRequest = youtubeService.Channels.List("snippet");
            searchChannelRequest.Id = channelId;
            try
            {
                var searchChannelResponse = searchChannelRequest.Execute();
                var channelInfo = searchChannelResponse.Items[0];
                if (channelInfo == null)
                {
                    await RespondAsync("ERROR LOAD CHANNEL");
                    return;
                }
                string linkChannel = $"https://www.youtube.com/{(string.IsNullOrEmpty(channelInfo.Snippet.CustomUrl) ? channelInfo.Id : channelInfo.Snippet.CustomUrl)}";
                await RespondAsync($"Start check live channel {MarkdownText.MaskedLinks(channelInfo.Snippet.Title, linkChannel)}");
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    var searchListRequest = youtubeService.Search.List("snippet");
                    searchListRequest.ChannelId = channelId;
                    searchListRequest.EventType = SearchResource.ListRequest.EventTypeEnum.Live;
                    searchListRequest.Type = "video";
                    searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
                    try
                    {
                        var searchListResponse = searchListRequest.Execute();

                        if (searchListResponse.Items.Count == 0) continue;

                        var videoLive = searchListResponse.Items.ToList()[0];

                        if (videoLive == null) continue;

                        if (IdVideoLive == videoLive.Id.VideoId) continue;
                        IdVideoLive = videoLive.Id.VideoId;

                        string liveVideoUrl = $"https://www.youtube.com/watch?v={videoLive.Id.VideoId}";
                        var embedYT = new EmbedBuilder
                        {
                            Author = new EmbedAuthorBuilder
                            {
                                Name = channelInfo.Snippet.Title,
                                IconUrl = channelInfo.Snippet.Thumbnails.Default__.Url,
                                Url = linkChannel
                            },
                            Title = $"{videoLive.Snippet.ChannelTitle} vừa live stream",
                            Description = $"Stream vào lúc {MarkdownText.BoldText(TimestampTag.FromDateTimeOffset(videoLive.Snippet.PublishedAtDateTimeOffset.Value, TimestampTagStyles.LongDateTime).ToString())}\n{MarkdownText.BoldText(MarkdownText.MaskedLinks(videoLive.Snippet.Title, liveVideoUrl))}",
                            Color = Color.Green,
                            Timestamp = DateTimeOffset.UtcNow,
                            ThumbnailUrl = channelInfo.Snippet.Thumbnails.Default__.Url,
                            ImageUrl = videoLive.Snippet.Thumbnails.Medium.Url,
                            Footer = new EmbedFooterBuilder
                            {
                                Text = $"From {Context.Guild.Name}",
                                IconUrl = Context.Guild.IconUrl,
                            }
                        };
                        await ReplyAsync(embed: embedYT.Build());
                    }
                    catch (Exception ex)
                    {
                        //await ReplyAsync($"Request failed: {ex.Message}");
                    }
                    await Task.Delay(60000);
                }
            }
            catch (Exception ex)
            {
                await RespondAsync($"Request failed: {ex.Message}");
            }

        }
    }
}
