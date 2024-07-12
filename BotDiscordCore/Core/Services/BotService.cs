using BotDiscordCore.Core.Services.Interfaces;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Serilog;

namespace BotDiscordCore.Core.Services
{
    public class BotService : IBotService
    {
        private readonly InteractionService _command;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public BotService(InteractionService command, DiscordSocketClient client, IServiceProvider services)
        {
            _command = command;
            _client = client;
            _services = services;
        }

        public async Task ConnectedClientAsync()
        {
            Log.Information($"{_client.CurrentUser.Username}#{_client.CurrentUser.Discriminator} connected ...");
            await _command.RegisterCommandsGloballyAsync();
            await _client.SetActivityAsync(new Game("Bot Discord.Net Core <2024>"));
        }

        public Task DisconnectedClientAsync(Exception e)
        {
            Log.Warning($"{_client.CurrentUser.Username}#{_client.CurrentUser.Discriminator} disconnected ...", e);
            return Task.CompletedTask;
        }

        public Task ReadyClientAsync()
        {
            var gateway = _client.GetBotGatewayAsync();
            Log.Information($"{_client.CurrentUser.Username}#{_client.CurrentUser.Discriminator} ready ...");
            return Task.CompletedTask;
        }
    }
}
