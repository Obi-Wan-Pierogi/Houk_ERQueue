// Lee Houk
// IT113
// NOTES: This project was quite the challenge and involved a ton of research to get working to spec.
//        I used a separate class for the PriorityQueue, Dictionary, and Patient in order to keep things 
//        more organized. I am using a Hospital class to manage requests from the menu. I found that by default 
//        the PriorityQueue did not have a stable sort, so I added a timestamp in the enqueue process in addition  
//        to the priority to keep track of when patient swere added to the queue. Then when comparing, I first
//        looked at priority, then at timestamp to keep FIFO. 
// BEHAVIORS NOT IMPLIMENTED AND WHY: I got them all working.
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;

namespace Houk_ERQueue
{
    internal class Program
    {       
        static void Main(string[] args)
        {
            // Create a hospital object
            Hospital Hospital = new();
            
            // Add patients to the queue from Patients - 1.csv, skipping the header line
            string[] lines = System.IO.File.ReadAllLines(@"Patients-1.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                string firstName = values[0];
                string lastName = values[1];
                var birthdate = DateOnly.ParseExact(values[2], "M/d/yyyy", CultureInfo.InvariantCulture);
                int priority = int.Parse(values[3]);
                Hospital.Add(new Patient(lastName, firstName, birthdate, priority));
            }

            Console.WriteLine("Welcome to the ER Priority Queue");
            bool exit = false;
            while (!exit)
            {
                // Variables for user input
                string choice;
                string lastName;
                string firstName;
                string input;
                int birthYear = 0;
                int birthMonth = 0;
                int birthDay = 0;
                int priority = 0;
                DateOnly birthdate;
                bool valid = false;

                // Generate a menu for the user to choose from a menu option using a hotkey in parantheses
                Console.WriteLine("Please choose from the following options:");
                Console.WriteLine("(A)dd a patient");
                Console.WriteLine("(P)rocess current patient");
                Console.WriteLine("(L)ist all patients in queue");
                Console.WriteLine("(Q)uit");

                // Get the user's choice
                choice = Console.ReadLine().ToUpper();

                //Process user's choice using a case switch
                switch (choice)
                {
                    case "A":
                        // Get the user's input for the patient's information
                        Console.WriteLine("Please enter the patient's last name:");
                        lastName = Console.ReadLine();
                        Console.WriteLine("Please enter the patient's first name:");
                        firstName = Console.ReadLine();
                        Console.WriteLine("Please enter the year the patient was born (YYYY):");
                        input = Console.ReadLine();
                        // Validate the year to be the current year or earlier
                        valid = false;
                        while (!valid)
                        {
                            if (int.TryParse(input, out birthYear) && birthYear < DateTime.Today.Year)
                            {
                                valid = true;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid year (YYYY):");
                                input = Console.ReadLine();
                            }
                        }

                        Console.WriteLine("Please enter the month the patient was born (1-12):");
                        input = Console.ReadLine();

                        // Validate the month to be between 1 and 12
                        valid = false;
                        while (!valid)
                        {
                            if (int.TryParse(input, out birthMonth) && birthMonth >= 1 && birthMonth <= 12)
                            {
                                valid = true;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid month (1-12):");
                                input = Console.ReadLine();
                            }
                        }

                        Console.WriteLine("Please enter the day the patient was born (1-" + DateTime.DaysInMonth(birthYear, birthMonth) + "):");
                        input = Console.ReadLine();
                        // Validate the day to be valid for the month provided
                        valid = false;
                        while (!valid)
                        {
                            if (int.TryParse(input, out birthDay) && birthDay >= 1 && birthDay <= DateTime.DaysInMonth(birthYear, birthMonth))
                            {
                                valid = true;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid day (1-" + DateTime.DaysInMonth(birthYear, birthMonth) + "):");
                                input = Console.ReadLine();
                            }
                        }
                        // Create a new DateOnly object using the validated year, month, and day
                        birthdate = new DateOnly(birthYear, birthMonth, birthDay);
                        // Get the patient's priority
                        Console.WriteLine("Please enter the patient's priority (1-5):");
                        input = Console.ReadLine();
                        // Validate the priority to be between 1 and 5
                        valid = false;
                        while (!valid)
                        {
                            if (int.TryParse(input, out priority) && priority >= 1 && priority <= 5)
                            {
                                valid = true;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid priority (1-5):");
                                input = Console.ReadLine();
                            }
                        }
                        // Add the patient to the priority queue
                        Console.WriteLine(Hospital.Add(new Patient(lastName, firstName, birthdate, priority)));
                        break;
                    case "P":
                        // Process the patient
                        Console.WriteLine(Hospital.Process());
                        break;
                    case "L":
                        // List all patients
                        Console.WriteLine(Hospital.ListAll());
                        break;
                    case "Q":
                        // Quit the program
                        exit = true;
                        break;
                }
            }            
        }
    }
}