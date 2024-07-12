using Discord.Interactions;
using Discord;
using Discord.WebSocket;

namespace BotDiscordCore.Core.Services.Interfaces
{
    public interface ISlashCommandService
    {
        Task InstallCommandsAsync();
        Task SlashCommandExecuted(SlashCommandInfo commandInfo, IInteractionContext context, IResult result);
        Task HandleInteraction(SocketInteraction interaction);
    }
}
