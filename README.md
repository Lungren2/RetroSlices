![Logo](logo.png)

# RetroSlice Loyalty Program Application

RetroSlice is an entertainment complex featuring an arcade, bowling alley, and pizzeria. This application helps Uncle Rocco Calzone, the owner of RetroSlice, manage and determine the list of loyal customers (Retroslicers) who qualify for game token credits and long-term loyalty awards. The application captures customer details, checks qualification criteria, calculates averages, and displays various statistics.

## Features

1. **Capture Customer Details**:

   - Input customer details such as name, age, high score rank, starting date, pizzas consumed, bowling high score, employment status, slush-puppy preference, and slush-puppies consumed.
   - Store the details in a collection.

2. **Check Game Token Credit Qualification**:

   - Determine if customers qualify for game token credits based on the following criteria:
     - Employment status (or parents' employment status for customers under 18).
     - Loyalty duration of at least 2 years.
     - High score rank above 2000, bowling high score above 1500, or a combined average above 1200.
     - Average consumption of at least 3 pizzas per month.
     - Consumed more than 4 slush-puppies per month and preference is not 'Gooey Gulp Galore'.
   - Store qualified customers in a separate collection.
   - Count and display the number of applicants granted and denied tokens.

3. **Show Current Stats**:

   - Display the statistics of qualified and denied applicants.

4. **Calculate Average Pizzas Consumed**:

   - Calculate and return the average number of pizzas consumed per visit.

5. **Find Youngest and Oldest Applicant**:

   - Retrieve and display the youngest and oldest applicants' ages.

6. **Check Long-term Loyalty Award**:

   - Determine if a customer qualifies for an unlimited number of credits by being a loyal customer for 10 years.

7. **Loading Effects**:

   - Implement time-based loading effects for actions that require longer wait times using threading in C#.

8. **Customer Data Persistence**:
   - Use JSON files to store and retrieve customer data, ensuring data persistence without relying on external databases.

## Authors

| Name               | Student Number |
| ------------------ | -------------- |
| Ethan Ogle         | 602114         |
| Aphiwe Shabalala   | 602517         |
| Ryno Hartman       | 60229          |
| Johan Bezuidenhout | 602509         |

## Demo

Insert gif or link to demo

## Run Locally

Clone the project

```bash
  git clone https://github.com/Lungren2/RetroSlices
```

Go to the project directory

```bash
  cd RetroSlices
```

Build and Run Program.cs

```bash
  Fn + F5
```

## File Structure

```bash
C:.
├───Classes
├───Methods
├───packages
│   ├───Microsoft.Bcl.AsyncInterfaces.8.0.0
│   ├───Spectre.Console.0.49.1
│   ├───Spectre.Console.Cli.0.49.1
│   ├───System.Buffers.4.5.1
│   ├───System.Memory.4.5.5
│   ├───System.Numerics.Vectors.4.5.0
│   ├───System.Runtime.CompilerServices.Unsafe.6.0.0
│   ├───System.Text.Encodings.Web.8.0.0
│   ├───System.Text.Json.8.0.3
│   ├───System.Threading.Tasks.Extensions.4.5.4
│   └───System.ValueTuple.4.5.0
├───Properties
└───Static
```

## Additional Features

- Data Persistance using JSON files
- Terminal UI using Spectre.Console
