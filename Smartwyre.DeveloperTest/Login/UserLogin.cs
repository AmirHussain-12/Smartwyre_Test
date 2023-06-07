using Smartwyre.DeveloperTest.BusinessLogic;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Model;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Security.Cryptography;

namespace Smartwyre.DeveloperTest.Login
{
    public class UserLogin
    {
        public static string Username;
        LoginLogics loginLogics = new();
        public void SignUp()
        {
            Console.WriteLine("=== Sign Up ===");
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            // Check if the username already exists
            if (loginLogics.IsUserExist(username))
            {
                Console.WriteLine("Username already taken! Please choose a different username.");
                return;
            }

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            Console.WriteLine("Choose your subscription:");
            Console.WriteLine("1. Basic");
            Console.WriteLine("2. Platinum");
            Console.WriteLine("3. Crown");

            int choice = int.Parse(Console.ReadLine());
            dynamic Subscribed;
            switch (choice)
            {
                case 1:

                    Console.WriteLine("You selected Basic subscription.");
                    Subscribed = Subscription.Basic;
                    break;

                case 2:
                    Console.WriteLine("You selected Platinum subscription.");
                    Subscribed = Subscription.Platinum;
                    break;

                case 3:
                    Console.WriteLine("You selected Crown subscription.");
                    Subscribed = Subscription.Crown;
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    return;
            }
            loginLogics.SignupLogic(password, username, Subscribed);
                
            Console.WriteLine("Sign up successful! Please login to continue.");
        }

        public bool Login()
        {
            Console.WriteLine("=== Login ===");
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = AuthSevice.ReadPassword();

            // Find the user with the given username
            

            bool valid = loginLogics.ValidCredentials(username,password);
               
            return valid;
        }

        
    }
}
