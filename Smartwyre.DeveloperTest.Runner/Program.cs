using Azure.Core;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Login;
using Smartwyre.DeveloperTest.Model;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{

    static void Main(string[] args)
    {
        try
        {
            UserLogin userLogin = new();
            bool isLogin = false;

            while (!isLogin)
            {
                Console.WriteLine("Welcome to SmartWyre Rebate Calculation System!");
                Console.WriteLine("1. Sign Up");
                Console.WriteLine("2. Login");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        userLogin.SignUp();
                        break;
                    case "2":
                        isLogin = userLogin.Login();
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }

                Console.WriteLine();
            }

            if (isLogin)
            {
                CalculateRebateRequest crr = new CalculateRebateRequest();
                ProductDataStore pds = new ProductDataStore();
                RebateDataStore rds = new RebateDataStore();
                Product productDetails = pds.PostProduct();
                crr.ProductIdentifier = productDetails.Identifier;
                crr.RebateIdentifier = rds.PostRebate(productDetails);

                Console.Write("Please enter the Volume to calculate Rebate : ");
                crr.Volume = int.Parse(Console.ReadLine());

                IRebateService rebateService = new RebateService();
                CalculateRebateResult result = rebateService.Calculate(crr);
                if (result.Success)
                {
                    Rebate rebate = rds.GetRebate(crr.RebateIdentifier);
                    Product product = pds.GetProduct(crr.ProductIdentifier);
                    Console.WriteLine("Rebate Calculation is Updated");
                    Console.WriteLine();
                    Console.WriteLine("-------------- Details of Product --------------");
                    Console.WriteLine($"Product Identifier: {product.Identifier}");
                    Console.WriteLine($"Product Price: {product.Price}");
                    Console.WriteLine($"Product unit of measurement: {product.Uom}");
                    Console.WriteLine($"Product Incentive: {product.SupportedIncentives}");
                    Console.WriteLine();
                    Console.WriteLine("-------------- Details of Rebate -------------- ");
                    Console.WriteLine($"Rebate Identifier: {rebate.Identifier}");
                    Console.WriteLine($"Updated Rebate Amount of current product: {rebate.Amount}");
                    Console.WriteLine($"Rebate Percentage: {rebate.Percentage}");
                    Console.WriteLine($"Rebate Incentive: {rebate.Incentive}");
                }
                else
                {
                    Console.Write("Rebate Calculation failed due to an issue");
                }
            }
            else
            {
                Console.WriteLine("Please Login First");
                Console.WriteLine();
                Main(args); // Recursive call to repeat the process
            }
        }
        catch
        {
            Console.Write("Calculate Rebate Request is not working");
        }
    }

}
