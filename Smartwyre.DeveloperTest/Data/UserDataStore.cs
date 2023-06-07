using Smartwyre.DeveloperTest.Model;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Data.SqlClient;


namespace Smartwyre.DeveloperTest.Data
{
    public class UserDataStore
    {
        private readonly IDbConnectionWrapper connectionWrapper = new SqlConnectionWrapper();

        // This method retrieves data against identifier from database 
        public User GetUser(string Username)
        {
            try
            {
                connectionWrapper.Open();

                string selectProductQuery = "SELECT Id, Username, PasswordHash, Salt, Subscription FROM [User] WHERE Username = @Username";

                using (SqlCommand selectCommand = connectionWrapper.CreateCommand())
                {
                    selectCommand.CommandText = selectProductQuery;
                    selectCommand.Parameters.AddWithValue("@Username", Username);

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                Salt = reader.GetString(3),
                                Subscription = (Subscription)Enum.Parse(typeof(Subscription), reader.GetString(4))

                            };
                            return user;
                        }
                    }
                }

                return null;
            }
            catch
            {
                Console.WriteLine("Error: Get Request is not working");
                Environment.Exit(0);
                return null;
            }
            finally
            {
                connectionWrapper.Close();
            }
        }

        // This Method Checks if any User is already Exists or Not and returns a boolean
        public bool IsUserExists(string Username)
        {
            
            bool isExists = false;
            connectionWrapper.Open();
            string getUsers = "SELECT COUNT(*) FROM [User] WHERE Username = @Username";
            using (SqlCommand selectCommand = connectionWrapper.CreateCommand())
            {
                selectCommand.CommandText = getUsers;
                selectCommand.Parameters.AddWithValue("@Username", Username);
                int count = (int)selectCommand.ExecuteScalar();
                isExists = count > 0;
            }
            connectionWrapper.Close();
            return isExists;
        }

        // This method stores data into the database
        public string CreateUser(User user)
        {
            try
            {
                connectionWrapper.Open();
                string insertProductQuery = "INSERT INTO [User] (Username, PasswordHash, Salt ,Subscription) VALUES (@Username, @PasswordHash, @Salt, @Subscription)";
                using (SqlCommand insertCommand = connectionWrapper.CreateCommand())
                {
                    insertCommand.CommandText = insertProductQuery;
                    insertCommand.Parameters.AddWithValue("@Username", user.Username);
                    insertCommand.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    insertCommand.Parameters.AddWithValue("@Salt", user.Salt);
                    insertCommand.Parameters.AddWithValue("@Subscription", user.Subscription);
                    insertCommand.ExecuteNonQuery();
                }
                connectionWrapper.Close();
                return "User is Created Successfully";
            }
            catch
            {
                Console.Write("Error : User can' be created");
                return null;
            }
        }
       
    }
}
