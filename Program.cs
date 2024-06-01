using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

namespace RetroSlices
{
    internal class Program
    {
        public enum Menu
        {
            CaptureDetails,
            CheckGameTokenCreditQualification,
            DisplayCustomerReport,
            CheckLongTermLoyaltyAward,
            FindYoungestAndOldestApplicant,
            CalculateAveragePizzasConsumed,
            ShowCurrentStats,
            Exit
        }

        public class Customer
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public int HighScoreRank { get; set; }
            public DateTime StartDate { get; set; }
            public int PizzasConsumed { get; set; }
            public int BowlingHighScore { get; set; }
            public bool IsEmployed { get; set; }
            public string SlushPuppyPreference { get; set; }
            public int SlushPuppiesConsumed { get; set; }

            public Customer(string name, int age, int highScoreRank, DateTime startDate, int pizzasConsumed, int bowlingHighScore, bool isEmployed, string slushPuppyPreference, int slushPuppiesConsumed)
            {
                Name = name;
                Age = age;
                HighScoreRank = highScoreRank;
                StartDate = startDate;
                PizzasConsumed = pizzasConsumed;
                BowlingHighScore = bowlingHighScore;
                IsEmployed = isEmployed;
                SlushPuppyPreference = slushPuppyPreference;
                SlushPuppiesConsumed = slushPuppiesConsumed;
            }
        }

        public static List<Customer> CheckQualification(List<Customer> customers, out int qualifiedCount, out int deniedCount)
        {
            var qualifiedCustomers = new List<Customer>();
            qualifiedCount = 0;
            deniedCount = 0;

            foreach (var customer in customers)
            {
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

                if (isQualified)
                {
                    qualifiedCustomers.Add(customer);
                    qualifiedCount++;
                }
                else
                {
                    deniedCount++;
                }
            }

            return qualifiedCustomers;
        }

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

        public static double CalculateAveragePizzasConsumed(List<Customer> customers)
        {
            if (customers.Count == 0) return 0;

            double totalPizzas = customers.Sum(customer => customer.PizzasConsumed);
            double averagePizzas = totalPizzas / customers.Count;

            return averagePizzas;
        }

        public static (int, int) GetYoungestAndOldestApplicant(List<Customer> customers)
        {
            if (customers.Count == 0) throw new InvalidOperationException("No customers in the list.");

            int youngest = int.MaxValue;
            int oldest = int.MinValue;

            foreach (var customer in customers)
            {
                if (customer.Age < youngest) youngest = customer.Age;
                if (customer.Age > oldest) oldest = customer.Age;
            }

            return (youngest, oldest);
        }

        public static bool CheckLongTermLoyaltyAward(Customer customer)
        {
            return (DateTime.Now - customer.StartDate).TotalDays >= 3650; // 10 years in days
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

        public static void DisplayCustomerReport(List<Customer> customers)
        {
            Console.WriteLine("Customer Report:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"Name: {customer.Name}, Age: {customer.Age}, High Score Rank: {customer.HighScoreRank}, Start Date: {customer.StartDate.ToShortDateString()}, Pizzas Consumed: {customer.PizzasConsumed}, Bowling High Score: {customer.BowlingHighScore}, Employed: {customer.IsEmployed}, Slush Puppy Preference: {customer.SlushPuppyPreference}, Slush Puppies Consumed: {customer.SlushPuppiesConsumed}");
            }
        }

        public static void SaveCustomersToFile(List<Customer> customers, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(customers, options);
            File.WriteAllText(filePath, jsonString);
        }

        public static List<Customer> LoadCustomersFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<Customer>();
            }

            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Customer>>(jsonString);
        }




        public static void Main()
        {
            string filePath = "customers.json";
            var customers = LoadCustomersFromFile(filePath);

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
                        SaveCustomersToFile(customers, filePath);
                        break;
                    case Menu.CheckGameTokenCreditQualification:
                        var qualifiedCustomers = CheckQualification(customers, out qualifiedCount, out deniedCount);
                        Console.WriteLine("Qualified Customers:");
                        foreach (var customer in qualifiedCustomers)
                        {
                            Console.WriteLine($"Name: {customer.Name}");
                        }
                        SaveCustomersToFile(customers, filePath);
                        break;
                    case Menu.ShowCurrentStats:
                        DisplayStats(qualifiedCount, deniedCount);
                        break;
                    case Menu.CalculateAveragePizzasConsumed:
                        double averagePizzas = CalculateAveragePizzasConsumed(customers);
                        Console.WriteLine($"Average Pizzas Consumed per First Visit: {averagePizzas}");
                        break;
                    case Menu.FindYoungestAndOldestApplicant:
                        var (youngest, oldest) = GetYoungestAndOldestApplicant(customers);
                        Console.WriteLine($"Youngest Applicant: {youngest}");
                        Console.WriteLine($"Oldest Applicant: {oldest}");
                        break;
                    case Menu.CheckLongTermLoyaltyAward:
                        Console.WriteLine("Enter applicant name to check for loyalty award:");
                        string name = Console.ReadLine();
                        var customerToCheck = customers.FirstOrDefault(c => c.Name == name);
                        if (customerToCheck != null)
                        {
                            bool isLoyal = CheckLongTermLoyaltyAward(customerToCheck);
                            Console.WriteLine(isLoyal ? "Customer qualifies for long-term loyalty award." : "Customer does not qualify for long-term loyalty award.");
                        }
                        else
                        {
                            Console.WriteLine("Customer not found.");
                        }
                        break;
                    case Menu.DisplayCustomerReport:
                        DisplayCustomerReport(customers);
                        break;
                    case Menu.Exit:
                        Console.WriteLine("Exiting the program.");
                        SaveCustomersToFile(customers, filePath);
                        return;
                }
                DisplayLoadingEffect("Processing", 3000); // 3 seconds loading effect
            }
        }
    }
}
