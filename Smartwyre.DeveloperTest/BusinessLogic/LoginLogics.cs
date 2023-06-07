using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Model;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.BusinessLogic
{
    public class LoginLogics
    {
        
        public static Subscription Subscription;
        static UserDataStore user = new();
        public bool ValidCredentials(string username,string password)
        {
            User getUser = user.GetUser(username);


            if (getUser != null)
            {
                // Hash the entered password with the stored salt and compare with the stored password hash
                byte[] salt = Convert.FromBase64String(getUser.Salt);
                string enteredPasswordHash = AuthSevice.HashPassword(password, salt);

                if (getUser.PasswordHash == enteredPasswordHash)
                {
                    Console.WriteLine("Login successful! Welcome, " + getUser.Username + ".");
                    Subscription = getUser.Subscription;
                    return true;
                }
                else
                {
                    Console.WriteLine("Invalid username or password!");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Invalid username or password!");
                return false;
            }
        }

        public void SignupLogic(string password,string username, dynamic Subscribed)
        {

            // Generate a salt and hash the password
            byte[] salt = AuthSevice.GenerateSalt();
            string passwordHash = AuthSevice.HashPassword(password, salt);

            // Create a new user object and add it to the list
            User newUser = new() { Username = username, PasswordHash = passwordHash, Salt = Convert.ToBase64String(salt), Subscription = Subscribed };
            user.CreateUser(newUser);
        }

        public bool IsUserExist(string username)
        {
            return user.IsUserExists(username);
        }
    }
}
