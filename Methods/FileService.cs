using Microsoft.AspNetCore.DataProtection;
using RetroSlices.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RetroSlices.Methods
{
    /// <summary>
    /// Provides methods for saving and loading customer data to/from a file with encryption.
    /// </summary>
    public static class FileService
    {
        private static IDataProtector _protector;

        /// <summary>
        /// Initializes the FileService class and creates a data protector.
        /// </summary>
        static FileService()
        {
            // Initialize the data protector with a purpose string (can be any unique identifier)
            IDataProtectionProvider provider = DataProtectionProvider.Create("RetroSlices.FileProtection");
            _protector = provider.CreateProtector("RetroSlices.Methods.FileService");
        }

        /// <summary>
        /// Saves the list of customers to a file with encryption.
        /// </summary>
        /// <param name="customers">The list of customers to save.</param>
        /// <param name="filePath">The file path where the encrypted data will be saved.</param>
        public static void SaveCustomersToFile(List<Customer> customers, string filePath)
        {
            // Serialize the customers list to JSON string
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(customers, options);

            // Encrypt the JSON string and write it to file
            string protectedData = _protector.Protect(jsonString);
            File.WriteAllText(filePath, protectedData);
        }

        /// <summary>
        /// Loads the list of customers from an encrypted file.
        /// </summary>
        /// <param name="filePath">The file path from which to load the encrypted data.</param>
        /// <returns>The list of customers loaded from the file.</returns>
        public static List<Customer> LoadCustomersFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<Customer>();
            }

            // Read protected data from file
            string protectedData = File.ReadAllText(filePath);

            // Decrypt the data and deserialize JSON string to customers list
            string jsonString = _protector.Unprotect(protectedData);
            return JsonSerializer.Deserialize<List<Customer>>(jsonString);
        }
    }
}
