using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RetroSlices.Classes;
using RetroSlices.Static;
using RetroSlices.Methods;

namespace RetroSlices
{
    internal class Program
    {

        public static void DisplayStats(int qualifiedCount, int deniedCount)
        {
            Console.WriteLine("Token Qualification Stats:");
            Console.WriteLine($"Qualified Applicants: {qualifiedCount}");
            Console.WriteLine($"Denied Applicants: {deniedCount}");
        }

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

                var customer = new Customer(name, age, highScoreRank, startDate, pizzasConsumed, bowlingHighScore, isEmployed, slushPuppyPreference, slushPuppiesConsumed);
                customers.Add(customer);

                Console.Write("Do you want to capture more applicants? (Y/N): ");
                string response = Console.ReadLine();
                continueCapturing = response.Equals("Y", StringComparison.OrdinalIgnoreCase);
            }

            return customers;
        }

        public static void DisplayLoadingEffect(string message, int duration)
        {
            Console.Write(message);
            for (int i = 0; i < duration / 1000; i++)
            {
                Thread.Sleep(1000);
                Console.Write(".");
            }
            Console.WriteLine();
        }


        public static void Main()
        {
            string filePath = "customers.json";
            var customers = FileService.LoadCustomersFromFile(filePath);

            int qualifiedCount = 0;
            int deniedCount = 0;

            while (true)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Capture Details");
                Console.WriteLine("2. Check Game Token Credit Qualification");
                Console.WriteLine("3. Show Current Stats");
                Console.WriteLine("4. Calculate Average Pizzas Consumed");
                Console.WriteLine("5. Find Youngest and Oldest Applicant");
                Console.WriteLine("6. Check Long-term Loyalty Award");
                Console.WriteLine("7. Display Customer Report");
                Console.WriteLine("8. Exit");

                int choice = int.Parse(Console.ReadLine());
                Menu selectedOption = (Menu)(choice - 1);

                switch (selectedOption)
                {
                    case Menu.CaptureDetails:
                        customers.AddRange(CaptureDetails());
                        FileService.SaveCustomersToFile(customers, filePath);
                        break;
                    case Menu.CheckGameTokenCreditQualification:
                        var qualifiedCustomers = CustomerService.CheckQualification(customers, out qualifiedCount, out deniedCount);
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
                    case Menu.DisplayCustomerReport:
                        CustomerService.DisplayCustomerReport(customers);
                        break;
                    case Menu.Exit:
                        Console.WriteLine("Exiting the program.");
                        FileService.SaveCustomersToFile(customers, filePath);
                        return;
                }
                DisplayLoadingEffect("Processing", 3000); // 3 seconds loading effect
            }
        }
    }
}
