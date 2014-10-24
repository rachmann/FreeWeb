using System.Configuration;

namespace FreeWeb.Logic
{
    public static class AppSettings
    {
        private static string GetSettingOrDefault(string setting, string defaultValue)
        {
            if (string.IsNullOrEmpty(setting))
            {
                return defaultValue;
            }
            else
            {
                return setting;
            }
        }

    }

    public static class ConnectionStrings
    {
        public static readonly ConnectionStringSettings LocalSqlServer = ConfigurationManager.ConnectionStrings["LocalSqlServer"];
        public static readonly ConnectionStringSettings DefaultConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"];
    }
}