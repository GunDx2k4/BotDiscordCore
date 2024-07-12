using Discord.WebSocket;

namespace BotDiscordCore.Core.Services.Interfaces
{
    public interface IMessageService
    {
        Task InstallCommandsAsync();
        Task HandleCommandAsync(SocketMessage messageParam);
    }
}
