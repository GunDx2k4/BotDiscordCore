using BotDiscordCore.Services;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

namespace BotDiscordCore.Bot
{
    public class BotDiscord
    {
        private static BotDiscord _instance;
        public static BotDiscord Instance => _instance ??= new BotDiscord();


        public DiscordSocketClient ClientBot {  get; private set; }

        public IServiceProvider ServiceProvider {  get; private set; }

        public SlashCommandServices InteractionHandler { get; private set; }

        public InteractionService Interaction { get; private set; }

        public MessageServices CommandHandler { get; private set; }

        public CommandService Command { get; private set; }

        public BotServices BotHandler { get; private set; }

        public UserService UserHandler { get; private set; }



        private DiscordSocketClient CreateBot()
        {
            var ClientBot = new DiscordSocketClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                MessageCacheSize = 100,
                GatewayIntents = GatewayIntents.All
            });

            Interaction = new InteractionService(ClientBot);
            InteractionHandler = new SlashCommandServices();

            Command = new CommandService();
            CommandHandler = new MessageServices();

            BotHandler = new BotServices();
            UserHandler = new UserService();

            ClientBot.Connected += BotHandler.ConnectedClientAsync;
            ClientBot.Disconnected += BotHandler.DisconnectedClientAsync;
            ClientBot.Ready += BotHandler.ReadyClientAsync;

            ClientBot.UserJoined += UserHandler.WelcomeNewUserAsync;

            return ClientBot;
        }

        public async Task ConnectBotAsync(string tokenBot)
        {
            ClientBot ??= CreateBot();

            await CommandHandler.InstallCommandsAsync();
            await InteractionHandler.InitializeAsync();

            await ClientBot.LoginAsync(TokenType.Bot, tokenBot);
            await ClientBot.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }
    }
}
