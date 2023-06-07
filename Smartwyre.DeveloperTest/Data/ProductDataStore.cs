using Microsoft.Extensions.Caching.Memory;
using Smartwyre.DeveloperTest.BusinessLogic;
using Smartwyre.DeveloperTest.Login;
using Smartwyre.DeveloperTest.Model;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore
{
    private readonly IDbConnectionWrapper connectionWrapper = new SqlConnectionWrapper();
    public static string UserSubscription = ConfigurationManager.AppSettings["Subscription"];

    // This method retrieves data against identifier from database 
    public Product GetProduct(string identifier)
    {
        try
        {
            connectionWrapper.Open();

            string selectProductQuery = "SELECT Identifier, Price, Uom, SupportedIncentive FROM Product WHERE Identifier = @Identifier";

            using (SqlCommand selectCommand = connectionWrapper.CreateCommand())
            {
                selectCommand.CommandText = selectProductQuery;
                selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Product product = new()
                        {
                            Identifier = reader.GetString(0),
                            Price = reader.GetDecimal(1),
                            Uom = reader.GetString(2),
                            SupportedIncentives = (SupportedIncentiveType)Enum.Parse(typeof(SupportedIncentiveType), reader.GetString(3))
                        };
                        return product;
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




    // This method stores data into the database
    public Product PostProduct()
    {
        try
        {
            Product product = GetProductDetails();
            connectionWrapper.Open();
            string insertProductQuery = "INSERT INTO Product (Identifier, Price, Uom, SupportedIncentive) VALUES (@Identifier, @Price, @Uom, @SupportedIncentive)";
            using (SqlCommand insertCommand = connectionWrapper.CreateCommand())
            {
                insertCommand.CommandText = insertProductQuery;
                insertCommand.Parameters.AddWithValue("@Identifier", product.Identifier);
                insertCommand.Parameters.AddWithValue("@Price", product.Price);
                insertCommand.Parameters.AddWithValue("@Uom", product.Uom);
                insertCommand.Parameters.AddWithValue("@SupportedIncentive", product.SupportedIncentives);
                insertCommand.ExecuteNonQuery();
            }

            Console.WriteLine("Product Data Inserted");
            Console.Write("Press anything to proceed...");
            Console.ReadLine();
            Console.Clear();
            connectionWrapper.Close();
            return product;
        }
        catch
        {
            Console.Write("Error : Post request is not working");
            Environment.Exit(0);
            return null;
        }
    }

    // This method takes data from the user through Console
    public static Product GetProductDetails()
    {
        try
        {
            Product details = new();
            Console.WriteLine("Please Enter Product Details:");
            details.Identifier = Guid.NewGuid().ToString();
            Console.Write("Price: ");
            details.Price = int.Parse(Console.ReadLine());
            Console.Write("UOM: ");
            details.Uom = Console.ReadLine();
            dynamic SupportedIncentives;

            if ((Subscription)Convert.ToInt16(LoginLogics.Subscription) == Subscription.Basic)
            {
  
                SupportedIncentives = IncentiveTypesForBasic();
                if(SupportedIncentives == null)
                {
                    return null;  
                }
                    details.SupportedIncentives = SupportedIncentives;
            }
            else if ((Subscription)Convert.ToInt16(LoginLogics.Subscription) == Subscription.Platinum)
            {
                SupportedIncentives = IncentiveTypesForPlatinum();
                if (SupportedIncentives == null)
                {
                    return null;
                }
                    details.SupportedIncentives = SupportedIncentives;
            }
            else if ((Subscription)Convert.ToInt16(LoginLogics.Subscription) == Subscription.Crown)
            {
                SupportedIncentives = IncentiveTypesForCrown();
                if (SupportedIncentives == null)
                {
                    return null;
                }
                    details.SupportedIncentives = SupportedIncentives;
            }
            else
            {
                Console.Write("Invalid Subscription");
                return null; // Or any appropriate value indicating failure
            }

            return details;
        }
        catch
        {
            Console.Write("Error: Invalid Product Input");
            Environment.Exit(0);
            return null;
        }
    }


    static SupportedIncentiveType? IncentiveTypesForBasic()
    {
        Console.WriteLine("Press 1 for Fixed Rate Rebate");
        Console.Write("Press 1 for Selection of Incentive Types: ");
        int select = int.Parse(Console.ReadLine());

        switch (select)
        {
            case 1:
                return SupportedIncentiveType.FixedRateRebate;
            default:
                Console.Write("Invalid Options");
                return null;
        }
    }

    static SupportedIncentiveType? IncentiveTypesForPlatinum()
    {
        Console.WriteLine("Press 1 for Fixed Rate Rebate");
        Console.WriteLine("Press 2 for Amount Per Uom");
        Console.Write("Press 1-2 for Selection of Incentive Types: ");
        int select1 = int.Parse(Console.ReadLine());

        switch (select1)
        {
            case 1:
                return SupportedIncentiveType.FixedRateRebate;
                
            case 2:
                return SupportedIncentiveType.AmountPerUom;
                
            default:
                Console.Write("Invalid Options");
                return null;
        }
    }

    static SupportedIncentiveType? IncentiveTypesForCrown()
    {
        Console.WriteLine("Press 1 for Fixed Rate Rebate");
        Console.WriteLine("Press 2 for Amount Per Uom");
        Console.WriteLine("Press 3 for Fixed Cash Amount");
        Console.Write("Press 1-3 for Selection of Incentive Types: ");
        int select2 = int.Parse(Console.ReadLine());

        switch (select2)
        {
            case 1:
                return SupportedIncentiveType.FixedRateRebate;
                
            case 2:
                return SupportedIncentiveType.AmountPerUom;
            case 3:
                return SupportedIncentiveType.FixedCashAmount;

            default:
                Console.Write("Invalid Options");
                return null;
        }
    }

}
