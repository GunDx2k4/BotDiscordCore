using BotDiscordCore.Core.Services.Interfaces;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using System.Reflection;

namespace BotDiscordCore.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public MessageService(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;


            if (message.Channel is SocketGuildChannel guildChannel)
            {
                Log.Information($"[Message/{message.Author.Username} form Channel/{guildChannel.Name}, Guild/{guildChannel.Guild.Name}] ==> {message.Content}");
            }
            else if (message.Channel is SocketDMChannel dmChannel)
            {
                Log.Information($"[Message/{message.Author.Username} form User/{dmChannel.Recipient.Username}] ==> {message.Content}");
            }

            int argPos = 0;

            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: _services);
        }
    }
}
