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
            ResourceManager resourceManager = new ResourceManager("BotDiscordCore.Properties.Resources", typeof(Program).Assembly);
            string TokenBot = resourceManager.GetString("TokenBot");
            try
            {
                TokenUtils.ValidateToken(TokenType.Bot, TokenBot);
            }
            catch (ArgumentNullException)
            {
                MyLogger.Log($"Token bot is null, empty, or contains only whitespace.", LogLevel.Error);
            }
            catch (ArgumentException)
            {
                MyLogger.Log($"Token bot [{TokenBot}] is invalid", LogLevel.Error);
            }

            Resources.Load();

            await BotDiscord.Instance.ConnectBotAsync(TokenBot);
        }
    }
}
