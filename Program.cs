using System;
using System.Collections.Generic;
using System.Linq;
using RetroSlices.Classes;
using RetroSlices.Static;
using RetroSlices.Methods;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace RetroSlices
{
    /// <summary>
    /// Main program class responsible for handling menu options and customer data.
    /// </summary>
    internal class Program
    {

        /// <summary>
        /// Main entry point of the program.
        /// </summary>
        public static void Main()
        {
            // Specify the path for customer data file
            string filePath = "customers.json";
            var customers = FileService.LoadCustomersFromFile(filePath);

            // Initialize counts for qualified and denied customers
            int qualifiedCount = 0;
            int deniedCount = 0;
            var qualifiedCustomers = CustomerService.CheckQualification(customers, out qualifiedCount, out deniedCount);

            AnsiConsole.Write(
            new FigletText("RetroSlicers")
            .LeftJustified()
            .Color(Color.Orange3));

            // Main menu loop
            while (true)
            {
                var selectedOption = MenuService.GetMenuChoice();

                switch (selectedOption)
                {
                    case Menu.Capture_Customer_Details:
                        Console.Clear();
                        // Capture details and save to file
                        customers.AddRange(CustomerService.CaptureDetails());
                        FileService.SaveCustomersToFile(customers, filePath);
                        break;

                    case Menu.Show_Number_Of_All_Qualified_And_Denied_Applicants:
                        Console.Clear();
                        // Update and display qualified customers
                        AnsiConsole.Write(new BreakdownChart()
                        .Width(60)
                        .AddItem("Qualified Applicants", qualifiedCount, Color.Green)
                        .AddItem("Denied Applicants", deniedCount, Color.Red));
                        qualifiedCustomers = CustomerService.CheckQualification(customers, out qualifiedCount, out deniedCount);
                        Console.WriteLine("Qualified Customers:");
                        foreach (var qualified in qualifiedCustomers)
                        {
                            Console.WriteLine($"qualified: {qualified}");
                        }
                        FileService.SaveCustomersToFile(customers, filePath);
                        break;

                    case Menu.Find_The_Age_Of_Youngest_And_Oldest_Applicant:
                        Console.Clear();
                        // Find and display youngest and oldest applicants
                        var (youngest, oldest) = CustomerService.GetYoungestAndOldestApplicant(customers);
                        Console.WriteLine($"Youngest Applicant: {youngest}");
                        Console.WriteLine($"Oldest Applicant: {oldest}");
                        break;

                    case Menu.Check_Applicant_Long_Term_Loyalty_Award_Qualification:
                        Console.Clear();
                        // Check for long-term loyalty award
                        Console.WriteLine("Enter applicant name to check for loyalty award:");
                        string name = Console.ReadLine();
                        var customerToCheck = customers.FirstOrDefault(c => c.Name == name);
                        if (customerToCheck != null)
                        {
                            bool isLoyal = CustomerService.CheckLongTermLoyaltyAward(customerToCheck);
                            Console.WriteLine(isLoyal ? "Customer qualifies for long-term loyalty award." : "Customer does not qualify for long-term loyalty award.");
                        }
                        else
                        {
                            Console.WriteLine("Customer not found.");
                        }
                        break;

                    case Menu.Display_Customer_Data_Records:
                        Console.Clear();
                        // Display customer report
                        CustomerService.DisplayCustomerReport(customers);
                        break;

                    case Menu.Clear_All_Data_Records:
                        Console.Clear();
                        if (AnsiConsole.Confirm("Are you sure you want to clear all data?"))
                        {
                            customers.Clear();
                            FileService.SaveCustomersToFile(customers, filePath);
                            qualifiedCount = 0;
                            deniedCount = 0;
                            AnsiConsole.MarkupLine("All data has been cleared");
                        } else
                        {
                            AnsiConsole.MarkupLine("Operation cancelled. Data has not been cleared");
                        }

                        break;

                    case Menu.Exit_Program:
                        // Exit the program
                        Console.Clear();
                        if (AnsiConsole.Confirm("Are you sure you want to exit the program?"))
                        {
                            FileService.SaveCustomersToFile(customers, filePath);
                            AnsiConsole.MarkupLine("Exiting Program...");
                        }
                        else
                        {
                            Console.Clear();
                        }
                        return;
                }
            }
        }
    }
}
