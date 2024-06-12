using System;
using RetroSlices.Static;
using RetroSlices.Methods;
using Spectre.Console;
using System.Linq;

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

            AnsiConsole.Write(
                new FigletText("RetroSlicers")
                .LeftJustified()
                .Color(Color.Orange3)
            );

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

                    case Menu.Display_Customer_Arcade_And_Bowling_Stats:
                        Console.Clear();
                        AnsiConsole.Markup("[bold blue]Enter customer name to display arcade and bowling stats:[/]\n");
                        string customerName = Console.ReadLine();
                        var (individualArcadeScore, individualBowlingScore, highestBowlingScore, bowlingHighScoreHolder, highestArcadeScore, arcadeHighScoreHolder) = CustomerService.GetHighScores(customers, customerName);

                        AnsiConsole.Markup($"[italic red]Individual Arcade Score for[/] {customerName}: {individualArcadeScore}\n");
                        AnsiConsole.Markup($"[italic red]Individual Bowling Score for[/] {customerName}: {individualBowlingScore}\n");
                        AnsiConsole.Markup($"[italic red]Highest Bowling Score:[/] {highestBowlingScore} held by {bowlingHighScoreHolder}\n");
                        AnsiConsole.Markup($"[italic red]Highest Arcade Score:[/] {highestArcadeScore} held by {arcadeHighScoreHolder}\n");
                        break;

                    case Menu.Show_Number_Of_All_Qualified_And_Denied_Applicants:
                        Console.Clear();
                        var qualificationResult = CustomerService.CheckQualification(customers);

                        AnsiConsole.Markup($"[bold blue]{qualificationResult.QualifiedCount + qualificationResult.DeniedCount} total applicants[/]\n");

                        // Display the total number of qualified and denied applicants
                        AnsiConsole.Write(new BreakdownChart()
                            .Width(60)
                            .AddItem("Qualified Applicants", qualificationResult.QualifiedCount, Color.Green)
                            .AddItem("Denied Applicants", qualificationResult.DeniedCount, Color.Red)
                        );

                        // Display qualified customers
                        var qualifiedTable = new Table();
                        qualifiedTable.AddColumn(new TableColumn("Qualified Customers").Centered());
                        foreach (var qualified in qualificationResult.QualifiedCustomers)
                        {
                            qualifiedTable.AddRow(qualified.Name);
                        }

                        // Display denied customers
                        var deniedTable = new Table();
                        deniedTable.AddColumn(new TableColumn("Denied Customers").Centered());
                        foreach (var denied in qualificationResult.DeniedCustomers)
                        {
                            deniedTable.AddRow(denied.Name);
                        }

                        AnsiConsole.Write(qualifiedTable);
                        AnsiConsole.Write(deniedTable);
                        FileService.SaveCustomersToFile(customers, filePath);
                        break;

                    case Menu.Calculate_Average_Pizzas_Consumed_Per_First_Visit:
                        Console.Clear();
                        // Calculate and display the average pizzas consumed per first visit
                        var averagePizzas = CustomerService.CalculateAveragePizzasConsumed(customers);
                        AnsiConsole.Markup($"[bold blue]Average pizzas consumed per first visit:[/] {averagePizzas:F2}\n");
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

                    case Menu.Remove_Customer_Data_Record:
                        Console.Clear();
                        // Remove a customer data record
                        Console.WriteLine("Enter customer name to remove record:");
                        string removeName = Console.ReadLine();
                        var customerToRemove = customers.FirstOrDefault(c => c.Name == removeName);
                        if (customerToRemove != null)
                        {
                            customers.Remove(customerToRemove);
                            FileService.SaveCustomersToFile(customers, filePath);
                            AnsiConsole.MarkupLine($"Customer record for {removeName} has been removed.");
                        }
                        else
                        {
                            Console.WriteLine("Customer not found.");
                        }
                        break;

                    case Menu.Update_Customer_Data_Record:
                        Console.Clear();
                        // Update a customer data record
                        Console.WriteLine("Enter customer name to update record:");
                        string updateName = Console.ReadLine();
                        var customerToUpdate = customers.FirstOrDefault(c => c.Name == updateName);
                        if (customerToUpdate != null)
                        {
                            CustomerService.UpdateCustomerDetails(customerToUpdate);
                            FileService.SaveCustomersToFile(customers, filePath);
                            AnsiConsole.MarkupLine($"Customer record for {updateName} has been updated.");
                        }
                        else
                        {
                            Console.WriteLine("Customer not found.");
                        }
                        break;

                    case Menu.Clear_All_Data_Records:
                        Console.Clear();
                        if (AnsiConsole.Confirm("Are you sure you want to clear all data?"))
                        {
                            customers.Clear();
                            FileService.SaveCustomersToFile(customers, filePath);
                            AnsiConsole.MarkupLine("All data has been cleared");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("Operation cancelled. Data has not been cleared");
                        }
                        break;

                    case Menu.Exit_Program:
                        Console.Clear();
                        if (AnsiConsole.Confirm("Are you sure you want to exit the program?"))
                        {
                            FileService.SaveCustomersToFile(customers, filePath);
                            AnsiConsole.MarkupLine("Exiting Program...");
                            return;
                        }
                        else
                        {
                            Console.Clear();
                        }
                        break;
                }
            }
        }
    }
}
