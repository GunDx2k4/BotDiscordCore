using BotDiscordCore.Bot;
using BotDiscordCore.Locales;
using BotDiscordCore.Logger;
using Discord;
using System.Resources;

namespace BotDiscordCore
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                TokenUtils.ValidateToken(TokenType.Bot, Config.TokenBot);
            }
            catch (ArgumentNullException)
            {
                MyLogger.Log($"Token bot is null, empty, or contains only whitespace.", LogLevel.Error);
                return;
            }
            catch (ArgumentException)
            {
                MyLogger.Log($"Token bot [{Config.TokenBot}] is invalid", LogLevel.Error);
                return;
            }

            Resources.Load();

            await BotDiscord.Instance.ConnectBotAsync(Config.TokenBot);
        }
    }
}
