using System;
using MySql.Data.MySqlClient;

namespace SchedulingApp.Data
{
    public static class DBConnection
    {
        private const string Server = "localhost";
        private const string Database = "client_schedule";
        private const string UserName = "sqlUser";
        private const string Password = "Passw0rd!";

        private static readonly string ConnectionString =
            $"Server={Server};Port=3306;Database={Database};User ID={UserName};Password={Password};";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB connection failed: " + ex.Message);
                return false;
            }
        }
    }
}