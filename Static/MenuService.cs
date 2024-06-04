using Spectre.Console;
using System;
using System.Collections.Generic;

namespace RetroSlices.Static
{
    /// <summary>
    /// Provides methods for displaying and handling menus using Spectre.Console library.
    /// </summary>
    public static class MenuService
    {
        /// <summary>
        /// Prompts the user to select a menu choice and returns the corresponding enum value.
        /// </summary>
        /// <returns>The selected menu choice as an enum value.</returns>
        public static Menu GetMenuChoice()
        {
            // Get the names of the enum values
            string[] menuNames = Enum.GetNames(typeof(Menu));

            // Create a list to hold the formatted menu options
            List<string> formattedMenuOptions = new List<string>();

            // Populate the list with formatted menu options
            for (int i = 0; i < menuNames.Length; i++)
            {
                formattedMenuOptions.Add($"{i + 1}. {menuNames[i].Replace('_', ' ')}");
            }

            // Prompt the user to select an action using the formatted menu options
            Console.WriteLine("");
            var selectedChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]USE THE UP & DOWN ARROWS TO NAVIGATE[/]" + 
                    "\x0A" +
                    "[red]PRESS ENTER TO SELECT AN OPTION[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(formattedMenuOptions)
            );

            // Find the index of the selected choice
            int choiceIndex = formattedMenuOptions.IndexOf(selectedChoice);

            // Return the corresponding enum value
            return (Menu)choiceIndex;
        }
    }
}
