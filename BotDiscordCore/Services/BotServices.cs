using BotDiscordCore.Bot;
using BotDiscordCore.Logger;
using Discord;

namespace BotDiscordCore.Services
{
    public class BotServices
    {
        public async Task ReadyClientAsync()
        {
            await BotDiscord.Instance.Interaction.RegisterCommandsGloballyAsync();
            await BotDiscord.Instance.ClientBot.SetActivityAsync(new Game(Config.StatusBot));

        }

        public Task ConnectedClientAsync()
        {
            MyLogger.Log($"[Connected,{BotDiscord.Instance.ClientBot.CurrentUser.Username}] ...");
            return Task.CompletedTask;
        }


        public Task DisconnectedClientAsync(Exception e)
        {
            MyLogger.Log($"[Disconnected/{BotDiscord.Instance.ClientBot.CurrentUser?.Username}] {e}", LogLevel.Error);
            Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}
