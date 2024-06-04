using System;

namespace RetroSlices.Classes
{
    /// <summary>
    /// Outlines the data required for a customer profile
    /// </summary>
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

        /// <summary>
        /// A constructor class used to initialize a new customer based on provided details
        /// </summary>
        /// <param></param>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <param name="highScoreRank"></param>
        /// <param name="startDate"></param>
        /// <param name="pizzasConsumed"></param>
        /// <param name="bowlingHighScore"></param>
        /// <param name="isEmployed"></param>
        /// <param name="slushPuppyPreference"></param>
        /// <param name="slushPuppiesConsumed"></param>
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
}
