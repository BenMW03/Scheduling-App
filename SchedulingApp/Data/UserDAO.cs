using System;
using MySql.Data.MySqlClient;
using SchedulingApp.Models;

namespace SchedulingApp.Data
{
    public static class UserDAO
    {
        public static User GetUserByCredentials(string username, string password)
        {
            User user = null;

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT userId, userName, password, active " +
                                   "FROM user WHERE userName = @username AND password = @password";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    UserId = reader.GetInt32("userId"),
                                    UserName = reader.GetString("userName"),
                                    Password = reader.GetString("password"),
                                    Active = reader.GetBoolean("active")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user: " + ex.Message);
            }

            return user;
        }
    }
}