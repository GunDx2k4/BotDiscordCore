using BotDiscordCore.Core.Extensions;
using BotDiscordCore.Core.Services.Interfaces;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;

namespace BotDiscordCore.Core.Services
{
    public class SlashCommandService : ISlashCommandService
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _services;

        public SlashCommandService(DiscordSocketClient client, InteractionService commands, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                if (interaction is SocketMessageComponent messageComponent)
                {

                }
                var ctx = new SocketInteractionContext(_client, interaction);
                var result = await _commands.ExecuteCommandAsync(ctx, null);
            }
            catch (Exception ex)
            {
                if (interaction.Type == InteractionType.ApplicationCommand)
                    await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }

        public async Task InstallCommandsAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _client.InteractionCreated += HandleInteraction;
            _commands.SlashCommandExecuted += SlashCommandExecuted;
        }

        public async Task SlashCommandExecuted(SlashCommandInfo commandInfo, IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        await context.Interaction.RespondAsync($"Unmet Precondition: {result.ErrorReason.ToBoldDiscord().ToHighlightDiscord()}.");
                        break;
                    case InteractionCommandError.UnknownCommand:
                        await context.Interaction.RespondAsync("Unknown command.");
                        break;
                    case InteractionCommandError.BadArgs:
                        await context.Interaction.RespondAsync("Invalid number or arguments.");
                        break;
                    case InteractionCommandError.Exception:
                        await context.Interaction.RespondAsync($"Command exception: {result.ErrorReason.ToBoldDiscord().ToHighlightDiscord()}");
                        break;
                    case InteractionCommandError.Unsuccessful:
                        await context.Interaction.RespondAsync("Command could not be executed.");
                        break;
                    default:
                        await context.Interaction.RespondAsync($"Unknow error: {result.Error.ToString().ToBoldDiscord().ToHighlightDiscord()}");
                        break;
                }
            }
        }
    }
}
