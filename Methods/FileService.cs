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
    }
}
