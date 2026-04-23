using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace StudentTeacherApp.Data
{
    public static class Database
    {
        private static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        public static MySqlConnection GetConnection()
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            return new MySqlConnection(connectionString);
        }
    }
}
