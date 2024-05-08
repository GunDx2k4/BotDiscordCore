namespace BotDiscordCore.Locales
{
    public class Resources
    {
        public static string UNMET_PRECONDITION = string.Empty;
        public static string UNKNOW_COMMAND = string.Empty;
        public static string BAD_ARGS = string.Empty;
        public static string EXCEPTION = string.Empty;
        public static string UNSUCCESSFULL = string.Empty;
        public static string UNKNOW_ERROR = string.Empty;
        public static string NOT_GUILD_ERROR_MESSAGE = string.Empty;
        public static string NOT_PERMISSION_USER = string.Empty;
        public static string NOT_PERMISSION_BOT = string.Empty;
        public static string USER_NOT_PERMISSION_CHANNEL = string.Empty;
        public static string BOT_NOT_PERMISSION_CHANNEL = string.Empty;

        public static void Load()
        {
            Vietnamese.Load();
        }
    }
}
