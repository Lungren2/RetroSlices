using System;

namespace RetroSlices.Static
{
    public static class MenuService
    {
        public static Menu GetMenuChoice()
        {
            // Get the names of the enum values
            string[] menuNames = Enum.GetNames(typeof(Menu));

            // Display the menu options dynamically
            Console.WriteLine("Select an option:");
            for (int i = 0; i < menuNames.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {menuNames[i].Replace('_', ' ')}");
            }

            int choice;
            // Ensure the user's choice is valid
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > menuNames.Length)
            {
                Console.WriteLine($"Invalid choice, please select a number between 1 and {menuNames.Length}.");
            }

            // Return the corresponding enum value
            return (Menu)(choice - 1);
        }

    }
}
