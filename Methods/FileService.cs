using RetroSlices.Classes;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace RetroSlices.Methods
{
    public static class FileService
    {
        public static void SaveCustomersToFile(List<Customer> customers, string filePath)
        {
            //serializes the the JSON file using customers as data and options to pretty format the JSON using WriteIndented
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(customers, options);
            //It then writes the JSON objects to customer.json located in the working directory's bin > Debug, folders
            File.WriteAllText(filePath, jsonString);
        }

        public static List<Customer> LoadCustomersFromFile(string filePath)
        {
            //Checks if customer.json exists in the file path, if not it returns an empty collect which our customers variable in Program.cs is assigned to
            if (!File.Exists(filePath))
            {
                return new List<Customer>();
            }

            //If it exists it will read from the file and receive the JSON string before deserializing it into a Collection of Customer data sets
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Customer>>(jsonString);
        }
    }
}
