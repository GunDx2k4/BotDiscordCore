using BotDiscordCore.Bot;
using BotDiscordCore.Locales;
using BotDiscordCore.Logger;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace BotDiscordCore.Services
{
    public class MessageServices
    {
        public async Task InstallCommandsAsync()
        {
            await BotDiscord.Instance.Command.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: BotDiscord.Instance.ServiceProvider);
            MyLogger.Log($"Loading Command", LogLevel.Debug);
            foreach (var command in BotDiscord.Instance.Command.Commands)
            {
                MyLogger.Log($"Loading Command [{command.Name}]", LogLevel.Debug);
            }
            MyLogger.Line();


            BotDiscord.Instance.ClientBot.MessageReceived += HandleCommandAsync;
            BotDiscord.Instance.Command.CommandExecuted += CommandExecuted; ;
        }

        private async Task CommandExecuted(Optional<CommandInfo> arg1, ICommandContext arg2, Discord.Commands.IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case CommandError.UnmetPrecondition:
                        await arg2.Message.ReplyAsync($"{Resources.UNMET_PRECONDITION} {arg3.ErrorReason}", allowedMentions : AllowedMentions.None);
                        break;
                    case CommandError.UnknownCommand:
                        await arg2.Message.ReplyAsync($"{Resources.UNKNOW_COMMAND}", allowedMentions: AllowedMentions.None); ;
                        break;
                    case CommandError.BadArgCount:
                        await arg2.Message.ReplyAsync($"{Resources.BAD_ARGS}", allowedMentions: AllowedMentions.None);
                        break;
                    case CommandError.Exception:
                        await arg2.Message.ReplyAsync($"{Resources.EXCEPTION} {arg3.ErrorReason}", allowedMentions: AllowedMentions.None);
                        break;
                    case CommandError.Unsuccessful:
                        await arg2.Message.ReplyAsync($"{Resources.UNSUCCESSFULL}", allowedMentions: AllowedMentions.None);
                        break;
                    default:
                        await arg2.Message.ReplyAsync($"{Resources.UNKNOW_ERROR} {arg3.Error}", allowedMentions: AllowedMentions.None);
                        break;
                }
            }
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;


            if (message.Channel is SocketGuildChannel guildChannel)
            {
                if(!string.IsNullOrEmpty(message.Content))
                    MyLogger.Log($"[Message/{message.Author.Username}] ==> [Channel/{guildChannel.Name}, Guild/{guildChannel.Guild.Name}] : {message.Content}", LogLevel.Debug);
            }
            else if (message.Channel is SocketDMChannel dmChannel)
            {
                if (!string.IsNullOrEmpty(message.Content))
                    MyLogger.Log($"[Message/{message.Author.Username}] ==> [User/{dmChannel.Recipient.Username}] : {message.Content}", LogLevel.Debug);
            }

            int argPos = 0;

            if (!(message.HasMentionPrefix(BotDiscord.Instance.ClientBot.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(BotDiscord.Instance.ClientBot, message);

            await BotDiscord.Instance.Command.ExecuteAsync(context: context, argPos: argPos, services: null);
        }
    }
}
