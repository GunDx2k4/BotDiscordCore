namespace BotDiscordCore.Core.Services.Interfaces
{
    public interface IBotService
    {
        Task ReadyClientAsync();
        Task ConnectedClientAsync();
        Task DisconnectedClientAsync(Exception e);
    }
}
