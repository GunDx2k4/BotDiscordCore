using BotDiscordCore.Logger;
using Discord;
using Discord.WebSocket;

namespace BotDiscordCore.Services
{
    public class UserService
    {
        public async Task ByeUserAsync(SocketGuild guild, SocketUser user)
        {
            var defaultChannel = guild.SystemChannel;

            MyLogger.Log($"User [{user.GlobalName} - {user.Id}] Left Guid [{guild.Name} - {guild.Id}]");

            var embedBye = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = user.GlobalName,
                    IconUrl = string.IsNullOrEmpty(user.GetAvatarUrl()) ? user.GetDefaultAvatarUrl() : user.GetAvatarUrl(),
                },
                Title = $"Tạm biệt bạn, cảm ơn bạn đã tham gia server.",
                Description = $"⏲Tuổi của tài khoản {user.Mention}:\n{TimestampTag.FromDateTimeOffset(user.CreatedAt, TimestampTagStyles.Relative)}",
                Color = Color.Blue,
                Timestamp = DateTimeOffset.UtcNow,
                ImageUrl = string.IsNullOrEmpty(user.GetAvatarUrl()) ? user.GetDefaultAvatarUrl() : user.GetAvatarUrl(),
                ThumbnailUrl = guild.IconUrl,
                Footer = new EmbedFooterBuilder
                {
                    Text = $"From {guild.Name}",
                    IconUrl = guild.IconUrl,
                }
            };

            await defaultChannel.SendMessageAsync(embed: embedBye.Build());
        }

        public async Task WelcomeNewUserAsync(SocketGuildUser newUser)
        {
            var guild = newUser.Guild;

            MyLogger.Log($"New User [{newUser.GlobalName} - {newUser.Id}] Join Guid [{guild.Name} - {guild.Id}]");

            var defaultChannel = guild.SystemChannel;
            IRole memberRole = guild.Roles.FirstOrDefault(r => r.Name == "Member");
            if (memberRole == null)
            {
                memberRole = await guild.CreateRoleAsync("Member", null, new Color(0x00deff), true, true);
            }
            await newUser.AddRoleAsync(memberRole.Id);

            var embedWelcome = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = newUser.GlobalName,
                    IconUrl = string.IsNullOrEmpty(newUser.GetAvatarUrl()) ? newUser.GetDefaultAvatarUrl() : newUser.GetAvatarUrl(),
                },
                Title = $"Chào mừng bạn đã tham gia server.",
                Description = $"⏲Tuổi của tài khoản {newUser.Mention}:\n{TimestampTag.FromDateTimeOffset(newUser.CreatedAt, TimestampTagStyles.Relative)}",
                Color = Color.Blue,
                Timestamp = DateTimeOffset.UtcNow,
                ImageUrl = string.IsNullOrEmpty(newUser.GetAvatarUrl()) ? newUser.GetDefaultAvatarUrl() : newUser.GetAvatarUrl(),
                ThumbnailUrl = guild.IconUrl,
                Footer = new EmbedFooterBuilder
                {
                    Text = $"From {guild.Name}",
                    IconUrl = guild.IconUrl,
                }
            };

            await defaultChannel.SendMessageAsync(embed: embedWelcome.Build());
        }
    }
}
