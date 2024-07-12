namespace BotDiscordCore.Core.Extensions
{
    public static class StringExtension
    {
        public static string ToItalicsDiscord(this string text)
        {
            return $"*{text}*";
        }

        public static string ToUnderlineDiscord(this string text)
        {
            return $"_{text}_";
        }

        public static string ToBoldDiscord(this string text)
        {
            return $"**{text}**";
        }

        public static string ToStrikethroughDiscord(this string text)
        {
            return $"~~{text}~~";
        }

        public static string ToSpoilerhDiscord(this string text)
        {
            return $"|{text}|";
        }

        public static string ToHeadersDiscord(this string text, int level = 1)
        {
            switch (level)
            {
                case 1:
                    return $"# {text}";
                case 2:
                    return $"## {text}";
                case 3:
                    return $"### {text}";
                default:
                    return $"{text}";
            }
        }

        public static string ToMaskedLinksDiscord(this string text, string link)
        {
            return $"[{text}]({link})";
        }

        public static string ToHighlightDiscord(this string text)
        {
            return $"`{text}`";
        }

        public static string ToCodeBlockDiscord(this string text)
        {
            return $"```{text}```";
        }

        public static string ToBlockQuotesDiscord(this string text)
        {
            return $"> {text}";
        }

        public static string ToBlockQuotesAllDiscord(this string text)
        {
            return $">>> {text}";
        }
    }
}
