using BotDiscordCore.Bot;
using BotDiscordCore.Locales;
using BotDiscordCore.Logger;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;

namespace BotDiscordCore.Services
{
    public class SlashCommandServices
    {
        public async Task InitializeAsync()
        {
            await BotDiscord.Instance.Interaction.AddModulesAsync(Assembly.GetEntryAssembly(), BotDiscord.Instance.ServiceProvider);

            foreach (var command in BotDiscord.Instance.Interaction.SlashCommands)
            {
                MyLogger.Log($"Loading Interaction [{command.Name}]", LogLevel.Debug);
            }
            MyLogger.Line();

            BotDiscord.Instance.ClientBot.InteractionCreated += HandleInteraction;
            BotDiscord.Instance.Interaction.SlashCommandExecuted += SlashCommandExecuted;
        }

        private async Task SlashCommandExecuted(SlashCommandInfo arg1, IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        await arg2.Interaction.RespondAsync($"{Resources.UNMET_PRECONDITION} {arg3.ErrorReason}");
                        break;
                    case InteractionCommandError.UnknownCommand:
                        await arg2.Interaction.RespondAsync($"{Resources.UNKNOW_COMMAND}");
                        break;
                    case InteractionCommandError.BadArgs:
                        await arg2.Interaction.RespondAsync($"{Resources.BAD_ARGS}");
                        break;
                    case InteractionCommandError.Exception:
                        await arg2.Interaction.RespondAsync($"{Resources.EXCEPTION} {arg3.ErrorReason}");
                        break;
                    case InteractionCommandError.Unsuccessful:
                        await arg2.Interaction.RespondAsync($"{Resources.UNSUCCESSFULL}");
                        break;
                    default:
                        await arg2.Interaction.RespondAsync($"{Resources.UNKNOW_ERROR} {arg3.Error}");
                        break;
                }
            }
        }


        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                if (arg is SocketMessageComponent messageComponent)
                {

                }
                var ctx = new SocketInteractionContext(BotDiscord.Instance.ClientBot, arg);
                var result = await BotDiscord.Instance.Interaction.ExecuteCommandAsync(ctx, null);
            }
            catch (Exception ex)
            {
                MyLogger.Log($"[Interaction/{arg.User.Username}] {ex.Message} ====> Fail in [Channel/{arg.Channel.Name}]", LogLevel.Error);
                if (arg.Type == InteractionType.ApplicationCommand)
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }
}
