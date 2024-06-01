using RetroSlices.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroSlices.Methods
{
    public static class CustomerService
    {
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

        public static void DisplayCustomerReport(List<Customer> customers)
        {
            Console.WriteLine("Customer Report:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"Name: {customer.Name}, Age: {customer.Age}, High Score Rank: {customer.HighScoreRank}, Start Date: {customer.StartDate.ToShortDateString()}, Pizzas Consumed: {customer.PizzasConsumed}, Bowling High Score: {customer.BowlingHighScore}, Employed: {customer.IsEmployed}, Slush Puppy Preference: {customer.SlushPuppyPreference}, Slush Puppies Consumed: {customer.SlushPuppiesConsumed}");
            }
        }

        public static bool CheckLongTermLoyaltyAward(Customer customer)
        {
            return (DateTime.Now - customer.StartDate).TotalDays >= 3650; // 10 years in days
        }

    }
}
