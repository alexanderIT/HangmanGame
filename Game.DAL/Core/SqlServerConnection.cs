using System.Configuration;

namespace Game.DAL.Core
{
    public static class SqlServerConnection
    {
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["HangmanDB"].ToString();
    }
}
