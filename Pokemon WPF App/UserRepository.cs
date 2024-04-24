using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_WPF_App
{
    public class UserRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["Pokemon"].ConnectionString;
        public bool RegisterUser(string username, string email, string password)
        {
            string query = "INSERT INTO [User].[Users] (Username, Email, Password) VALUES (@Username, @Email, @Password)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public User AuthenticateUser(string username, string password)
        {
            string query = "SELECT * FROM [User].[Users] WHERE Username = @Username AND Password = @Password";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = reader.GetInt32(0);
                            string email = reader.GetString(2);
                            return new User(userId, username, email);
                        }
                    }
                }
            }
            return null;
        }

        

    }
}
