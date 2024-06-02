using RetroSlices.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

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
                //The state of isQualified is true, however if any of the below conditions are truthy it will be set to false, denying the customer

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

                //Checks to see if the customer is qualified, if so increment qualifiedCount and add the customers data to the collection "qualifiedCustomers" or else increment deniedCound
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

            //Return the collection of qualifed customers
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
            //Check if the customer has been a member for 10+ years
            return (DateTime.Now - customer.StartDate).TotalDays >= 3650; // 10 years in days
        }

    }
}
