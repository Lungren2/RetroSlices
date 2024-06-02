using System;
using System.Collections.Generic;
using System.Linq;
using RetroSlices.Classes;
using RetroSlices.Static;
using RetroSlices.Methods;

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
                Console.WriteLine("Enter Customer Details:");
                Console.Write("Name: ");
                string name = Console.ReadLine();
                Console.Write("Age: ");
                int age = int.Parse(Console.ReadLine());
                Console.Write("High Score Rank: ");
                int highScoreRank = int.Parse(Console.ReadLine());
                Console.Write("Start Date (yyyy-mm-dd): ");
                DateTime startDate = DateTime.Parse(Console.ReadLine());
                Console.Write("Pizzas Consumed: ");
                int pizzasConsumed = int.Parse(Console.ReadLine());
                Console.Write("Bowling High Score: ");
                int bowlingHighScore = int.Parse(Console.ReadLine());
                Console.Write("Employed (true/false): ");
                bool isEmployed = bool.Parse(Console.ReadLine());
                Console.Write("Slush Puppy Preference: ");
                string slushPuppyPreference = Console.ReadLine();
                Console.Write("Slush Puppies Consumed: ");
                int slushPuppiesConsumed = int.Parse(Console.ReadLine());

                //Here we take the input from the user above and save the data object using the Customer class as it's schema
                //We then add the data object to the Collection
                var customer = new Customer(name, age, highScoreRank, startDate, pizzasConsumed, bowlingHighScore, isEmployed, slushPuppyPreference, slushPuppiesConsumed);
                customers.Add(customer);

                //We prompt the user to capture more, if Y then continueCapturing stays true and the while loop runs once more
                Console.Write("Do you want to capture more applicants? (Y/N): ");
                string response = Console.ReadLine();
                continueCapturing = response.Equals("Y", StringComparison.OrdinalIgnoreCase);
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
                    case Menu.CaptureDetails:
                        //Here we use the AddRange method to add the return value of Capture Details to the Collection then use the SaveCustomersToFile() method to save the collection to our JSON object
                        customers.AddRange(CaptureDetails());
                        FileService.SaveCustomersToFile(customers, filePath);
                        break;

                    case Menu.CheckGameTokenCreditQualification:
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

                    case Menu.ShowCurrentStats:
                        DisplayStats(qualifiedCount, deniedCount);
                        break;

                    case Menu.CalculateAveragePizzasConsumed:
                        double averagePizzas = CustomerService.CalculateAveragePizzasConsumed(customers);
                        Console.WriteLine($"Average Pizzas Consumed per First Visit: {averagePizzas}");
                        break;

                    case Menu.FindYoungestAndOldestApplicant:
                        var (youngest, oldest) = CustomerService.GetYoungestAndOldestApplicant(customers);
                        Console.WriteLine($"Youngest Applicant: {youngest}");
                        Console.WriteLine($"Oldest Applicant: {oldest}");
                        break;

                    case Menu.CheckLongTermLoyaltyAward:
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

                    case Menu.DisplayCustomerReport:
                        //Displays the data of all the customers
                        CustomerService.DisplayCustomerReport(customers);
                        break;

                    case Menu.ClearAllData:
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
