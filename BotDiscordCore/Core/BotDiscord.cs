﻿using BotDiscordCore.Core.Services;
using BotDiscordCore.Core.Services.Interfaces;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;

namespace BotDiscordCore.Core
{
    public class BotDiscord
    {
        private ConfigBot _configBot { get; set; }
        private DiscordSocketClient _client {  get; set; }
        private IServiceProvider _services { get; set; }
        private IServiceCollection _serviceCollection { get; set; }
        private IBotService _botService { get; set; }
        private IMessageService _messageService { get; set; }
        private ISlashCommandService _slashCommandService { get; set; }

        public BotDiscord() 
        { 
            InitConfig();
            InitServices();
        }

        private void InitConfig()
        {
            try
            {
                var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                var json = File.ReadAllText("config.json");
                _configBot = JsonConvert.DeserializeObject<ConfigBot>(json);
            }
            catch (Exception)
            {
                throw;
            }

            Log.Information("Init completed config.");
        }

        private void InitServices()
        {
            var config = new DiscordSocketConfig()
            {
                GatewayIntents = _configBot.GatewayIntents,
                MessageCacheSize = _configBot.MessageCacheSize,
                AlwaysDownloadUsers = _configBot.AlwaysDownloadUsers,
            };
            _serviceCollection = new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<IBotService,BotService>()
                .AddSingleton<CommandService>()
                .AddSingleton(provider => new InteractionService(provider.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<IMessageService, MessageService>()
                .AddSingleton<ISlashCommandService, SlashCommandService>()
                .AddSingleton(config);
            Log.Information("Init completed services.");
        }

        private async Task InstallCommandAsync()
        {
            await _messageService.InstallCommandsAsync();
            await _slashCommandService.InstallCommandsAsync();
            Log.Information("Install command completed.");
        }

        public void AddMessageService<T>() where T : class, IMessageService
        {
            _serviceCollection.AddSingleton<IMessageService,T>();
            Log.Information($"Add new MessageService {typeof(T).Name}");
        }

        public void AddBotService<T>() where T : class, IBotService
        {
            _serviceCollection.AddSingleton<IBotService, T>();
            Log.Information($"Add new BotService {typeof(T).Name}");
        }

        public void AddSlashCommandService<T>() where T : class, ISlashCommandService
        {
            _serviceCollection.AddSingleton<ISlashCommandService, T>();
            Log.Information($"Add new CommandService {typeof(T).Name}");
        }

        public async Task ConfigureAsync()
        {
            _services = _serviceCollection.BuildServiceProvider();

            _client = _services.GetRequiredService<DiscordSocketClient>();
            _botService = _services.GetRequiredService<IBotService>();
            _slashCommandService = _services.GetRequiredService<ISlashCommandService>();
            _messageService = _services.GetRequiredService<IMessageService>();

            _client.Ready += _botService.ReadyClientAsync;
            _client.Connected += _botService.ConnectedClientAsync;
            _client.Disconnected += _botService.DisconnectedClientAsync;
            _client.JoinedGuild += _botService.JoinedGuildAsync;
            _client.LeftGuild += _botService.LeftGuildAsync;

            await InstallCommandAsync();
            Log.Information("Load config completed.");
        }

        public async Task StartAsync()
        {
            try
            {
                TokenUtils.ValidateToken(TokenType.Bot, _configBot.TokenBot);
            }
            catch (ArgumentException)
            {
                Log.Error("Error token bot is invalid !");
                return;
            }

            await _client.LoginAsync(TokenType.Bot, _configBot.TokenBot);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }
    }
}
