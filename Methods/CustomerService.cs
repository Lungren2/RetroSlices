using RetroSlices.Classes;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RetroSlices.Methods
{

    /// <summary>
    /// Provides methods for customer-related operations such as qualification checks, calculations, and reporting.
    /// </summary
    public static class CustomerService
    {
        //Milestone 1
        /// <summary>
        /// This method is used to capture customer data to a collection, which is then written to a JSON object for local persistent storage.
        /// </summary>
        /// <returns>The list of captured customers.</returns>
        public static List<Customer> CaptureDetails()
        {
            var customers = new List<Customer>();
            bool continueCapturing = true;

            while (continueCapturing)
            {
                AnsiConsole.Markup("[blue]Capture Customer Details:[/]" + "\x0A");
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

                var existingCustomer = customers.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

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
                        .AddChoices(new[] { "true", "false" }))
                    );
                Console.WriteLine("Employment Status:", isEmployed);

                string slushPuppyPreference = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Slush Puppy Preference:")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                        .AddChoices(new[] { "Slurp Mix", "Rasberry Red", "Blueberry Blue", "Macha Green", "Strawberry Pink", "Double Orange" }));
                Console.WriteLine("Slush Puppy Preference:", slushPuppyPreference);

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
                //We then add the data object to the Collection if it doesn't already exist in the list
                if (existingCustomer != null)
                {
                    existingCustomer.Age = age;
                    existingCustomer.HighScoreRank = highScoreRank;
                    existingCustomer.StartDate = startDate;
                    existingCustomer.PizzasConsumed = pizzasConsumed;
                    existingCustomer.BowlingHighScore = bowlingHighScore;
                    existingCustomer.IsEmployed = isEmployed;
                    existingCustomer.SlushPuppyPreference = slushPuppyPreference;
                    existingCustomer.SlushPuppiesConsumed = slushPuppiesConsumed;
                }
                else
                {
                    var customer = new Customer(name, age, highScoreRank, startDate, pizzasConsumed, bowlingHighScore, isEmployed, slushPuppyPreference, slushPuppiesConsumed);
                    customers.Add(customer);
                }

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

        /// <summary>
        /// This method is used to update customer data to a collection, which is then written to a JSON object for local persistent storage.
        /// </summary>
        public static void UpdateCustomerDetails(Customer customer)
        {
            // Display current customer details
            Console.WriteLine($"Current details for {customer.Name}:");
            Console.WriteLine($"Name: {customer.Name}");
            Console.WriteLine($"Age: {customer.Age}");
            Console.WriteLine($"HighScore Rank: {customer.HighScoreRank}");
            Console.WriteLine($"Start Date: {customer.StartDate:yyyy-MM-dd}");
            Console.WriteLine($"Pizzas Consumed: {customer.PizzasConsumed}");
            Console.WriteLine($"Bowling High Score: {customer.BowlingHighScore}");
            Console.WriteLine($"Employment Status: {customer.IsEmployed}");
            Console.WriteLine($"Slush Puppy Preference: {customer.SlushPuppyPreference}");
            Console.WriteLine($"Slush Puppies Consumed: {customer.SlushPuppiesConsumed}");
            Console.WriteLine();

            AnsiConsole.Markup("[blue]Update Customer Details:[/]\n");

            Console.WriteLine("Leave fields empty to keep current values.");

            string newName = AnsiConsole.Prompt(new TextPrompt<string>("Customer Name:")
                .AllowEmpty()
                .Validate(customerName =>
                {
                    if (string.IsNullOrEmpty(customerName) || Regex.IsMatch(customerName, "^[A-Za-z\\s]+$"))
                    {
                        return ValidationResult.Success();
                    }
                    else
                    {
                        return ValidationResult.Error("[red]Invalid Name! Try Again[/]");
                    }
                }));

            if (!string.IsNullOrEmpty(newName))
            {
                customer.Name = newName;
            }

            string newAgeInput = AnsiConsole.Prompt(new TextPrompt<string>("Customer Age:")
                .AllowEmpty()
                .Validate(age =>
                {
                    if (string.IsNullOrEmpty(age) || int.TryParse(age, out int parsedAge) && parsedAge > 0)
                    {
                        return ValidationResult.Success();
                    }
                    else
                    {
                        return ValidationResult.Error("[red]Invalid Age! Try Again[/]");
                    }
                }));

            if (int.TryParse(newAgeInput, out int newAge))
            {
                customer.Age = newAge;
            }

            string newHighScoreRankInput = AnsiConsole.Prompt(new TextPrompt<string>("Customer HighScore Rank:")
                .AllowEmpty()
                .Validate(rank =>
                {
                    if (string.IsNullOrEmpty(rank) || int.TryParse(rank, out int parsedRank) && parsedRank > 0)
                    {
                        return ValidationResult.Success();
                    }
                    else
                    {
                        return ValidationResult.Error("[red]Invalid HighScore Rank! Try Again[/]");
                    }
                }));

            if (int.TryParse(newHighScoreRankInput, out int newHighScoreRank))
            {
                customer.HighScoreRank = newHighScoreRank;
            }

            string newStartDateInput = AnsiConsole.Prompt(new TextPrompt<string>("Customer Start Date (YYYY-MM-DD):")
                .AllowEmpty()
                .Validate(startDate =>
                {
                    if (string.IsNullOrEmpty(startDate) || DateTime.TryParse(startDate, out _))
                    {
                        return ValidationResult.Success();
                    }
                    else
                    {
                        return ValidationResult.Error("[red]Invalid Start Date! Try Again[/]");
                    }
                }));

            if (DateTime.TryParse(newStartDateInput, out DateTime newStartDate))
            {
                customer.StartDate = newStartDate;
            }

            string newPizzasConsumedInput = AnsiConsole.Prompt(new TextPrompt<string>("Pizzas Consumed:")
                .AllowEmpty()
                .Validate(pizzas =>
                {
                    if (string.IsNullOrEmpty(pizzas) || int.TryParse(pizzas, out int parsedPizzas) && parsedPizzas >= 0)
                    {
                        return ValidationResult.Success();
                    }
                    else
                    {
                        return ValidationResult.Error("[red]Invalid Number of Pizzas Consumed! Try Again[/]");
                    }
                }));

            if (int.TryParse(newPizzasConsumedInput, out int newPizzasConsumed))
            {
                customer.PizzasConsumed = newPizzasConsumed;
            }

            string newBowlingHighScoreInput = AnsiConsole.Prompt(new TextPrompt<string>("Customer Bowling Score History:")
                .AllowEmpty()
                .Validate(bowlingScore =>
                {
                    if (string.IsNullOrEmpty(bowlingScore) || int.TryParse(bowlingScore, out int parsedBowlingScore) && parsedBowlingScore >= 0)
                    {
                        return ValidationResult.Success();
                    }
                    else
                    {
                        return ValidationResult.Error("[red]Invalid Bowling Score History! Try Again[/]");
                    }
                }));

            if (int.TryParse(newBowlingHighScoreInput, out int newBowlingHighScore))
            {
                customer.BowlingHighScore = newBowlingHighScore;
            }

            bool newIsEmployed = bool.Parse(
                AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Are you employed?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(new[] { "true", "false" }))
                );

            customer.IsEmployed = newIsEmployed;

            string newSlushPuppyPreference = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Slush Puppy Preference:")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(new[] { "Slurp Mix", "Rasberry Red", "Blueberry Blue", "Macha Green", "Strawberry Pink", "Double Orange" }));

            customer.SlushPuppyPreference = newSlushPuppyPreference;

            string newSlushPuppiesConsumedInput = AnsiConsole.Prompt(new TextPrompt<string>("Slushies Consumed:")
                .AllowEmpty()
                .Validate(slushies =>
                {
                    if (string.IsNullOrEmpty(slushies) || int.TryParse(slushies, out int parsedSlushies) && parsedSlushies >= 0)
                    {
                        return ValidationResult.Success();
                    }
                    else
                    {
                        return ValidationResult.Error("[red]Invalid Number of Slushies Consumed! Try Again[/]");
                    }
                }));

            if (int.TryParse(newSlushPuppiesConsumedInput, out int newSlushPuppiesConsumed))
            {
                customer.SlushPuppiesConsumed = newSlushPuppiesConsumed;
            }

            Console.WriteLine("Customer details have been updated.");
        }


        /// <summary>
        /// Retrieves the highest scores and their holders for a given customer name from a list of customers.
        /// </summary>
        /// <param name="customers">The list of customers to search.</param>
        /// <param name="name">The name of the customer to search for.</param>
        /// <returns>A tuple containing the highest bowling score, the holder of the highest bowling score, the highest arcade score, and the holder of the highest arcade score.</returns>
        //TODO Seperate into it's own class
        public static (int IndividualArcadeScore, int IndividualBowlingScore, int BowlingHighScore, string BowlingHighScoreHolder, int ArcadeHighScore, string ArcadeHighScoreHolder) GetHighScores(List<Customer> customers, string name)
        {
            // Initialize variables to track highest scores and their holders
            int highestBowlingScore = 0;
            string bowlingHighScoreHolder = null;
            int highestArcadeScore = 0;
            string arcadeHighScoreHolder = null;

            int IndividualBowlingScore = 0;
            int IndividualArcadeScore = 0;

            // Loop through customers to find matching name and update highest scores
            foreach (var customer in customers)
            {

                if (customer.BowlingHighScore > highestBowlingScore)
                {
                    highestBowlingScore = customer.BowlingHighScore;
                    bowlingHighScoreHolder = customer.Name;
                }

                if (customer.HighScoreRank > highestArcadeScore)
                {
                    highestArcadeScore = customer.HighScoreRank;
                    arcadeHighScoreHolder = customer.Name;
                }

                if (customer.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    IndividualBowlingScore = customer.BowlingHighScore;
                    IndividualArcadeScore = customer.HighScoreRank;
                }

            }

            // Return the highest scores and their holders
            return (IndividualArcadeScore, IndividualBowlingScore, highestBowlingScore, bowlingHighScoreHolder, highestArcadeScore, arcadeHighScoreHolder);
        }


        /// <summary>
        /// Checks the qualification of customers based on various criteria.
        /// </summary>
        /// <param name="customers">The list of customers to check.</param>
        /// <param name="qualifiedCount">Output parameter to store the count of qualified customers.</param>
        /// <param name="deniedCount">Output parameter to store the count of denied customers.</param>
        /// <returns>The list of qualified customers.</returns>
        public static QualificationResult CheckQualification(List<Customer> customers)
        {
            var qualifiedCustomers = new List<Customer>();
            var deniedCustomers = new List<Customer>();

            foreach (var customer in customers)
            {
                ///<summary>
                ///The state of isQualified is true, however if any of the below conditions are truthy it will be set to false, denying the customer
                ///</summary>
                bool isQualified = true;

                if (customer.Age < 18 && !customer.IsEmployed)
                {
                    isQualified = false;
                }

                if ((DateTime.Now - customer.StartDate).TotalDays < 730)
                {
                    isQualified = false;
                }

                if (customer.HighScoreRank <= 2000 && customer.BowlingHighScore <= 1500 && (customer.HighScoreRank + customer.BowlingHighScore) / 2 <= 1200)
                {
                    isQualified = false;
                }

                if ((customer.PizzasConsumed / ((DateTime.Now - customer.StartDate).TotalDays / 30)) < 3)
                {
                    isQualified = false;
                }

                if ((customer.SlushPuppiesConsumed / ((DateTime.Now - customer.StartDate).TotalDays / 30)) <= 4 || customer.SlushPuppyPreference == "Gooey Gulp Galore")
                {
                    isQualified = false;
                }

                ///<summary>
                ///Checks to see if the customer is qualified, if so increment qualifiedCount and add the customers data to the collection "qualifiedCustomers" or else increment deniedCound
                ///</summary>
                if (isQualified)
                {
                    qualifiedCustomers.Add(customer);
                }
                else
                {
                    deniedCustomers.Add(customer);
                }

            }

            return new QualificationResult
            {
                QualifiedCustomers = qualifiedCustomers,
                DeniedCustomers = deniedCustomers,
                QualifiedCount = qualifiedCustomers.Count,
                DeniedCount = deniedCustomers.Count
            };
        }

        /// <summary>
        /// Calculates the average number of pizzas consumed by customers.
        /// </summary>
        /// <param name="customers">The list of customers to calculate the average for.</param>
        /// <returns>The average number of pizzas consumed.</returns>
        public static double CalculateAveragePizzasConsumed(List<Customer> customers)
        {
            if (customers.Count == 0) return 0;

            double totalPizzas = customers.Sum(customer => customer.PizzasConsumed);
            double averagePizzas = totalPizzas / customers.Count;

            return averagePizzas;
        }

        //Milestone 2
        /// <summary>
        /// Retrieves the ages of the youngest and oldest customers in the list.
        /// </summary>
        /// <param name="customers">The list of customers to analyze.</param>
        /// <returns>A tuple containing the ages of the youngest and oldest customers.</returns>
        public static (int, int) GetYoungestAndOldestApplicant(List<Customer> customers)
        {
            // Check if the customers list is empty
            // If it is, throw an InvalidOperationException
            if (customers.Count == 0) throw new InvalidOperationException("No customers in the list.");

            // Initialize variables to hold the ages of the youngest and oldest customers
            // Set youngest to the maximum possible integer value
            // Set oldest to the minimum possible integer value
            int youngest = int.MaxValue;
            int oldest = int.MinValue;

            // Iterate through each customer in the list
            foreach (var customer in customers)
            {
                // Update youngest if the current customer's age is less than the current youngest
                if (customer.Age < youngest) youngest = customer.Age;

                // Update oldest if the current customer's age is greater than the current oldest
                if (customer.Age > oldest) oldest = customer.Age;
            }

            // Return a tuple containing the ages of the youngest and oldest customers
            return (youngest, oldest);
        }

        /// <summary>
        /// Checks if a customer qualifies for a long-term loyalty award.
        /// </summary>
        /// <param name="customer">The customer to check.</param>
        /// <returns>True if the customer qualifies for the award, otherwise false.</returns>
        public static bool CheckLongTermLoyaltyAward(Customer customer)
        {
            //Check if the customer has been a member for at least 3650 days (10 years in days)
            return (DateTime.Now - customer.StartDate).TotalDays >= 3650;
        }


        //Additional Functionality
        /// <summary>
        /// Displays a customer report in the console.
        /// </summary>
        /// <param name="customers">The list of customers to include in the report.</param>
        public static void DisplayCustomerReport(List<Customer> customers)
        {
            // Create a list to store customer data
            List<Customer> customerDataList = new List<Customer>();

            // Populate customer data list
            foreach (var customer in customers)
            {
                Customer customerData = new Customer(
                    customer.Name,
                     customer.Age,
                    customer.HighScoreRank,
                    customer.StartDate,
                    customer.PizzasConsumed,
                    customer.BowlingHighScore,
                    customer.IsEmployed,
                    customer.SlushPuppyPreference,
                    customer.SlushPuppiesConsumed
                 );



                customerDataList.Add(customerData);
            }

            // Create a table
            var table = new Table();

            // Add columns to the table
            table.HeavyBorder()
             .AddColumn(new TableColumn("Name").Centered())
            .AddColumn(new TableColumn("Age").Centered())
            .AddColumn(new TableColumn("High Score Rank").Centered())
            .AddColumn(new TableColumn("Start Date").Centered())
            .AddColumn(new TableColumn("Pizzas Consumed").Centered())
            .AddColumn(new TableColumn("Bowling High Score").Centered())
            .AddColumn(new TableColumn("Employment Status").Centered())
            .AddColumn(new TableColumn("Slush Puppy Preference").Centered())
            .AddColumn(new TableColumn("Slush Puppies Consumed").Centered());

            // Add rows to the table
            foreach (var customerData in customerDataList)
            {
                table.AddRow(
                    customerData.Name.ToString(),
                    customerData.Age.ToString(),
                    customerData.HighScoreRank.ToString(),
                    customerData.StartDate.ToShortDateString(),
                    customerData.PizzasConsumed.ToString(),
                    customerData.BowlingHighScore.ToString(),
                    customerData.IsEmployed ? "Employed" : "Unemployed",
                    customerData.SlushPuppyPreference.ToString(),
                    customerData.SlushPuppiesConsumed.ToString()
                );
            }

            // Render the table to the console
            AnsiConsole.Write(table);
        }

    }
}
