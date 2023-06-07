using Smartwyre.DeveloperTest.Model;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Data.SqlClient;
using System.Runtime.ConstrainedExecution;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore
{

    private readonly IDbConnectionWrapper connectionWrapper = new SqlConnectionWrapper();

    
    // Gets details of Rebate against Idenifier from database 
    public Rebate GetRebate(string rebateIdentifier)
    {
        try
        {
            connectionWrapper.Open();

            string getRebateQuery = "SELECT * FROM Rebate WHERE Identifier = @Identifier";
            SqlCommand getRebateCommand = connectionWrapper.CreateCommand();
            getRebateCommand.CommandText = getRebateQuery;
            getRebateCommand.Parameters.AddWithValue("@Identifier", rebateIdentifier);

            SqlDataReader reader = getRebateCommand.ExecuteReader();

            Rebate rebate = null;

            if (reader.Read())
            {
                rebate = new Rebate();
                rebate.Identifier = reader.GetString(reader.GetOrdinal("Identifier"));
                rebate.Amount = reader.GetDecimal(reader.GetOrdinal("Amount"));

                string incentiveTypeString = reader.GetString(reader.GetOrdinal("IncentiveType"));
                if (Enum.TryParse(incentiveTypeString, out IncentiveType incentiveType))
                {
                    rebate.Incentive = incentiveType;
                }
                else
                {
                    // Handle the case when the value in the database is not a valid IncentiveType
                    // You can assign a default value or take appropriate action.
                    rebate.Incentive = IncentiveType.FixedRateRebate;
                }
                rebate.Percentage = reader.GetDecimal(reader.GetOrdinal("Percentage"));
            }

            reader.Close();
            connectionWrapper.Close();

            return rebate;
        }
        catch
        {
            Console.WriteLine("Error : Product Get Request is not Working");
            Environment.Exit(0);
            return null;
        }
        
    }

    // This method updates the value of Amount in Rebate and replace with rebate amount.  
    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        try
        {
            connectionWrapper.Open();

            string updateRebateQuery = "UPDATE Rebate SET Amount = @RebateAmount WHERE Identifier = @Identifier";
            using (SqlCommand updateRebate = connectionWrapper.CreateCommand())
            {
                updateRebate.CommandText = updateRebateQuery;
                updateRebate.Parameters.AddWithValue("@RebateAmount", rebateAmount);
                updateRebate.Parameters.AddWithValue("@Identifier", account.Identifier);

                updateRebate.ExecuteNonQuery();
            }

            // Additional code for updating account in the database if necessary

            connectionWrapper.Close();
        }
        catch
        {
            Console.Write("Error : Sorry We can't update Rebate Amount");
        }
        
    }

    // This method stores the details of Rebate
    public string PostRebate(Product product)
    {
        try
        {
            //Rebate
            Rebate rebate = GetRebateDetails();

            
            var Incentive = (IncentiveType)product.SupportedIncentives;
            if(Incentive.ToString() == "4")
            {
                rebate.Incentive = IncentiveType.FixedCashAmount;
            }else if(Incentive.ToString() == "1")
            {
                rebate.Incentive= IncentiveType.FixedRateRebate;
            }else if(Incentive.ToString()=="2" || Incentive.ToString()== "FixedCashAmount") {
                rebate.Incentive = IncentiveType.AmountPerUom;
            }

            

            connectionWrapper.Open();
            string insertRebateQuery = "INSERT INTO Rebate (Identifier, Amount, IncentiveType, Percentage) VALUES (@Identifier, @Amount, @IncentiveType, @Percentage)";
            
            using (SqlCommand insertRebateCommand = connectionWrapper.CreateCommand())
            {
                insertRebateCommand.CommandText = insertRebateQuery;
                insertRebateCommand.Parameters.AddWithValue("@Identifier", product.Identifier);
                insertRebateCommand.Parameters.AddWithValue("@Amount", rebate.Amount);
                insertRebateCommand.Parameters.AddWithValue("@IncentiveType", rebate.Incentive);
                insertRebateCommand.Parameters.AddWithValue("@Percentage", rebate.Percentage);
                insertRebateCommand.ExecuteNonQuery();
            }
            Console.WriteLine("Rebate Data Inserted");
            Console.Write("Press anything to proceed...");
            Console.ReadLine();
            Console.Clear();
            connectionWrapper.Close();  
            return product.Identifier;
        }
        catch
        {
            Console.Write("Invalid Input");
            Environment.Exit(0);
            return null;
        }
    }

    // This method get the Details of Rebate from User 
    public Rebate GetRebateDetails()
    {
        try
        {
            Rebate details = new();
            Console.WriteLine("Please Enter Rebate Details:");
            Console.Write("Initial Amount :");
            details.Amount = Decimal.Parse(Console.ReadLine());
            Console.Write("Percentage: ");
            details.Percentage = Decimal.Parse(Console.ReadLine());
            return details;

        }
        catch {
            Console.Write("Error : Invalid Rebate Input");
            Environment.Exit(0);
            return null;
        }
    }
}
