using BotDiscordCore.Core;
using Serilog;
using System.Text;

namespace BotDiscordCore

{
    internal class Program
    {
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var botDiscord = new BotDiscord();
            Log.Information($"Start ....");
            
            await botDiscord.ConfigureAsync();

            await botDiscord.StartAsync();
        }
    }
}
