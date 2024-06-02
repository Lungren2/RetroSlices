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
    internal class Program
    {

        //This method displays the numebr of qualified/denied applicants counted in the app so far
        public static void DisplayStats(int qualifiedCount, int deniedCount)
        {
            Console.WriteLine("Token Qualification Stats:");
            Console.WriteLine($"Qualified Applicants: {qualifiedCount}");
            Console.WriteLine($"Denied Applicants: {deniedCount}");
        }

        //This method is used to capture our customer data to a Collection which is then written to a JSON object for local persistent storage
        public static List<Customer> CaptureDetails()
        {
            var customers = new List<Customer>();
            bool continueCapturing = true;

            while (continueCapturing)
            {
                Console.WriteLine("Capture Customer Details:");
                string name = AnsiConsole.Prompt(new TextPrompt<string>("Customer Name:")
                    .Validate(customerName =>
                    {
                        // Check if the name is not empty and does not contain numbers or punctuation
                        if (!string.IsNullOrEmpty(customerName) && Regex.IsMatch(customerName, "^[A-Za-z\\s]+$"))
                        {
                            return ValidationResult.Success();
                        }
                        else
                        {
                            return ValidationResult.Error("[red]Invalid Name! Try Again[/]");
                        }
                    }));

                int age = AnsiConsole.Prompt(new TextPrompt<int>("Customer Age:")
                    .Validate(customerAge =>
                    {
                        if (customerAge > 0 && !(customerAge < 0))
                        {
                            return ValidationResult.Success();
                        }
                        else
                        {
                            return ValidationResult.Error("[red]Invalid Age! Try Again[/]");
                        }
                    }));

                int highScoreRank = AnsiConsole.Prompt(new TextPrompt<int>("Customer HighScore Rank:")
                    .Validate(customerHighScoreRank =>
                    {
                        if (customerHighScoreRank > 0 && !(customerHighScoreRank < 0))
                        {
                            return ValidationResult.Success();
                        }
                        else
                        {
                            return ValidationResult.Error("[red]Invalid HighScore Rank! Try Again[/]");
                        }
                    }));

                DateTime startDate = DateTime.Parse(
                    AnsiConsole.Prompt(new TextPrompt<string>("Customer Start Date (YYYY-MM-DD):")
                    .Validate(customerStartDate =>
                    {
                        // Check if the name is not empty and does not contain numbers or punctuation
                        if (!string.IsNullOrEmpty(customerStartDate) && Regex.IsMatch(customerStartDate, @"^\d{4}-\d{2}-\d{2}$"))
                        {
                            return ValidationResult.Success();
                        }
                        else
                        {
                            return ValidationResult.Error("[red]Invalid Start Date! Try Again[/]");
                        }
                    }))
                    );

                int pizzasConsumed = AnsiConsole.Prompt(new TextPrompt<int>("Pizzas Consumed:")
                    .Validate(customerPizzasConsumed =>
                    {
                        if (customerPizzasConsumed >= 0 && !(customerPizzasConsumed < 0))
                        {
                            return ValidationResult.Success();
                        }
                        else
                        {
                            return ValidationResult.Error("[red]Invalid Number of Pizzas Consumed! Try Again[/]");
                        }
                    }));

                int bowlingHighScore = AnsiConsole.Prompt(new TextPrompt<int>("Customer Bowling Score History:")
                    .Validate(customerBowlingScore =>
                    {
                        if (customerBowlingScore > 0 && !(customerBowlingScore < 0))
                        {
                            return ValidationResult.Success();
                        }
                        else
                        {
                            return ValidationResult.Error("[red]Invalid Bowling Score History! Try Again[/]");
                        }
                    }));

                bool isEmployed = bool.Parse(
                    AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Are you employed?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                        .AddChoices(new[] {"true", "false"}))
                    );
                Console.WriteLine("Employment Status:",  isEmployed);

                string slushPuppyPreference = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Slush Puppy Preference:")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                        .AddChoices(new[] { "Slurp Mix", "Rasberry Red", "Blueberry Blue", "Macha Green", "Strawberry Pink", "Double Orange" }));
                Console.WriteLine("Slush Puppy Preference:",  slushPuppyPreference);

                int slushPuppiesConsumed = AnsiConsole.Prompt(new TextPrompt<int>("Slushies Consumed:")
                    .Validate(customerSlushiesConsumed =>
                    {
                        if (customerSlushiesConsumed >= 0 && !(customerSlushiesConsumed < 0))
                        {
                            return ValidationResult.Success();
                        }
                        else
                        {
                            return ValidationResult.Error("[red]Invalid Number of Slushies Consumed! Try Again[/]");
                        }
                    }));

                //Here we take the input from the user above and save the data object using the Customer class as it's schema
                //We then add the data object to the Collection
                var customer = new Customer(name, age, highScoreRank, startDate, pizzasConsumed, bowlingHighScore, isEmployed, slushPuppyPreference, slushPuppiesConsumed);
                customers.Add(customer);

                //We prompt the user to capture more, if Y then continueCapturing stays true and the while loop runs once more
                Console.Write("Do you want to capture more applicants? (Y/N): ");
                string response = Console.ReadLine();
                continueCapturing = response.Equals("Y", StringComparison.OrdinalIgnoreCase);
                if (continueCapturing == false)
                {
                    Console.Clear();
                }
            }

            return customers;
        }

        public static void Main()
        {
            //This code specifies the path the customer data file will be saved to and initializes customers which in this case is the Collection we wrote to the file
            string filePath = "customers.json";
            var customers = FileService.LoadCustomersFromFile(filePath);

            //Here we initialize the count of those who are qualified or not
            //The qualifiedCustomers variable gets the customers from the JSON file where we store our customer data and checks if the currently saved customers are qualified
            int qualifiedCount = 0;
            int deniedCount = 0;
            var qualifiedCustomers = CustomerService.CheckQualification(customers, out qualifiedCount, out deniedCount);

            //This while loop will essentially run forever since it is set to true
            //This is necessary as when we complete our current task we want to revert back to the main menu to begin a new one
            while (true)
            {

                var selectedOption = MenuService.GetMenuChoice();

                //This switch statement takes in the user's selected option
                //In the case that the user selects Capture Details (number 1) that case will run, as with all the other commands 1-9
                //The "break;" at the end of the case indicates that the switch statement will not continue executing other cases once it returns truthy on that one
                switch (selectedOption)
                {
                    case Menu.Capture_Details:
                        Console.Clear();
                        //Here we use the AddRange method to add the return value of Capture Details to the Collection then use the SaveCustomersToFile() method to save the collection to our JSON object
                        customers.AddRange(CaptureDetails());
                        FileService.SaveCustomersToFile(customers, filePath);
                        break;

                    case Menu.Check_Game_Token_Credit_Qualification:
                        Console.Clear();
                        //Here we re-assign qualifiedCustomers in order to update it's evaluation since the first time it was ran on application mount
                        qualifiedCustomers = CustomerService.CheckQualification(customers, out qualifiedCount, out deniedCount);
                        //Using the foreach below we indicate which customers are qualified
                        Console.WriteLine("Qualified Customers:");
                        foreach (var customer in qualifiedCustomers)
                        {
                            Console.WriteLine($"Name: {customer.Name}");
                        }
                        FileService.SaveCustomersToFile(customers, filePath);
                        break;

                    case Menu.Show_Current_Stats:
                        Console.Clear();
                        DisplayStats(qualifiedCount, deniedCount);
                        break;

                    case Menu.Calculate_Average_Pizzas_Consumed:
                        Console.Clear();
                        double averagePizzas = CustomerService.CalculateAveragePizzasConsumed(customers);
                        Console.WriteLine($"Average Pizzas Consumed per First Visit: {averagePizzas}");
                        break;

                    case Menu.Find_Youngest_And_Oldest_Applicant:
                        Console.Clear();
                        var (youngest, oldest) = CustomerService.GetYoungestAndOldestApplicant(customers);
                        Console.WriteLine($"Youngest Applicant: {youngest}");
                        Console.WriteLine($"Oldest Applicant: {oldest}");
                        break;

                    case Menu.Check_Long_Term_Loyalty_Award:
                        Console.Clear();
                        Console.WriteLine("Enter applicant name to check for loyalty award:");
                        string name = Console.ReadLine();
                        //customerToCheck is set to the first customer with the name specified
                        var customerToCheck = customers.FirstOrDefault(c => c.Name == name);
                        if (customerToCheck != null)
                        {
                            //Check if the customer qualifies for the long term loyalty award and print a response accordingly
                            bool isLoyal = CustomerService.CheckLongTermLoyaltyAward(customerToCheck);
                            Console.WriteLine(isLoyal ? "Customer qualifies for long-term loyalty award." : "Customer does not qualify for long-term loyalty award.");
                        }
                        //If there isn't a match then print an error message
                        else
                        {
                            Console.WriteLine("Customer not found.");
                        }
                        break;

                    case Menu.Display_Customer_Report:
                        Console.Clear();
                        //Displays the data of all the customers
                        CustomerService.DisplayCustomerReport(customers);
                        break;

                    case Menu.Clear_All_Data:
                        Console.Clear();
                        Console.WriteLine("Are you sure you want to clear all data? (Y/N): ");
                        string response = Console.ReadLine();
                        if (response.Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            //Clears all data from the customer data JSON file
                            customers.Clear();
                            FileService.SaveCustomersToFile(customers, filePath);
                            qualifiedCount = 0;
                            deniedCount = 0;
                            Console.WriteLine("All data has been cleared.");
                        }
                        else
                        {
                            Console.WriteLine("Operation cancelled. Data has not been cleared.");
                        }
                        break;

                    case Menu.Exit:
                        Console.WriteLine("Exiting the program.");
                        FileService.SaveCustomersToFile(customers, filePath);
                        return;
                }
            }
        }
    }
}
